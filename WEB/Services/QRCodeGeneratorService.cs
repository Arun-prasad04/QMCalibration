using QRCoder;
using Microsoft.Extensions.Options;
using WEB.Models;
using WEB.Services.Interface;
using CMT.DAL;
using CMT.DATAMODELS;
using AutoMapper;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace WEB.Services
{
     public class QRCodeGeneratorService : IQRCodeGeneratorService
    {
        private QRCodeSettings _qrCodeSettings { get; set; }
        private IConfiguration _configuration;

        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }
        public QRCodeGeneratorService(IOptions<QRCodeSettings> qrCodeSettings, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _qrCodeSettings = qrCodeSettings.Value;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ResponseViewModel<QRCodeFilesViewModel> InsertQRCodeFiles(List<QRCodeFilesViewModel> qrCodeFilesViewModels)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _unitOfWork.Repository<QRCodeFiles>().Insert(_mapper.Map<QRCodeFiles>(qrCodeFilesViewModels));
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();

                return new ResponseViewModel<QRCodeFilesViewModel>
                {
                    ResponseCode = 200,
                    ResponseMessage = "Success",
                    ResponseData = null,
                    ResponseDataList = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseViewModel<QRCodeFilesViewModel>
                {
                    ResponseCode = 500,
                    ResponseMessage = "Failure",
                    ErrorMessage = ex.Message,
                    ResponseData = null,
                    ResponseDataList = null,
                    ResponseService = "User",
                    ResponseServiceMethod = "InsertQRCodeFiles"
                };

            }

        }

        public QRCodeFilesViewModel GetQRCodeDetails(QRCodeFilesViewModel qrCodeFilesViewModel)
        {
            try
            {
                QRCodeFilesViewModel qrCodeFilesCurrentData = _mapper.Map<QRCodeFilesViewModel>
                                                                   (_unitOfWork.Repository<QRCodeFiles>()
                                                                                  .GetQueryAsNoTracking()
                                                                                  .Where(x => x.RequestId == qrCodeFilesViewModel.RequestId
                                                                                        && x.InstrumentId == qrCodeFilesViewModel.InstrumentId)
                                                                                  .OrderByDescending(x => x.Id)
                                                                                  .ToList()
                                                                                  .FirstOrDefault()                                                                         
                                                                   );
                return qrCodeFilesCurrentData;
            }
            catch (Exception ex)
            {
                return new QRCodeFilesViewModel() { };
            }
        }
        public QRCodeFilesViewModel GetQRCodeByGuid(Guid guid)
        {
            try
            {
                QRCodeFilesViewModel qrCodeFilesCurrentData = _mapper.Map<QRCodeFilesViewModel>
                                                                   (_unitOfWork.Repository<QRCodeFiles>()
                                                                                  .GetQueryAsNoTracking()
                                                                                  .Where(x => x.UrlGuid == guid)
                                                                                  .OrderByDescending(x => x.Id)
                                                                                  .SingleOrDefault()
                                                                   );
                return qrCodeFilesCurrentData;
            }
            catch (Exception ex)
            {
                return new QRCodeFilesViewModel() { };
            }
        }

        public RequestViewModel GetRequestData(int requestId)
        {
            try
            {
                RequestViewModel requestData = _mapper.Map<RequestViewModel>
                                                   (
                                                       _unitOfWork.Repository<Request>()
                                                        .GetQueryAsNoTracking(Q => Q.Id == requestId)
                                                        .SingleOrDefault()
                                                   );

                return requestData;
            }
            catch (Exception ex)
            {
                return new RequestViewModel() { };
            }
        }
        public QRCodeFilesViewModel QRCodeGeneration(QRCodeFilesViewModel qrCodeGenInputViewModel)
        {
            //get Existing QR Code Data
            QRCodeFilesViewModel existingData = GetQRCodeDetails(qrCodeGenInputViewModel);

            string applicationUrl = _configuration["AppUrl"];
            string templateName = qrCodeGenInputViewModel.TemplateName;
            string qrEncodeText = string.Concat(applicationUrl, "/", templateName);
            string? requestNo = qrCodeGenInputViewModel.RequestNo;

            Guid guid = Guid.NewGuid();
            string amendmentNo = string.Empty;
            string drawText = string.Empty;
            string fileName = string.Empty;

            if (existingData != null)
            {
                guid = existingData.UrlGuid;
                amendmentNo = existingData.AmendmentNo;
                fileName = existingData.FileName;
            }

            drawText = string.Concat(requestNo, " ", amendmentNo);
            qrEncodeText = string.Concat(qrEncodeText, guid.ToString());

            Bitmap qrCodeImage = GetQRCodeImage(qrEncodeText);
            qrCodeImage = GenerateQRBitmapTextandImage(qrCodeImage, drawText);
            var QRDecodeByteData = BitmapToBytes(qrCodeImage);

            QRCodeFilesViewModel outputData = new QRCodeFilesViewModel()
            {
                QRImageUrl = String.Format(Constants.QRCODE_IMAGE_FORMAT, Convert.ToBase64String(QRDecodeByteData)),
                DecodeText =  ResizeImage(QRDecodeByteData),
                RequestId = qrCodeGenInputViewModel.RequestId,
                InstrumentId = qrCodeGenInputViewModel.InstrumentId,
                RequestNo = requestNo,
                UrlGuid = guid,
                FileName = fileName,
                AmendmentNo = amendmentNo,
                QRFilepath = string.Concat(applicationUrl, "/", templateName,"/")
            };

            return outputData;
        }

        private Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Byte value is converted to bitmap image format
        /// </summary>
        /// <param name="bmImage"></param>
        /// <param name="drawText"></param>
        /// <returns>Bitmap image</returns>
        private Bitmap GenerateQRBitmapTextandImage(Bitmap bmImage, string drawText)
        {
            var newBitmap = new Bitmap(bmImage.Width + 150, bmImage.Height + 150);

            Graphics graphics = Graphics.FromImage(newBitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            StringFormat format = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Far
            };

            if (!string.IsNullOrEmpty(drawText))
            {
                RectangleF rectfDrawText = new RectangleF(0, 0, newBitmap.Width-100, newBitmap.Height-60);
                Rectangle rectDrawImage = new Rectangle(0, 0, bmImage.Width, bmImage.Height);

                graphics.DrawString(drawText, new Font(Constants.QRCODE_FONT_NAME, Constants.QRCODE_FONT_SIZE, FontStyle.Bold), Brushes.Black, rectfDrawText, format);
                graphics.DrawImage(bmImage, rectDrawImage);
                graphics.Flush();
            }
            return newBitmap;
        }
        private Bitmap GetQRCodeImage(string qrcodeUrl)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrcodeUrl, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(10);

            Bitmap qrCodeImage;
            using (var ms = new MemoryStream(qrCodeAsPngByteArr))
            {
                qrCodeImage = new Bitmap(ms);
            }
            return qrCodeImage;
        }

        private byte[] ResizeImage(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                var image = Image.FromStream(ms);

                var ratioX = (double)100 / image.Width;
                var ratioY = (double)100 / image.Height;

                var ratio = Math.Min(ratioX, ratioY);

                var width = (int)(image.Width * ratio);
                var height = (int)(image.Height * ratio);

                var newImage = new Bitmap(width, height);

                Graphics.FromImage(newImage).DrawImage(image, 0, 0, width, height);

                Bitmap bmp = new Bitmap(newImage);

                ImageConverter converter = new ImageConverter();

                data = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

               // return "data:image/*;base64," + Convert.ToBase64String(data);

               return data;
            }
        }
    }
}
