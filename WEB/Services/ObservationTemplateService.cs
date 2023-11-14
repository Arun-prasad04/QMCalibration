using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using AutoMapper;
using WEB.Models;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using Microsoft.Extensions.Options;
using System.Text;
using HtmlAgilityPack;
using DATAMODELS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WEB.Controllers;
using MathNet.Numerics;
using System;
using WEB.Models.Templates;
using Microsoft.VisualBasic;

namespace WEB.Services;
public class ObservationTemplateService : IObservationTemplateService
{
	private readonly IMapper _mapper;
	private IUnitOfWork _unitOfWork { get; set; }
	private IQRCodeGeneratorService _qrCodeGeneratorService { get; set; }
	private QRCodeSettings _qrCodeSettings { get; set; }

	private IConfiguration _configuration;

	private IHttpContextAccessor _contextAccessor { get; set; }
	private IEmailService _emailService;
	private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
	public ObservationTemplateService(IOptions<QRCodeSettings> qrCodeSettings, IUnitOfWork unitOfWork, IMapper mapper,
										IUtilityService utilityService, IQRCodeGeneratorService qrCodeGeneratorService,
										 IConfiguration configuration,
										Microsoft.AspNetCore.Hosting.IHostingEnvironment environment,IHttpContextAccessor contextAccessor, IEmailService emailService
									   )
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_qrCodeGeneratorService = qrCodeGeneratorService;
		_qrCodeSettings = qrCodeSettings.Value;
		_environment = environment;
		_configuration = configuration;
		_contextAccessor = contextAccessor;
		_emailService = emailService;

	}
	#region "Lever Type Dial Template"
	public ResponseViewModel<LeverTypeDialViewModel> InsertLeverDial(LeverTypeDialViewModel levertypedial)
	{

		try
		{
			_unitOfWork.BeginTransaction();
			int templateObservationId = 0;

			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
														 .GetQueryAsNoTracking(Q => Q.RequestId == levertypedial.RequestId
																					&& Q.InstrumentId == levertypedial.InstrumentId)
														 .SingleOrDefault();

			// User labTechnicalManager = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4).SingleOrDefault();
			//if (levertypedial.Id == 0)
			if ((levertypedial.Id == 0) && (observationById == null))

			{
				//if (observationById == null)
				//{
				TemplateObservation templateObservation = new TemplateObservation()
				{
					InstrumentId = levertypedial.InstrumentId,
					RequestId = levertypedial.RequestId,
					TempStart = levertypedial.TempStart,
					TempEnd = levertypedial.TempEnd,
					Humidity = levertypedial.Humidity,
					InstrumentCondition = levertypedial.DialIndicatiorCondition,
					RefWi = levertypedial.RefWi,
					Allvalues = levertypedial.Allvalues,
					CreatedOn = DateTime.Now,
					CreatedBy = levertypedial.CreatedBy,
					// CalibrationReviewedBy = labTechnicalManager.Id,
					CalibrationReviewedDate = DateTime.Now
				};

				_unitOfWork.Repository<TemplateObservation>().Insert(templateObservation);
				_unitOfWork.SaveChanges();
				templateObservationId = templateObservation.Id;

				//}

			}
			else
			{
				//TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
				//												 .GetQueryAsNoTracking(Q => Q.RequestId == levertypedial.RequestId
				//																			&& Q.InstrumentId == levertypedial.InstrumentId)
				//												 .SingleOrDefault();
				if (observationById != null)
				{

					if (levertypedial.TempStart != null)
					{
						observationById.TempStart = levertypedial.TempStart;
					}

					if (levertypedial.TempEnd != null)
					{
						observationById.TempEnd = levertypedial.TempEnd;
					}

					if (levertypedial.Humidity != null)
					{
						observationById.Humidity = levertypedial.Humidity;
					}

					if (levertypedial.DialIndicatiorCondition != null)
					{
						observationById.InstrumentCondition = levertypedial.DialIndicatiorCondition;
					}
					if (levertypedial.RefWi != null)
					{
						observationById.RefWi = levertypedial.RefWi;
					}
					if (levertypedial.Allvalues != null)
					{
						observationById.Allvalues = levertypedial.Allvalues;
					}
					_unitOfWork.Repository<TemplateObservation>().Update(observationById);
					_unitOfWork.SaveChanges();
				}
			}

			if (levertypedial.Id == 0)// && (levertypedial == null))
			{
				if (observationById != null)
				{
					templateObservationId = observationById.Id;
				}
				ObsTemplateLeverTypeDial leverdial = new ObsTemplateLeverTypeDial()
				{
					ObservationId = templateObservationId,
					DirectionA1 = levertypedial.MeasuringRangeDirectionA1,
					DirectionA2 = levertypedial.ScaleDivisionDirectionA2,
					DirectionA3 = levertypedial.HysteresisDirectionA3,
					DirectionA4 = levertypedial.RepeatabilityDirectionA4,
					DirectionB1 = levertypedial.MeasuringRangeDirectionB1,
					DirectionB2 = levertypedial.ScaleDivisionDirectionB2,
					DirectionB3 = levertypedial.HysteresisDirectionB3,
					DirectionB4 = levertypedial.RepeatabilityDirectionB4,
					Specification1 = levertypedial.MeasuringRangeSpec,
					Specification2 = levertypedial.ScaleDivisionSpec,
					Specification3 = levertypedial.HysteresisSpec,
					Specification4 = levertypedial.RepeatabilitySpec,
				};
				_unitOfWork.Repository<ObsTemplateLeverTypeDial>().Insert(leverdial);
			}
			else
			{
				ObsTemplateLeverTypeDial leverTypeDialById = _unitOfWork.Repository<ObsTemplateLeverTypeDial>()
																		.GetQueryAsNoTracking(Q => Q.Id == levertypedial.Id)
																		.SingleOrDefault();
				if (levertypedial.MeasuringRangeDirectionA1 != null)
				{
					leverTypeDialById.DirectionA1 = levertypedial.MeasuringRangeDirectionA1;
				}
				if (levertypedial.MeasuringRangeDirectionB1 != null)
				{
					leverTypeDialById.DirectionB1 = levertypedial.MeasuringRangeDirectionB1;
				}

				if (levertypedial.ScaleDivisionDirectionA2 != null)
				{
					leverTypeDialById.DirectionA2 = levertypedial.ScaleDivisionDirectionA2;
				}
				if (levertypedial.ScaleDivisionDirectionB2 != null)
				{
					leverTypeDialById.DirectionB2 = levertypedial.ScaleDivisionDirectionB2;
				}


				if (levertypedial.HysteresisDirectionA3 != null)
				{
					leverTypeDialById.DirectionA3 = levertypedial.HysteresisDirectionA3;
				}
				if (levertypedial.HysteresisDirectionB3 != null)
				{
					leverTypeDialById.DirectionB3 = levertypedial.HysteresisDirectionB3;
				}

				if (levertypedial.RepeatabilityDirectionA4 != null)
				{
					leverTypeDialById.DirectionA4 = levertypedial.RepeatabilityDirectionA4;
				}
				if (levertypedial.RepeatabilityDirectionA4 != null)
				{
					leverTypeDialById.DirectionB4 = levertypedial.RepeatabilityDirectionB4;
				}
				if (levertypedial.MeasuringRangeSpec != null)
				{
					leverTypeDialById.Specification1 = levertypedial.MeasuringRangeSpec;
				}
				if (levertypedial.ScaleDivisionSpec != null)
				{
					leverTypeDialById.Specification2 = levertypedial.ScaleDivisionSpec;
				}
				if (levertypedial.HysteresisSpec != null)
				{
					leverTypeDialById.Specification3 = levertypedial.HysteresisSpec;
				}
				if (levertypedial.RepeatabilitySpec != null)
				{
					leverTypeDialById.Specification4 = levertypedial.RepeatabilitySpec;
				}
				_unitOfWork.Repository<ObsTemplateLeverTypeDial>().Update(leverTypeDialById);
			}
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			return new ResponseViewModel<LeverTypeDialViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - InsertData Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<LeverTypeDialViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = levertypedial,
				ResponseDataList = null,
				ResponseService = "ObsTemplateLeverTypeDial",
				ResponseServiceMethod = "InsertData"
			};
		}

	}
	public ResponseViewModel<LeverTypeDialViewModel> GetLeverDialById(int requestId, int instrumentId)
	{
		try
		{
			LeverTypeDialViewModel leverTypeDial = _unitOfWork.Repository<TemplateObservation>()
			.GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
			.Select(s => new LeverTypeDialViewModel()
			{
				TemplateObservationId = s.Id,
				TempStart = s.TempStart,
				TempEnd = s.TempEnd,
				Humidity = s.Humidity,
				RefWi = s.RefWi,
				Allvalues = s.Allvalues,
				ReviewStatus = s.ReviewStatus,
				CreatedBy = s.CreatedBy,
				CalibrationReviewedBy = s.CalibrationReviewedBy,
				CalibrationPerformedDate = s.CreatedOn,
				CalibrationReviewedDate = s.CalibrationReviewedDate,
				DialIndicatiorCondition = s.InstrumentCondition,
				ULRNumber = s.ULRNumber,
				CertificateNumber = s.CertificateNumber,
				Id = s.LeverTypeDialModel.Select(s => s.Id).FirstOrDefault(),
				MeasuringRangeSpec = s.LeverTypeDialModel.Select(S => S.Specification1).SingleOrDefault(),
				MeasuringRangeDirectionA1 = s.LeverTypeDialModel.Select(S => S.DirectionA1).SingleOrDefault(),
				MeasuringRangeDirectionB1 = s.LeverTypeDialModel.Select(S => S.DirectionB1).SingleOrDefault(),
				ScaleDivisionSpec = s.LeverTypeDialModel.Select(S => S.Specification2).SingleOrDefault(),
				ScaleDivisionDirectionA2 = s.LeverTypeDialModel.Select(S => S.DirectionA2).SingleOrDefault(),
				ScaleDivisionDirectionB2 = s.LeverTypeDialModel.Select(S => S.DirectionB2).SingleOrDefault(),
				HysteresisSpec = s.LeverTypeDialModel.Select(S => S.Specification3).SingleOrDefault(),
				HysteresisDirectionA3 = s.LeverTypeDialModel.Select(S => S.DirectionA3).SingleOrDefault(),
				HysteresisDirectionB3 = s.LeverTypeDialModel.Select(S => S.DirectionB3).SingleOrDefault(),
				RepeatabilitySpec = s.LeverTypeDialModel.Select(S => S.Specification4).SingleOrDefault(),
				RepeatabilityDirectionA4 = s.LeverTypeDialModel.Select(S => S.DirectionA4).SingleOrDefault(),
				RepeatabilityDirectionB4 = s.LeverTypeDialModel.Select(S => S.DirectionB4).SingleOrDefault(),
				EnvironmentCondition = s.LeverTypeDialModel.Select(S => S.EnvironmentCondition).SingleOrDefault(),
				Uncertainity = s.LeverTypeDialModel.Select(S => S.Uncertainity).SingleOrDefault(),
				CalibrationResult = s.LeverTypeDialModel.Select(S => S.CalibrationResult).SingleOrDefault(),
				Remarks = s.LeverTypeDialModel.Select(S => S.Remarks).SingleOrDefault(),
			}).SingleOrDefault();

			if (leverTypeDial != null)
			{
				List<string> performedUserData = GetUserName(leverTypeDial.CreatedBy);

				if (performedUserData.Count >= 3)
				{
					leverTypeDial.CalibrationPerformedBy = performedUserData[0];
					leverTypeDial.PerformedBySign = performedUserData[1];
					leverTypeDial.PerformedByDesignation = performedUserData[2];
				}

				List<string> reviewedUserData = GetUserName(leverTypeDial.CalibrationReviewedBy);

				if (reviewedUserData.Count >= 3)
				{
					leverTypeDial.ReviewedBy = reviewedUserData[0];
					leverTypeDial.ReviewedBySign = reviewedUserData[1];
					leverTypeDial.ReviewedByDesignation = reviewedUserData[2];
				}

				int? ulrNumber = leverTypeDial.ULRNumber == null ? 0 : leverTypeDial.ULRNumber;
				int? certificateNumber = leverTypeDial.CertificateNumber == null ? 0 : leverTypeDial.CertificateNumber;
				List<string> formatList = GetULRAndCertificateNumber(ulrNumber, certificateNumber);

				if (formatList.Count >= 2)
				{
					leverTypeDial.ULRFormat = formatList[0];
					leverTypeDial.CertificateFormat = formatList[1];
				}

			}
			return new ResponseViewModel<LeverTypeDialViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = leverTypeDial,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("ObservationTemplateService - GetLeverDialById Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<LeverTypeDialViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "GetLeverDialById"
			};
		}
	}

	#endregion

	#region "Micrometer"
	public ResponseViewModel<MicrometerViewModel> InsertMicrometer(MicrometerViewModel micrometer)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			int tempobsId = 0;
			int ObjMicroData = 0;
			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
																 .GetQueryAsNoTracking(Q => Q.RequestId == micrometer.RequestId
																  && Q.InstrumentId == micrometer.InstrumentId).SingleOrDefault();
			//User labTechnicalManager = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4).SingleOrDefault();
			if ((micrometer.TemplateObservationId == 0) && (observationById == null))

			{
				//if (observationById == null)
				//{
				TemplateObservation templateObservation = new TemplateObservation()
				{
					InstrumentId = micrometer.InstrumentId,
					RequestId = micrometer.RequestId,
					TempStart = micrometer.TempStart,
					TempEnd = micrometer.TempEnd,
					Humidity = micrometer.Humidity,
					InstrumentCondition = micrometer.MicrometerCondition,
					RefWi = micrometer.RefWi,
					Allvalues = micrometer.Allvalues,
					CreatedOn = DateTime.Now,
					CreatedBy = micrometer.CreatedBy,
					//   CalibrationReviewedBy = labTechnicalManager.Id,
					CalibrationReviewedDate = DateTime.Now
				};
				//tempobsId = templateObservation.Id;
				_unitOfWork.Repository<TemplateObservation>().Insert(templateObservation);
				_unitOfWork.SaveChanges();
				tempobsId = templateObservation.Id;
				//}
			}
			else
			{
				//TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
				//												 .GetQueryAsNoTracking(Q => Q.RequestId == micrometer.RequestId
				//												  && Q.InstrumentId == micrometer.InstrumentId).SingleOrDefault();
				//tempobsId = observationById.Id;
				if (observationById != null)
				{


					if (micrometer.TempStart != null)
					{
						observationById.TempStart = micrometer.TempStart;
					}

					if (micrometer.TempEnd != null)
					{
						observationById.TempEnd = micrometer.TempEnd;
					}

					if (micrometer.Humidity != null)
					{
						observationById.Humidity = micrometer.Humidity;
					}

					if (micrometer.MicrometerCondition != null)
					{
						observationById.InstrumentCondition = micrometer.MicrometerCondition;
					}
					if (micrometer.RefWi != null)
					{
						observationById.RefWi = micrometer.RefWi;
					}
					if (micrometer.Allvalues != null)
					{
						observationById.Allvalues = micrometer.Allvalues;
					}

					_unitOfWork.Repository<TemplateObservation>().Update(observationById);
					_unitOfWork.SaveChanges();
				}
			}

			if (micrometer.Id == 0)
			{
				if (observationById != null)
				{
					tempobsId = observationById.Id;
				}

				ObsTemplateMicrometer obsTemplateMicrometer = new ObsTemplateMicrometer()
				{

					ObservationId = tempobsId,

					Flatness1 = micrometer.Flatness1, //  To check the Flatness
													  //Flatness2 = micrometer.Flatness2,
					ParallelismSpec = micrometer.ParallelismSpec, //  To check the Parallelism

					Actuals = micrometer.Actuals, // To check the Instrument Error

					ActualsT11 = micrometer.ActualsT11,
					Avg1 = micrometer.Avg1,
					MuInterval1 = micrometer.MuInterval1,

					//ActualsT21 = micrometer.ActualsT21,
					//ActualsT31 = micrometer.ActualsT31,
					ActualsT12 = micrometer.ActualsT12,
					Avg2 = micrometer.Avg2,
					MuInterval2 = micrometer.MuInterval2,
					//ActualsT22 = micrometer.ActualsT22,
					//ActualsT32 = micrometer.ActualsT32,

					ActualsT13 = micrometer.ActualsT13,
					Avg3 = micrometer.Avg3,
					MuInterval3 = micrometer.MuInterval3,

					ActualsT14 = micrometer.ActualsT14,
					Avg4 = micrometer.Avg4,
					Measurement4 = micrometer.Measurement4,

					ActualsT15 = micrometer.ActualsT15,
					Avg5 = micrometer.Avg5,
					Measurement5 = micrometer.Measurement5,

					ActualsT16 = micrometer.ActualsT16,
					Avg6 = micrometer.Avg6,
					Measurement6 = micrometer.Measurement6,

					ActualsT17 = micrometer.ActualsT17,
					Avg7 = micrometer.Avg7,
					Measurement7 = micrometer.Measurement7,

					//ActualsT23 = micrometer.ActualsT23,
					//ActualsT33 = micrometer.ActualsT33,

					//ActualsT14 = micrometer.ActualsT14,
					//ActualsT24 = micrometer.ActualsT24,
					//ActualsT34 = micrometer.ActualsT34,
					//Avg4 = micrometer.Avg4,
					//MuInterval4 = micrometer.MuInterval4,
					//ActualsT15 = micrometer.MuInterval4,
					//ActualsT25 = micrometer.ActualsT25,
					//ActualsT35 = micrometer.ActualsT35,
					//Avg5 = micrometer.Avg5,
					//MuInterval5 = micrometer.MuInterval5,
					//ActualsT16 = micrometer.ActualsT16,
					//ActualsT26 = micrometer.ActualsT26,
					//ActualsT36 = micrometer.ActualsT36,
					//Avg6 = micrometer.Avg6,
					//ActualsT17 = micrometer.ActualsT17,
					//ActualsT27 = micrometer.ActualsT27,
					//ActualsT37 = micrometer.ActualsT37,
					//Avg7 = micrometer.Avg7,
					//ActualsT18 = micrometer.ActualsT18,
					//ActualsT28 = micrometer.ActualsT28,
					//ActualsT38 = micrometer.ActualsT38,
					//Avg8 = micrometer.Avg8,
					//ActualsT19 = micrometer.ActualsT19,
					//ActualsT29 = micrometer.ActualsT29,
					//ActualsT39 = micrometer.ActualsT39,
					//Avg9 = micrometer.Avg9,
					//ActualsT110 = micrometer.ActualsT110,
					//ActualsT210 = micrometer.ActualsT210,
					//ActualsT310 = micrometer.ActualsT310,
					//Avg10 = micrometer.Avg10,
					//ActualsT111 = micrometer.ActualsT111,
					//ActualsT211 = micrometer.ActualsT211,
					//ActualsT311 = micrometer.ActualsT311,
					//Avg11 = micrometer.Avg11,
					//Measurement1 = micrometer.Measurement1,
					//Measurement2 = micrometer.Measurement2,
					//Measurement3 = micrometer.Measurement3,
					//Measurement4 = micrometer.Measurement4,
					//Measurement5 = micrometer.Measurement5,
					//Measurement6 = micrometer.Measurement6,
					//Measurement7 = micrometer.Measurement7,
					//Measurement8 = micrometer.Measurement8,
					//Measurement9 = micrometer.Measurement9,
					//Measurement10 = micrometer.Measurement10,
					//Measurement11 = micrometer.Measurement11,
					//MURemarks = micrometer.MURemarks,
					CreatedOn = DateTime.Now,
					CreatedBy = micrometer.CreatedBy,
					//CalibrationReviewedBy = labTechnicalManager.Id,
					CalibrationReviewedDate = DateTime.Now,
					FlatnessMeasure = micrometer.FlatnessMeasure,
					FlatnessInserr = micrometer.FlatnessInserr,
					FlatnessActual = micrometer.FlatnessActual
				};
				_unitOfWork.Repository<ObsTemplateMicrometer>().Insert(obsTemplateMicrometer);
				_unitOfWork.SaveChanges();
				if (observationById != null)
				{

					tempobsId = observationById.Id;
				}
				if (micrometer.MicrometerAddResultViewModelList != null)
				{
					micrometer.MicrometerAddResultViewModelList.ForEach(x => x.ParentId = tempobsId);
					var detailData = _mapper.Map<ObsMicrometerValues[]>(micrometer.MicrometerAddResultViewModelList
											.Where(x => x.Id > 0).ToList());
					if (detailData.Any())
					{
						foreach (var updateData in detailData)
						{
							_unitOfWork.Repository<ObsMicrometerValues>().Update(updateData);

							_unitOfWork.SaveChanges();
						}
					}

					detailData = _mapper.Map<ObsMicrometerValues[]>(micrometer.MicrometerAddResultViewModelList.Where(x => x.Id == null).ToList());

					if (detailData.Any())
					{
						_unitOfWork.Repository<ObsMicrometerValues>().InsertRange(detailData);
						_unitOfWork.SaveChanges();
					}
				}
			}
			else
			{
				ObsTemplateMicrometer micrometerById = _unitOfWork.Repository<ObsTemplateMicrometer>()
																.GetQueryAsNoTracking(Q => Q.Id == micrometer.Id)
																.SingleOrDefault();

				if (micrometer.ActualsT11 != null)
					micrometerById.ActualsT11 = micrometer.ActualsT11;

				if (micrometer.Avg1 != null)
					micrometerById.Avg1 = micrometer.Avg1;

				if (micrometer.MuInterval1 != null)
					micrometerById.MuInterval1 = micrometer.MuInterval1;


				//if (micrometer.ActualsT21 != null)
				//	micrometerById.ActualsT21 = micrometer.ActualsT21;

				//if (micrometer.ActualsT31 != null)
				//	micrometerById.ActualsT31 = micrometer.ActualsT31;



				//if (micrometer.ActualsT22 != null)
				//	micrometerById.ActualsT22 = micrometer.ActualsT22;

				//if (micrometer.ActualsT32 != null)
				//	micrometerById.ActualsT32 = micrometer.ActualsT32;

				if (micrometer.Avg2 != null)
					micrometerById.Avg2 = micrometer.Avg2;

				if (micrometer.MuInterval2 != null)
					micrometerById.MuInterval2 = micrometer.MuInterval2;

				if (micrometer.ActualsT12 != null)
					micrometerById.ActualsT12 = micrometer.ActualsT12;



				//if (micrometer.ActualsT23 != null)
				//	micrometerById.ActualsT23 = micrometer.ActualsT23;

				//if (micrometer.ActualsT33 != null)
				//	micrometerById.ActualsT33 = micrometer.ActualsT33;

				if (micrometer.Avg3 != null)
					micrometerById.Avg3 = micrometer.Avg3;

				if (micrometer.MuInterval3 != null)
					micrometerById.MuInterval3 = micrometer.MuInterval3;

				if (micrometer.ActualsT13 != null)
					micrometerById.ActualsT13 = micrometer.ActualsT13;



				if (micrometer.Avg4 != null)
					micrometerById.Avg4 = micrometer.Avg4;

				if (micrometer.Measurement4 != null)
					micrometerById.Measurement4 = micrometer.Measurement4;

				if (micrometer.ActualsT14 != null)
					micrometerById.ActualsT14 = micrometer.ActualsT14;



				if (micrometer.Avg5 != null)
					micrometerById.Avg5 = micrometer.Avg5;

				if (micrometer.Measurement5 != null)
					micrometerById.Measurement5 = micrometer.Measurement5;

				if (micrometer.ActualsT15 != null)
					micrometerById.ActualsT15 = micrometer.ActualsT15;


				if (micrometer.Avg6 != null)
					micrometerById.Avg6 = micrometer.Avg6;

				if (micrometer.Measurement6 != null)
					micrometerById.Measurement6 = micrometer.Measurement6;

				if (micrometer.ActualsT16 != null)
					micrometerById.ActualsT16 = micrometer.ActualsT16;


				if (micrometer.Avg7 != null)
					micrometerById.Avg7 = micrometer.Avg7;

				if (micrometer.Measurement7 != null)
					micrometerById.Measurement7 = micrometer.Measurement7;

				if (micrometer.ActualsT17 != null)
					micrometerById.ActualsT17 = micrometer.ActualsT17;

				//if (micrometer.ActualsT24 != null)
				//	micrometerById.ActualsT24 = micrometer.ActualsT24;

				//if (micrometer.ActualsT34 != null)
				//	micrometerById.ActualsT34 = micrometer.ActualsT34;

				//if (micrometer.Avg4 != null)
				//	micrometerById.Avg4 = micrometer.Avg4;

				//if (micrometer.MuInterval4 != null)
				//	micrometerById.MuInterval4 = micrometer.MuInterval4;

				//if (micrometer.ActualsT15 != null)
				//	micrometerById.ActualsT15 = micrometer.MuInterval4;

				//if (micrometer.ActualsT25 != null)
				//	micrometerById.ActualsT25 = micrometer.ActualsT25;

				//if (micrometer.ActualsT35 != null)
				//	micrometerById.ActualsT35 = micrometer.ActualsT35;

				//if (micrometer.Avg5 != null)
				//	micrometerById.Avg5 = micrometer.Avg5;

				//if (micrometer.MuInterval5 != null)
				//	micrometerById.MuInterval5 = micrometer.MuInterval5;

				//if (micrometer.ActualsT16 != null)
				//	micrometerById.ActualsT16 = micrometer.ActualsT16;

				//if (micrometer.ActualsT26 != null)
				//	micrometerById.ActualsT26 = micrometer.ActualsT26;

				//if (micrometer.ActualsT36 != null)
				//	micrometerById.ActualsT36 = micrometer.ActualsT36;

				//if (micrometer.Avg6 != null)
				//	micrometerById.Avg6 = micrometer.Avg6;

				//if (micrometer.ActualsT17 != null)
				//	micrometerById.ActualsT17 = micrometer.ActualsT17;

				//if (micrometer.ActualsT27 != null)
				//	micrometerById.ActualsT27 = micrometer.ActualsT27;

				//if (micrometer.ActualsT37 != null)
				//	micrometerById.ActualsT37 = micrometer.ActualsT37;

				//if (micrometer.Avg7 != null)
				//	micrometerById.Avg7 = micrometer.Avg7;

				//if (micrometer.ActualsT18 != null)
				//	micrometerById.ActualsT18 = micrometer.ActualsT18;

				//if (micrometer.ActualsT28 != null)
				//	micrometerById.ActualsT28 = micrometer.ActualsT28;

				//if (micrometer.ActualsT38 != null)
				//	micrometerById.ActualsT38 = micrometer.ActualsT38;

				//if (micrometer.Avg8 != null)
				//	micrometerById.Avg8 = micrometer.Avg8;

				//if (micrometer.ActualsT19 != null)
				//	micrometerById.ActualsT19 = micrometer.ActualsT19;

				//if (micrometer.ActualsT29 != null)
				//	micrometerById.ActualsT29 = micrometer.ActualsT29;

				//if (micrometer.ActualsT39 != null)
				//	micrometerById.ActualsT39 = micrometer.ActualsT39;

				//if (micrometer.Avg9 != null)
				//	micrometerById.Avg9 = micrometer.Avg9;

				//if (micrometer.ActualsT110 != null)
				//	micrometerById.ActualsT110 = micrometer.ActualsT110;


				//if (micrometer.ActualsT210 != null)
				//	micrometerById.ActualsT210 = micrometer.ActualsT210;

				//if (micrometer.ActualsT310 != null)
				//	micrometerById.ActualsT310 = micrometer.ActualsT310;

				//if (micrometer.Avg10 != null)
				//	micrometerById.Avg10 = micrometer.Avg10;

				//if (micrometer.ActualsT111 != null)
				//	micrometerById.ActualsT111 = micrometer.ActualsT111;

				//if (micrometer.ActualsT211 != null)
				//	micrometerById.ActualsT211 = micrometer.ActualsT211;

				//if (micrometer.ActualsT311 != null)
				//	micrometerById.ActualsT311 = micrometer.ActualsT311;

				//if (micrometer.Avg11 != null)
				//	micrometerById.Avg11 = micrometer.Avg11;

				//if (micrometer.Measurement1 != null)
				//	micrometerById.Measurement1 = micrometer.Measurement1;

				//if (micrometer.Measurement2 != null)
				//	micrometerById.Measurement2 = micrometer.Measurement2;

				//if (micrometer.Measurement3 != null)
				//	micrometerById.Measurement3 = micrometer.Measurement3;

				//if (micrometer.Measurement4 != null)
				//	micrometerById.Measurement4 = micrometer.Measurement4;

				//if (micrometer.Measurement5 != null)
				//	micrometerById.Measurement5 = micrometer.Measurement5;

				//if (micrometer.Measurement6 != null)
				//	micrometerById.Measurement6 = micrometer.Measurement6;

				//if (micrometer.Measurement7 != null)
				//	micrometerById.Measurement7 = micrometer.Measurement7;

				//if (micrometer.Measurement8 != null)
				//	micrometerById.Measurement8 = micrometer.Measurement8;

				//if (micrometer.Measurement9 != null)
				//	micrometerById.Measurement9 = micrometer.Measurement9;

				//if (micrometer.Measurement10 != null)
				//	micrometerById.Measurement10 = micrometer.Measurement10;

				//if (micrometer.Measurement11 != null)
				//	micrometerById.Measurement11 = micrometer.Measurement11;

				if (micrometer.Flatness1 != null)
					//micrometerById.Flatness1 = "1";//micrometer.Flatness1; have to change the value according to the micro type

					//if (micrometer.Flatness2 != null)
					//	micrometerById.Flatness2 = micrometer.Flatness2;

					if (micrometer.ParallelismSpec != null)
						micrometerById.ParallelismSpec = "0"; //micrometer.ParallelismSpec; have to change the value according to the micro type

				//if (micrometer.Actuals != null)
				//	micrometerById.Actuals = micrometer.Actuals;

				if (micrometer.MURemarks != null)
					micrometerById.MURemarks = micrometer.MURemarks;

				//FlatnessMeasure,FlatnessInserr,FlatnessActual,InstrumentErrValue
				if (micrometer.FlatnessMeasure != null)
					micrometerById.FlatnessMeasure = micrometer.FlatnessMeasure;

				if (micrometer.FlatnessInserr != null)
					micrometerById.FlatnessInserr = micrometer.FlatnessInserr;

				if (micrometer.FlatnessActual != null)
					micrometerById.FlatnessActual = micrometer.FlatnessActual;

				if (micrometer.InstrumentErrValue != null)
					micrometerById.InstrumentErrValue = "1";//micrometer.InstrumentErrValue;have to change the value according to the micro type

				_unitOfWork.Repository<ObsTemplateMicrometer>().Update(micrometerById);
				_unitOfWork.SaveChanges();

				if (observationById != null)
				{

					tempobsId = observationById.Id;
				}
				if (micrometer.MicrometerAddResultViewModelList != null)
				{
					micrometer.MicrometerAddResultViewModelList.ForEach(x => x.ParentId = tempobsId);
					var detailData = _mapper.Map<ObsMicrometerValues[]>(micrometer.MicrometerAddResultViewModelList
											.Where(x => x.Id > 0).ToList());//x.Id > 0 && 
					if (detailData.Any())
					{
						foreach (var updateData in detailData)
						{
							_unitOfWork.Repository<ObsMicrometerValues>().Update(updateData);
							_unitOfWork.SaveChanges();
						}
					}
					//micrometer.MicrometerAddResultViewModelList.ForEach()
					detailData = _mapper.Map<ObsMicrometerValues[]>(micrometer.MicrometerAddResultViewModelList.Where(x => x.Id == null).ToList());

					if (detailData.Any())
					{
						_unitOfWork.Repository<ObsMicrometerValues>().InsertRange(detailData);
						_unitOfWork.SaveChanges();
					}
				}
			}
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			ErrorViewModelTest.Log("ObservationTemplateService - InsertMicrometer Method One");
			return new ResponseViewModel<MicrometerViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - InsertMicrometer Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			//ErrorViewModelTest.Log("FlatnessActual - " + micrometer.FlatnessActual);
			//ErrorViewModelTest.Log("FlatnessMeasure - " + micrometer.FlatnessMeasure);
			//ErrorViewModelTest.Log("FlatnessInserr - " + micrometer.FlatnessInserr);
			return new ResponseViewModel<MicrometerViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = micrometer,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "InsertMicrometer"


			};
		}
	}
	public ResponseViewModel<MasterViewModel> GetEquipmentListByInstrumentId(int MasterInstrument1, int MasterInstrument2, int MasterInstrument3, int MasterInstrument4)
	{
		try
		{
			List<MasterViewModel> masterViewModelList = _mapper.Map<List<MasterViewModel>>(_unitOfWork.Repository<Master>()
				.GetQueryAsNoTracking(Q => Q.Id == MasterInstrument1 || Q.Id == MasterInstrument2 || Q.Id == MasterInstrument3 || Q.Id == MasterInstrument4).ToList());


			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = masterViewModelList
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("MasterService - GetEquipmentListByLabId Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "Master",
				ResponseServiceMethod = "GetEquipmentListByLabId"
			};
		}

	}
	public ResponseViewModel<MicrometerViewModel> GetMicrometerById(int requestId, int instrumentId)
	{
		try
		{
			MicrometerViewModel micrometer = _unitOfWork.Repository<TemplateObservation>()
			.GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
			.Include(I => I.MicromerterModel)

			.Select(s => new MicrometerViewModel()
			{
				InstrumentId = s.InstrumentId,
				TemplateObservationId = s.Id,
				TempStart = s.TempStart,
				TempEnd = s.TempEnd,
				Humidity = s.Humidity,
				RefWi = s.RefWi,
				Allvalues = s.Allvalues,
				ReviewStatus = s.ReviewStatus,
				CalibrationPerformedDate = s.CreatedOn,
				CalibrationReviewedDate = s.CalibrationReviewedDate,
				CreatedBy = s.CreatedBy,
				CalibrationReviewedBy = s.CalibrationReviewedBy,
				MicrometerCondition = s.InstrumentCondition,
				ULRNumber = s.ULRNumber,
				CertificateNumber = s.CertificateNumber,
				Id = s.MicromerterModel.Select(s => s.Id).FirstOrDefault(),
				Flatness1 = s.MicromerterModel.Select(s => s.Flatness1).FirstOrDefault(),
				FlatnessMeasure = s.MicromerterModel.Select(s => s.FlatnessMeasure).FirstOrDefault(),
				FlatnessInserr = s.MicromerterModel.Select(s => s.FlatnessInserr).FirstOrDefault(),
				FlatnessActual = s.MicromerterModel.Select(s => s.FlatnessActual).FirstOrDefault(),

				//Flatness2 = s.MicromerterModel.Select(s => s.Flatness2).FirstOrDefault(),
				//ParallelismSpec = s.MicromerterModel.Select(s => s.ParallelismSpec).FirstOrDefault(),
				//Actuals = s.MicromerterModel.Select(s => s.Actuals).FirstOrDefault(),
				ActualsT11 = s.MicromerterModel.Select(s => s.ActualsT11).SingleOrDefault(),
				Avg1 = s.MicromerterModel.Select(s => s.Avg1).FirstOrDefault(),
				MuInterval1 = s.MicromerterModel.Select(s => s.MuInterval1).FirstOrDefault(),

				//ActualsT21 = s.MicromerterModel.Select(s => s.ActualsT21).FirstOrDefault(),
				//ActualsT31 = s.MicromerterModel.Select(s => s.ActualsT31).FirstOrDefault(),

				ActualsT12 = s.MicromerterModel.Select(s => s.ActualsT12).FirstOrDefault(),
				Avg2 = s.MicromerterModel.Select(s => s.Avg2).FirstOrDefault(),
				MuInterval2 = s.MicromerterModel.Select(s => s.MuInterval2).FirstOrDefault(),
				//ActualsT22 = s.MicromerterModel.Select(s => s.ActualsT22).FirstOrDefault(),
				//ActualsT32 = s.MicromerterModel.Select(s => s.ActualsT32).FirstOrDefault(),


				ActualsT13 = s.MicromerterModel.Select(s => s.ActualsT13).FirstOrDefault(),
				Avg3 = s.MicromerterModel.Select(s => s.Avg3).FirstOrDefault(),
				MuInterval3 = s.MicromerterModel.Select(s => s.MuInterval3).FirstOrDefault(),

				//ActualsT23 = s.MicromerterModel.Select(s => s.ActualsT23).FirstOrDefault(),
				//ActualsT33 = s.MicromerterModel.Select(s => s.ActualsT33).FirstOrDefault(),

				ActualsT14 = s.MicromerterModel.Select(s => s.ActualsT14).FirstOrDefault(),
				Avg4 = s.MicromerterModel.Select(s => s.Avg4).FirstOrDefault(),
				Measurement4 = s.MicromerterModel.Select(s => s.Measurement4).FirstOrDefault(),

				//ActualsT24 = s.MicromerterModel.Select(s => s.ActualsT24).FirstOrDefault(),
				//ActualsT34 = s.MicromerterModel.Select(s => s.ActualsT34).FirstOrDefault(),

				ActualsT15 = s.MicromerterModel.Select(s => s.ActualsT15).FirstOrDefault(),
				Avg5 = s.MicromerterModel.Select(s => s.Avg5).FirstOrDefault(),
				Measurement5 = s.MicromerterModel.Select(s => s.Measurement5).FirstOrDefault(),

				//ActualsT25 = s.MicromerterModel.Select(s => s.ActualsT25).FirstOrDefault(),
				//ActualsT35 = s.MicromerterModel.Select(s => s.ActualsT35).FirstOrDefault(),
				Avg6 = s.MicromerterModel.Select(s => s.Avg6).FirstOrDefault(),
				ActualsT16 = s.MicromerterModel.Select(s => s.ActualsT16).FirstOrDefault(),
				Measurement6 = s.MicromerterModel.Select(s => s.Measurement6).FirstOrDefault(),

				Avg7 = s.MicromerterModel.Select(s => s.Avg7).FirstOrDefault(),
				ActualsT17 = s.MicromerterModel.Select(s => s.ActualsT17).FirstOrDefault(),
				Measurement7 = s.MicromerterModel.Select(s => s.Measurement7).FirstOrDefault(),

				//ActualsT26 = s.MicromerterModel.Select(s => s.ActualsT26).FirstOrDefault(),
				//ActualsT36 = s.MicromerterModel.Select(s => s.ActualsT36).FirstOrDefault(),
				//Avg6 = s.MicromerterModel.Select(s => s.Avg6).FirstOrDefault(),
				//ActualsT17 = s.MicromerterModel.Select(s => s.ActualsT17).FirstOrDefault(),
				//ActualsT27 = s.MicromerterModel.Select(s => s.ActualsT27).FirstOrDefault(),
				//ActualsT37 = s.MicromerterModel.Select(s => s.ActualsT37).FirstOrDefault(),
				//Avg7 = s.MicromerterModel.Select(s => s.Avg7).FirstOrDefault(),
				//ActualsT18 = s.MicromerterModel.Select(s => s.ActualsT18).FirstOrDefault(),
				//ActualsT28 = s.MicromerterModel.Select(s => s.ActualsT28).FirstOrDefault(),
				//ActualsT38 = s.MicromerterModel.Select(s => s.ActualsT38).FirstOrDefault(),
				//Avg8 = s.MicromerterModel.Select(s => s.Avg8).FirstOrDefault(),
				//ActualsT19 = s.MicromerterModel.Select(s => s.ActualsT19).FirstOrDefault(),
				//ActualsT29 = s.MicromerterModel.Select(s => s.ActualsT29).FirstOrDefault(),
				//ActualsT39 = s.MicromerterModel.Select(s => s.ActualsT39).FirstOrDefault(),
				//Avg9 = s.MicromerterModel.Select(s => s.Avg9).FirstOrDefault(),
				//ActualsT110 = s.MicromerterModel.Select(s => s.ActualsT110).FirstOrDefault(),
				//ActualsT210 = s.MicromerterModel.Select(s => s.ActualsT210).FirstOrDefault(),
				//ActualsT310 = s.MicromerterModel.Select(s => s.ActualsT310).FirstOrDefault(),
				//Avg10 = s.MicromerterModel.Select(s => s.Avg10).FirstOrDefault(),
				//ActualsT111 = s.MicromerterModel.Select(s => s.ActualsT111).FirstOrDefault(),
				//ActualsT211 = s.MicromerterModel.Select(s => s.ActualsT211).FirstOrDefault(),
				//ActualsT311 = s.MicromerterModel.Select(s => s.ActualsT311).FirstOrDefault(),
				//Avg11 = s.MicromerterModel.Select(s => s.Avg11).FirstOrDefault(),
				//Measurement1 = s.MicromerterModel.Select(s => s.Measurement1).FirstOrDefault(),
				//Measurement2 = s.MicromerterModel.Select(s => s.Measurement2).FirstOrDefault(),
				//Measurement3 = s.MicromerterModel.Select(s => s.Measurement3).FirstOrDefault(),
				//Measurement4 = s.MicromerterModel.Select(s => s.Measurement4).FirstOrDefault(),
				//Measurement5 = s.MicromerterModel.Select(s => s.Measurement5).FirstOrDefault(),

				//Measurement7 = s.MicromerterModel.Select(s => s.Measurement7).FirstOrDefault(),
				//Measurement8 = s.MicromerterModel.Select(s => s.Measurement8).FirstOrDefault(),
				//Measurement9 = s.MicromerterModel.Select(s => s.Measurement9).FirstOrDefault(),
				//Measurement10 = s.MicromerterModel.Select(s => s.Measurement10).FirstOrDefault(),
				//Measurement11 = s.MicromerterModel.Select(s => s.Measurement11).FirstOrDefault(),
				//MURemarks = s.MicromerterModel.Select(s => s.MURemarks).FirstOrDefault(),
				EnvironmentCondition = s.MicromerterModel.Select(S => S.EnvironmentCondition).SingleOrDefault(),
				//Uncertainity = s.MicromerterModel.Select(S => S.Uncertainity).SingleOrDefault(),
				//CalibrationResult = s.MicromerterModel.Select(S => S.CalibrationResult).SingleOrDefault(),
				//Remarks = s.MicromerterModel.Select(S => S.Remarks).SingleOrDefault(),


			}).SingleOrDefault();
			if (micrometer == null)
			{
				return new ResponseViewModel<MicrometerViewModel>
				{
					ResponseCode = 200,
					ResponseMessage = "No records found",
					ResponseData = null,
					ResponseDataList = null
				};
			}
			else if (micrometer != null)
			{

				List<string> performedUserData = GetUserName(micrometer.CreatedBy);

				if (performedUserData.Count >= 3)
				{
					micrometer.CalibrationPerformedBy = performedUserData[0];
					micrometer.PerformedBySign = performedUserData[1];
					micrometer.PerformedByDesignation = performedUserData[2];
				}

				List<string> reviewedUserData = GetUserName(micrometer.CalibrationReviewedBy);

				//List<string> MasterEqiupmentList = _mapper.Map<List<MasterViewModel>>(_unitOfWork.Repository<Master>().GetQueryAsNoTracking(Q => Q.Id == micrometer.MasterInstrument1 || Q.Id == instrumentById.MasterInstrument2 || Q.Id == instrumentById.MasterInstrument3 || Q.Id == instrumentById.MasterInstrument4).ToList());

				if (reviewedUserData.Count >= 3)
				{
					micrometer.ReviewedBy = reviewedUserData[0];
					micrometer.ReviewedBySign = reviewedUserData[1];
					micrometer.ReviewedByDesignation = reviewedUserData[2];
				}

				int? ulrNumber = micrometer.ULRNumber == null ? 0 : micrometer.ULRNumber;
				int? certificateNumber = micrometer.CertificateNumber == null ? 0 : micrometer.CertificateNumber;
				List<string> formatList = GetULRAndCertificateNumber(ulrNumber, certificateNumber);

				if (formatList.Count >= 2)
				{
					micrometer.ULRFormat = formatList[0];
					micrometer.CertificateFormat = formatList[1];
				}


			}
			var parentVM = _mapper.Map<MicrometerViewModel>(micrometer);
			var childData1 = _unitOfWork.Repository<ObsMicrometerValues>()
									.GetQueryAsNoTracking(x => x.ParentId == parentVM.TemplateObservationId && x.InstrumentError == 1);
			var childListVM1 = _mapper.Map<List<MicrometerResultViewModel>>(childData1);
			micrometer.MicrometerAddResultViewModelList = childListVM1;


			//var parentVM2 = _mapper.Map<MicrometerViewModel>(micrometer);
			//var childData2 = _unitOfWork.Repository<ObsMicrometerValues>()
			//						.GetQueryAsNoTracking(x => x.ParentId == parentVM.TemplateObservationId && x.Parallelism == 1);
			//var childListVM2 = _mapper.Map<List<MicrometerResultViewModel>>(childData1);

			//micrometer.MicrometerAddResultViewModelTwoList = childListVM2;

			return new ResponseViewModel<MicrometerViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = micrometer,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("ObservationTemplateService - micrometerId Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MicrometerViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "micrometerId"
			};
		}
	}

	#endregion

	#region "Plunger Dial"
	public ResponseViewModel<PlungerDialViewModel> InsertPlungerDial(PlungerDialViewModel plungerDial)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			int templateObservationId = 0;
			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
												 .GetQueryAsNoTracking(Q => Q.RequestId == plungerDial.RequestId
												 && Q.InstrumentId == plungerDial.InstrumentId).SingleOrDefault();

			//  User labTechnicalManager = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4).SingleOrDefault();
			if ((plungerDial.TemplateObservationId == 0) && (observationById == null))

			{
				//if (observationById == null)
				//{
				TemplateObservation templateObservation = new TemplateObservation()
				{
					InstrumentId = plungerDial.InstrumentId,
					RequestId = plungerDial.RequestId,
					TempStart = plungerDial.TempStart,
					TempEnd = plungerDial.TempEnd,
					Humidity = plungerDial.Humidity,
					InstrumentCondition = plungerDial.ConditionAndObservation,
					RefWi = plungerDial.RefWi,
					Allvalues = plungerDial.Allvalues,
					CreatedOn = DateTime.Now,
					CreatedBy = plungerDial.CreatedBy,
					// CalibrationReviewedBy = labTechnicalManager.Id,
					CalibrationReviewedDate = DateTime.Now
				};
				_unitOfWork.Repository<TemplateObservation>().Insert(templateObservation);
				_unitOfWork.SaveChanges();
				templateObservationId = templateObservation.Id;
				//}
			}
			else
			{
				//TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
				//												 .GetQueryAsNoTracking(Q => Q.RequestId == plungerDial.RequestId
				//												 && Q.InstrumentId == plungerDial.InstrumentId).SingleOrDefault();
				if (observationById != null)
				{
					if (plungerDial.TempStart != null)
					{
						observationById.TempStart = plungerDial.TempStart;
					}

					if (plungerDial.TempEnd != null)
					{
						observationById.TempEnd = plungerDial.TempEnd;
					}

					if (plungerDial.Humidity != null)
					{
						observationById.Humidity = plungerDial.Humidity;
					}

					if (plungerDial.ConditionAndObservation != null)
					{
						observationById.InstrumentCondition = plungerDial.ConditionAndObservation;
					}
					if (plungerDial.RefWi != null)
					{

						observationById.RefWi = plungerDial.RefWi;
					}
					if (plungerDial.Allvalues != null)
					{
						observationById.Allvalues = plungerDial.Allvalues;
					}

					_unitOfWork.Repository<TemplateObservation>().Update(observationById);
					_unitOfWork.SaveChanges();
				}
			}

			if (plungerDial.Id == 0)//((plungerDial.Id == 0) && (plungerDial == null))
			{
				if (observationById != null)
				{
					templateObservationId = observationById.Id;
				}
				ObsTemplatePlungerDial obsTemplatePlungerDial = new ObsTemplatePlungerDial()
				{
					ObservationId = templateObservationId,
					Spec1 = plungerDial.Spec1,
					Spec2 = plungerDial.Spec2,
					Spec3 = plungerDial.Spec3,
					Spec4 = plungerDial.Spec4,
					Spec5 = plungerDial.Spec5,
					Spec6 = plungerDial.Spec6,
					Actual1 = plungerDial.Actual1,
					Actual2 = plungerDial.Actual2,
					Actual3 = plungerDial.Actual3,
					Actual4 = plungerDial.Actual4,
					Actual5 = plungerDial.Actual5,
					Actual6 = plungerDial.Actual6,
					Interval1 = plungerDial.Interval1,
					Interval2 = plungerDial.Interval2,
					Interval3 = plungerDial.Interval3,
					Interval4 = plungerDial.Interval4,
					Interval5 = plungerDial.Interval5,
					Remarks = plungerDial.Remarks,
					CreatedOn = DateTime.Now,
					CreatedBy = plungerDial.CreatedBy,
					//CalibrationReviewedBy = labTechnicalManager.Id,
					CalibrationReviewedDate = DateTime.Now

				};
				_unitOfWork.Repository<ObsTemplatePlungerDial>().Insert(obsTemplatePlungerDial);
			}
			else
			{
				ObsTemplatePlungerDial plungerdialById = _unitOfWork.Repository<ObsTemplatePlungerDial>()
																	.GetQueryAsNoTracking(Q => Q.Id == plungerDial.Id)
																	.SingleOrDefault();
				if (plungerdialById != null)
				{
					if (plungerDial.Spec1 != null)
						plungerdialById.Spec1 = plungerDial.Spec1;

					if (plungerDial.Spec2 != null)
						plungerdialById.Spec2 = plungerDial.Spec2;

					if (plungerDial.Spec3 != null)
						plungerdialById.Spec3 = plungerDial.Spec3;

					if (plungerDial.Spec4 != null)
						plungerdialById.Spec4 = plungerDial.Spec4;

					if (plungerDial.Spec5 != null)
						plungerdialById.Spec5 = plungerDial.Spec5;

					if (plungerDial.Spec6 != null)
						plungerdialById.Spec6 = plungerDial.Spec6;

					if (plungerDial.Actual1 != null)
						plungerdialById.Actual1 = plungerDial.Actual1;

					if (plungerDial.Actual2 != null)
						plungerdialById.Actual2 = plungerDial.Actual2;

					if (plungerDial.Actual3 != null)
						plungerdialById.Actual3 = plungerDial.Actual3;

					if (plungerDial.Actual4 != null)
						plungerdialById.Actual4 = plungerDial.Actual4;

					if (plungerDial.Actual5 != null)
						plungerdialById.Actual5 = plungerDial.Actual5;

					if (plungerDial.Actual6 != null)
						plungerdialById.Actual6 = plungerDial.Actual6;

					if (plungerDial.Interval1 != null)
						plungerdialById.Interval1 = plungerDial.Interval1;

					if (plungerDial.Interval2 != null)
						plungerdialById.Interval2 = plungerDial.Interval2;

					if (plungerDial.Interval3 != null)
						plungerdialById.Interval3 = plungerDial.Interval3;

					if (plungerDial.Interval4 != null)
						plungerdialById.Interval4 = plungerDial.Interval4;

					if (plungerDial.Interval5 != null)
						plungerdialById.Interval5 = plungerDial.Interval5;

					if (plungerDial.Remarks != null)
						plungerdialById.Remarks = plungerDial.Remarks;

					_unitOfWork.Repository<ObsTemplatePlungerDial>().Update(plungerdialById);
				}
			}
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			return new ResponseViewModel<PlungerDialViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - InsertPlungerDial Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<PlungerDialViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = plungerDial,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "InsertPlungerDial"
			};
		}
	}
	public ResponseViewModel<PlungerDialViewModel> GetPlungerDialById(int requestId, int instrumentId)
	{
		try
		{
			PlungerDialViewModel plungerDialViewModel = _unitOfWork.Repository<TemplateObservation>()
																	.GetQueryAsNoTracking(Q => Q.RequestId == requestId
																							&& Q.InstrumentId == instrumentId)
																	.Include(I => I.PlungerDialModel)
													.Select(s => new PlungerDialViewModel()
													{
														TemplateObservationId = s.Id,
														TempStart = s.TempStart,
														TempEnd = s.TempEnd,
														Humidity = s.Humidity,
														RefWi = s.RefWi,
														Allvalues = s.Allvalues,
														ReviewStatus = s.ReviewStatus,
														CreatedBy = s.CreatedBy,
														ULRNumber = s.ULRNumber,
														CertificateNumber = s.CertificateNumber,
														CalibrationReviewedBy = s.CalibrationReviewedBy,
														CalibrationPerformedDate = s.CreatedOn,
														CalibrationReviewedDate = s.CalibrationReviewedDate,
														ConditionAndObservation = s.InstrumentCondition,
														Id = s.PlungerDialModel.Select(s => s.Id).FirstOrDefault(),
														Spec1 = s.PlungerDialModel.Select(s => s.Spec1).FirstOrDefault(),
														Spec2 = s.PlungerDialModel.Select(s => s.Spec2).FirstOrDefault(),
														Spec3 = s.PlungerDialModel.Select(s => s.Spec3).FirstOrDefault(),
														Spec4 = s.PlungerDialModel.Select(s => s.Spec4).FirstOrDefault(),
														Spec5 = s.PlungerDialModel.Select(s => s.Spec5).FirstOrDefault(),
														Spec6 = s.PlungerDialModel.Select(s => s.Spec6).FirstOrDefault(),
														Actual1 = s.PlungerDialModel.Select(s => s.Actual1).FirstOrDefault(),
														Actual2 = s.PlungerDialModel.Select(s => s.Actual2).FirstOrDefault(),
														Actual3 = s.PlungerDialModel.Select(s => s.Actual3).FirstOrDefault(),
														Actual4 = s.PlungerDialModel.Select(s => s.Actual4).FirstOrDefault(),
														Actual5 = s.PlungerDialModel.Select(s => s.Actual5).FirstOrDefault(),
														Actual6 = s.PlungerDialModel.Select(s => s.Actual6).FirstOrDefault(),
														Interval1 = s.PlungerDialModel.Select(s => s.Interval1).FirstOrDefault(),
														Interval2 = s.PlungerDialModel.Select(s => s.Interval2).FirstOrDefault(),
														Interval3 = s.PlungerDialModel.Select(s => s.Interval3).FirstOrDefault(),
														Interval4 = s.PlungerDialModel.Select(s => s.Interval4).FirstOrDefault(),
														Interval5 = s.PlungerDialModel.Select(s => s.Interval5).FirstOrDefault(),
														EnvironmentCondition = s.PlungerDialModel.Select(S => S.EnvironmentCondition).SingleOrDefault(),
														Uncertainity = s.PlungerDialModel.Select(S => S.Uncertainity).SingleOrDefault(),
														CalibrationResult = s.PlungerDialModel.Select(S => S.CalibrationResult).SingleOrDefault(),
														Remarks = s.PlungerDialModel.Select(S => S.Remarks).SingleOrDefault(),
													}).SingleOrDefault();


			if (plungerDialViewModel != null)
			{
				List<string> performedUserData = GetUserName(plungerDialViewModel.CreatedBy);

				if (performedUserData.Count >= 3)
				{
					plungerDialViewModel.CalibrationPerformedBy = performedUserData[0];
					plungerDialViewModel.PerformedBySign = performedUserData[1];
					plungerDialViewModel.PerformedByDesignation = performedUserData[2];
				}

				List<string> reviewedUserData = GetUserName(plungerDialViewModel.CalibrationReviewedBy);

				if (reviewedUserData.Count >= 3)
				{
					plungerDialViewModel.ReviewedBy = reviewedUserData[0];
					plungerDialViewModel.ReviewedBySign = reviewedUserData[1];
					plungerDialViewModel.ReviewedByDesignation = reviewedUserData[2];
				}

				int? ulrNumber = plungerDialViewModel.ULRNumber == null ? 0 : plungerDialViewModel.ULRNumber;
				int? certificateNumber = plungerDialViewModel.CertificateNumber == null ? 0 : plungerDialViewModel.CertificateNumber;
				List<string> formatList = GetULRAndCertificateNumber(ulrNumber, certificateNumber);

				if (formatList.Count >= 2)
				{
					plungerDialViewModel.ULRFormat = formatList[0];
					plungerDialViewModel.CertificateFormat = formatList[1];
				}
			}


			return new ResponseViewModel<PlungerDialViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = plungerDialViewModel,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("ObservationTemplateService - plungerdialId Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<PlungerDialViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "plungerdialId"
			};
		}
	}

	#endregion
	#region "Thread Guages"
	public ResponseViewModel<ThreadGaugesViewModel> InsertThreadGuages(ThreadGaugesViewModel threadGauges)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			int templateObservationId = 0;
			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
																.GetQueryAsNoTracking(Q => Q.RequestId == threadGauges.RequestId
																					  && Q.InstrumentId == threadGauges.InstrumentId)
																 .SingleOrDefault();

			// User labTechnicalManager = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4).SingleOrDefault();
			if ((threadGauges.TemplateObservationId == 0) && (observationById == null))

			{
				//if(observationById == null)
				//{ 
				TemplateObservation templateObservation = new TemplateObservation()
				{
					InstrumentId = threadGauges.InstrumentId,
					RequestId = threadGauges.RequestId,
					TempStart = threadGauges.TempStart,
					TempEnd = threadGauges.TempEnd,
					Humidity = threadGauges.Humidity,
					RefWi = threadGauges.RefWi,
					InstrumentCondition = threadGauges.ThreadgaugeCondtion,
					Allvalues = threadGauges.Allvalues,
					CreatedBy = threadGauges.CreatedBy,
					CreatedOn = DateTime.Now,
					//CalibrationReviewedBy = labTechnicalManager.Id,
					CalibrationReviewedDate = DateTime.Now
				};
				_unitOfWork.Repository<TemplateObservation>().Insert(templateObservation);
				_unitOfWork.SaveChanges();
				templateObservationId = templateObservation.Id;
				//}
			}
			else
			{
				//TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
				//												.GetQueryAsNoTracking(Q => Q.RequestId == threadGauges.RequestId
				//																	  && Q.InstrumentId == threadGauges.InstrumentId)
				//												 .SingleOrDefault();
				if (observationById != null)
				{

					if (threadGauges.TempStart != null)
					{
						observationById.TempStart = threadGauges.TempStart;
					}

					if (threadGauges.TempEnd != null)
					{
						observationById.TempEnd = threadGauges.TempEnd;
					}

					if (threadGauges.Humidity != null)
					{
						observationById.Humidity = threadGauges.Humidity;
					}

					if (threadGauges.ThreadgaugeCondtion != null)
					{
						observationById.InstrumentCondition = threadGauges.ThreadgaugeCondtion;
					}
					if (threadGauges.RefWi != null)
					{
						observationById.RefWi = threadGauges.RefWi;
					}
					if (threadGauges.Allvalues != null)
					{
						observationById.Allvalues = threadGauges.Allvalues;
					}
					_unitOfWork.Repository<TemplateObservation>().Update(observationById);
				}
			}
			_unitOfWork.SaveChanges();
			if (threadGauges.Id == 0)
			{

				if (observationById != null)
				{
					templateObservationId = observationById.Id;
				}
				ObsTemplateThreadGauges ObsTemplateThreadGauges = new ObsTemplateThreadGauges()
				{
					ObservationId = templateObservationId,
					Max1 = threadGauges.Max1,
					Max2 = threadGauges.Max2,
					Min1 = threadGauges.Min1,
					Min2 = threadGauges.Min2,
					WearLimit1 = threadGauges.WearLimit1,
					WearLimit2 = threadGauges.WearLimit2,
					Plane1 = threadGauges.Plane1,
					Plane2 = threadGauges.Plane2,
					Plane3 = threadGauges.Plane3,
					Plane4 = threadGauges.Plane4,
					Plane5 = threadGauges.Plane5,
					Repeatability1 = threadGauges.Repeatability1,
					Repeatability2 = threadGauges.Repeatability2,
					Repeatability3 = threadGauges.Repeatability3,
					Repeatability4 = threadGauges.Repeatability4,
					Repeatability5 = threadGauges.Repeatability5,
					//CalibrationReviewedBy = threadGauges.CalibrationReviewedBy,
					ReviewDate = threadGauges.CalibrationReviewedDate,
					CreatedBy = threadGauges.CreatedBy,
					CreatedOn = DateTime.Now,
					//CalibrationReviewedBy = labTechnicalManager.Id,
					CalibrationReviewedDate = DateTime.Now
				};
				_unitOfWork.Repository<ObsTemplateThreadGauges>().Insert(ObsTemplateThreadGauges);
			}
			else
			{
				ObsTemplateThreadGauges threadGaugesById = _unitOfWork.Repository<ObsTemplateThreadGauges>()
																	  .GetQueryAsNoTracking(Q => Q.Id == threadGauges.Id)
																	  .SingleOrDefault();
				if (threadGauges.Max1 != null)
					threadGaugesById.Max1 = threadGauges.Max1;

				if (threadGauges.Max2 != null)
					threadGaugesById.Max2 = threadGauges.Max2;

				if (threadGauges.Min1 != null)
					threadGaugesById.Min1 = threadGauges.Min1;

				if (threadGauges.Min2 != null)
					threadGaugesById.Min2 = threadGauges.Min2;

				if (threadGauges.WearLimit1 != null)
					threadGaugesById.WearLimit1 = threadGauges.WearLimit1;

				if (threadGauges.WearLimit2 != null)
					threadGaugesById.WearLimit2 = threadGauges.WearLimit2;

				if (threadGauges.Plane1 != null)
					threadGaugesById.Plane1 = threadGauges.Plane1;

				if (threadGauges.Plane2 != null)
					threadGaugesById.Plane2 = threadGauges.Plane2;

				if (threadGauges.Plane3 != null)
					threadGaugesById.Plane3 = threadGauges.Plane3;

				if (threadGauges.Plane4 != null)
					threadGaugesById.Plane4 = threadGauges.Plane4;

				if (threadGauges.Plane5 != null)
					threadGaugesById.Plane5 = threadGauges.Plane5;

				if (threadGauges.Repeatability1 != null)
					threadGaugesById.Repeatability1 = threadGauges.Repeatability1;

				if (threadGauges.Repeatability2 != null)
					threadGaugesById.Repeatability2 = threadGauges.Repeatability2;

				if (threadGauges.Repeatability3 != null)
					threadGaugesById.Repeatability3 = threadGauges.Repeatability3;

				if (threadGauges.Repeatability4 != null)
					threadGaugesById.Repeatability4 = threadGauges.Repeatability4;

				if (threadGauges.Repeatability5 != null)
					threadGaugesById.Repeatability5 = threadGauges.Repeatability5;

				_unitOfWork.Repository<ObsTemplateThreadGauges>().Update(threadGaugesById);
			}
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			return new ResponseViewModel<ThreadGaugesViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - InsertThreadGuages Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ThreadGaugesViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = threadGauges,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "InsertThreadGuages"
			};
		}
	}
	public ResponseViewModel<ThreadGaugesViewModel> GetThreadGaugesById(int requestId, int instrumentId)
	{
		try
		{
			ThreadGaugesViewModel threadGauges = _unitOfWork.Repository<TemplateObservation>()
												.GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
												.Include(I => I.ThreadGaugesDialModel)
												.Select(s => new ThreadGaugesViewModel()
												{
													TemplateObservationId = s.Id,
													TempStart = s.TempStart,
													TempEnd = s.TempEnd,
													Humidity = s.Humidity,
													RefWi = s.RefWi,
													Allvalues = s.Allvalues,
													CreatedBy = s.CreatedBy,
													ULRNumber = s.ULRNumber,
													CertificateNumber = s.CertificateNumber,
													CalibrationReviewedBy = s.CalibrationReviewedBy,
													CalibrationPerformedDate = s.CreatedOn,
													CalibrationReviewedDate = s.CalibrationReviewedDate,
													ThreadgaugeCondtion = s.InstrumentCondition,
													ReviewStatus = s.ReviewStatus,
													Id = s.ThreadGaugesDialModel.Select(s => s.Id).FirstOrDefault(),
													Max1 = s.ThreadGaugesDialModel.Select(S => S.Max1).FirstOrDefault(),
													Max2 = s.ThreadGaugesDialModel.Select(S => S.Max2).FirstOrDefault(),
													Min1 = s.ThreadGaugesDialModel.Select(S => S.Min1).FirstOrDefault(),
													Min2 = s.ThreadGaugesDialModel.Select(S => S.Min2).FirstOrDefault(),
													WearLimit1 = s.ThreadGaugesDialModel.Select(S => S.WearLimit1).FirstOrDefault(),
													WearLimit2 = s.ThreadGaugesDialModel.Select(S => S.WearLimit2).FirstOrDefault(),
													Plane1 = s.ThreadGaugesDialModel.Select(S => S.Plane1).FirstOrDefault(),
													Plane2 = s.ThreadGaugesDialModel.Select(S => S.Plane2).FirstOrDefault(),
													Plane3 = s.ThreadGaugesDialModel.Select(S => S.Plane3).FirstOrDefault(),
													Plane4 = s.ThreadGaugesDialModel.Select(S => S.Plane4).FirstOrDefault(),
													Plane5 = s.ThreadGaugesDialModel.Select(S => S.Plane5).FirstOrDefault(),
													Repeatability1 = s.ThreadGaugesDialModel.Select(S => S.Repeatability1).FirstOrDefault(),
													Repeatability2 = s.ThreadGaugesDialModel.Select(S => S.Repeatability2).FirstOrDefault(),
													Repeatability3 = s.ThreadGaugesDialModel.Select(S => S.Repeatability3).FirstOrDefault(),
													Repeatability4 = s.ThreadGaugesDialModel.Select(S => S.Repeatability4).FirstOrDefault(),
													Repeatability5 = s.ThreadGaugesDialModel.Select(S => S.Repeatability5).FirstOrDefault(),
													EnvironmentCondition = s.ThreadGaugesDialModel.Select(S => S.EnvironmentCondition).SingleOrDefault(),
													Uncertainity = s.ThreadGaugesDialModel.Select(S => S.Uncertainity).SingleOrDefault(),
													CalibrationResult = s.ThreadGaugesDialModel.Select(S => S.CalibrationResult).SingleOrDefault(),
													Remarks = s.ThreadGaugesDialModel.Select(S => S.Remarks).SingleOrDefault(),
												}).SingleOrDefault();

			if (threadGauges != null)
			{
				List<string> performedUserData = GetUserName(threadGauges.CreatedBy);

				if (performedUserData.Count >= 3)
				{
					threadGauges.CalibrationPerformedBy = performedUserData[0];
					threadGauges.PerformedBySign = performedUserData[1];
					threadGauges.PerformedByDesignation = performedUserData[2];
				}

				List<string> reviewedUserData = GetUserName(threadGauges.CalibrationReviewedBy);

				if (reviewedUserData.Count >= 3)
				{
					threadGauges.ReviewedBy = reviewedUserData[0];
					threadGauges.ReviewedBySign = reviewedUserData[1];
					threadGauges.ReviewedByDesignation = reviewedUserData[2];
				}

				int? ulrNumber = threadGauges.ULRNumber == null ? 0 : threadGauges.ULRNumber;
				int? certificateNumber = threadGauges.CertificateNumber == null ? 0 : threadGauges.CertificateNumber;
				List<string> formatList = GetULRAndCertificateNumber(ulrNumber, certificateNumber);

				if (formatList.Count >= 2)
				{
					threadGauges.ULRFormat = formatList[0];
					threadGauges.CertificateFormat = formatList[1];
				}
			}

			return new ResponseViewModel<ThreadGaugesViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = threadGauges,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("ObservationTemplateService - threadgaugesId Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ThreadGaugesViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "threadgaugesId"
			};
		}
	}


	#endregion

	#region "TorqueWrenches"
	public ResponseViewModel<TorqueWrenchesViewModel> InsertTWobs(TorqueWrenchesViewModel torquewrenches)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			int templateObservationId = 0;
			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
																 .GetQueryAsNoTracking(Q => Q.RequestId == torquewrenches.RequestId
																 && Q.InstrumentId == torquewrenches.InstrumentId).SingleOrDefault();

			//templateObservationId = observationById.Id;
			if ((torquewrenches.TemplateObservationId == 0) && (observationById == null))

			{
				//if(observationById == null)
				//{ 
				TemplateObservation templateObservation = new TemplateObservation()
				{
					InstrumentId = torquewrenches.InstrumentId,
					RequestId = torquewrenches.RequestId,
					TempStart = torquewrenches.TempStart,
					TempEnd = torquewrenches.TempEnd,
					Humidity = torquewrenches.Humidity,
					RefWi = torquewrenches.RefWi,
					InstrumentCondition = torquewrenches.ConditionOfTW,
					Allvalues = torquewrenches.Allvalues,
					CreatedBy = torquewrenches.CreatedBy,
					CreatedOn = DateTime.Now,
					CalibrationReviewedDate = DateTime.Now
				};
				_unitOfWork.Repository<TemplateObservation>().Insert(templateObservation);
				_unitOfWork.SaveChanges();
				templateObservationId = templateObservation.Id;
				//}
			}
			else
			{
				//TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
				//												 .GetQueryAsNoTracking(Q => Q.RequestId == torquewrenches.RequestId
				//												 && Q.InstrumentId == torquewrenches.InstrumentId).SingleOrDefault();
				if (observationById != null)
				{

					if (torquewrenches.TempStart != null)
					{
						observationById.TempStart = torquewrenches.TempStart;
					}

					if (torquewrenches.TempEnd != null)
					{
						observationById.TempEnd = torquewrenches.TempEnd;
					}

					if (torquewrenches.Humidity != null)
					{
						observationById.Humidity = torquewrenches.Humidity;
					}

					if (torquewrenches.ConditionOfTW != null)
					{
						observationById.InstrumentCondition = torquewrenches.ConditionOfTW;
					}
					if (torquewrenches.RefWi != null)
					{
						observationById.RefWi = torquewrenches.RefWi;
					}
					if (torquewrenches.Allvalues != null)
					{
						observationById.Allvalues = torquewrenches.Allvalues;
					}

					_unitOfWork.Repository<TemplateObservation>().Update(observationById);
				}
				_unitOfWork.SaveChanges();

			}
			if (torquewrenches.Id == 0)
			{
				if (observationById != null)
				{
					templateObservationId = observationById.Id;

				}
				ObsTemplateTWobs obsTemplateTWobs = new ObsTemplateTWobs()
				{
					ObservationId = templateObservationId,
					SpecMax = torquewrenches.SpecMax,
					SpecMin = torquewrenches.SpecMin,
					ActualInOne = torquewrenches.ActualInOne,
					ActualInTwo = torquewrenches.ActualInTwo,
					ActualInThree = torquewrenches.ActualInThree,
					ActualInFour = torquewrenches.ActualInFour,
					ActualInFive = torquewrenches.ActualInFive,
					ActualInSix = torquewrenches.ActualInSix,
					ActualInSeven = torquewrenches.ActualInSeven,
					ActualInEight = torquewrenches.ActualInEight,
					ActualInNine = torquewrenches.ActualInNine,
					ActualInTen = torquewrenches.ActualInTen,
					Nominal20 = torquewrenches.Nominal20,
					Nominal60 = torquewrenches.Nominal60,
					Nominal100 = torquewrenches.Nominal100,
					Spec20 = torquewrenches.Spec20,
					Spec60 = torquewrenches.Spec60,
					Spec100 = torquewrenches.Spec100,
					Comments = torquewrenches.Comments,
					Value1 = torquewrenches.Value1,
					Value2 = torquewrenches.Value2,
					Value3 = torquewrenches.Value3,
					Value4 = torquewrenches.Value4,
					Value5 = torquewrenches.Value5,
					Value6 = torquewrenches.Value6,
					Value7 = torquewrenches.Value7,
					Value8 = torquewrenches.Value8,
					Value9 = torquewrenches.Value9,
					Value10 = torquewrenches.Value10,
					Value11 = torquewrenches.Value11,
					Value12 = torquewrenches.Value12,
					Value13 = torquewrenches.Value13,
					Value14 = torquewrenches.Value14,
					Value15 = torquewrenches.Value15,
					SetValue = torquewrenches.SetValue,
					CreatedBy = torquewrenches.CreatedBy,
					AWSTransducers = torquewrenches.AWSTransducers,
					CreatedOn = DateTime.Now,
					CalibrationReviewedDate = DateTime.Now
				};
				_unitOfWork.Repository<ObsTemplateTWobs>().Insert(obsTemplateTWobs);
				_unitOfWork.SaveChanges();
			}
			else
			{
				ObsTemplateTWobs torquewrenchesById = _unitOfWork.Repository<ObsTemplateTWobs>()
																 .GetQueryAsNoTracking(Q => Q.Id == torquewrenches.Id)
																 .SingleOrDefault();
				if (torquewrenches.SpecMax != null)
					torquewrenchesById.SpecMax = torquewrenches.SpecMax;

				if (torquewrenches.SpecMin != null)
					torquewrenchesById.SpecMin = torquewrenches.SpecMin;

				if (torquewrenches.ActualInOne != null)
					torquewrenchesById.ActualInOne = torquewrenches.ActualInOne;

				if (torquewrenches.ActualInTwo != null)
					torquewrenchesById.ActualInTwo = torquewrenches.ActualInTwo;

				if (torquewrenches.ActualInThree != null)
					torquewrenchesById.ActualInThree = torquewrenches.ActualInThree;

				if (torquewrenches.ActualInFour != null)
					torquewrenchesById.ActualInFour = torquewrenches.ActualInFour;

				if (torquewrenches.ActualInFive != null)
					torquewrenchesById.ActualInFive = torquewrenches.ActualInFive;

				if (torquewrenches.ActualInSix != null)
					torquewrenchesById.ActualInSix = torquewrenches.ActualInSix;

				if (torquewrenches.ActualInSeven != null)
					torquewrenchesById.ActualInSeven = torquewrenches.ActualInSeven;

				if (torquewrenches.ActualInEight != null)
					torquewrenchesById.ActualInEight = torquewrenches.ActualInEight;

				if (torquewrenches.ActualInNine != null)
					torquewrenchesById.ActualInNine = torquewrenches.ActualInNine;

				if (torquewrenches.ActualInTen != null)
					torquewrenchesById.ActualInTen = torquewrenches.ActualInTen;

				if (torquewrenches.Nominal20 != null)
					torquewrenchesById.Nominal20 = torquewrenches.Nominal20;

				if (torquewrenches.Nominal60 != null)
					torquewrenchesById.Nominal60 = torquewrenches.Nominal60;

				if (torquewrenches.Nominal100 != null)
					torquewrenchesById.Nominal100 = torquewrenches.Nominal100;

				if (torquewrenches.Spec20 != null)
					torquewrenchesById.Spec20 = torquewrenches.Spec20;

				if (torquewrenches.Spec60 != null)
					torquewrenchesById.Spec60 = torquewrenches.Spec60;

				if (torquewrenches.Spec100 != null)
					torquewrenchesById.Spec100 = torquewrenches.Spec100;

				if (torquewrenches.Comments != null)
					torquewrenchesById.Comments = torquewrenches.Comments;

				if (torquewrenches.Value1 != null)
					torquewrenchesById.Value1 = torquewrenches.Value1;

				if (torquewrenches.Value2 != null)
					torquewrenchesById.Value2 = torquewrenches.Value2;

				if (torquewrenches.Value3 != null)
					torquewrenchesById.Value3 = torquewrenches.Value3;

				if (torquewrenches.Value4 != null)
					torquewrenchesById.Value4 = torquewrenches.Value4;

				if (torquewrenches.Value5 != null)
					torquewrenchesById.Value5 = torquewrenches.Value5;

				if (torquewrenches.Value6 != null)
					torquewrenchesById.Value6 = torquewrenches.Value6;

				if (torquewrenches.Value7 != null)
					torquewrenchesById.Value7 = torquewrenches.Value7;

				if (torquewrenches.Value8 != null)
					torquewrenchesById.Value8 = torquewrenches.Value8;

				if (torquewrenches.Value9 != null)
					torquewrenchesById.Value9 = torquewrenches.Value9;

				if (torquewrenches.Value10 != null)
					torquewrenchesById.Value10 = torquewrenches.Value10;

				if (torquewrenches.Value11 != null)
					torquewrenchesById.Value11 = torquewrenches.Value11;

				if (torquewrenches.Value12 != null)
					torquewrenchesById.Value12 = torquewrenches.Value12;

				if (torquewrenches.Value13 != null)
					torquewrenchesById.Value13 = torquewrenches.Value13;

				if (torquewrenches.Value14 != null)
					torquewrenchesById.Value14 = torquewrenches.Value14;

				if (torquewrenches.Value15 != null)
					torquewrenchesById.Value15 = torquewrenches.Value15;

				if (torquewrenches.SetValue != null)
					torquewrenchesById.SetValue = torquewrenches.SetValue;

				if (torquewrenches.AWSTransducers != null)
					torquewrenchesById.AWSTransducers = torquewrenches.AWSTransducers;

				_unitOfWork.Repository<ObsTemplateTWobs>().Update(torquewrenchesById);

				_unitOfWork.SaveChanges();
			}
			_unitOfWork.Commit();
			return new ResponseViewModel<TorqueWrenchesViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - InsertTWobs Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<TorqueWrenchesViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = torquewrenches,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "InsertTWobs"
			};
		}
	}

	public ResponseViewModel<TorqueWrenchesViewModel> GetTorqueWrenchesById(int requestId, int instrumentId)
	{
		try
		{
			TorqueWrenchesViewModel torqueWrenches = _unitOfWork.Repository<TemplateObservation>()
													.GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
													.Include(I => I.TwosModel)
													.Select(s => new TorqueWrenchesViewModel()
													{
														TemplateObservationId = s.Id,
														TempStart = s.TempStart,
														TempEnd = s.TempEnd,
														Humidity = s.Humidity,
														RefWi = s.RefWi,
														Allvalues = s.Allvalues,
														ULRNumber = s.ULRNumber,
														CertificateNumber = s.CertificateNumber,
														ReviewStatus = s.ReviewStatus,
														CalibrationReviewedBy = s.CalibrationReviewedBy,
														CalibrationReviewedDate = s.CalibrationReviewedDate,
														CreatedBy = s.CreatedBy,
														CreatedOn = s.CreatedOn,
														CalibrationPerformedDate = s.CreatedOn,
														Id = s.TwosModel.Select(S => S.Id).FirstOrDefault(),
														ConditionOfTW = s.InstrumentCondition,
														SpecMax = s.TwosModel.Select(S => S.SpecMax).FirstOrDefault(),
														SpecMin = s.TwosModel.Select(S => S.SpecMin).FirstOrDefault(),
														ActualInOne = s.TwosModel.Select(S => S.ActualInOne).FirstOrDefault(),
														ActualInTwo = s.TwosModel.Select(S => S.ActualInTwo).FirstOrDefault(),
														ActualInThree = s.TwosModel.Select(S => S.ActualInThree).FirstOrDefault(),
														ActualInFour = s.TwosModel.Select(S => S.ActualInFour).FirstOrDefault(),
														ActualInFive = s.TwosModel.Select(S => S.ActualInFive).FirstOrDefault(),
														ActualInSix = s.TwosModel.Select(S => S.ActualInSix).FirstOrDefault(),
														ActualInSeven = s.TwosModel.Select(S => S.ActualInSeven).FirstOrDefault(),
														ActualInEight = s.TwosModel.Select(S => S.ActualInEight).FirstOrDefault(),
														ActualInNine = s.TwosModel.Select(S => S.ActualInNine).FirstOrDefault(),
														ActualInTen = s.TwosModel.Select(S => S.ActualInTen).FirstOrDefault(),
														Nominal20 = s.TwosModel.Select(S => S.Nominal20).FirstOrDefault(),
														Nominal60 = s.TwosModel.Select(S => S.Nominal60).FirstOrDefault(),
														Nominal100 = s.TwosModel.Select(S => S.Nominal100).FirstOrDefault(),
														Spec20 = s.TwosModel.Select(S => S.Spec20).FirstOrDefault(),
														Spec60 = s.TwosModel.Select(S => S.Spec60).FirstOrDefault(),
														Spec100 = s.TwosModel.Select(S => S.Spec100).FirstOrDefault(),
														Comments = s.TwosModel.Select(S => S.Comments).FirstOrDefault(),
														CalibBy = s.TwosModel.Select(S => S.CalibBy).FirstOrDefault(),
														Calib_Date = s.TwosModel.Select(S => S.Calib_Date).FirstOrDefault(),
														Reviewed_By = s.TwosModel.Select(S => S.Reviewed_By).FirstOrDefault(),
														Review_Date = s.TwosModel.Select(S => S.Review_Date).FirstOrDefault(),
														Value1 = s.TwosModel.Select(S => S.Value1).FirstOrDefault(),
														Value2 = s.TwosModel.Select(S => S.Value2).FirstOrDefault(),
														Value3 = s.TwosModel.Select(S => S.Value3).FirstOrDefault(),
														Value4 = s.TwosModel.Select(S => S.Value4).FirstOrDefault(),
														Value5 = s.TwosModel.Select(S => S.Value5).FirstOrDefault(),
														Value6 = s.TwosModel.Select(S => S.Value6).FirstOrDefault(),
														Value7 = s.TwosModel.Select(S => S.Value7).FirstOrDefault(),
														Value8 = s.TwosModel.Select(S => S.Value8).FirstOrDefault(),
														Value9 = s.TwosModel.Select(S => S.Value9).FirstOrDefault(),
														Value10 = s.TwosModel.Select(S => S.Value10).FirstOrDefault(),
														Value11 = s.TwosModel.Select(S => S.Value11).FirstOrDefault(),
														Value12 = s.TwosModel.Select(S => S.Value12).FirstOrDefault(),
														Value13 = s.TwosModel.Select(S => S.Value13).FirstOrDefault(),
														Value14 = s.TwosModel.Select(S => S.Value14).FirstOrDefault(),
														Value15 = s.TwosModel.Select(S => S.Value15).FirstOrDefault(),
														SetValue = s.TwosModel.Select(S => S.SetValue).FirstOrDefault(),
														AWSTransducers = s.TwosModel.Select(S => S.AWSTransducers).FirstOrDefault(),
														NorbarTransducers = s.TwosModel.Select(S => S.NorbarTransducers).FirstOrDefault(),
														EnvironmentCondition = s.TwosModel.Select(S => S.EnvironmentCondition).SingleOrDefault(),
														Uncertainity = s.TwosModel.Select(S => S.Uncertainity).SingleOrDefault(),
														CalibrationResult = s.TwosModel.Select(S => S.CalibrationResult).SingleOrDefault(),
														Remarks = s.TwosModel.Select(S => S.Remarks).SingleOrDefault(),
													}).SingleOrDefault();

			if (torqueWrenches != null)
			{

				List<string> performedUserData = GetUserName(torqueWrenches.CreatedBy);

				if (performedUserData.Count >= 3)
				{
					torqueWrenches.CalibrationPerformedBy = performedUserData[0];
					torqueWrenches.PerformedBySign = performedUserData[1];
					torqueWrenches.PerformedByDesignation = performedUserData[2];
				}

				List<string> reviewedUserData = GetUserName(torqueWrenches.CalibrationReviewedBy);

				if (reviewedUserData.Count >= 3)
				{
					torqueWrenches.ReviewedBy = reviewedUserData[0];
					torqueWrenches.ReviewedBySign = reviewedUserData[1];
					torqueWrenches.ReviewedByDesignation = reviewedUserData[2];
				}

				int? ulrNumber = torqueWrenches.ULRNumber == null ? 0 : torqueWrenches.ULRNumber;
				int? certificateNumber = torqueWrenches.CertificateNumber == null ? 0 : torqueWrenches.CertificateNumber;
				List<string> formatList = GetULRAndCertificateNumber(ulrNumber, certificateNumber);

				if (formatList.Count >= 2)
				{
					torqueWrenches.ULRFormat = formatList[0];
					torqueWrenches.CertificateFormat = formatList[1];
				}
			}

			return new ResponseViewModel<TorqueWrenchesViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = torqueWrenches,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("ObservationTemplateService - torquewrenchesId Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<TorqueWrenchesViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "torquewrenchesId"
			};
		}
	}

	#endregion

	#region "VernierCaliber"
	public ResponseViewModel<VernierCaliperViewModel> InsertVernierCaliper(VernierCaliperViewModel verniercaliper)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			int templateObservationId = 0;
			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
																 .GetQueryAsNoTracking(Q => Q.RequestId == verniercaliper.RequestId
																			&& Q.InstrumentId == verniercaliper.InstrumentId)
																 .SingleOrDefault();

			// User labTechnicalManager = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4).SingleOrDefault();
			if ((verniercaliper.TemplateObservationId == 0) && (observationById == null))

			{
				//if(observationById == null)
				//{ 
				TemplateObservation templateObservation = new TemplateObservation()
				{
					InstrumentId = verniercaliper.InstrumentId,
					RequestId = verniercaliper.RequestId,
					TempStart = verniercaliper.TempStart,
					TempEnd = verniercaliper.TempEnd,
					Humidity = verniercaliper.Humidity,
					InstrumentCondition = verniercaliper.ConditionOfVernierCaliper,
					RefWi = verniercaliper.RefWi,
					Allvalues = verniercaliper.Allvalues,
					CreatedOn = DateTime.Now,
					CreatedBy = verniercaliper.CreatedBy,
					//  CalibrationReviewedBy = labTechnicalManager.Id,
					CalibrationReviewedDate = DateTime.Now
				};
				_unitOfWork.Repository<TemplateObservation>().Insert(templateObservation);
				_unitOfWork.SaveChanges();
				templateObservationId = templateObservation.Id;
				//}
			}
			else
			{
				//TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
				//												 .GetQueryAsNoTracking(Q => Q.RequestId == verniercaliper.RequestId
				//															&& Q.InstrumentId == verniercaliper.InstrumentId)
				//												 .SingleOrDefault();
				if (observationById != null)
				{

					if (verniercaliper.TempStart != null)
					{
						observationById.TempStart = verniercaliper.TempStart;
					}

					if (verniercaliper.TempEnd != null)
					{
						observationById.TempEnd = verniercaliper.TempEnd;
					}

					if (verniercaliper.Humidity != null)
					{
						observationById.Humidity = verniercaliper.Humidity;
					}

					if (verniercaliper.ConditionOfVernierCaliper != null)
					{
						observationById.InstrumentCondition = verniercaliper.ConditionOfVernierCaliper;
					}
					if (verniercaliper.RefWi != null)
					{
						observationById.RefWi = verniercaliper.RefWi;
					}
					if (verniercaliper.Allvalues != null)
					{
						observationById.Allvalues = verniercaliper.Allvalues;
					}

					_unitOfWork.Repository<TemplateObservation>().Update(observationById);

				}
			}
			_unitOfWork.SaveChanges();
			if (verniercaliper.Id == 0)
			{
				if (observationById != null)
				{
					templateObservationId = observationById.Id;
				}
				ObsTemplateVernierCaliper obsTemplateVernierCaliper = new ObsTemplateVernierCaliper()
				{
					ObservationId = templateObservationId,
					Measured1_1 = verniercaliper.Measured1_1,
					Actuals1_T_1 = verniercaliper.Actuals1_T_1,
					Actuals1_T_2 = verniercaliper.Actuals1_T_2,
					Actuals1_T_3 = verniercaliper.Actuals1_T_3,
					Avg1_1 = verniercaliper.Avg1_1,
					Measured1_2 = verniercaliper.Measured1_2,
					Actuals1_T_4 = verniercaliper.Actuals1_T_4,
					Actuals1_T_5 = verniercaliper.Actuals1_T_5,
					Actuals1_T_6 = verniercaliper.Actuals1_T_6,
					Avg1_2 = verniercaliper.Avg1_2,
					Measured1_3 = verniercaliper.Measured1_3,
					Actuals1_T_7 = verniercaliper.Actuals1_T_7,
					Actuals1_T_8 = verniercaliper.Actuals1_T_8,
					Actuals1_T_9 = verniercaliper.Actuals1_T_9,
					Avg1_3 = verniercaliper.Avg1_3,
					Measured1_4 = verniercaliper.Measured1_4,
					Actuals1_T_10 = verniercaliper.Actuals1_T_10,
					Actuals1_T_11 = verniercaliper.Actuals1_T_11,
					Actuals1_T_12 = verniercaliper.Actuals1_T_12,
					Avg1_4 = verniercaliper.Avg1_4,
					Measured1_5 = verniercaliper.Measured1_5,
					Actuals1_T_13 = verniercaliper.Actuals1_T_13,
					Actuals1_T_14 = verniercaliper.Actuals1_T_14,
					Actuals1_T_15 = verniercaliper.Actuals1_T_15,
					Avg1_5 = verniercaliper.Avg1_5,
					Measured2_1 = verniercaliper.Measured2_1,
					Actuals2_T_1 = verniercaliper.Actuals2_T_1,
					Actuals2_T_2 = verniercaliper.Actuals2_T_2,
					Actuals2_T_3 = verniercaliper.Actuals2_T_3,
					Avg2_1 = verniercaliper.Avg2_1,
					Measured2_2 = verniercaliper.Measured2_2,
					Actuals2_T_4 = verniercaliper.Actuals2_T_4,
					Actuals2_T_5 = verniercaliper.Actuals2_T_5,
					Actuals2_T_6 = verniercaliper.Actuals2_T_6,
					Avg2_2 = verniercaliper.Avg2_2,
					Measured2_3 = verniercaliper.Measured2_3,
					Actuals2_T_7 = verniercaliper.Actuals2_T_7,
					Actuals2_T_8 = verniercaliper.Actuals2_T_8,
					Actuals2_T_9 = verniercaliper.Actuals2_T_9,
					Avg2_3 = verniercaliper.Avg2_3,
					Measured2_4 = verniercaliper.Measured2_4,
					Actuals2_T_10 = verniercaliper.Actuals2_T_10,
					Actuals2_T_11 = verniercaliper.Actuals2_T_11,
					Actuals2_T_12 = verniercaliper.Actuals2_T_12,
					Avg2_4 = verniercaliper.Avg2_4,
					Measured2_5 = verniercaliper.Measured2_5,
					Actuals2_T_13 = verniercaliper.Actuals2_T_13,
					Actuals2_T_14 = verniercaliper.Actuals2_T_14,
					Actuals2_T_15 = verniercaliper.Actuals2_T_15,
					Avg2_5 = verniercaliper.Avg2_5,
					Measured3_1 = verniercaliper.Measured3_1,
					Actuals3_T_1 = verniercaliper.Actuals3_T_1,
					Actuals3_T_2 = verniercaliper.Actuals3_T_2,
					Actuals3_T_3 = verniercaliper.Actuals3_T_3,
					Avg3_1 = verniercaliper.Avg3_1,
					Measured3_2 = verniercaliper.Measured3_2,
					Actuals3_T_4 = verniercaliper.Actuals3_T_4,
					Actuals3_T_5 = verniercaliper.Actuals3_T_5,
					Actuals3_T_6 = verniercaliper.Actuals3_T_6,
					Avg3_2 = verniercaliper.Avg3_2,
					Measured3_3 = verniercaliper.Measured3_3,
					Actuals3_T_7 = verniercaliper.Actuals3_T_7,
					Actuals3_T_8 = verniercaliper.Actuals3_T_8,
					Actuals3_T_9 = verniercaliper.Actuals3_T_9,
					Avg3_3 = verniercaliper.Avg3_3,
					Measured3_4 = verniercaliper.Measured3_4,
					Actuals3_T_10 = verniercaliper.Actuals3_T_10,
					Actuals3_T_11 = verniercaliper.Actuals3_T_11,
					Actuals3_T_12 = verniercaliper.Actuals3_T_12,
					Avg3_4 = verniercaliper.Avg3_4,
					Measured3_5 = verniercaliper.Measured3_5,
					Actuals3_T_13 = verniercaliper.Actuals3_T_13,
					Actuals3_T_14 = verniercaliper.Actuals3_T_14,
					Actuals3_T_15 = verniercaliper.Actuals3_T_15,
					Avg3_5 = verniercaliper.Avg3_5,
					Measured4_1 = verniercaliper.Measured4_1,
					Actuals4_T_1 = verniercaliper.Actuals4_T_1,
					Actuals4_T_2 = verniercaliper.Actuals4_T_2,
					Actuals4_T_3 = verniercaliper.Actuals4_T_3,
					Avg4_1 = verniercaliper.Avg4_1,
					Measured4_2 = verniercaliper.Measured4_2,
					Actuals4_T_4 = verniercaliper.Actuals4_T_4,
					Actuals4_T_5 = verniercaliper.Actuals4_T_5,
					Actuals4_T_6 = verniercaliper.Actuals4_T_6,
					Avg4_2 = verniercaliper.Avg4_2,
					Measured4_3 = verniercaliper.Measured4_3,
					Actuals4_T_7 = verniercaliper.Actuals4_T_7,
					Actuals4_T_8 = verniercaliper.Actuals4_T_8,
					Actuals4_T_9 = verniercaliper.Actuals4_T_9,
					Avg4_3 = verniercaliper.Avg4_3,
					Measured4_4 = verniercaliper.Measured4_4,
					Actuals4_T_10 = verniercaliper.Actuals4_T_10,
					Actuals4_T_11 = verniercaliper.Actuals4_T_11,
					Actuals4_T_12 = verniercaliper.Actuals4_T_12,
					Avg4_4 = verniercaliper.Avg4_4,
					Measured4_5 = verniercaliper.Measured4_5,
					Actuals4_T_13 = verniercaliper.Actuals4_T_13,
					Actuals4_T_14 = verniercaliper.Actuals4_T_14,
					Actuals4_T_15 = verniercaliper.Actuals4_T_15,
					Avg4_5 = verniercaliper.Avg4_5,
					Measured5_1 = verniercaliper.Measured5_1,
					Actuals5_T_1 = verniercaliper.Actuals5_T_1,
					Actuals5_T_2 = verniercaliper.Actuals5_T_2,
					Actuals5_T_3 = verniercaliper.Actuals5_T_3,
					Avg5_1 = verniercaliper.Avg5_1,
					Measured5_2 = verniercaliper.Measured5_2,
					Actuals5_T_4 = verniercaliper.Actuals5_T_4,
					Actuals5_T_5 = verniercaliper.Actuals5_T_5,
					Actuals5_T_6 = verniercaliper.Actuals5_T_6,
					Avg5_2 = verniercaliper.Avg5_2,
					Measured5_3 = verniercaliper.Measured5_3,
					Actuals5_T_7 = verniercaliper.Actuals5_T_7,
					Actuals5_T_8 = verniercaliper.Actuals5_T_8,
					Actuals5_T_9 = verniercaliper.Actuals5_T_9,
					Avg5_3 = verniercaliper.Avg5_3,
					Measured5_4 = verniercaliper.Measured5_4,
					Actuals5_T_10 = verniercaliper.Actuals5_T_10,
					Actuals5_T_11 = verniercaliper.Actuals5_T_11,
					Actuals5_T_12 = verniercaliper.Actuals5_T_12,
					Avg5_4 = verniercaliper.Avg5_4,
					Measured5_5 = verniercaliper.Measured5_5,
					Actuals5_T_13 = verniercaliper.Actuals5_T_13,
					Actuals5_T_14 = verniercaliper.Actuals5_T_14,
					Actuals5_T_15 = verniercaliper.Actuals5_T_15,
					Avg5_5 = verniercaliper.Avg5_5,
					Measured6_1 = verniercaliper.Measured6_1,
					Actuals6_T_1 = verniercaliper.Actuals6_T_1,
					Actuals6_T_2 = verniercaliper.Actuals6_T_2,
					Actuals6_T_3 = verniercaliper.Actuals6_T_3,
					Avg6_1 = verniercaliper.Avg6_1,
					Measured6_2 = verniercaliper.Measured6_2,
					Actuals6_T_4 = verniercaliper.Actuals6_T_4,
					Actuals6_T_5 = verniercaliper.Actuals6_T_5,
					Actuals6_T_6 = verniercaliper.Actuals6_T_6,
					Avg6_2 = verniercaliper.Avg6_2,
					Measured6_3 = verniercaliper.Measured6_3,
					Actuals6_T_7 = verniercaliper.Actuals6_T_7,
					Actuals6_T_8 = verniercaliper.Actuals6_T_8,
					Actuals6_T_9 = verniercaliper.Actuals6_T_9,
					Avg6_3 = verniercaliper.Avg6_3,
					Measured6_4 = verniercaliper.Measured6_4,
					Actuals6_T_10 = verniercaliper.Actuals6_T_10,
					Actuals6_T_11 = verniercaliper.Actuals6_T_11,
					Actuals6_T_12 = verniercaliper.Actuals6_T_12,
					Avg6_4 = verniercaliper.Avg6_4,
					Measured6_5 = verniercaliper.Measured6_5,
					Actuals6_T_13 = verniercaliper.Actuals6_T_13,
					Actuals6_T_14 = verniercaliper.Actuals6_T_14,
					Actuals6_T_15 = verniercaliper.Actuals6_T_15,
					Avg6_5 = verniercaliper.Avg6_5,
					MuLeftValue1 = verniercaliper.MuLeftValue1,
					MuRightValue1 = verniercaliper.MuRightValue1,
					MuLeftValue2 = verniercaliper.MuLeftValue2,
					MuRightValue2 = verniercaliper.MuRightValue2,
					MuLeftValue3 = verniercaliper.MuLeftValue3,
					MuRightValue3 = verniercaliper.MuRightValue3,
					MuLeftValue4 = verniercaliper.MuLeftValue4,
					MuRightValue4 = verniercaliper.MuRightValue4,
					MuLeftValue5 = verniercaliper.MuLeftValue5,
					MuRightValue5 = verniercaliper.MuRightValue5,
					ExternalParallelismDetails = verniercaliper.ExternalParallelismDetails,
					InternaljawParallelismDetails = verniercaliper.InternaljawParallelismDetails,
					CalibrationPerformedBy = verniercaliper.CalibrationPerformedBy,
					CalibrationPerformedDate = verniercaliper.CalibrationPerformedDate.ToString("dd/MM/yyyy"),
					CreatedOn = DateTime.Now,
					CreatedBy = verniercaliper.CreatedBy,
					ReviewedDate = verniercaliper.CalibrationPerformedDate.ToString("dd/MM/yyyy"),
					CalibrationReviewedDate = DateTime.Now
					//CalibrationReviewedBy = labTechnicalManager.Id,
					//ReviewedBy = verniercaliper.ReviewedBy
				};
				_unitOfWork.Repository<ObsTemplateVernierCaliper>().Insert(obsTemplateVernierCaliper);
			}
			else
			{
				ObsTemplateVernierCaliper verniercaliperById = _unitOfWork.Repository<ObsTemplateVernierCaliper>()
																		  .GetQueryAsNoTracking(Q => Q.Id == verniercaliper.Id)
																		  .SingleOrDefault();

				if (verniercaliper.Measured1_1 != null)
					verniercaliperById.Measured1_1 = verniercaliper.Measured1_1;

				if (verniercaliper.Actuals1_T_1 != null)
					verniercaliperById.Actuals1_T_1 = verniercaliper.Actuals1_T_1;

				if (verniercaliper.Actuals1_T_2 != null)
					verniercaliperById.Actuals1_T_2 = verniercaliper.Actuals1_T_2;

				if (verniercaliper.Actuals1_T_3 != null)
					verniercaliperById.Actuals1_T_3 = verniercaliper.Actuals1_T_3;

				if (verniercaliper.Avg1_1 != null)
					verniercaliperById.Avg1_1 = verniercaliper.Avg1_1;

				if (verniercaliper.Measured1_2 != null)
					verniercaliperById.Measured1_2 = verniercaliper.Measured1_2;

				if (verniercaliper.Actuals1_T_4 != null)
					verniercaliperById.Actuals1_T_4 = verniercaliper.Actuals1_T_4;

				if (verniercaliper.Actuals1_T_5 != null)
					verniercaliperById.Actuals1_T_5 = verniercaliper.Actuals1_T_5;

				if (verniercaliper.Actuals1_T_6 != null)
					verniercaliperById.Actuals1_T_6 = verniercaliper.Actuals1_T_6;

				if (verniercaliper.Avg1_2 != null)
					verniercaliperById.Avg1_2 = verniercaliper.Avg1_2;

				if (verniercaliper.Measured1_3 != null)
					verniercaliperById.Measured1_3 = verniercaliper.Measured1_3;

				if (verniercaliper.Actuals1_T_7 != null)
					verniercaliperById.Actuals1_T_7 = verniercaliper.Actuals1_T_7;

				if (verniercaliper.Actuals1_T_8 != null)
					verniercaliperById.Actuals1_T_8 = verniercaliper.Actuals1_T_8;

				if (verniercaliper.Actuals1_T_9 != null)
					verniercaliperById.Actuals1_T_9 = verniercaliper.Actuals1_T_9;

				if (verniercaliper.Avg1_3 != null)
					verniercaliperById.Avg1_3 = verniercaliper.Avg1_3;

				if (verniercaliper.Measured1_4 != null)
					verniercaliperById.Measured1_4 = verniercaliper.Measured1_4;

				if (verniercaliper.Actuals1_T_10 != null)
					verniercaliperById.Actuals1_T_10 = verniercaliper.Actuals1_T_10;

				if (verniercaliper.Actuals1_T_11 != null)
					verniercaliperById.Actuals1_T_11 = verniercaliper.Actuals1_T_11;

				if (verniercaliper.Actuals1_T_12 != null)
					verniercaliperById.Actuals1_T_12 = verniercaliper.Actuals1_T_12;

				if (verniercaliper.Avg1_4 != null)
					verniercaliperById.Avg1_4 = verniercaliper.Avg1_4;

				if (verniercaliper.Measured1_5 != null)
					verniercaliperById.Measured1_5 = verniercaliper.Measured1_5;

				if (verniercaliper.Actuals1_T_13 != null)
					verniercaliperById.Actuals1_T_13 = verniercaliper.Actuals1_T_13;

				if (verniercaliper.Actuals1_T_14 != null)
					verniercaliperById.Actuals1_T_14 = verniercaliper.Actuals1_T_14;

				if (verniercaliper.Actuals1_T_15 != null)
					verniercaliperById.Actuals1_T_15 = verniercaliper.Actuals1_T_15;

				if (verniercaliper.Avg1_5 != null)
					verniercaliperById.Avg1_5 = verniercaliper.Avg1_5;

				if (verniercaliper.Measured2_1 != null)
					verniercaliperById.Measured2_1 = verniercaliper.Measured2_1;

				if (verniercaliper.Actuals2_T_1 != null)
					verniercaliperById.Actuals2_T_1 = verniercaliper.Actuals2_T_1;

				if (verniercaliper.Actuals2_T_2 != null)
					verniercaliperById.Actuals2_T_2 = verniercaliper.Actuals2_T_2;

				if (verniercaliper.Actuals2_T_3 != null)
					verniercaliperById.Actuals2_T_3 = verniercaliper.Actuals2_T_3;

				if (verniercaliper.Avg2_1 != null)
					verniercaliperById.Avg2_1 = verniercaliper.Avg2_1;

				if (verniercaliper.Measured2_2 != null)
					verniercaliperById.Measured2_2 = verniercaliper.Measured2_2;

				if (verniercaliper.Actuals2_T_4 != null)
					verniercaliperById.Actuals2_T_4 = verniercaliper.Actuals2_T_4;

				if (verniercaliper.Actuals2_T_5 != null)
					verniercaliperById.Actuals2_T_5 = verniercaliper.Actuals2_T_5;

				if (verniercaliper.Actuals2_T_6 != null)
					verniercaliperById.Actuals2_T_6 = verniercaliper.Actuals2_T_6;

				if (verniercaliper.Avg2_2 != null)
					verniercaliperById.Avg2_2 = verniercaliper.Avg2_2;

				if (verniercaliper.Measured2_3 != null)
					verniercaliperById.Measured2_3 = verniercaliper.Measured2_3;

				if (verniercaliper.Actuals2_T_7 != null)
					verniercaliperById.Actuals2_T_7 = verniercaliper.Actuals2_T_7;

				if (verniercaliper.Actuals2_T_8 != null)
					verniercaliperById.Actuals2_T_8 = verniercaliper.Actuals2_T_8;

				if (verniercaliper.Actuals2_T_9 != null)
					verniercaliperById.Actuals2_T_9 = verniercaliper.Actuals2_T_9;

				if (verniercaliper.Avg2_3 != null)
					verniercaliperById.Avg2_3 = verniercaliper.Avg2_3;

				if (verniercaliper.Measured2_4 != null)
					verniercaliperById.Measured2_4 = verniercaliper.Measured2_4;

				if (verniercaliper.Actuals2_T_10 != null)
					verniercaliperById.Actuals2_T_10 = verniercaliper.Actuals2_T_10;

				if (verniercaliper.Actuals2_T_11 != null)
					verniercaliperById.Actuals2_T_11 = verniercaliper.Actuals2_T_11;

				if (verniercaliper.Actuals2_T_12 != null)
					verniercaliperById.Actuals2_T_12 = verniercaliper.Actuals2_T_12;

				if (verniercaliper.Avg2_4 != null)
					verniercaliperById.Avg2_4 = verniercaliper.Avg2_4;

				if (verniercaliper.Measured2_5 != null)
					verniercaliperById.Measured2_5 = verniercaliper.Measured2_5;

				if (verniercaliper.Actuals2_T_13 != null)
					verniercaliperById.Actuals2_T_13 = verniercaliper.Actuals2_T_13;

				if (verniercaliper.Actuals2_T_14 != null)
					verniercaliperById.Actuals2_T_14 = verniercaliper.Actuals2_T_14;

				if (verniercaliper.Measured1_1 != null)
					verniercaliperById.Actuals2_T_15 = verniercaliper.Actuals2_T_15;

				if (verniercaliper.Avg2_5 != null)
					verniercaliperById.Avg2_5 = verniercaliper.Avg2_5;

				if (verniercaliper.Measured3_1 != null)
					verniercaliperById.Measured3_1 = verniercaliper.Measured3_1;

				if (verniercaliper.Actuals3_T_1 != null)
					verniercaliperById.Actuals3_T_1 = verniercaliper.Actuals3_T_1;

				if (verniercaliper.Actuals3_T_2 != null)
					verniercaliperById.Actuals3_T_2 = verniercaliper.Actuals3_T_2;

				if (verniercaliper.Actuals3_T_3 != null)
					verniercaliperById.Actuals3_T_3 = verniercaliper.Actuals3_T_3;

				if (verniercaliper.Avg3_1 != null)
					verniercaliperById.Avg3_1 = verniercaliper.Avg3_1;

				if (verniercaliper.Measured3_2 != null)
					verniercaliperById.Measured3_2 = verniercaliper.Measured3_2;

				if (verniercaliper.Actuals3_T_4 != null)
					verniercaliperById.Actuals3_T_4 = verniercaliper.Actuals3_T_4;

				if (verniercaliper.Actuals3_T_5 != null)
					verniercaliperById.Actuals3_T_5 = verniercaliper.Actuals3_T_5;

				if (verniercaliper.Actuals3_T_6 != null)
					verniercaliperById.Actuals3_T_6 = verniercaliper.Actuals3_T_6;

				if (verniercaliper.Avg3_2 != null)
					verniercaliperById.Avg3_2 = verniercaliper.Avg3_2;

				if (verniercaliper.Measured3_3 != null)
					verniercaliperById.Measured3_3 = verniercaliper.Measured3_3;

				if (verniercaliper.Actuals3_T_7 != null)
					verniercaliperById.Actuals3_T_7 = verniercaliper.Actuals3_T_7;

				if (verniercaliper.Actuals3_T_8 != null)
					verniercaliperById.Actuals3_T_8 = verniercaliper.Actuals3_T_8;

				if (verniercaliper.Actuals3_T_9 != null)
					verniercaliperById.Actuals3_T_9 = verniercaliper.Actuals3_T_9;

				if (verniercaliper.Avg3_3 != null)
					verniercaliperById.Avg3_3 = verniercaliper.Avg3_3;

				if (verniercaliper.Measured3_4 != null)
					verniercaliperById.Measured3_4 = verniercaliper.Measured3_4;

				if (verniercaliper.Actuals3_T_10 != null)
					verniercaliperById.Actuals3_T_10 = verniercaliper.Actuals3_T_10;

				if (verniercaliper.Actuals3_T_11 != null)
					verniercaliperById.Actuals3_T_11 = verniercaliper.Actuals3_T_11;

				if (verniercaliper.Actuals3_T_12 != null)
					verniercaliperById.Actuals3_T_12 = verniercaliper.Actuals3_T_12;

				if (verniercaliper.Avg3_4 != null)
					verniercaliperById.Avg3_4 = verniercaliper.Avg3_4;

				if (verniercaliper.Measured3_5 != null)
					verniercaliperById.Measured3_5 = verniercaliper.Measured3_5;

				if (verniercaliper.Actuals3_T_13 != null)
					verniercaliperById.Actuals3_T_13 = verniercaliper.Actuals3_T_13;

				if (verniercaliper.Actuals3_T_14 != null)
					verniercaliperById.Actuals3_T_14 = verniercaliper.Actuals3_T_14;

				if (verniercaliper.Actuals3_T_15 != null)
					verniercaliperById.Actuals3_T_15 = verniercaliper.Actuals3_T_15;

				if (verniercaliper.Avg3_5 != null)
					verniercaliperById.Avg3_5 = verniercaliper.Avg3_5;

				if (verniercaliper.Measured4_1 != null)
					verniercaliperById.Measured4_1 = verniercaliper.Measured4_1;

				if (verniercaliper.Actuals4_T_1 != null)
					verniercaliperById.Actuals4_T_1 = verniercaliper.Actuals4_T_1;

				if (verniercaliper.Actuals4_T_2 != null)
					verniercaliperById.Actuals4_T_2 = verniercaliper.Actuals4_T_2;

				if (verniercaliper.Actuals4_T_3 != null)
					verniercaliperById.Actuals4_T_3 = verniercaliper.Actuals4_T_3;

				if (verniercaliper.Avg4_1 != null)
					verniercaliperById.Avg4_1 = verniercaliper.Avg4_1;

				if (verniercaliper.Measured4_2 != null)
					verniercaliperById.Measured4_2 = verniercaliper.Measured4_2;

				if (verniercaliper.Actuals4_T_4 != null)
					verniercaliperById.Actuals4_T_4 = verniercaliper.Actuals4_T_4;

				if (verniercaliper.Actuals4_T_5 != null)
					verniercaliperById.Actuals4_T_5 = verniercaliper.Actuals4_T_5;

				if (verniercaliper.Actuals4_T_6 != null)
					verniercaliperById.Actuals4_T_6 = verniercaliper.Actuals4_T_6;

				if (verniercaliper.Avg4_2 != null)
					verniercaliperById.Avg4_2 = verniercaliper.Avg4_2;

				if (verniercaliper.Measured4_3 != null)
					verniercaliperById.Measured4_3 = verniercaliper.Measured4_3;

				if (verniercaliper.Actuals4_T_7 != null)
					verniercaliperById.Actuals4_T_7 = verniercaliper.Actuals4_T_7;

				if (verniercaliper.Actuals4_T_8 != null)
					verniercaliperById.Actuals4_T_8 = verniercaliper.Actuals4_T_8;

				if (verniercaliper.Actuals4_T_9 != null)
					verniercaliperById.Actuals4_T_9 = verniercaliper.Actuals4_T_9;

				if (verniercaliper.Avg4_3 != null)
					verniercaliperById.Avg4_3 = verniercaliper.Avg4_3;

				if (verniercaliper.Measured4_4 != null)
					verniercaliperById.Measured4_4 = verniercaliper.Measured4_4;

				if (verniercaliper.Actuals4_T_10 != null)
					verniercaliperById.Actuals4_T_10 = verniercaliper.Actuals4_T_10;

				if (verniercaliper.Actuals4_T_11 != null)
					verniercaliperById.Actuals4_T_11 = verniercaliper.Actuals4_T_11;

				if (verniercaliper.Actuals4_T_12 != null)
					verniercaliperById.Actuals4_T_12 = verniercaliper.Actuals4_T_12;

				if (verniercaliper.Avg4_4 != null)
					verniercaliperById.Avg4_4 = verniercaliper.Avg4_4;

				if (verniercaliper.Measured4_5 != null)
					verniercaliperById.Measured4_5 = verniercaliper.Measured4_5;

				if (verniercaliper.Actuals4_T_13 != null)
					verniercaliperById.Actuals4_T_13 = verniercaliper.Actuals4_T_13;

				if (verniercaliper.Actuals4_T_14 != null)
					verniercaliperById.Actuals4_T_14 = verniercaliper.Actuals4_T_14;

				if (verniercaliper.Actuals4_T_15 != null)
					verniercaliperById.Actuals4_T_15 = verniercaliper.Actuals4_T_15;

				if (verniercaliper.Avg4_5 != null)
					verniercaliperById.Avg4_5 = verniercaliper.Avg4_5;

				if (verniercaliper.Measured5_1 != null)
					verniercaliperById.Measured5_1 = verniercaliper.Measured5_1;

				if (verniercaliper.Actuals5_T_1 != null)
					verniercaliperById.Actuals5_T_1 = verniercaliper.Actuals5_T_1;

				if (verniercaliper.Actuals5_T_2 != null)
					verniercaliperById.Actuals5_T_2 = verniercaliper.Actuals5_T_2;

				if (verniercaliper.Actuals5_T_3 != null)
					verniercaliperById.Actuals5_T_3 = verniercaliper.Actuals5_T_3;

				if (verniercaliper.Avg5_1 != null)
					verniercaliperById.Avg5_1 = verniercaliper.Avg5_1;

				if (verniercaliper.Measured5_2 != null)
					verniercaliperById.Measured5_2 = verniercaliper.Measured5_2;

				if (verniercaliper.Actuals5_T_4 != null)
					verniercaliperById.Actuals5_T_4 = verniercaliper.Actuals5_T_4;

				if (verniercaliper.Actuals5_T_5 != null)
					verniercaliperById.Actuals5_T_5 = verniercaliper.Actuals5_T_5;

				if (verniercaliper.Actuals5_T_6 != null)
					verniercaliperById.Actuals5_T_6 = verniercaliper.Actuals5_T_6;

				if (verniercaliper.Avg5_2 != null)
					verniercaliperById.Avg5_2 = verniercaliper.Avg5_2;

				if (verniercaliper.Measured5_3 != null)
					verniercaliperById.Measured5_3 = verniercaliper.Measured5_3;

				if (verniercaliper.Actuals5_T_7 != null)
					verniercaliperById.Actuals5_T_7 = verniercaliper.Actuals5_T_7;

				if (verniercaliper.Actuals5_T_8 != null)
					verniercaliperById.Actuals5_T_8 = verniercaliper.Actuals5_T_8;

				if (verniercaliper.Actuals5_T_9 != null)
					verniercaliperById.Actuals5_T_9 = verniercaliper.Actuals5_T_9;

				if (verniercaliper.Avg5_3 != null)
					verniercaliperById.Avg5_3 = verniercaliper.Avg5_3;

				if (verniercaliper.Measured5_4 != null)
					verniercaliperById.Measured5_4 = verniercaliper.Measured5_4;

				if (verniercaliper.Actuals5_T_10 != null)
					verniercaliperById.Actuals5_T_10 = verniercaliper.Actuals5_T_10;

				if (verniercaliper.Actuals5_T_11 != null)
					verniercaliperById.Actuals5_T_11 = verniercaliper.Actuals5_T_11;

				if (verniercaliper.Actuals5_T_12 != null)
					verniercaliperById.Actuals5_T_12 = verniercaliper.Actuals5_T_12;

				if (verniercaliper.Avg5_4 != null)
					verniercaliperById.Avg5_4 = verniercaliper.Avg5_4;

				if (verniercaliper.Measured5_5 != null)
					verniercaliperById.Measured5_5 = verniercaliper.Measured5_5;

				if (verniercaliper.Actuals5_T_13 != null)
					verniercaliperById.Actuals5_T_13 = verniercaliper.Actuals5_T_13;

				if (verniercaliper.Actuals5_T_14 != null)
					verniercaliperById.Actuals5_T_14 = verniercaliper.Actuals5_T_14;

				if (verniercaliper.Actuals5_T_15 != null)
					verniercaliperById.Actuals5_T_15 = verniercaliper.Actuals5_T_15;

				if (verniercaliper.Avg5_5 != null)
					verniercaliperById.Avg5_5 = verniercaliper.Avg5_5;

				if (verniercaliper.Measured6_1 != null)
					verniercaliperById.Measured6_1 = verniercaliper.Measured6_1;

				if (verniercaliper.Actuals6_T_1 != null)
					verniercaliperById.Actuals6_T_1 = verniercaliper.Actuals6_T_1;

				if (verniercaliper.Actuals6_T_2 != null)
					verniercaliperById.Actuals6_T_2 = verniercaliper.Actuals6_T_2;

				if (verniercaliper.Actuals6_T_3 != null)
					verniercaliperById.Actuals6_T_3 = verniercaliper.Actuals6_T_3;

				if (verniercaliper.Avg6_1 != null)
					verniercaliperById.Avg6_1 = verniercaliper.Avg6_1;

				if (verniercaliper.Measured6_2 != null)
					verniercaliperById.Measured6_2 = verniercaliper.Measured6_2;

				if (verniercaliper.Actuals6_T_4 != null)
					verniercaliperById.Actuals6_T_4 = verniercaliper.Actuals6_T_4;

				if (verniercaliper.Actuals6_T_5 != null)
					verniercaliperById.Actuals6_T_5 = verniercaliper.Actuals6_T_5;

				if (verniercaliper.Actuals6_T_6 != null)
					verniercaliperById.Actuals6_T_6 = verniercaliper.Actuals6_T_6;

				if (verniercaliper.Avg6_2 != null)
					verniercaliperById.Avg6_2 = verniercaliper.Avg6_2;

				if (verniercaliper.Measured6_3 != null)
					verniercaliperById.Measured6_3 = verniercaliper.Measured6_3;

				if (verniercaliper.Actuals6_T_7 != null)
					verniercaliperById.Actuals6_T_7 = verniercaliper.Actuals6_T_7;

				if (verniercaliper.Actuals6_T_8 != null)
					verniercaliperById.Actuals6_T_8 = verniercaliper.Actuals6_T_8;

				if (verniercaliper.Actuals6_T_9 != null)
					verniercaliperById.Actuals6_T_9 = verniercaliper.Actuals6_T_9;

				if (verniercaliper.Avg6_3 != null)
					verniercaliperById.Avg6_3 = verniercaliper.Avg6_3;

				if (verniercaliper.Measured6_4 != null)
					verniercaliperById.Measured6_4 = verniercaliper.Measured6_4;

				if (verniercaliper.Actuals6_T_10 != null)
					verniercaliperById.Actuals6_T_10 = verniercaliper.Actuals6_T_10;

				if (verniercaliper.Actuals6_T_11 != null)
					verniercaliperById.Actuals6_T_11 = verniercaliper.Actuals6_T_11;

				if (verniercaliper.Actuals6_T_12 != null)
					verniercaliperById.Actuals6_T_12 = verniercaliper.Actuals6_T_12;

				if (verniercaliper.Avg6_4 != null)
					verniercaliperById.Avg6_4 = verniercaliper.Avg6_4;

				if (verniercaliper.Measured6_5 != null)
					verniercaliperById.Measured6_5 = verniercaliper.Measured6_5;

				if (verniercaliper.Actuals6_T_13 != null)
					verniercaliperById.Actuals6_T_13 = verniercaliper.Actuals6_T_13;

				if (verniercaliper.Actuals6_T_14 != null)
					verniercaliperById.Actuals6_T_14 = verniercaliper.Actuals6_T_14;

				if (verniercaliper.Actuals6_T_15 != null)
					verniercaliperById.Actuals6_T_15 = verniercaliper.Actuals6_T_15;

				if (verniercaliper.Avg6_5 != null)
					verniercaliperById.Avg6_5 = verniercaliper.Avg6_5;

				if (verniercaliper.MuLeftValue1 != null)
					verniercaliperById.MuLeftValue1 = verniercaliper.MuLeftValue1;

				if (verniercaliper.MuRightValue1 != null)
					verniercaliperById.MuRightValue1 = verniercaliper.MuRightValue1;

				if (verniercaliper.MuLeftValue2 != null)
					verniercaliperById.MuLeftValue2 = verniercaliper.MuLeftValue2;

				if (verniercaliper.MuRightValue2 != null)
					verniercaliperById.MuRightValue2 = verniercaliper.MuRightValue2;

				if (verniercaliper.MuLeftValue3 != null)
					verniercaliperById.MuLeftValue3 = verniercaliper.MuLeftValue3;

				if (verniercaliper.MuRightValue3 != null)
					verniercaliperById.MuRightValue3 = verniercaliper.MuRightValue3;

				if (verniercaliper.MuLeftValue4 != null)
					verniercaliperById.MuLeftValue4 = verniercaliper.MuLeftValue4;

				if (verniercaliper.MuRightValue4 != null)
					verniercaliperById.MuRightValue4 = verniercaliper.MuRightValue4;

				if (verniercaliper.MuLeftValue5 != null)
					verniercaliperById.MuLeftValue5 = verniercaliper.MuLeftValue5;

				if (verniercaliper.MuRightValue5 != null)
					verniercaliperById.MuRightValue5 = verniercaliper.MuRightValue5;

				if (verniercaliper.ExternalParallelismDetails != null)
					verniercaliperById.ExternalParallelismDetails = verniercaliper.ExternalParallelismDetails;

				if (verniercaliper.InternaljawParallelismDetails != null)
					verniercaliperById.InternaljawParallelismDetails = verniercaliper.InternaljawParallelismDetails;

				_unitOfWork.Repository<ObsTemplateVernierCaliper>().Update(verniercaliperById);
			}
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			return new ResponseViewModel<VernierCaliperViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - InsertVernierCaliper Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<VernierCaliperViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = verniercaliper,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "InsertVernierCaliper"
			};
		}
	}

	public ResponseViewModel<VernierCaliperViewModel> GetVernierCaliperById(int requestId, int instrumentId)
	{
		try
		{
			VernierCaliperViewModel vernierCaliper = _unitOfWork.Repository<TemplateObservation>()
			.GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
			.Include(I => I.VerniercaliperModel)
			.Select(s => new VernierCaliperViewModel()
			{
				TemplateObservationId = s.Id,
				TempStart = s.TempStart,
				TempEnd = s.TempEnd,
				Humidity = s.Humidity,
				RefWi = s.RefWi,
				Allvalues = s.Allvalues,
				ConditionOfVernierCaliper = s.InstrumentCondition,
				CreatedBy = s.CreatedBy,
				CalibrationReviewedBy = s.CalibrationReviewedBy,
				ULRNumber = s.ULRNumber,
				CertificateNumber = s.CertificateNumber,
				CalibrationPerformedDate = s.CreatedOn,
				CalibrationReviewedDate = s.CalibrationReviewedDate,
				ReviewStatus = s.ReviewStatus,
				Id = s.VerniercaliperModel.Select(s => s.Id).FirstOrDefault(),
				Measured1_1 = s.VerniercaliperModel.Select(S => S.Measured1_1).FirstOrDefault(),
				Actuals1_T_1 = s.VerniercaliperModel.Select(S => S.Actuals1_T_1).FirstOrDefault(),
				Actuals1_T_2 = s.VerniercaliperModel.Select(S => S.Actuals1_T_2).FirstOrDefault(),
				Actuals1_T_3 = s.VerniercaliperModel.Select(S => S.Actuals1_T_3).FirstOrDefault(),
				Avg1_1 = s.VerniercaliperModel.Select(S => S.Avg1_1).FirstOrDefault(),
				Measured1_2 = s.VerniercaliperModel.Select(S => S.Measured1_2).FirstOrDefault(),
				Actuals1_T_4 = s.VerniercaliperModel.Select(S => S.Actuals1_T_4).FirstOrDefault(),
				Actuals1_T_5 = s.VerniercaliperModel.Select(S => S.Actuals1_T_5).FirstOrDefault(),
				Actuals1_T_6 = s.VerniercaliperModel.Select(S => S.Actuals1_T_6).FirstOrDefault(),
				Avg1_2 = s.VerniercaliperModel.Select(S => S.Avg1_2).FirstOrDefault(),
				Measured1_3 = s.VerniercaliperModel.Select(S => S.Measured1_3).FirstOrDefault(),
				Actuals1_T_7 = s.VerniercaliperModel.Select(S => S.Actuals1_T_7).FirstOrDefault(),
				Actuals1_T_8 = s.VerniercaliperModel.Select(S => S.Actuals1_T_8).FirstOrDefault(),
				Actuals1_T_9 = s.VerniercaliperModel.Select(S => S.Actuals1_T_9).FirstOrDefault(),
				Avg1_3 = s.VerniercaliperModel.Select(S => S.Avg1_3).FirstOrDefault(),
				Measured1_4 = s.VerniercaliperModel.Select(S => S.Measured1_4).FirstOrDefault(),
				Actuals1_T_10 = s.VerniercaliperModel.Select(S => S.Actuals1_T_10).FirstOrDefault(),
				Actuals1_T_11 = s.VerniercaliperModel.Select(S => S.Actuals1_T_11).FirstOrDefault(),
				Actuals1_T_12 = s.VerniercaliperModel.Select(S => S.Actuals1_T_12).FirstOrDefault(),
				Avg1_4 = s.VerniercaliperModel.Select(S => S.Avg1_4).FirstOrDefault(),
				Measured1_5 = s.VerniercaliperModel.Select(S => S.Measured1_5).FirstOrDefault(),
				Actuals1_T_13 = s.VerniercaliperModel.Select(S => S.Actuals1_T_13).FirstOrDefault(),
				Actuals1_T_14 = s.VerniercaliperModel.Select(S => S.Actuals1_T_14).FirstOrDefault(),
				Actuals1_T_15 = s.VerniercaliperModel.Select(S => S.Actuals1_T_15).FirstOrDefault(),
				Avg1_5 = s.VerniercaliperModel.Select(S => S.Avg1_5).FirstOrDefault(),
				Measured2_1 = s.VerniercaliperModel.Select(S => S.Measured2_1).FirstOrDefault(),
				Actuals2_T_1 = s.VerniercaliperModel.Select(S => S.Actuals2_T_1).FirstOrDefault(),
				Actuals2_T_2 = s.VerniercaliperModel.Select(S => S.Actuals2_T_2).FirstOrDefault(),
				Actuals2_T_3 = s.VerniercaliperModel.Select(S => S.Actuals2_T_3).FirstOrDefault(),
				Avg2_1 = s.VerniercaliperModel.Select(S => S.Avg2_1).FirstOrDefault(),
				Measured2_2 = s.VerniercaliperModel.Select(S => S.Measured2_2).FirstOrDefault(),
				Actuals2_T_4 = s.VerniercaliperModel.Select(S => S.Actuals2_T_4).FirstOrDefault(),
				Actuals2_T_5 = s.VerniercaliperModel.Select(S => S.Actuals2_T_5).FirstOrDefault(),
				Actuals2_T_6 = s.VerniercaliperModel.Select(S => S.Actuals2_T_6).FirstOrDefault(),
				Avg2_2 = s.VerniercaliperModel.Select(S => S.Avg2_2).FirstOrDefault(),
				Measured2_3 = s.VerniercaliperModel.Select(S => S.Measured2_3).FirstOrDefault(),
				Actuals2_T_7 = s.VerniercaliperModel.Select(S => S.Actuals2_T_7).FirstOrDefault(),
				Actuals2_T_8 = s.VerniercaliperModel.Select(S => S.Actuals2_T_8).FirstOrDefault(),
				Actuals2_T_9 = s.VerniercaliperModel.Select(S => S.Actuals2_T_9).FirstOrDefault(),
				Avg2_3 = s.VerniercaliperModel.Select(S => S.Avg2_3).FirstOrDefault(),
				Measured2_4 = s.VerniercaliperModel.Select(S => S.Measured2_4).FirstOrDefault(),
				Actuals2_T_10 = s.VerniercaliperModel.Select(S => S.Actuals2_T_10).FirstOrDefault(),
				Actuals2_T_11 = s.VerniercaliperModel.Select(S => S.Actuals2_T_11).FirstOrDefault(),
				Actuals2_T_12 = s.VerniercaliperModel.Select(S => S.Actuals2_T_12).FirstOrDefault(),
				Avg2_4 = s.VerniercaliperModel.Select(S => S.Avg2_4).FirstOrDefault(),
				Measured2_5 = s.VerniercaliperModel.Select(S => S.Measured2_5).FirstOrDefault(),
				Actuals2_T_13 = s.VerniercaliperModel.Select(S => S.Actuals2_T_13).FirstOrDefault(),
				Actuals2_T_14 = s.VerniercaliperModel.Select(S => S.Actuals2_T_14).FirstOrDefault(),
				Actuals2_T_15 = s.VerniercaliperModel.Select(S => S.Actuals2_T_15).FirstOrDefault(),
				Avg2_5 = s.VerniercaliperModel.Select(S => S.Avg2_5).FirstOrDefault(),
				Measured3_1 = s.VerniercaliperModel.Select(S => S.Measured3_1).FirstOrDefault(),
				Actuals3_T_1 = s.VerniercaliperModel.Select(S => S.Actuals3_T_1).FirstOrDefault(),
				Actuals3_T_2 = s.VerniercaliperModel.Select(S => S.Actuals3_T_2).FirstOrDefault(),
				Actuals3_T_3 = s.VerniercaliperModel.Select(S => S.Actuals3_T_3).FirstOrDefault(),
				Avg3_1 = s.VerniercaliperModel.Select(S => S.Avg3_1).FirstOrDefault(),
				Measured3_2 = s.VerniercaliperModel.Select(S => S.Measured3_2).FirstOrDefault(),
				Actuals3_T_4 = s.VerniercaliperModel.Select(S => S.Actuals3_T_4).FirstOrDefault(),
				Actuals3_T_5 = s.VerniercaliperModel.Select(S => S.Actuals3_T_5).FirstOrDefault(),
				Actuals3_T_6 = s.VerniercaliperModel.Select(S => S.Actuals3_T_6).FirstOrDefault(),
				Avg3_2 = s.VerniercaliperModel.Select(S => S.Avg3_2).FirstOrDefault(),
				Measured3_3 = s.VerniercaliperModel.Select(S => S.Measured3_3).FirstOrDefault(),
				Actuals3_T_7 = s.VerniercaliperModel.Select(S => S.Actuals3_T_7).FirstOrDefault(),
				Actuals3_T_8 = s.VerniercaliperModel.Select(S => S.Actuals3_T_8).FirstOrDefault(),
				Actuals3_T_9 = s.VerniercaliperModel.Select(S => S.Actuals3_T_9).FirstOrDefault(),
				Avg3_3 = s.VerniercaliperModel.Select(S => S.Avg3_3).FirstOrDefault(),
				Measured3_4 = s.VerniercaliperModel.Select(S => S.Measured3_4).FirstOrDefault(),
				Actuals3_T_10 = s.VerniercaliperModel.Select(S => S.Actuals3_T_10).FirstOrDefault(),
				Actuals3_T_11 = s.VerniercaliperModel.Select(S => S.Actuals3_T_11).FirstOrDefault(),
				Actuals3_T_12 = s.VerniercaliperModel.Select(S => S.Actuals3_T_12).FirstOrDefault(),
				Avg3_4 = s.VerniercaliperModel.Select(S => S.Avg3_4).FirstOrDefault(),
				Measured3_5 = s.VerniercaliperModel.Select(S => S.Measured3_5).FirstOrDefault(),
				Actuals3_T_13 = s.VerniercaliperModel.Select(S => S.Actuals3_T_13).FirstOrDefault(),
				Actuals3_T_14 = s.VerniercaliperModel.Select(S => S.Actuals3_T_14).FirstOrDefault(),
				Actuals3_T_15 = s.VerniercaliperModel.Select(S => S.Actuals3_T_15).FirstOrDefault(),
				Avg3_5 = s.VerniercaliperModel.Select(S => S.Avg3_5).FirstOrDefault(),
				Measured4_1 = s.VerniercaliperModel.Select(S => S.Measured4_1).FirstOrDefault(),
				Actuals4_T_1 = s.VerniercaliperModel.Select(S => S.Actuals4_T_1).FirstOrDefault(),
				Actuals4_T_2 = s.VerniercaliperModel.Select(S => S.Actuals4_T_2).FirstOrDefault(),
				Actuals4_T_3 = s.VerniercaliperModel.Select(S => S.Actuals4_T_3).FirstOrDefault(),
				Avg4_1 = s.VerniercaliperModel.Select(S => S.Avg4_1).FirstOrDefault(),
				Measured4_2 = s.VerniercaliperModel.Select(S => S.Measured4_2).FirstOrDefault(),
				Actuals4_T_4 = s.VerniercaliperModel.Select(S => S.Actuals4_T_4).FirstOrDefault(),
				Actuals4_T_5 = s.VerniercaliperModel.Select(S => S.Actuals4_T_5).FirstOrDefault(),
				Actuals4_T_6 = s.VerniercaliperModel.Select(S => S.Actuals4_T_6).FirstOrDefault(),
				Avg4_2 = s.VerniercaliperModel.Select(S => S.Avg4_2).FirstOrDefault(),
				Measured4_3 = s.VerniercaliperModel.Select(S => S.Measured4_3).FirstOrDefault(),
				Actuals4_T_7 = s.VerniercaliperModel.Select(S => S.Actuals4_T_7).FirstOrDefault(),
				Actuals4_T_8 = s.VerniercaliperModel.Select(S => S.Actuals4_T_8).FirstOrDefault(),
				Actuals4_T_9 = s.VerniercaliperModel.Select(S => S.Actuals4_T_9).FirstOrDefault(),
				Avg4_3 = s.VerniercaliperModel.Select(S => S.Avg4_3).FirstOrDefault(),
				Measured4_4 = s.VerniercaliperModel.Select(S => S.Measured4_4).FirstOrDefault(),
				Actuals4_T_10 = s.VerniercaliperModel.Select(S => S.Actuals4_T_10).FirstOrDefault(),
				Actuals4_T_11 = s.VerniercaliperModel.Select(S => S.Actuals4_T_11).FirstOrDefault(),
				Actuals4_T_12 = s.VerniercaliperModel.Select(S => S.Actuals4_T_12).FirstOrDefault(),
				Avg4_4 = s.VerniercaliperModel.Select(S => S.Avg4_4).FirstOrDefault(),
				Measured4_5 = s.VerniercaliperModel.Select(S => S.Measured4_5).FirstOrDefault(),
				Actuals4_T_13 = s.VerniercaliperModel.Select(S => S.Actuals4_T_13).FirstOrDefault(),
				Actuals4_T_14 = s.VerniercaliperModel.Select(S => S.Actuals4_T_14).FirstOrDefault(),
				Actuals4_T_15 = s.VerniercaliperModel.Select(S => S.Actuals4_T_15).FirstOrDefault(),
				Avg4_5 = s.VerniercaliperModel.Select(S => S.Avg4_5).FirstOrDefault(),
				Measured5_1 = s.VerniercaliperModel.Select(S => S.Measured5_1).FirstOrDefault(),
				Actuals5_T_1 = s.VerniercaliperModel.Select(S => S.Actuals5_T_1).FirstOrDefault(),
				Actuals5_T_2 = s.VerniercaliperModel.Select(S => S.Actuals5_T_2).FirstOrDefault(),
				Actuals5_T_3 = s.VerniercaliperModel.Select(S => S.Actuals5_T_3).FirstOrDefault(),
				Avg5_1 = s.VerniercaliperModel.Select(S => S.Avg5_1).FirstOrDefault(),
				Measured5_2 = s.VerniercaliperModel.Select(S => S.Measured5_2).FirstOrDefault(),
				Actuals5_T_4 = s.VerniercaliperModel.Select(S => S.Actuals5_T_4).FirstOrDefault(),
				Actuals5_T_5 = s.VerniercaliperModel.Select(S => S.Actuals5_T_5).FirstOrDefault(),
				Actuals5_T_6 = s.VerniercaliperModel.Select(S => S.Actuals5_T_6).FirstOrDefault(),
				Avg5_2 = s.VerniercaliperModel.Select(S => S.Avg5_2).FirstOrDefault(),
				Measured5_3 = s.VerniercaliperModel.Select(S => S.Measured5_3).FirstOrDefault(),
				Actuals5_T_7 = s.VerniercaliperModel.Select(S => S.Actuals5_T_7).FirstOrDefault(),
				Actuals5_T_8 = s.VerniercaliperModel.Select(S => S.Actuals5_T_8).FirstOrDefault(),
				Actuals5_T_9 = s.VerniercaliperModel.Select(S => S.Actuals5_T_9).FirstOrDefault(),
				Avg5_3 = s.VerniercaliperModel.Select(S => S.Avg5_3).FirstOrDefault(),
				Measured5_4 = s.VerniercaliperModel.Select(S => S.Measured5_4).FirstOrDefault(),
				Actuals5_T_10 = s.VerniercaliperModel.Select(S => S.Actuals5_T_10).FirstOrDefault(),
				Actuals5_T_11 = s.VerniercaliperModel.Select(S => S.Actuals5_T_11).FirstOrDefault(),
				Actuals5_T_12 = s.VerniercaliperModel.Select(S => S.Actuals5_T_12).FirstOrDefault(),
				Avg5_4 = s.VerniercaliperModel.Select(S => S.Avg5_4).FirstOrDefault(),
				Measured5_5 = s.VerniercaliperModel.Select(S => S.Measured5_5).FirstOrDefault(),
				Actuals5_T_13 = s.VerniercaliperModel.Select(S => S.Actuals5_T_13).FirstOrDefault(),
				Actuals5_T_14 = s.VerniercaliperModel.Select(S => S.Actuals5_T_14).FirstOrDefault(),
				Actuals5_T_15 = s.VerniercaliperModel.Select(S => S.Actuals5_T_15).FirstOrDefault(),
				Avg5_5 = s.VerniercaliperModel.Select(S => S.Avg5_5).FirstOrDefault(),
				Measured6_1 = s.VerniercaliperModel.Select(S => S.Measured6_1).FirstOrDefault(),
				Actuals6_T_1 = s.VerniercaliperModel.Select(S => S.Actuals6_T_1).FirstOrDefault(),
				Actuals6_T_2 = s.VerniercaliperModel.Select(S => S.Actuals6_T_2).FirstOrDefault(),
				Actuals6_T_3 = s.VerniercaliperModel.Select(S => S.Actuals6_T_3).FirstOrDefault(),
				Avg6_1 = s.VerniercaliperModel.Select(S => S.Avg6_1).FirstOrDefault(),
				Measured6_2 = s.VerniercaliperModel.Select(S => S.Measured6_2).FirstOrDefault(),
				Actuals6_T_4 = s.VerniercaliperModel.Select(S => S.Actuals6_T_4).FirstOrDefault(),
				Actuals6_T_5 = s.VerniercaliperModel.Select(S => S.Actuals6_T_5).FirstOrDefault(),
				Actuals6_T_6 = s.VerniercaliperModel.Select(S => S.Actuals6_T_6).FirstOrDefault(),
				Avg6_2 = s.VerniercaliperModel.Select(S => S.Avg6_2).FirstOrDefault(),
				Measured6_3 = s.VerniercaliperModel.Select(S => S.Measured6_3).FirstOrDefault(),
				Actuals6_T_7 = s.VerniercaliperModel.Select(S => S.Actuals6_T_7).FirstOrDefault(),
				Actuals6_T_8 = s.VerniercaliperModel.Select(S => S.Actuals6_T_8).FirstOrDefault(),
				Actuals6_T_9 = s.VerniercaliperModel.Select(S => S.Actuals6_T_9).FirstOrDefault(),
				Avg6_3 = s.VerniercaliperModel.Select(S => S.Avg6_3).FirstOrDefault(),
				Measured6_4 = s.VerniercaliperModel.Select(S => S.Measured6_4).FirstOrDefault(),
				Actuals6_T_10 = s.VerniercaliperModel.Select(S => S.Actuals6_T_10).FirstOrDefault(),
				Actuals6_T_11 = s.VerniercaliperModel.Select(S => S.Actuals6_T_11).FirstOrDefault(),
				Actuals6_T_12 = s.VerniercaliperModel.Select(S => S.Actuals6_T_12).FirstOrDefault(),
				Avg6_4 = s.VerniercaliperModel.Select(S => S.Avg6_4).FirstOrDefault(),
				Measured6_5 = s.VerniercaliperModel.Select(S => S.Measured6_5).FirstOrDefault(),
				Actuals6_T_13 = s.VerniercaliperModel.Select(S => S.Actuals6_T_13).FirstOrDefault(),
				Actuals6_T_14 = s.VerniercaliperModel.Select(S => S.Actuals6_T_14).FirstOrDefault(),
				Actuals6_T_15 = s.VerniercaliperModel.Select(S => S.Actuals6_T_15).FirstOrDefault(),
				Avg6_5 = s.VerniercaliperModel.Select(S => S.Avg6_5).FirstOrDefault(),
				MuLeftValue1 = s.VerniercaliperModel.Select(S => S.MuLeftValue1).FirstOrDefault(),
				MuRightValue1 = s.VerniercaliperModel.Select(S => S.MuRightValue1).FirstOrDefault(),
				MuLeftValue2 = s.VerniercaliperModel.Select(S => S.MuLeftValue2).FirstOrDefault(),
				MuRightValue2 = s.VerniercaliperModel.Select(S => S.MuRightValue2).FirstOrDefault(),
				MuLeftValue3 = s.VerniercaliperModel.Select(S => S.MuLeftValue3).FirstOrDefault(),
				MuRightValue3 = s.VerniercaliperModel.Select(S => S.MuRightValue3).FirstOrDefault(),
				MuLeftValue4 = s.VerniercaliperModel.Select(S => S.MuLeftValue4).FirstOrDefault(),
				MuRightValue4 = s.VerniercaliperModel.Select(S => S.MuRightValue4).FirstOrDefault(),
				MuLeftValue5 = s.VerniercaliperModel.Select(S => S.MuLeftValue5).FirstOrDefault(),
				MuRightValue5 = s.VerniercaliperModel.Select(S => S.MuRightValue5).FirstOrDefault(),
				ExternalParallelismDetails = s.VerniercaliperModel.Select(S => S.ExternalParallelismDetails).FirstOrDefault(),
				InternaljawParallelismDetails = s.VerniercaliperModel.Select(S => S.InternaljawParallelismDetails).FirstOrDefault(),
				EnvironmentCondition = s.VerniercaliperModel.Select(S => S.EnvironmentCondition).SingleOrDefault(),
				Uncertainity = s.VerniercaliperModel.Select(S => S.Uncertainity).SingleOrDefault(),
				CalibrationResult = s.VerniercaliperModel.Select(S => S.CalibrationResult).SingleOrDefault(),
				Remarks = s.VerniercaliperModel.Select(S => S.Remarks).SingleOrDefault(),
			}).SingleOrDefault();

			if (vernierCaliper != null)
			{

				List<string> performedUserData = GetUserName(vernierCaliper.CreatedBy);

				if (performedUserData.Count >= 3)
				{
					vernierCaliper.CalibrationPerformedBy = performedUserData[0];
					vernierCaliper.PerformedBySign = performedUserData[1];
					vernierCaliper.PerformedByDesignation = performedUserData[2];

				}

				List<string> reviewedUserData = GetUserName(vernierCaliper.CalibrationReviewedBy);

				if (reviewedUserData.Count >= 3)
				{
					vernierCaliper.ReviewedBy = reviewedUserData[0];
					vernierCaliper.ReviewedBySign = reviewedUserData[1];
					vernierCaliper.ReviewedByDesignation = reviewedUserData[2];
				}

				int? ulrNumber = vernierCaliper.ULRNumber == null ? 0 : vernierCaliper.ULRNumber;

				int? certificateNumber = vernierCaliper.CertificateNumber == null ? 0 : vernierCaliper.CertificateNumber;
				List<string> formatList = GetULRAndCertificateNumber(ulrNumber, certificateNumber);

				if (formatList.Count >= 2)
				{
					vernierCaliper.ULRFormat = formatList[0];
					vernierCaliper.CertificateFormat = formatList[1];
				}
			}

			return new ResponseViewModel<VernierCaliperViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = vernierCaliper,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("ObservationTemplateService - verniercaliperId Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<VernierCaliperViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "verniercaliperId"
			};
		}
	}
	#endregion

	#region "GeneralNew"
	public ResponseViewModel<GeneralNewViewModel> InsertGeneralnewobs(GeneralNewViewModel GeneralNew)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			int templateObservationId = 0;

			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
																 .GetQueryAsNoTracking(Q => Q.RequestId == GeneralNew.RequestId
																			&& Q.InstrumentId == GeneralNew.InstrumentId)
																 .SingleOrDefault();

			// User labTechnicalManager = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4).SingleOrDefault();
			if (GeneralNew.TemplateObservationId == 0)
			{
				if (observationById == null)
				{
					TemplateObservation templateObservation = new TemplateObservation()
					{
						InstrumentId = GeneralNew.InstrumentId,
						RequestId = GeneralNew.RequestId,
						TempStart = GeneralNew.TempStart,
						TempEnd = GeneralNew.TempEnd,
						Humidity = GeneralNew.Humidity,
						InstrumentCondition = GeneralNew.ConditionOfVernierCaliper,
						RefWi = GeneralNew.RefWi,
						Allvalues = GeneralNew.Allvalues,
						CreatedOn = DateTime.Now,
						CreatedBy = GeneralNew.CreatedBy,
						//  CalibrationReviewedBy = labTechnicalManager.Id,
						CalibrationReviewedDate = DateTime.Now
					};
					_unitOfWork.Repository<TemplateObservation>().Insert(templateObservation);
					_unitOfWork.SaveChanges();
					templateObservationId = templateObservation.Id;
				}
			}
			else
			{
				//TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
				//												 .GetQueryAsNoTracking(Q => Q.RequestId == GeneralNew.RequestId
				//															&& Q.InstrumentId == GeneralNew.InstrumentId)
				//												 .SingleOrDefault();

				if (observationById != null)
				{

					if (GeneralNew.TempStart != null)
					{
						observationById.TempStart = GeneralNew.TempStart;
					}

					if (GeneralNew.TempEnd != null)
					{
						observationById.TempEnd = GeneralNew.TempEnd;
					}

					if (GeneralNew.Humidity != null)
					{
						observationById.Humidity = GeneralNew.Humidity;
					}

					if (GeneralNew.ConditionOfVernierCaliper != null)
					{
						observationById.InstrumentCondition = GeneralNew.ConditionOfVernierCaliper;
					}
					if (GeneralNew.RefWi != null)
					{
						observationById.RefWi = GeneralNew.RefWi;
					}
					if (GeneralNew.Allvalues != null)
					{
						observationById.Allvalues = GeneralNew.Allvalues;
					}

					_unitOfWork.Repository<TemplateObservation>().Update(observationById);

				}
			}
			_unitOfWork.SaveChanges();
			if (GeneralNew.Id == 0)
			{
				if (observationById != null)
				{
					templateObservationId = observationById.Id;
				}
				ObsTemplateGeneralNew obsgeneralNew = new ObsTemplateGeneralNew()
				{
					ObservationId = templateObservationId,
					EnvironmentCondition = GeneralNew.EnvironmentCondition,
					Uncertainity = GeneralNew.Uncertainity,
					CreatedOn = DateTime.Now,
					CreatedBy = GeneralNew.CreatedBy,
					ReviewedDate = GeneralNew.CalibrationPerformedDate.ToString("dd/MM/yyyy"),
					CalibrationReviewedDate = DateTime.Now,
					CalibrationReviewedBy = GeneralNew.CalibrationReviewedBy,
					// CalibrationReviewedDate=GeneralNew.CalibrationReviewedDate,
					CalibrationPerformedBy = GeneralNew.CalibrationPerformedBy,
					ReviewedBy = GeneralNew.ReviewedBy,
					CalibrationPerformedDate = GeneralNew.CalibrationPerformedDate.ToString("dd/MM/yyyy"),
					//ReviewedDate=GeneralNew.Review_Date.ToString("dd/MM/yyyy"),
					CalibrationResult = GeneralNew.CalibrationResult,
					ErrorinDMS1_1 = GeneralNew.ErrorinDMS1_1,
					ErrorinDMS1_2 = GeneralNew.ErrorinDMS1_2,
					ErrorinDMS1_3 = GeneralNew.ErrorinDMS1_3,
					ErrorinDMS1_4 = GeneralNew.ErrorinDMS1_4,
					ErrorinDMS1_5 = GeneralNew.ErrorinDMS1_5,
					ErrorinDMS2_1 = GeneralNew.ErrorinDMS2_1,
					ErrorinDMS2_2 = GeneralNew.ErrorinDMS2_2,
					ErrorinDMS2_3 = GeneralNew.ErrorinDMS2_3,
					ErrorinDMS2_4 = GeneralNew.ErrorinDMS2_4,
					ErrorinDMS3_1 = GeneralNew.ErrorinDMS3_1,
					ErrorinDMS3_2 = GeneralNew.ErrorinDMS3_2,
					ErrorinDMS3_3 = GeneralNew.ErrorinDMS3_3,
					ErrorinDMS3_4 = GeneralNew.ErrorinDMS3_4,
					ErrorinDMS4_1 = GeneralNew.ErrorinDMS4_1,
					ErrorinDMS4_2 = GeneralNew.ErrorinDMS4_2,
					ErrorinDMS4_3 = GeneralNew.ErrorinDMS4_3,
					Straightness_spec = GeneralNew.Straightness_spec,
					Straightness_Actual = GeneralNew.Straightness_Actual,
					Straightness_DevfromNom = GeneralNew.Straightness_DevfromNom,
					Parallelism_Spec = GeneralNew.Parallelism_Spec,
					Parallelism_Actual = GeneralNew.Parallelism_Actual,
					Parallelism_DevfromNom = GeneralNew.Parallelism_DevfromNom,
					FlatnessofBlade_spec_1 = GeneralNew.FlatnessofBlade_spec_1,
					FlatnessofBlade_Actual_1 = GeneralNew.FlatnessofBlade_Actual_1,
					FlatnessofBlade_DevfromNom_1 = GeneralNew.FlatnessofBlade_DevfromNom_1,
					FlatnessofBlade_spec_2 = GeneralNew.FlatnessofBlade_spec_2,
					FlatnessofBlade_Actual_2 = GeneralNew.FlatnessofBlade_Actual_2,
					FlatnessofBlade_DevfromNom_2 = GeneralNew.FlatnessofBlade_DevfromNom_2
				};
				_unitOfWork.Repository<ObsTemplateGeneralNew>().Insert(obsgeneralNew);
			}
			else
			{
				ObsTemplateGeneralNew GeneralNewObsById = _unitOfWork.Repository<ObsTemplateGeneralNew>()
																		  .GetQueryAsNoTracking(Q => Q.Id == GeneralNew.Id)
																		  .SingleOrDefault();

				if (GeneralNew.ErrorinDMS1_1 != null)
					GeneralNewObsById.ErrorinDMS1_1 = GeneralNew.ErrorinDMS1_1;

				if (GeneralNew.ErrorinDMS1_2 != null)
					GeneralNewObsById.ErrorinDMS1_2 = GeneralNew.ErrorinDMS1_2;

				if (GeneralNew.ErrorinDMS1_3 != null)
					GeneralNewObsById.ErrorinDMS1_3 = GeneralNew.ErrorinDMS1_3;

				if (GeneralNew.ErrorinDMS1_4 != null)
					GeneralNewObsById.ErrorinDMS1_4 = GeneralNew.ErrorinDMS1_4;

				if (GeneralNew.ErrorinDMS1_5 != null)
					GeneralNewObsById.ErrorinDMS1_5 = GeneralNew.ErrorinDMS1_5;

				if (GeneralNew.ErrorinDMS2_1 != null)
					GeneralNewObsById.ErrorinDMS2_1 = GeneralNew.ErrorinDMS2_1;

				if (GeneralNew.ErrorinDMS2_2 != null)
					GeneralNewObsById.ErrorinDMS2_2 = GeneralNew.ErrorinDMS2_2;

				if (GeneralNew.ErrorinDMS2_3 != null)
					GeneralNewObsById.ErrorinDMS2_3 = GeneralNew.ErrorinDMS2_3;

				if (GeneralNew.ErrorinDMS2_4 != null)
					GeneralNewObsById.ErrorinDMS2_4 = GeneralNew.ErrorinDMS2_4;

				if (GeneralNew.ErrorinDMS3_1 != null)
					GeneralNewObsById.ErrorinDMS3_1 = GeneralNew.ErrorinDMS3_1;

				if (GeneralNew.ErrorinDMS3_2 != null)
					GeneralNewObsById.ErrorinDMS3_2 = GeneralNew.ErrorinDMS3_2;

				if (GeneralNew.ErrorinDMS3_3 != null)
					GeneralNewObsById.ErrorinDMS3_3 = GeneralNew.ErrorinDMS3_3;

				if (GeneralNew.ErrorinDMS3_4 != null)
					GeneralNewObsById.ErrorinDMS3_4 = GeneralNew.ErrorinDMS3_4;

				if (GeneralNew.ErrorinDMS4_1 != null)
					GeneralNewObsById.ErrorinDMS4_1 = GeneralNew.ErrorinDMS4_1;

				if (GeneralNew.ErrorinDMS4_2 != null)
					GeneralNewObsById.ErrorinDMS4_2 = GeneralNew.ErrorinDMS4_2;

				if (GeneralNew.ErrorinDMS4_3 != null)
					GeneralNewObsById.ErrorinDMS4_3 = GeneralNew.ErrorinDMS4_3;

				if (GeneralNew.Straightness_spec != null)
					GeneralNewObsById.Straightness_spec = GeneralNew.Straightness_spec;

				if (GeneralNew.Straightness_Actual != null)
					GeneralNewObsById.Straightness_Actual = GeneralNew.Straightness_Actual;

				if (GeneralNew.Straightness_DevfromNom != null)
					GeneralNewObsById.Straightness_DevfromNom = GeneralNew.Straightness_DevfromNom;

				if (GeneralNew.Parallelism_Spec != null)
					GeneralNewObsById.Parallelism_Spec = GeneralNew.Parallelism_Spec;

				if (GeneralNew.Parallelism_Actual != null)
					GeneralNewObsById.Parallelism_Actual = GeneralNew.Parallelism_Actual;

				if (GeneralNew.Parallelism_DevfromNom != null)
					GeneralNewObsById.Parallelism_DevfromNom = GeneralNew.Parallelism_DevfromNom;

				if (GeneralNew.FlatnessofBlade_spec_1 != null)
					GeneralNewObsById.FlatnessofBlade_spec_1 = GeneralNew.FlatnessofBlade_spec_1;

				if (GeneralNew.FlatnessofBlade_Actual_1 != null)
					GeneralNewObsById.FlatnessofBlade_Actual_1 = GeneralNew.FlatnessofBlade_Actual_1;

				if (GeneralNew.FlatnessofBlade_DevfromNom_1 != null)
					GeneralNewObsById.FlatnessofBlade_DevfromNom_1 = GeneralNew.FlatnessofBlade_DevfromNom_1;

				if (GeneralNew.FlatnessofBlade_spec_2 != null)
					GeneralNewObsById.FlatnessofBlade_spec_2 = GeneralNew.FlatnessofBlade_spec_2;

				if (GeneralNew.FlatnessofBlade_Actual_2 != null)
					GeneralNewObsById.FlatnessofBlade_Actual_2 = GeneralNew.FlatnessofBlade_Actual_2;

				if (GeneralNew.FlatnessofBlade_DevfromNom_2 != null)
					GeneralNewObsById.FlatnessofBlade_DevfromNom_2 = GeneralNew.FlatnessofBlade_DevfromNom_2;

				_unitOfWork.Repository<ObsTemplateGeneralNew>().Update(GeneralNewObsById);
			}
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			return new ResponseViewModel<GeneralNewViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - InsertGeneralnewobs Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<GeneralNewViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = GeneralNew,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "InsertGeneralnewobs"
			};
		}
	}

	#endregion

	#region  GeneralNew

	public ResponseViewModel<GeneralNewViewModel> GetGeneralNewById(int requestId, int instrumentId)
	{
		try
		{
			GeneralNewViewModel generalnew = _unitOfWork.Repository<TemplateObservation>()
			.GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
			.Include(I => I.GeneralNewModel)
			.Select(s => new GeneralNewViewModel()
			{
				TemplateObservationId = s.Id,
				TempStart = s.TempStart,
				TempEnd = s.TempEnd,
				Humidity = s.Humidity,
				RefWi = s.RefWi,
				Allvalues = s.Allvalues,
				ConditionOfVernierCaliper = s.InstrumentCondition,
				CreatedBy = s.CreatedBy,
				CalibrationReviewedBy = s.CalibrationReviewedBy,
				ULRNumber = s.ULRNumber,
				CertificateNumber = s.CertificateNumber,
				CalibrationPerformedDate = s.CreatedOn,
				CalibrationReviewedDate = s.CalibrationReviewedDate,
				ReviewStatus = s.ReviewStatus,
				Id = s.GeneralNewModel.Select(s => s.Id).FirstOrDefault(),
				ErrorinDMS1_1 = s.GeneralNewModel.Select(S => S.ErrorinDMS1_1).FirstOrDefault(),
				ErrorinDMS1_2 = s.GeneralNewModel.Select(S => S.ErrorinDMS1_2).FirstOrDefault(),
				ErrorinDMS1_3 = s.GeneralNewModel.Select(S => S.ErrorinDMS1_3).FirstOrDefault(),
				ErrorinDMS1_4 = s.GeneralNewModel.Select(S => S.ErrorinDMS1_4).FirstOrDefault(),
				ErrorinDMS1_5 = s.GeneralNewModel.Select(S => S.ErrorinDMS1_5).FirstOrDefault(),
				ErrorinDMS2_1 = s.GeneralNewModel.Select(S => S.ErrorinDMS2_1).FirstOrDefault(),
				ErrorinDMS2_2 = s.GeneralNewModel.Select(S => S.ErrorinDMS2_2).FirstOrDefault(),
				ErrorinDMS2_3 = s.GeneralNewModel.Select(S => S.ErrorinDMS2_3).FirstOrDefault(),
				ErrorinDMS2_4 = s.GeneralNewModel.Select(S => S.ErrorinDMS2_4).FirstOrDefault(),
				ErrorinDMS3_1 = s.GeneralNewModel.Select(S => S.ErrorinDMS3_1).FirstOrDefault(),
				ErrorinDMS3_2 = s.GeneralNewModel.Select(S => S.ErrorinDMS3_2).FirstOrDefault(),
				ErrorinDMS3_3 = s.GeneralNewModel.Select(S => S.ErrorinDMS3_3).FirstOrDefault(),
				ErrorinDMS3_4 = s.GeneralNewModel.Select(S => S.ErrorinDMS3_4).FirstOrDefault(),
				ErrorinDMS4_1 = s.GeneralNewModel.Select(S => S.ErrorinDMS4_1).FirstOrDefault(),
				ErrorinDMS4_2 = s.GeneralNewModel.Select(S => S.ErrorinDMS4_2).FirstOrDefault(),
				ErrorinDMS4_3 = s.GeneralNewModel.Select(S => S.ErrorinDMS4_3).FirstOrDefault(),
				Straightness_spec = s.GeneralNewModel.Select(S => S.Straightness_spec).FirstOrDefault(),
				Straightness_Actual = s.GeneralNewModel.Select(S => S.Straightness_Actual).FirstOrDefault(),
				Straightness_DevfromNom = s.GeneralNewModel.Select(S => S.Straightness_DevfromNom).FirstOrDefault(),
				Parallelism_Spec = s.GeneralNewModel.Select(S => S.Parallelism_Spec).FirstOrDefault(),
				Parallelism_Actual = s.GeneralNewModel.Select(S => S.Parallelism_Actual).FirstOrDefault(),
				Parallelism_DevfromNom = s.GeneralNewModel.Select(S => S.Parallelism_DevfromNom).FirstOrDefault(),
				FlatnessofBlade_spec_1 = s.GeneralNewModel.Select(S => S.FlatnessofBlade_spec_1).FirstOrDefault(),
				FlatnessofBlade_Actual_1 = s.GeneralNewModel.Select(S => S.FlatnessofBlade_Actual_1).FirstOrDefault(),
				FlatnessofBlade_DevfromNom_1 = s.GeneralNewModel.Select(S => S.FlatnessofBlade_DevfromNom_1).FirstOrDefault(),
				FlatnessofBlade_spec_2 = s.GeneralNewModel.Select(S => S.FlatnessofBlade_spec_2).FirstOrDefault(),
				FlatnessofBlade_Actual_2 = s.GeneralNewModel.Select(S => S.FlatnessofBlade_Actual_2).FirstOrDefault(),
				FlatnessofBlade_DevfromNom_2 = s.GeneralNewModel.Select(S => S.FlatnessofBlade_DevfromNom_2).FirstOrDefault(),
				EnvironmentCondition = s.GeneralNewModel.Select(S => S.EnvironmentCondition).SingleOrDefault(),
				Uncertainity = s.GeneralNewModel.Select(S => S.Uncertainity).SingleOrDefault(),
				CalibrationResult = s.GeneralNewModel.Select(S => S.CalibrationResult).SingleOrDefault(),
				Remarks = s.GeneralNewModel.Select(S => S.Remarks).SingleOrDefault()
			}).SingleOrDefault();

			if (generalnew != null)
			{

				List<string> performedUserData = GetUserName(generalnew.CreatedBy);

				if (performedUserData.Count >= 3)
				{
					generalnew.CalibrationPerformedBy = performedUserData[0];
					generalnew.PerformedBySign = performedUserData[1];
					generalnew.PerformedByDesignation = performedUserData[2];

				}

				List<string> reviewedUserData = GetUserName(generalnew.CalibrationReviewedBy);

				if (reviewedUserData.Count >= 3)
				{
					generalnew.ReviewedBy = reviewedUserData[0];
					generalnew.ReviewedBySign = reviewedUserData[1];
					generalnew.ReviewedByDesignation = reviewedUserData[2];
				}

				int? ulrNumber = generalnew.ULRNumber == null ? 0 : generalnew.ULRNumber;

				int? certificateNumber = generalnew.CertificateNumber == null ? 0 : generalnew.CertificateNumber;
				List<string> formatList = GetULRAndCertificateNumber(ulrNumber, certificateNumber);

				if (formatList.Count >= 2)
				{
					generalnew.ULRFormat = formatList[0];
					generalnew.CertificateFormat = formatList[1];
				}
			}

			return new ResponseViewModel<GeneralNewViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = generalnew,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("ObservationTemplateService - GetGeneralNewById Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<GeneralNewViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "GetGeneralNewById"
			};
		}
	}
	#endregion

	#region "General"
	public ResponseViewModel<GeneralViewModel> InsertGeneral(GeneralViewModel general)
	{

		try
		{
			_unitOfWork.BeginTransaction();
			int generalId = 0;
			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
																 .GetQueryAsNoTracking(Q => Q.RequestId == general.RequestId
																						&& Q.InstrumentId == general.InstrumentId)
																 .SingleOrDefault();
			// User labTechnicalManager = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4).SingleOrDefault();
			if ((general.TemplateObservationId == 0) && (observationById == null))

			{
				//if (observationById == null)
				//{
				TemplateObservation templateObservation = new TemplateObservation()
				{
					InstrumentId = general.InstrumentId,
					RequestId = general.RequestId,
					TempStart = general.TempStart,
					TempEnd = general.TempEnd,
					Humidity = general.Humidity,
					RefWi = general.RefWi,
					InstrumentCondition = general.DialIndicatiorCondition,
					Allvalues = general.Allvalues,
					CreatedBy = general.CreatedBy,
					CreatedOn = DateTime.Now,
					// CalibrationReviewedBy = labTechnicalManager.Id,
					CalibrationReviewedDate = DateTime.Now
				};
				_unitOfWork.Repository<TemplateObservation>().Insert(templateObservation);
				_unitOfWork.SaveChanges();
				generalId = templateObservation.Id;
				//}
				ObsTemplateGeneral obsTemplateGeneral = new ObsTemplateGeneral()
				{
					ObservationId = generalId,
					CreatedBy = general.CreatedBy,
					CreatedOn = DateTime.Now,
					CalibrationReviewedDate = DateTime.Now
				};
				_unitOfWork.Repository<ObsTemplateGeneral>().Insert(obsTemplateGeneral);
				_unitOfWork.SaveChanges();
				//}
			}
			else
			{
				//TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
				//												 .GetQueryAsNoTracking(Q => Q.RequestId == general.RequestId
				//																		&& Q.InstrumentId == general.InstrumentId)
				//												 .SingleOrDefault();

				if (observationById != null)
				{

					if (general.TempStart != null)
					{
						observationById.TempStart = general.TempStart;
					}

					if (general.TempEnd != null)
					{
						observationById.TempEnd = general.TempEnd;
					}

					if (general.Humidity != null)
					{
						observationById.Humidity = general.Humidity;
					}

					if (general.DialIndicatiorCondition != null)
					{
						observationById.InstrumentCondition = general.DialIndicatiorCondition;
					}
					if (general.RefWi != null)
					{
						observationById.RefWi = general.RefWi;
					}
					if (general.Allvalues != null)
					{
						observationById.Allvalues = general.Allvalues;
					}
					//generalId = observationById.Id;
					_unitOfWork.Repository<TemplateObservation>().Update(observationById);
				}
			}
			_unitOfWork.SaveChanges();
			if (observationById != null)
			{

				generalId = observationById.Id;
			}
			general.GeneralAddResultViewModelList.ForEach(x => x.ParentId = generalId);
			var detailData = _mapper.Map<ObsGeneralMeasuredValues[]>(general.GeneralAddResultViewModelList
									.Where(x => x.Id > 0).ToList());
			if (detailData.Any())
			{
				foreach (var updateData in detailData)
				{
					_unitOfWork.Repository<ObsGeneralMeasuredValues>().Update(updateData);
					_unitOfWork.SaveChanges();
				}
			}

			detailData = _mapper.Map<ObsGeneralMeasuredValues[]>(general.GeneralAddResultViewModelList
									.Where(x => x.Id == null).ToList());
			if (detailData.Any())
			{
				_unitOfWork.Repository<ObsGeneralMeasuredValues>().InsertRange(detailData);
				_unitOfWork.SaveChanges();
			}

			general.GeneralManualAddResultViewModelList.ForEach(x => x.ParentId = generalId);

			var dynamicData = _mapper.Map<ObsGeneralDynamicValues[]>(general.GeneralManualAddResultViewModelList
									 .Where(x => x.Id > 0).ToList());
			if (dynamicData.Any())
			{
				foreach (var updateData in dynamicData)
				{
					_unitOfWork.Repository<ObsGeneralDynamicValues>().Update(updateData);
					_unitOfWork.SaveChanges();
				}
			}

			dynamicData = _mapper.Map<ObsGeneralDynamicValues[]>(general.GeneralManualAddResultViewModelList
								 .Where(x => x.Id == null).ToList());
			if (dynamicData.Any())
			{
				_unitOfWork.Repository<ObsGeneralDynamicValues>().InsertRange(dynamicData);
				_unitOfWork.SaveChanges();
			}

			_unitOfWork.Commit();
			return new ResponseViewModel<GeneralViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - InsertGeneral Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<GeneralViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = general,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "InsertGeneral"
			};
		}
	}

	public ResponseViewModel<GeneralViewModel> GetGeneralById(int requestId, int instrumentId)
	{
		try
		{
			GeneralViewModel generalViewModel = _unitOfWork.Repository<TemplateObservation>()
			.GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
			.Include(I => I.GeneralModel)
			.Select(s => new GeneralViewModel()
			{
				Id = s.Id,
				TempStart = s.TempStart,
				TempEnd = s.TempEnd,
				Humidity = s.Humidity,
				RefWi = s.RefWi,
				Allvalues = s.Allvalues,
				CalibrationPerformedDate = s.CreatedOn,
				CreatedBy = s.CreatedBy,
				CalibrationReviewedBy = s.CalibrationReviewedBy,
				CalibrationReviewedDate = s.CalibrationReviewedDate,
				DialIndicatiorCondition = s.InstrumentCondition,
				ReviewStatus = s.ReviewStatus,
				ULRNumber = s.ULRNumber,
				CertificateNumber = s.CertificateNumber,
				Uncertainity = s.GeneralModel.Select(x => x.Uncertainity).SingleOrDefault(),
				CalibrationResult = s.GeneralModel.Select(x => x.CalibrationResult).SingleOrDefault(),
				Remarks = s.GeneralModel.Select(x => x.Remarks).SingleOrDefault()
			}).SingleOrDefault();

			if (generalViewModel == null)
			{
				return new ResponseViewModel<GeneralViewModel>
				{
					ResponseCode = 200,
					ResponseMessage = "No records found",
					ResponseData = null,
					ResponseDataList = null
				};
			}
			else
			{

				List<string> performedUserData = GetUserName(generalViewModel.CreatedBy);

				if (performedUserData.Count >= 3)
				{
					generalViewModel.CalibrationPerformedBy = performedUserData[0];
					generalViewModel.PerformedBySign = performedUserData[1];
					generalViewModel.PerformedByDesignation = performedUserData[2];
				}

				List<string> reviewedUserData = GetUserName(generalViewModel.CalibrationReviewedBy);

				if (reviewedUserData.Count >= 3)
				{
					generalViewModel.ReviewedBy = reviewedUserData[0];
					generalViewModel.ReviewedBySign = reviewedUserData[1];
					generalViewModel.ReviewedByDesignation = reviewedUserData[2];
				}

				int? ulrNumber = generalViewModel.ULRNumber == null ? 0 : generalViewModel.ULRNumber;
				int? certificateNumber = generalViewModel.CertificateNumber == null ? 0 : generalViewModel.CertificateNumber;
				List<string> formatList = GetULRAndCertificateNumber(ulrNumber, certificateNumber);

				if (formatList.Count >= 2)
				{
					generalViewModel.ULRFormat = formatList[0];
					generalViewModel.CertificateFormat = formatList[1];
				}
			}

			var parentVM = _mapper.Map<GeneralViewModel>(generalViewModel);

			var childData = _unitOfWork.Repository<ObsGeneralMeasuredValues>()
										.GetQueryAsNoTracking(x => x.ParentId == parentVM.Id);
			var childListVM = _mapper.Map<List<GeneralResultViewModel>>(childData);

			generalViewModel.GeneralAddResultViewModelList = childListVM;



			var parentVM1 = _mapper.Map<GeneralViewModel>(generalViewModel);

			var childData1 = _unitOfWork.Repository<ObsGeneralDynamicValues>()
										.GetQueryAsNoTracking(x => x.ParentId == parentVM.Id);
			var childListVM1 = _mapper.Map<List<GeneralManualResultViewModel>>(childData1);

			generalViewModel.GeneralManualAddResultViewModelList = childListVM1;

			return new ResponseViewModel<GeneralViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = generalViewModel,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("ObservationTemplateService - generalId Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<GeneralViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "generalId"
			};
		}
	}

	#endregion


	public ResponseViewModel<LeverTypeDialViewModel> SaveLeverDialCertificate(int requestId, int instrumentId, string EnvironmentCondition,
																				string Uncertainity, string CalibrationResult,
																				string Remarks, int userId, string exportData)
	{
		try
		{

			_unitOfWork.BeginTransaction();



			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
																 .GetQueryAsNoTracking(Q => Q.RequestId == requestId
																						&& Q.InstrumentId == instrumentId)
																 .SingleOrDefault();


			ObsTemplateLeverTypeDial leverTypeDialById = _unitOfWork.Repository<ObsTemplateLeverTypeDial>()
																	.GetQueryAsNoTracking(Q => Q.ObservationId == observationById.Id)
																	.SingleOrDefault();
			leverTypeDialById.EnvironmentCondition = EnvironmentCondition;
			leverTypeDialById.Uncertainity = Uncertainity;
			leverTypeDialById.CalibrationResult = CalibrationResult;
			leverTypeDialById.Remarks = Remarks;
			_unitOfWork.Repository<ObsTemplateLeverTypeDial>().Update(leverTypeDialById);
			_unitOfWork.SaveChanges();


			ResponseViewModel<QRCodeFilesViewModel> qrCodeResponseData = InsertQRCodeFiles(requestId, instrumentId, userId, exportData);

			if (qrCodeResponseData.ResponseCode == 500)
			{
				_unitOfWork.RollBack();
				return new ResponseViewModel<LeverTypeDialViewModel>
				{
					ResponseCode = 500,
					ResponseMessage = qrCodeResponseData.ResponseMessage,
					ErrorMessage = qrCodeResponseData.ErrorMessage,
					ResponseData = null,
					ResponseDataList = null,
					ResponseService = qrCodeResponseData.ResponseService,
					ResponseServiceMethod = qrCodeResponseData.ResponseServiceMethod
				};
			}

			_unitOfWork.Commit();
			return new ResponseViewModel<LeverTypeDialViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - SaveLeverDialCertificate Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<LeverTypeDialViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "SaveLeverDialCertificate"
			};
		}
	}

	public ResponseViewModel<MicrometerViewModel> SaveMicrometerCertificate(int requestId, int instrumentId, string EnvironmentCondition,
																				string Uncertainity, string CalibrationResult,
																				string Remarks, int userId, string exportData)
	{
		try
		{
			_unitOfWork.BeginTransaction();

			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
															 .GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
															 .SingleOrDefault();

			ObsTemplateMicrometer MicrometerById = _unitOfWork.Repository<ObsTemplateMicrometer>()
																	.GetQueryAsNoTracking(Q => Q.ObservationId == observationById.Id)
																	.SingleOrDefault();
			MicrometerById.EnvironmentCondition = EnvironmentCondition;
			MicrometerById.Uncertainity = Uncertainity;
			MicrometerById.CalibrationResult = CalibrationResult;
			MicrometerById.Remarks = Remarks;
			_unitOfWork.Repository<ObsTemplateMicrometer>().Update(MicrometerById);
			_unitOfWork.SaveChanges();

			ResponseViewModel<QRCodeFilesViewModel> qrCodeResponseData = InsertQRCodeFiles(requestId, instrumentId, userId, exportData);

			if (qrCodeResponseData.ResponseCode == 500)
			{
				_unitOfWork.RollBack();
				return new ResponseViewModel<MicrometerViewModel>
				{
					ResponseCode = 500,
					ResponseMessage = qrCodeResponseData.ResponseMessage,
					ErrorMessage = qrCodeResponseData.ErrorMessage,
					ResponseData = null,
					ResponseDataList = null,
					ResponseService = qrCodeResponseData.ResponseService,
					ResponseServiceMethod = qrCodeResponseData.ResponseServiceMethod
				};
			}

			_unitOfWork.Commit();
			return new ResponseViewModel<MicrometerViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - SaveMicrometerCertificate Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MicrometerViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "SaveMicrometerCertificate"
			};
		}
	}

	public ResponseViewModel<PlungerDialViewModel> SavePlungerDialCertificate(int requestId, int instrumentId, string EnvironmentCondition,
																				string Uncertainity, string CalibrationResult,
																				string Remarks, int userId, string exportData)
	{
		try
		{
			_unitOfWork.BeginTransaction();

			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
															 .GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
															 .SingleOrDefault();

			ObsTemplatePlungerDial MicrometerById = _unitOfWork.Repository<ObsTemplatePlungerDial>()
																	.GetQueryAsNoTracking(Q => Q.ObservationId == observationById.Id)
																	.SingleOrDefault();
			MicrometerById.EnvironmentCondition = EnvironmentCondition;
			MicrometerById.Uncertainity = Uncertainity;
			MicrometerById.CalibrationResult = CalibrationResult;
			MicrometerById.Remarks = Remarks;
			_unitOfWork.Repository<ObsTemplatePlungerDial>().Update(MicrometerById);
			_unitOfWork.SaveChanges();

			ResponseViewModel<QRCodeFilesViewModel> qrCodeResponseData = InsertQRCodeFiles(requestId, instrumentId, userId, exportData);

			if (qrCodeResponseData.ResponseCode == 500)
			{
				_unitOfWork.RollBack();
				return new ResponseViewModel<PlungerDialViewModel>
				{
					ResponseCode = 500,
					ResponseMessage = qrCodeResponseData.ResponseMessage,
					ErrorMessage = qrCodeResponseData.ErrorMessage,
					ResponseData = null,
					ResponseDataList = null,
					ResponseService = qrCodeResponseData.ResponseService,
					ResponseServiceMethod = qrCodeResponseData.ResponseServiceMethod
				};
			}

			_unitOfWork.Commit();
			return new ResponseViewModel<PlungerDialViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - SavePlungerDialCertificate Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<PlungerDialViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "SavePlungerDialCertificate"
			};
		}
	}

	public ResponseViewModel<ThreadGaugesViewModel> SaveThreadGaugesCertificate(int requestId, int instrumentId, string EnvironmentCondition,
																				string Uncertainity, string CalibrationResult,
																				string Remarks, int userId, string exportData)
	{
		try
		{

			_unitOfWork.BeginTransaction();

			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
															 .GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
															 .SingleOrDefault();
			ObsTemplateThreadGauges MicrometerById = _unitOfWork.Repository<ObsTemplateThreadGauges>()
																	.GetQueryAsNoTracking(Q => Q.ObservationId == observationById.Id)
																	.SingleOrDefault();
			MicrometerById.EnvironmentCondition = EnvironmentCondition;
			MicrometerById.Uncertainity = Uncertainity;
			MicrometerById.CalibrationResult = CalibrationResult;
			MicrometerById.Remarks = Remarks;
			_unitOfWork.Repository<ObsTemplateThreadGauges>().Update(MicrometerById);
			_unitOfWork.SaveChanges();

			ResponseViewModel<QRCodeFilesViewModel> qrCodeResponseData = InsertQRCodeFiles(requestId, instrumentId, userId, exportData);

			if (qrCodeResponseData.ResponseCode == 500)
			{
				_unitOfWork.RollBack();
				return new ResponseViewModel<ThreadGaugesViewModel>
				{
					ResponseCode = 500,
					ResponseMessage = qrCodeResponseData.ResponseMessage,
					ErrorMessage = qrCodeResponseData.ErrorMessage,
					ResponseData = null,
					ResponseDataList = null,
					ResponseService = qrCodeResponseData.ResponseService,
					ResponseServiceMethod = qrCodeResponseData.ResponseServiceMethod
				};
			}

			_unitOfWork.Commit();
			return new ResponseViewModel<ThreadGaugesViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - SaveThreadGaugesCertificate Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ThreadGaugesViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "SaveThreadGaugesCertificate"
			};
		}
	}

	public ResponseViewModel<TorqueWrenchesViewModel> SaveTorqueWrenchesCertificate(int requestId, int instrumentId, string EnvironmentCondition,
																				string Uncertainity, string CalibrationResult,
																				string Remarks, int userId, string exportData)
	{
		try
		{

			_unitOfWork.BeginTransaction();
			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
															 .GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
															 .SingleOrDefault();
			ObsTemplateTWobs MicrometerById = _unitOfWork.Repository<ObsTemplateTWobs>()
																	.GetQueryAsNoTracking(Q => Q.ObservationId == observationById.Id)
																	.SingleOrDefault();
			MicrometerById.EnvironmentCondition = EnvironmentCondition;
			MicrometerById.Uncertainity = Uncertainity;
			MicrometerById.CalibrationResult = CalibrationResult;
			MicrometerById.Remarks = Remarks;
			_unitOfWork.Repository<ObsTemplateTWobs>().Update(MicrometerById);
			_unitOfWork.SaveChanges();



			ResponseViewModel<QRCodeFilesViewModel> qrCodeResponseData = InsertQRCodeFiles(requestId, instrumentId, userId, exportData);

			if (qrCodeResponseData.ResponseCode == 500)
			{
				_unitOfWork.RollBack();
				return new ResponseViewModel<TorqueWrenchesViewModel>
				{
					ResponseCode = 500,
					ResponseMessage = qrCodeResponseData.ResponseMessage,
					ErrorMessage = qrCodeResponseData.ErrorMessage,
					ResponseData = null,
					ResponseDataList = null,
					ResponseService = qrCodeResponseData.ResponseService,
					ResponseServiceMethod = qrCodeResponseData.ResponseServiceMethod
				};
			}

			_unitOfWork.Commit();
			return new ResponseViewModel<TorqueWrenchesViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - SaveTorqueWrenchesCertificate Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<TorqueWrenchesViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "SaveTorqueWrenchesCertificate"
			};
		}
	}
	public ResponseViewModel<VernierCaliperViewModel> SaveVernierCaliperCertificate(int requestId, int instrumentId, string EnvironmentCondition,
																				string Uncertainity, string CalibrationResult,
																				string Remarks, int userId, string exportData)
	{
		try
		{
			_unitOfWork.BeginTransaction();

			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
															 .GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
															 .SingleOrDefault();

			ObsTemplateVernierCaliper MicrometerById = _unitOfWork.Repository<ObsTemplateVernierCaliper>()
																	.GetQueryAsNoTracking(Q => Q.ObservationId == observationById.Id)
																	.SingleOrDefault();

			MicrometerById.EnvironmentCondition = EnvironmentCondition;
			MicrometerById.Uncertainity = Uncertainity;
			MicrometerById.CalibrationResult = CalibrationResult;
			MicrometerById.Remarks = Remarks;
			_unitOfWork.Repository<ObsTemplateVernierCaliper>().Update(MicrometerById);
			_unitOfWork.SaveChanges();

			ResponseViewModel<QRCodeFilesViewModel> qrCodeResponseData = InsertQRCodeFiles(requestId, instrumentId, userId, exportData);

			if (qrCodeResponseData.ResponseCode == 500)
			{
				_unitOfWork.RollBack();
				return new ResponseViewModel<VernierCaliperViewModel>
				{
					ResponseCode = 500,
					ResponseMessage = qrCodeResponseData.ResponseMessage,
					ErrorMessage = qrCodeResponseData.ErrorMessage,
					ResponseData = null,
					ResponseDataList = null,
					ResponseService = qrCodeResponseData.ResponseService,
					ResponseServiceMethod = qrCodeResponseData.ResponseServiceMethod
				};
			}

			_unitOfWork.Commit();
			return new ResponseViewModel<VernierCaliperViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - SaveVernierCaliperCertificate Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<VernierCaliperViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "SaveVernierCaliperCertificate"
			};
		}
	}

	public ResponseViewModel<GeneralNewViewModel> SaveGeneralNewCertificate(int requestId, int instrumentId, string EnvironmentCondition,
																				string Uncertainity, string CalibrationResult,
																				string Remarks, int userId, string exportData)
	{

		try
		{
			_unitOfWork.BeginTransaction();

			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
															 .GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
															 .SingleOrDefault();

			ObsTemplateGeneralNew MicrometerById = _unitOfWork.Repository<ObsTemplateGeneralNew>()
																	.GetQueryAsNoTracking(Q => Q.ObservationId == observationById.Id)
																	.SingleOrDefault();

			MicrometerById.EnvironmentCondition = EnvironmentCondition;
			MicrometerById.Uncertainity = Uncertainity;
			MicrometerById.CalibrationResult = CalibrationResult;
			MicrometerById.Remarks = Remarks;
			_unitOfWork.Repository<ObsTemplateGeneralNew>().Update(MicrometerById);
			_unitOfWork.SaveChanges();

			ResponseViewModel<QRCodeFilesViewModel> qrCodeResponseData = InsertQRCodeFiles(requestId, instrumentId, userId, exportData);
			if (qrCodeResponseData.ResponseCode == 500)
			{
				_unitOfWork.RollBack();
				return new ResponseViewModel<GeneralNewViewModel>
				{
					ResponseCode = 500,
					ResponseMessage = qrCodeResponseData.ResponseMessage,
					ErrorMessage = qrCodeResponseData.ErrorMessage,
					ResponseData = null,
					ResponseDataList = null,
					ResponseService = qrCodeResponseData.ResponseService,
					ResponseServiceMethod = qrCodeResponseData.ResponseServiceMethod
				};
			}

			_unitOfWork.Commit();
			return new ResponseViewModel<GeneralNewViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};



		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - SaveGeneralNewCertificate Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<GeneralNewViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "SaveGeneralNewCertificate"
			};
		}


	}

	public ResponseViewModel<GeneralViewModel> SaveGeneralCertificate(int requestId, int instrumentId, string EnvironmentCondition,
																				string Uncertainity, string CalibrationResult,
																				string Remarks, int userId, string exportData)
	{
		try
		{
			_unitOfWork.BeginTransaction();

			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
															 .GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
															 .SingleOrDefault();

			ObsTemplateGeneral MicrometerById = _unitOfWork.Repository<ObsTemplateGeneral>()
																	.GetQueryAsNoTracking(Q => Q.ObservationId == observationById.Id)
																	.SingleOrDefault();

			MicrometerById.EnvironmentCondition = EnvironmentCondition;
			MicrometerById.Uncertainity = Uncertainity;
			MicrometerById.CalibrationResult = CalibrationResult;
			MicrometerById.Remarks = Remarks;
			_unitOfWork.Repository<ObsTemplateGeneral>().Update(MicrometerById);
			_unitOfWork.SaveChanges();

			ResponseViewModel<QRCodeFilesViewModel> qrCodeResponseData = InsertQRCodeFiles(requestId, instrumentId, userId, exportData);

			if (qrCodeResponseData.ResponseCode == 500)
			{
				_unitOfWork.RollBack();
				return new ResponseViewModel<GeneralViewModel>
				{
					ResponseCode = 500,
					ResponseMessage = qrCodeResponseData.ResponseMessage,
					ErrorMessage = qrCodeResponseData.ErrorMessage,
					ResponseData = null,
					ResponseDataList = null,
					ResponseService = qrCodeResponseData.ResponseService,
					ResponseServiceMethod = qrCodeResponseData.ResponseServiceMethod
				};
			}

			_unitOfWork.Commit();
			return new ResponseViewModel<GeneralViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - SaveGeneralCertificate Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<GeneralViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "SaveGeneralCertificate"
			};
		}
	}


	public ResponseViewModel<LeverTypeDialViewModel> SubmitReview(int observationId, DateTime reviewDate, int reviewStatus, int reviewedBy,string Remarks,int RequestId, DateTime DueDate)
	{
		try
		{
			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
															 .GetQueryAsNoTracking(Q => Q.Id == observationId)
															 .SingleOrDefault();

			Instrument instrumentData = _unitOfWork.Repository<Instrument>()
																	.GetQueryAsNoTracking(Q => Q.Id == observationById.InstrumentId)
																	.SingleOrDefault();

			Request ReqstData = _unitOfWork.Repository<Request>()
																	.GetQueryAsNoTracking(Q => Q.Id == RequestId)
																	.SingleOrDefault();

			EmailServiceStatus emailService = _unitOfWork.Repository<EmailServiceStatus>()
																		.GetQueryAsNoTracking(Q => Q.RequestId ==RequestId)
																		.SingleOrDefault();

			var numberList = GenerateULRAndCertificateNumber(instrumentData.IsNABL);


			_unitOfWork.BeginTransaction();
			observationById.CalibrationReviewedDate = reviewDate;
            if (instrumentData.TypeOfEquipment == "External" || instrumentData.TypeOfEquipment == "外部の")
            {
                observationById.ExternalObsStatus = reviewStatus;
            }
            else
            {
                observationById.ReviewStatus = reviewStatus;
            }
            //observationById.ReviewStatus = reviewStatus;
			observationById.ULRNumber = numberList[0];
			observationById.CertificateNumber = numberList[1];
			observationById.CalibrationReviewedBy = reviewedBy;
            if (reviewStatus == 2)
            {
				observationById.CalibrationResult = "Not Ok";
                observationById.Remarks = Remarks;
            }
            if (reviewStatus == 1)
            {
				observationById.CalibrationResult = "Ok";
                observationById.Remarks = "OK";
            }
            _unitOfWork.Repository<TemplateObservation>().Update(observationById);
			_unitOfWork.SaveChanges();

			RequestStatus reqestStatus = new RequestStatus();
			reqestStatus.RequestId = RequestId;
			if (instrumentData.TypeOfEquipment == "External" || instrumentData.TypeOfEquipment == "外部の")
			{
				//reqestStatus.StatusId = reviewStatus == 1 ? (Int32)EnumRequestStatus.Sent : (Int32)EnumRequestStatus.Rejected;
                if ((instrumentData.IdNo != "") && ((ReqstData.TypeOfReqest == 2 || ReqstData.TypeOfReqest == 3)))
                {

                    reqestStatus.StatusId = reviewStatus == 1 ? (Int32)EnumRequestStatus.Closed : (Int32)EnumRequestStatus.CalibrationReject;

                }
                else
                {
                    reqestStatus.StatusId = reviewStatus == 1 ? (Int32)EnumRequestStatus.Sent : (Int32)EnumRequestStatus.CalibrationReject;
                }
            }
			else
			{
				if((instrumentData.IdNo != "") && ((ReqstData.TypeOfReqest == 2 || ReqstData.TypeOfReqest == 3)))
				{
					
						reqestStatus.StatusId = reviewStatus == 1 ? (Int32)EnumRequestStatus.Closed : (Int32)EnumRequestStatus.Rejected;
					
				}
				else
				{
					reqestStatus.StatusId = reviewStatus == 1 ? (Int32)EnumRequestStatus.Sent : (Int32)EnumRequestStatus.CalibrationReject;
				}
			}

			reqestStatus.CreatedOn = DateTime.Now;
			reqestStatus.CreatedBy = reviewedBy;
			if (reviewStatus == 1)
			{
                reqestStatus.Comment = "Pass";
            }
			else
			{
                reqestStatus.Comment = Remarks;
            }			
			_unitOfWork.Repository<RequestStatus>().Insert(reqestStatus);
			_unitOfWork.SaveChanges();

			//----------------------New update for listing Approved Request start---------------------------
			//Request Tempreqests = new Request();
			ReqstData.Id = RequestId;
			if (instrumentData.TypeOfEquipment == "External" || instrumentData.TypeOfEquipment == "外部の")
			{
                //ReqstData.StatusId = reviewStatus == 1 ? (Int32)EnumRequestStatus.Closed : (Int32)EnumRequestStatus.Rejected;
                if ((instrumentData.IdNo != "") && ((ReqstData.TypeOfReqest == 2 || ReqstData.TypeOfReqest == 3)))
                {

                    ReqstData.StatusId = reviewStatus == 1 ? (Int32)EnumRequestStatus.Closed : (Int32)EnumRequestStatus.CalibrationReject;
					ReqstData.ReqDueDate = DueDate;

                }
                else
                {
                    ReqstData.StatusId = reviewStatus == 1 ? (Int32)EnumRequestStatus.Sent : (Int32)EnumRequestStatus.CalibrationReject;
                }
            }
			else
			{

				if ((instrumentData.IdNo != null) && (ReqstData.TypeOfReqest == 2 || ReqstData.TypeOfReqest == 3))
				{
						
					ReqstData.StatusId = reviewStatus == 1 ? (Int32)EnumRequestStatus.Closed : (Int32)EnumRequestStatus.CalibrationReject;
					ReqstData.ReqDueDate = DueDate;
				}
				else
				{
					ReqstData.StatusId = reviewStatus == 1 ? (Int32)EnumRequestStatus.Sent : (Int32)EnumRequestStatus.CalibrationReject;
				}
			}
			_unitOfWork.Repository<Request>().Update(ReqstData);
			_unitOfWork.SaveChanges();
			//----------------------New update for listing Approved Request end---------------------------

			//To Update ToolInventory Status
			if (instrumentData.ToolInventory != null && instrumentData.ToolInventory == "Yes")
			{
				if (ReqstData.TypeOfReqest == 2 || ReqstData.TypeOfReqest == 3)
				{
					if (reviewStatus == 1)
					{
						instrumentData.ToolInventoryStatus = (Int32)ToolInventoryStatus.ClosedTool;
						instrumentData.ToolRoomStatus = (Int32)ToolRoomStatus.Pending;
						instrumentData.ReplacementLabID = null;
					}
					else
					{
						instrumentData.ToolInventoryStatus = (Int32)ToolInventoryStatus.RejectedTool;

					}
				}
				else if (ReqstData.TypeOfReqest == 1)
				{
					instrumentData.ToolInventoryStatus = reviewStatus == 1 ? (Int32)ToolInventoryStatus.SentTool : (Int32)ToolInventoryStatus.RejectedTool;
				}
			}
			string objToolInventory = instrumentData.ToolInventory;
			_unitOfWork.Repository<Instrument>().Update(instrumentData);
			_unitOfWork.SaveChanges();

			//TO UPDATE THE EMAIL SERVICE
			if(emailService != null)
			{ 
				if ((instrumentData.IdNo != null) && (emailService.Status != null) && ((ReqstData.TypeOfReqest == 2 || ReqstData.TypeOfReqest == 3)))
				{
					emailService.Status = 1;
			
				_unitOfWork.Repository<EmailServiceStatus>().Update(emailService);
				_unitOfWork.SaveChanges();
				}
			}
			_unitOfWork.Commit();
			//For Regular / Recalibration Mail
			string SendingEmailRegular = "";
			string SendingEmailRecalibration = ""; 
			string ObjSendingEmailRegular = "";
			string ObjSendingEmailRecalibration = "";
			int iRegularCount = 0;
			int iRecalibrationCount = 0;
			CMTDL _cmtdl = new CMTDL(_configuration);
			string UserId = _contextAccessor.HttpContext.Session.GetString("LoggedId");
			UserViewModel UserById = _cmtdl.GetUserMasterById(Convert.ToInt32(UserId));
			List<RequestMailList> getrqlisting = _cmtdl.GetRequestDetailsForEMail(ReqstData.Id);
			foreach (var getrqlist in getrqlisting)
			{
				
				if (getrqlist.EquipmentType == "Regular")
				{
					// SendEmailRegular(getrqlist);

					SendingEmailRegular = " <tr><td>$S.No$</td><td>$RequestNo$</td><td>$LabId$</td><td>$EquType$</td><td>$EquName$</td><td>$Subcode$</td><td>$CalibType$</td></tr>";

					SendingEmailRegular = SendingEmailRegular.Replace("$S.No$", getrqlist.SNo.ToString()).Replace("$RequestNo$", getrqlist.RequestNo).Replace("$LabId$", getrqlist.LabId).Replace("$EquType$", getrqlist.EquipmentType).Replace("$EquName$", getrqlist.EquipmentName).Replace("$Subcode$", getrqlist.SubsectionCode).Replace("$CalibType$", getrqlist.CalibrationType);
					ObjSendingEmailRegular += SendingEmailRegular;
					iRegularCount += 1;

				}
				else if (getrqlist.EquipmentType == "Recalibration")
				{
					SendingEmailRecalibration = " <tr><td>$S.No$</td><td>$RequestNo$</td><td>$LabId$</td><td>$EquType$</td><td>$EquName$</td><td>$Subcode$</td><td>$CalibType$</td></tr>";

					SendingEmailRecalibration = SendingEmailRecalibration.Replace("$S.No$", getrqlist.SNo.ToString()).Replace("$RequestNo$", getrqlist.RequestNo).Replace("$LabId$", getrqlist.LabId).Replace("$EquType$", getrqlist.EquipmentType).Replace("$EquName$", getrqlist.EquipmentName).Replace("$Subcode$", getrqlist.SubsectionCode).Replace("$CalibType$", getrqlist.CalibrationType);
					ObjSendingEmailRecalibration += SendingEmailRecalibration;
					iRecalibrationCount += 1;
					//SendEmailRecalibration(getrqlist);
				}

			}

			
			string mailbody = "";
			if (iRegularCount > 0)
			{

				//Mail For Instrument Created User-Start  
				mailbody = "<!DOCTYPE html> <html lang=\"en\">  <head>   <meta charset=\"UTF-8\" />   <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />   <title></title>   <style>     table,     th,     td {         border: 1px solid black;         border-collapse: collapse;     }     </style> </head>  <body>      <p>Dear User,</p>   <p>Regular instrument calibration request has been closed by Lab.</p>          <table>       <tr>         <th>Sr.No</th>         <th>Request No.</th>         <th>Lab Id</th>         <th>Equipment Type</th>         <th>Equipment Name</th>         <th>Sub Section Code</th>         <th>Calibration Type</th>       </tr>    " + ObjSendingEmailRegular + "    </table>    <p>Regards</p><p>Lab team</p>  <br/>   <br/>      <p>親愛なるユーザー</p>   <p>計量器定期検査依頼がありました。</p>      <table>       <tr>         <th>シリアル№</th>         <th>依頼№</th>         <th>計量器№</th>         <th>計量器タイプ</th>         <th>計量器名</th>         <th>部門コード</th>         <th>内部校正or外部校正</th>       </tr>" + ObjSendingEmailRegular + "</table>      <p><a href='http://s365id1qdg044/cmtlive/'>CMT Portal</a></p>   <p>計量管理部門</p>   </body>  </html>";
				mailbody = mailbody.Replace("$USERNAME$", UserById.FirstName + " " + UserById.LastName);
				_emailService.EmailSendingFunction(UserById.Email.Trim(), mailbody, "Regular instrument calibration request / 計量器定期検査の依頼の件");
				//Mail For Instrument Created User-End  
			}
			if (iRecalibrationCount > 0)
			{
				//Mail For Instrument Created User-Start  
				mailbody = "<!DOCTYPE html> <html lang=\"en\">  <head>   <meta charset=\"UTF-8\" />   <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />   <title></title>   <style>     table,     th,     td {         border: 1px solid black;         border-collapse: collapse;     }     </style> </head>  <body>      <p>Dear User,</p>   <p>Regular instrument calibration request has been closed by Lab.</p>          <table>       <tr>         <th>Sr.No</th>         <th>Request No.</th>         <th>Lab Id</th>         <th>Equipment Type</th>         <th>Equipment Name</th>         <th>Sub Section Code</th>         <th>Calibration Type</th>       </tr>    " + ObjSendingEmailRegular + "    </table>    <p>Regards</p><p>Lab team</p>  <br/>  <br/> <p>親愛なるユーザー</p>   <p>計量器定期検査依頼がありました。</p>      <table>       <tr>         <th>シリアル№</th>         <th>依頼№</th>         <th>計量器№</th>         <th>計量器タイプ</th>         <th>計量器名</th>         <th>部門コード</th>         <th>内部校正or外部校正</th>       </tr>" + ObjSendingEmailRecalibration + "</table>      <p><a href='http://s365id1qdg044/cmtlive/'>CMT Portal</a></p>   <p>計量管理部門</p>   </body>  </html>";
				mailbody = mailbody.Replace("$USERNAME$", UserById.FirstName + " " + UserById.LastName);
				_emailService.EmailSendingFunction(UserById.Email.Trim(), mailbody, "Re-calibration calibration request / 計量器臨時検査依頼の件");
				//Mail For Instrument Created User-End 
			}
			return new ResponseViewModel<LeverTypeDialViewModel>
				{
					ResponseCode = 200,
					ResponseMessage = "Success",
					ResponseData = null,
					ResponseDataList = null
				};
			}
		
		catch (Exception e)
		{

			ErrorViewModelTest.Log("ObservationTemplateService - SubmitReview Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			_unitOfWork.RollBack();
			return new ResponseViewModel<LeverTypeDialViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "SubmitReview"
			};
		}
	}

	public List<string> GetUserName(int? Id)
	{
		string userName = string.Empty;
		string userSign = string.Empty;
		string userDesignation = string.Empty;

		List<string> userData = new List<string>();

		var user = _unitOfWork.Repository<User>().GetQueryAsNoTracking(x => x.Id == Id);
		if (user.Any())
		{
			userName = user.First().FirstName + " " + user.First().LastName;
			userSign = user.First().SignImageName;
			userDesignation = _unitOfWork.Repository<Lovs>()
										  .GetQueryAsNoTracking(Q => Q.AttrName == "Designation" && Q.Id == user.First().Designation)
										  .First().AttrValue;
		}

		userData.Add(userName);
		userData.Add(userSign);
		userData.Add(userDesignation);

		return userData;
	}

	private List<int?> GenerateULRAndCertificateNumber(bool? isNABL) // parameter bool? isNABL
    {
		int? ulrNumber = 0;
		int? certificateNumber = 0;
		int? year = 2022;
		isNABL = true;


        var startDate = DateTime.Parse(string.Concat("01-01-", year.ToString()));
		var endDate = DateTime.Parse(string.Concat("31-12-", DateTime.Now.Year.ToString()));


		var templateObservation = _unitOfWork.Repository<TemplateObservation>()
											 .GetQueryAsNoTracking(x => x.CalibrationReviewedDate.Date >= startDate.Date
																   && x.CalibrationReviewedDate.Date <= endDate.Date)
											 .ToList();


		if (templateObservation != null && isNABL.Value == true)
		{
			ulrNumber = templateObservation.OrderByDescending(x => x.ULRNumber).First().ULRNumber + 1;
			ulrNumber = ulrNumber == null ? 1 : ulrNumber;
		}

		if (templateObservation != null)
		{
			certificateNumber = templateObservation.OrderByDescending(x => x.CertificateNumber).First().CertificateNumber + 1;
			certificateNumber = certificateNumber == null ? 1 : certificateNumber;
		}

		List<int?> numberList = new List<int?>();
		numberList.Add(ulrNumber);
		numberList.Add(certificateNumber);

		return numberList;

	}

	private List<string> GetULRAndCertificateNumber(int? ULRNumber, int? certificateNumber)
	{

		List<string> formatList = new List<string>();
		int currentYear = DateTime.Now.Year - 2000;

		int ULRNumberLength = ULRNumber.ToString().Length;

		int zeroPading = Constants.ZeroCount - ULRNumberLength;
		int certificateZeroPading = Constants.certificateZeroCount - ULRNumberLength;

		string formatedNumber = ULRNumber.ToString().PadLeft(zeroPading, '0');

		string ulrNumberFormat = string.Format(Constants.ULRNumberFormat, currentYear, formatedNumber);

		formatedNumber = certificateNumber.ToString().PadLeft(certificateZeroPading, '0');

		string certificateFormat = string.Format(Constants.CertificateNumberFormat, currentYear, formatedNumber);

		formatList.Add(ulrNumberFormat);
		formatList.Add(certificateFormat);

		return formatList;
	}



	private ResponseViewModel<QRCodeFilesViewModel> InsertQRCodeFiles(int requestId, int instrumentId, int loginBy, string exportData)
	{
		try
		{

			RequestViewModel requestData = _qrCodeGeneratorService.GetRequestData(requestId);

			QRCodeFilesViewModel qrCodeInputData = new QRCodeFilesViewModel()
			{
				RequestId = requestId,
				InstrumentId = instrumentId,
				RequestNo = requestData.ReqestNo,
				FileName = string.Concat(requestData.ReqestNo, ".pdf"),
				AmendmentNo = string.Empty,
				CreatedBy = loginBy,
				CreatedOn = DateTime.Now.Date,
				UrlGuid = Guid.NewGuid()
			};

			QRCodeFilesViewModel qrCodeInsertData = new QRCodeFilesViewModel();
			QRCodeFilesViewModel qrCodeExistingData = _qrCodeGeneratorService.GetQRCodeDetails(qrCodeInputData);

			string amendmentNo = string.Empty;
			if (qrCodeExistingData != null)
			{
				amendmentNo = qrCodeExistingData.AmendmentNo;
				if (string.IsNullOrEmpty(amendmentNo))
				{
					amendmentNo = Constants.AMENDNO_CHARACTER + "1";
				}
				else
				{
					string substringNumber = amendmentNo.Replace(Constants.AMENDNO_CHARACTER, "");
					int subsequentNo = substringNumber == string.Empty ? 0 : int.Parse(substringNumber);
					amendmentNo = Constants.AMENDNO_CHARACTER + Convert.ToString(++subsequentNo);
				}
				qrCodeInsertData.RequestId = requestId;
				qrCodeInsertData.RequestNo = requestData.ReqestNo;
				qrCodeInsertData.InstrumentId = instrumentId;
				qrCodeInsertData.AmendmentNo = amendmentNo;
				qrCodeInsertData.FileName = string.Concat(requestData.ReqestNo, "-", amendmentNo, ".pdf");
				qrCodeInsertData.CreatedBy = loginBy;
				qrCodeInsertData.CreatedOn = DateTime.Now.Date;
				qrCodeInsertData.UrlGuid = Guid.NewGuid();
			}
			else
			{
				qrCodeInsertData = qrCodeInputData;
			}

			var qrCodeData = _mapper.Map<QRCodeFiles>(qrCodeInsertData);
			_unitOfWork.Repository<QRCodeFiles>().Insert(qrCodeData);
			_unitOfWork.SaveChanges();

			//After saving QR Code Generation based on the Request number
			QRCodeFilesViewModel qrCodeGenInputViewModel = new QRCodeFilesViewModel()
			{
				TemplateName = Constants.CONTROLLERNAME,
				RequestId = requestId,
				InstrumentId = instrumentId,
				RequestNo = requestData.ReqestNo
			};

			QRCodeFilesViewModel qrCodeGenOutputViewModel = _qrCodeGeneratorService.QRCodeGeneration(qrCodeGenInputViewModel);

			if (qrCodeGenOutputViewModel != null)
				System.IO.File.WriteAllBytes(_configuration["TempQRCodePath"], qrCodeGenOutputViewModel.DecodeText);

			//Pdf File generations...
			SavePdfFile(exportData, qrCodeInsertData.UrlGuid.ToString(), qrCodeInsertData.FileName);

			return new ResponseViewModel<QRCodeFilesViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("ObservationTemplateService - InsertQRCodeFiles Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<QRCodeFilesViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "InsertQRCodeFiles"
			};
		}
	}

	private void SavePdfFile(string ExportData, string newDirectoryName, string fileName)
	{
		HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
		HtmlNode.ElementsFlags["p"] = HtmlElementFlag.Closed;
		HtmlNode.ElementsFlags["input"] = HtmlElementFlag.Closed;
		HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Closed;
		HtmlDocument doc = new HtmlDocument();
		doc.OptionFixNestedTags = true;
		doc.LoadHtml(ExportData);
		ExportData = doc.DocumentNode.OuterHtml;
		using (MemoryStream sourceStream = new System.IO.MemoryStream())
		{
			TextReader reader = new StringReader(ExportData);
			//PageSize.A4, 10.0F, 10.0F, 100.0F, 0.0F)PageSize.A4, 10, 10, 10, 20

			//Document PdfFile = new Document(PageSize.LETTER.Rotate(), 10, 10, 10, 20);
			//Document PdfFile = new Document(new iTextSharp.text.Rectangle(410f, 288f).Rotate());
			Document PdfFile = new Document(iTextSharp.text.PageSize.A4.Rotate(), 10, 10, 10, 20);
			PdfWriter writer = PdfWriter.GetInstance(PdfFile, sourceStream);            
            //  String Export_css = @"html { font-family: SmartFontUI; font-size: 10px; }  h1, h2 strong { font-family: SmartFontUI; font-weight: normal; display: inline; }  h1 { font-size: 18pt; }   .main { font-size: 12pt; color: black; font-family: SmartFontUI, Arial, Sans-Serif; background-color: white; text-align: left; line-height: 1.4em; margin: 2%; } .mainDesc { width: 100%; margin: 10px 0; }  span { float: left; display: block; width: 100%; margin-bottom: 5px; }  .bodyText1SerNo { width: 2em; }  .bodyText1LIC { width: 22em; }  .txtboxspan { width: 32em; }  .tableBody { width: 100%; } .nc { margin-left: 100px; } .subtitle { font-size: 26pt;font-weight: 900; }";
            String Export_css = @"html { font-family: SmartFontUI; font-size: 10px; }  h1, h2, strong { font-family: SmartFontUI; font-weight: normal; display: inline; }  h1 { font-size: 18pt; }   .main { font-size: 12pt; color: black; font-family: SmartFontUI, Arial, Sans-Serif; background-color: white; text-align: left; line-height: 1.4em; margin: 2%; } .mainDesc { width: 100%; margin: 10px 0; }  span { float: left; display: block; width: 100%; margin-bottom: 5px; }  .bodyText1SerNo { width: 2em; }  .bodyText1LIC { width: 22em; }  .txtboxspan { width: 32em; }  .tableBody1 { width: 100%; } .nc { margin-left: 100px; } .subtitle { font-size: 16pt; }";
			MemoryStream msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(Export_css));
			MemoryStream msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(ExportData));

			PdfFile.Open();
			string FontPath1 = Path.Combine(_configuration["TempFontCodePath"], "ARIALUNI.TTF");
			UnicodeFontFactory smartUIFontObj = new UnicodeFontFactory(FontPath1);


			XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, msHtml, msCss, System.Text.Encoding.UTF8, smartUIFontObj);


			PdfFile.Close();

			/********************Write Pdf document header and footer Section Start*****************************************************/
			//Document document = new Document(PageSize.A4, 36, 36, 36 + <height of table>, 36); // note height should be set here
			/*PdfHeaderService events = new PdfHeaderService();
            PdfWriter pw = PdfWriter.GetInstance(PdfFile, new FileStream("TableTest.pdf", FileMode.Create));
           
            pw.PageEvent = events;
            PdfFile.Open();

            for(int i=0;i<100;i++)
            {
                PdfFile.Add(new Phrase("TESTING\n"));
            }

            PdfFile.Close();*/

			/*******************Write Pdf document header and footer Section End ******************************************************/

			// StringReader stringReader = new StringReader(ExportData);         
			// Document PDFdoc = new Document(PageSize.A4, 10.0F, 10.0F, 10.0F, 0.0F);
			// HTMLWorker htmlparser =   new HTMLWorker(PDFdoc);
			// PdfWriter.GetInstance(PDFdoc, sourceStream);

			// PDFdoc.Open();
			// htmlparser.Parse(stringReader);
			// //XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, reader);
			// PDFdoc.Close();

			string uploadDirectoryName = Path.Combine(Constants.Certification_FolderName, newDirectoryName);
			string filePath = Path.Combine(this._environment.WebRootPath, uploadDirectoryName);

			if (!Directory.Exists(filePath))
			{
				Directory.CreateDirectory(filePath);
			}

			fileName = Path.Combine(filePath, fileName);

			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}

			File.WriteAllBytes(fileName, sourceStream.ToArray());
		}
	}

	public class UnicodeFontFactory : FontFactoryImp
	{
		private string _configuration;
		//var Path =  _configuration["UNICodeFontPath"];

		//private static readonly string FontPath = @"D:\CMTNEW\WEB\wwwroot\css\ARIALUNI.ttf";
		private readonly BaseFont _baseFont;

		public UnicodeFontFactory(string Fpath)
		{
			_baseFont = BaseFont.CreateFont(Fpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
		}
		//private string FontPath1 = Path.Combine(_configuration["TempQRCodePath"], "ARIALUNI.TTF");
		public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color, bool cached)
		{
			return new Font(_baseFont, size, style, BaseColor.BLACK);

		}
	}


	public ResponseViewModel<MetalRulesViewModel> GetMetalRulesId(int requestId, int instrumentId)
	{
		try
		{
			//MetalRulesViewModel metalViewModel = _unitOfWork.Repository<TemplateObservation>()
			//.GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
			//.Include(I => I.MetalRulesModel)
			//.Select(s => new MetalRulesViewModel()
			//{
			//	Id = s.Id,
			//	TempStart = s.TempStart,
			//	TempEnd = s.TempEnd,
			//	Humidity = s.Humidity,
			//	RefWi = s.RefWi,
			//	Allvalues = s.Allvalues,
			//	CalibrationPerformedDate = s.CreatedOn,
			//	CreatedBy = s.CreatedBy,
			//	CalibrationReviewedBy = s.CalibrationReviewedBy,
			//	CalibrationReviewedDate = s.CalibrationReviewedDate,
			//	MetalRulesCondition = s.InstrumentCondition,
			//	ReviewStatus = s.ReviewStatus,
			//	ULRNumber = s.ULRNumber,
			//	CertificateNumber = s.CertificateNumber,
			//	Uncertainity = s.GeneralModel.Select(x => x.Uncertainity).SingleOrDefault(),
			//	CalibrationResult = s.GeneralModel.Select(x => x.CalibrationResult).SingleOrDefault(),
			//	Remarks = s.GeneralModel.Select(x => x.Remarks).SingleOrDefault()
			//}).SingleOrDefault();

			//if (metalViewModel == null)
			//{
			//	return new ResponseViewModel<MetalRulesViewModel>
			//	{
			//		ResponseCode = 200,
			//		ResponseMessage = "No records found",
			//		ResponseData = null,
			//		ResponseDataList = null
			//	};
			//}
			//else
			//{

			//	List<string> performedUserData = GetUserName(metalViewModel.CreatedBy);

			//	if (performedUserData.Count >= 3)
			//	{
			//		metalViewModel.CalibrationPerformedBy = performedUserData[0];
			//		metalViewModel.PerformedBySign = performedUserData[1];
			//		metalViewModel.PerformedByDesignation = performedUserData[2];
			//	}

			//	List<string> reviewedUserData = GetUserName(metalViewModel.CalibrationReviewedBy);

			//	if (reviewedUserData.Count >= 3)
			//	{
			//		metalViewModel.ReviewedBy = reviewedUserData[0];
			//		metalViewModel.ReviewedBySign = reviewedUserData[1];
			//		metalViewModel.ReviewedByDesignation = reviewedUserData[2];
			//	}

			//	int? ulrNumber = metalViewModel.ULRNumber == null ? 0 : metalViewModel.ULRNumber;
			//	int? certificateNumber = metalViewModel.CertificateNumber == null ? 0 : metalViewModel.CertificateNumber;
			//	List<string> formatList = GetULRAndCertificateNumber(ulrNumber, certificateNumber);

			//	if (formatList.Count >= 2)
			//	{
			//		metalViewModel.ULRFormat = formatList[0];
			//		metalViewModel.CertificateFormat = formatList[1];
			//	}
			//}

			//var parentVM = _mapper.Map<MetalRulesViewModel>(metalViewModel);

			//var childData1 = _unitOfWork.Repository<ObsTemplateValues>()
			//							.GetQueryAsNoTracking(x => x.ParentId == parentVM.Id && x.MasterView1 == 1);
			//         var childListVM = _mapper.Map<List<MetalRuleResultViewModel>>(childData1);

			//         metalViewModel.MetalRuleAddResultViewModelList1 = childListVM;

			//         var childData2 = _unitOfWork.Repository<ObsTemplateValues>()
			//                                     .GetQueryAsNoTracking(x => x.ParentId == parentVM.Id && x.MasterView2 == 1);
			//         var childListVM2 = _mapper.Map<List<MetalRuleResultViewModel>>(childData2);

			//         metalViewModel.MetalRuleAddResultViewModelList2 = childListVM2;




			//var parentVM1 = _mapper.Map<MetalRulesViewModel>(metalViewModel);

			//var childData1 = _unitOfWork.Repository<ObsGeneralDynamicValues>()
			//							.GetQueryAsNoTracking(x => x.ParentId == parentVM.Id);
			//var childListVM1 = _mapper.Map<List<GeneralManualResultViewModel>>(childData1);

			//metalViewModel.GeneralManualAddResultViewModelList = childListVM1;

			return new ResponseViewModel<MetalRulesViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				//ResponseData = metalViewModel,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("ObservationTemplateService - GetMetalRulesId Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MetalRulesViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "GetMetalRulesId"
			};
		}
	}

	public ResponseViewModel<MetalRulesViewModel> InsertMetalRule(MetalRulesViewModel metalrule)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			int tempobsId = 0;
			int ObjMicroData = 0;
			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
																 .GetQueryAsNoTracking(Q => Q.RequestId == metalrule.RequestId
																  && Q.InstrumentId == metalrule.InstrumentId).SingleOrDefault();

			if ((metalrule.TemplateObservationId == 0) && (observationById == null))

			{
				TemplateObservation templateObservation = new TemplateObservation()
				{
					InstrumentId = metalrule.InstrumentId,
					RequestId = metalrule.RequestId,
					TempStart = metalrule.TempStart,
					TempEnd = metalrule.TempEnd,
					Humidity = metalrule.Humidity,
					InstrumentCondition = metalrule.MetalRulesCondition,
					RefWi = metalrule.RefWi,
					Allvalues = metalrule.Allvalues,
					CreatedOn = DateTime.Now,
					CreatedBy = metalrule.CreatedBy,
					CalibrationReviewedDate = DateTime.Now
				};
				_unitOfWork.Repository<TemplateObservation>().Insert(templateObservation);
				_unitOfWork.SaveChanges();
				tempobsId = templateObservation.Id;
			}
			else
			{
				if (observationById != null)
				{


					if (metalrule.TempStart != null)
					{
						observationById.TempStart = metalrule.TempStart;
					}

					if (metalrule.TempEnd != null)
					{
						observationById.TempEnd = metalrule.TempEnd;
					}

					if (metalrule.Humidity != null)
					{
						observationById.Humidity = metalrule.Humidity;
					}

					if (metalrule.MetalRulesCondition != null)
					{
						observationById.InstrumentCondition = metalrule.MetalRulesCondition;
					}
					if (metalrule.RefWi != null)
					{
						observationById.RefWi = metalrule.RefWi;
					}
					if (metalrule.Allvalues != null)
					{
						observationById.Allvalues = metalrule.Allvalues;
					}

					_unitOfWork.Repository<TemplateObservation>().Update(observationById);
					_unitOfWork.SaveChanges();
				}
			}

			if (metalrule.Id == 0)
			{
				if (observationById != null)
				{
					tempobsId = observationById.Id;
				}

				ObsTemplateMetalRules obsTemplateMetalRules = new ObsTemplateMetalRules()
				{

					ParentId = tempobsId,
					CreatedOn = DateTime.Now,
					CreatedBy = metalrule.CreatedBy,
				};
				_unitOfWork.Repository<ObsTemplateMetalRules>().Insert(obsTemplateMetalRules);
				_unitOfWork.SaveChanges();
				if (observationById != null)
				{

					tempobsId = observationById.Id;
				}
				if (metalrule.MetalRuleAddResultViewModelList != null)
				{
					metalrule.MetalRuleAddResultViewModelList.ForEach(x => x.ParentId = tempobsId);
					var detailData = _mapper.Map<ObsTemplateValues[]>(metalrule.MetalRuleAddResultViewModelList
											.Where(x => x.Id > 0).ToList());
					if (detailData.Any())
					{
						foreach (var updateData in detailData)
						{
							_unitOfWork.Repository<ObsTemplateValues>().Update(updateData);
							_unitOfWork.SaveChanges();
						}
					}

					detailData = _mapper.Map<ObsTemplateValues[]>(metalrule.MetalRuleAddResultViewModelList.Where(x => x.Id == null).ToList());

					if (detailData.Any())
					{
						_unitOfWork.Repository<ObsTemplateValues>().InsertRange(detailData);
						_unitOfWork.SaveChanges();
					}
				}
			}
			else
			{
				ObsTemplateMetalRules micrometerById = _unitOfWork.Repository<ObsTemplateMetalRules>()
																.GetQueryAsNoTracking(Q => Q.ParentId == metalrule.Id)
																.SingleOrDefault();

				if (observationById != null)
				{

					tempobsId = observationById.Id;
				}
				if (metalrule.MetalRuleAddResultViewModelList != null)
				{
					metalrule.MetalRuleAddResultViewModelList.ForEach(x => x.ParentId = tempobsId);
					var detailData = _mapper.Map<ObsTemplateValues[]>(metalrule.MetalRuleAddResultViewModelList
											.Where(x => x.Id > 0).ToList());//x.Id > 0 && 
					if (detailData.Any())
					{
						foreach (var updateData in detailData)
						{
							_unitOfWork.Repository<ObsTemplateValues>().Update(updateData);
							_unitOfWork.SaveChanges();
						}
					}
					//micrometer.MicrometerAddResultViewModelList.ForEach()
					detailData = _mapper.Map<ObsTemplateValues[]>(metalrule.MetalRuleAddResultViewModelList.Where(x => x.Id == null).ToList());

					if (detailData.Any())
					{
						_unitOfWork.Repository<ObsTemplateValues>().InsertRange(detailData);
						_unitOfWork.SaveChanges();
					}
				}
			}
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			//ErrorViewModelTest.Log("ObservationTemplateService - InsertMicrometer Method One");
			return new ResponseViewModel<MetalRulesViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - InsertMicrometer Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MetalRulesViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = metalrule,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "InsertMicrometer"
			};
		}
	}

	#region "Dynamic Observation"
	public ResponseViewModel<ObservationContentValuesViewModel> GetObservationContentValuesById (int InstrumentId, int RequestId)
	{
		try
		{

			List<ObservationContentValuesViewModel> ObservationInstrument = new List<ObservationContentValuesViewModel>();

			DataSet dsObservationContent = GetContentValuesById(InstrumentId, RequestId);
			if (dsObservationContent != null && dsObservationContent.Tables.Count > 0 && dsObservationContent.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsObservationContent.Tables[0].Rows)
				{
					ObservationContentValuesViewModel dynamicViewModel = new ObservationContentValuesViewModel
					{

						Id = Convert.ToInt32(dr["Id"]),
						ParentId = Convert.ToInt32(dr["ParentId"]),
						Sno = Convert.ToInt32(dr["Sno"]),
						MeasuedValue = dr["MeasuedValue"].ToString(),
						ActualValue = dr["ActualValue"].ToString(),
						InstrumentError = dr["InstrumentError"].ToString(),
						Diff = dr["Diff"].ToString(),
						MeasuedValue1 = dr["MeasuedValue1"].ToString(),
						MeasuedValue2 = dr["MeasuedValue2"].ToString(),
						MeasuedValue3 = dr["MeasuedValue3"].ToString(),
						Average = dr["Average"].ToString(),
						Percent = dr["Percent"].ToString(),
						ContentId = Convert.ToInt32(dr["ContentId"]),
						ContentMappingId = Convert.ToInt32(dr["ContentMappingId"])

					};
					ObservationInstrument.Add(dynamicViewModel);
				}
			}
			return new ResponseViewModel<ObservationContentValuesViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = ObservationInstrument
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("DynamicObservationService - GetObservationContentValuesById Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ObservationContentValuesViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "DynamicObservationService",
				ResponseServiceMethod = "GetObservationContentValuesById"
			};
		}
	}

	public DataSet GetContentValuesById(int InstrumentId,int RequestId)//, int deptid)@InstrumentId int,@RequestId int
	{
		var connectionString = _configuration.GetConnectionString("CMTDatabase");
		SqlCommand cmd = new SqlCommand("GetObservationContentValuesById");//GetObservationContentsById
		cmd.CommandType = CommandType.StoredProcedure;
		cmd.Parameters.AddWithValue("@InstrumentId", InstrumentId);
		cmd.Parameters.AddWithValue("@RequestId", RequestId);
		SqlConnection sqlConn = new SqlConnection(connectionString);
		DataSet dsResults = new DataSet();
		SqlDataAdapter sqlAdapter = new SqlDataAdapter();
		cmd.Connection = sqlConn;
		cmd.CommandTimeout = 2000;
		sqlAdapter.SelectCommand = cmd;
		sqlAdapter.Fill(dsResults);
		return dsResults;
	}
	public ResponseViewModel<ObservationContentViewModel> GetObservationById(int InstrumentId, int RequestId,int TemplateObservationId)
	{
		try
		{

			List<ObservationContentViewModel> ObservationInstrument = new List<ObservationContentViewModel>();

			DataSet dsObservationContent = GetObservationContent(InstrumentId, RequestId, TemplateObservationId);
			if (dsObservationContent != null && dsObservationContent.Tables.Count > 0 && dsObservationContent.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsObservationContent.Tables[0].Rows)
				{
					ObservationContentViewModel dynamicViewModel = new ObservationContentViewModel
					{

						ObservationTemplate = Convert.ToInt32(dr["ObservationTemplate"]),
						ObservationType = Convert.ToInt32(dr["ObservationType"]),
						ContentName = dr["ContentName"].ToString(),
						ContentValue = dr["ContentValue"].ToString(),
						ContentCount = dr["ContentCount"].ToString(),
						ContentTitle1 = dr["ContentTitle1"].ToString(),
						ContentTitle2 = dr["ContentTitle2"].ToString(),
						ContentSubTitle1 = dr["ContentSubTitle1"].ToString(),
						ContentSubTitle2 = dr["ContentSubTitle2"].ToString(),
						ContentSubTitle3 = dr["ContentSubTitle3"].ToString(),
						ContentSubTitle4 = dr["ContentSubTitle4"].ToString(),
						ContentSubTitle5 = dr["ContentSubTitle5"].ToString(),
						Id = Convert.ToInt32(dr["Id"]),
						TypeOfContent = dr["TypeOfContent"].ToString(),
						//////////////////test////////////////
						ObsContentValueId = Convert.ToInt32(dr["ObsContentValueId"]),
						ParentId = Convert.ToInt32(dr["ParentId"]),
						Sno = Convert.ToInt32(dr["Sno"]),
						MeasuedValue = dr["MeasuedValue"].ToString(),
						ActualValue = dr["ActualValue"].ToString(),
						InstrumentError = dr["InstrumentError"].ToString(),
						Diff = dr["Diff"].ToString(),
						MeasuedValue1 = dr["MeasuedValue1"].ToString(),
						MeasuedValue2 = dr["MeasuedValue2"].ToString(),
						MeasuedValue3 = dr["MeasuedValue3"].ToString(),
						Average = dr["Average"].ToString(),
						Percent = dr["Percent"].ToString(),
						ContentId = Convert.ToInt32(dr["ContentId"]),
						ContentMappingId = Convert.ToInt32(dr["ContentMappingId"])

                    };
					ObservationInstrument.Add(dynamicViewModel);
				}
			}
			return new ResponseViewModel<ObservationContentViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = ObservationInstrument
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("DynamicObservationService - GetObservationById Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ObservationContentViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "DynamicObservationService",
				ResponseServiceMethod = "GetObservationById"
			};
		}
	}
	public DataSet GetObservationContent(int InstrumentId,int RequestId,int TemplateObservationId)//, int deptid)
	{
		var connectionString = _configuration.GetConnectionString("CMTDatabase");
		//SqlCommand cmd = new SqlCommand("GetObservationContents");
		SqlCommand cmd = new SqlCommand("GetObsContentValuesById");
		cmd.CommandType = CommandType.StoredProcedure;
		cmd.Parameters.AddWithValue("@InstrumentId", InstrumentId);
		cmd.Parameters.AddWithValue("@RequestId", RequestId);
		cmd.Parameters.AddWithValue("@TemplateObservationId", TemplateObservationId);
		SqlConnection sqlConn = new SqlConnection(connectionString);
		DataSet dsResults = new DataSet();
		SqlDataAdapter sqlAdapter = new SqlDataAdapter();
		cmd.Connection = sqlConn;
		cmd.CommandTimeout = 2000;
		sqlAdapter.SelectCommand = cmd;
		sqlAdapter.Fill(dsResults);
		return dsResults;
	}
	public ResponseViewModel<DynamicViewModel> GetObservationInstrumentById(int InstrumentId, int RequestId)
	{

		try
		{

			//List<DynamicViewModel> ObservationInstrument = new List<DynamicViewModel>();
			DynamicViewModel ObservationInstrument = new DynamicViewModel();
			DataSet dsObservationContent = GetObservationInstrument(InstrumentId, RequestId);

			//foreach (DataRow dr in dsObservationContent.Tables[0].Rows)
			//{
			//	DynamicViewModel dynamicViewModel = new DynamicViewModel
			//	{
			if (dsObservationContent != null && dsObservationContent.Tables.Count > 0 && dsObservationContent.Tables[0].Rows.Count > 0)
			{
				DataRow dr = dsObservationContent.Tables[0].Rows[0];
				ObservationInstrument.IdNo = dr["IdNo"].ToString();
				ObservationInstrument.Title = dr["Title"].ToString();
				ObservationInstrument.SubTitle = dr["SubTitle"].ToString();
				ObservationInstrument.FormatNoTitle = dr["FormatNoTitle"].ToString();
				ObservationInstrument.FormatNo = dr["FormatNo"].ToString();
				//ObservationTemplate = Convert.ToInt32(dr["ObservationTemplate"]),
				ObservationInstrument.Id = Convert.ToInt32(dr["Id"]);
				//ObservationType = Convert.ToInt32(dr["ObservationTemplate"]),
				ObservationInstrument.TempStart = dr["TempStart"].ToString();
				//TempEnd = dr["TempEnd"].ToString(),
				ObservationInstrument.Humidity = dr["Humidity"].ToString();
				ObservationInstrument.RefStd = dr["StandardReffered"].ToString();
				ObservationInstrument.Units = dr["Units"].ToString();
				ObservationInstrument.ReviewStatus = Convert.ToInt32(dr["ReviewStatus"]);
				ObservationInstrument.CalibrationPerformedDate = Convert.ToDateTime(dr["CalibrationPerformedDate"]);
				ObservationInstrument.CalibrationReviewedDate = Convert.ToDateTime(dr["CalibrationReviewedDate"]);
				ObservationInstrument.CalibrationPerformedBy = dr["CalibrationPerformedBy"].ToString();
				ObservationInstrument.CalibrationReviewedBy = dr["CalibrationReviewedBy"].ToString();
				ObservationInstrument.Condition = dr["Condition"].ToString();
				ObservationInstrument.Name = dr["Name"].ToString();
				ObservationInstrument.SlNo = dr["SlNo"].ToString();
				ObservationInstrument.Range = dr["Range"].ToString();
				ObservationInstrument.Make = dr["Make"].ToString();
				ObservationInstrument.RequestId = RequestId;// Convert.ToInt32(dr["RequestId"]);
				ObservationInstrument.Grade = dr["Grade"].ToString();
				ObservationInstrument.TemplateObservationId = Convert.ToInt32(dr["TemplateObservationId"]);
				ObservationInstrument.StatusId = Convert.ToInt32(dr["StatusId"]);
				ObservationInstrument.CalibFreq = Convert.ToInt32(dr["CalibFreq"]);
			}
			if (dsObservationContent != null && dsObservationContent.Tables.Count > 0 && dsObservationContent.Tables[1].Rows.Count > 0)
			{

				var ObservationContent = new List<ObservationContentViewModel>();
				foreach (DataRow dr in dsObservationContent.Tables[1].Rows)
				{
					ObservationContent.Add(new ObservationContentViewModel
					{
						Id = Convert.ToInt32(dr["Id"]),
						ContentName = Convert.ToString(dr["ContentValue"])
					});

				}

				ObservationInstrument.ObservationContentList = ObservationContent;
			}
			if (dsObservationContent != null && dsObservationContent.Tables.Count > 0 && dsObservationContent.Tables[2].Rows.Count > 0)
			{

				var EqiupmentList = new List<MasterViewModel>();
				foreach (DataRow dr in dsObservationContent.Tables[2].Rows)
				{
					EqiupmentList.Add(new MasterViewModel
					{
						Id = Convert.ToInt32(dr["Id"]),
						EquipName = Convert.ToString(dr["EquipName"])
					});

				}

				ObservationInstrument.MasterEqiupmentList = EqiupmentList;
			}
			if (dsObservationContent != null && dsObservationContent.Tables.Count > 0 && dsObservationContent.Tables[3].Rows.Count > 0)
			{
				var UserContentMapping = new List<UserContentMappingView>();
				foreach (DataRow dr in dsObservationContent.Tables[3].Rows)
				{
					UserContentMapping.Add(new UserContentMappingView
					{
					
						//Id = Convert.ToInt32(dr["Id"]),
						ContentId = Convert.ToInt32(dr["ContentId"]),
						//ObservationId = Convert.ToInt32(dr["ObservationId"]),
						//InstrumentId = Convert.ToInt32(dr["InstrumentId"])
					});
				}

				ObservationInstrument.UserContentMappingViewModel = UserContentMapping;
			}

			return new ResponseViewModel<DynamicViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = ObservationInstrument,
				ResponseDataList = null,
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("DynamicObservationService - GetObservationInstrumentById Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<DynamicViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "DynamicObservationService",
				ResponseServiceMethod = "GetObservationInstrumentById"
			};
		}
	}
	public DataSet GetObservationInstrument(int InstrumentId, int RequestId)//, int deptid)
	{
		var connectionString = _configuration.GetConnectionString("CMTDatabase");
		SqlCommand cmd = new SqlCommand("GetObservationInstrument");
		cmd.CommandType = CommandType.StoredProcedure;

		cmd.Parameters.AddWithValue("@InstrumentId", InstrumentId);
		cmd.Parameters.AddWithValue("@RequestId", RequestId);
		SqlConnection sqlConn = new SqlConnection(connectionString);
		DataSet dsResults = new DataSet();
		SqlDataAdapter sqlAdapter = new SqlDataAdapter();
		cmd.Connection = sqlConn;
		cmd.CommandTimeout = 2000;
		sqlAdapter.SelectCommand = cmd;
		sqlAdapter.Fill(dsResults);
		return dsResults;
	}
	public ResponseViewModel<ObservationContentViewModel> GetSelectedObservationContentById(int ContentId,int InstrumentId,int RequestId)
	{
		try
		{

			List<ObservationContentViewModel> ObservationInstrument = new List<ObservationContentViewModel>();

			DataSet dsObservationContent = GetSelectedObservationContent(ContentId, InstrumentId,RequestId);
			if (dsObservationContent != null && dsObservationContent.Tables.Count > 0 && dsObservationContent.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsObservationContent.Tables[0].Rows)
				{
					ObservationContentViewModel dynamicViewModel = new ObservationContentViewModel
					{
						ObservationTemplate = Convert.ToInt32(dr["ObservationTemplate"]),
						ObservationType = Convert.ToInt32(dr["ObservationType"]),
						ContentName = dr["ContentName"].ToString(),
						ContentValue = dr["ContentValue"].ToString(),
						ContentCount = dr["ContentCount"].ToString(),
						ContentTitle1 = dr["ContentTitle1"].ToString(),
						ContentTitle2 = dr["ContentTitle2"].ToString(),
						ContentSubTitle1 = dr["ContentSubTitle1"].ToString(),
						ContentSubTitle2 = dr["ContentSubTitle2"].ToString(),
						ContentSubTitle3 = dr["ContentSubTitle3"].ToString(),
						ContentSubTitle4 = dr["ContentSubTitle4"].ToString(),
						ContentSubTitle5 = dr["ContentSubTitle5"].ToString(),
						Id = Convert.ToInt32(dr["Id"]),
						TypeOfContent = dr["TypeOfContent"].ToString(),


					};
					ObservationInstrument.Add(dynamicViewModel);
				}
			}
			return new ResponseViewModel<ObservationContentViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = ObservationInstrument
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("DynamicObservationService - GetObservationById Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ObservationContentViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "DynamicObservationService",
				ResponseServiceMethod = "GetObservationById"
			};
		}
	}
	//[GetSelectedObservationContent]
	public DataSet GetSelectedObservationContent(int ContentId,int InstrumentId,int RequestId)//, int deptid)
	{
		var connectionString = _configuration.GetConnectionString("CMTDatabase");
		SqlCommand cmd = new SqlCommand("GetSelectedObservationContent");
		cmd.CommandType = CommandType.StoredProcedure;

		cmd.Parameters.AddWithValue("@ContentId", ContentId);
		cmd.Parameters.AddWithValue("@InstrumentId", InstrumentId); 
		cmd.Parameters.AddWithValue("@RequestId", InstrumentId);
		SqlConnection sqlConn = new SqlConnection(connectionString);
		DataSet dsResults = new DataSet();
		SqlDataAdapter sqlAdapter = new SqlDataAdapter();
		cmd.Connection = sqlConn;
		cmd.CommandTimeout = 2000;
		sqlAdapter.SelectCommand = cmd;
		sqlAdapter.Fill(dsResults);
		return dsResults;
	}

	public ResponseViewModel<DynamicViewModel> InsertObservation(DynamicViewModel dynamic)
	{

		try
		{
			_unitOfWork.BeginTransaction();
			int dynamicId = 0;
			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
																 .GetQueryAsNoTracking(Q => Q.InstrumentId == dynamic.InstrumentId)
																 .SingleOrDefault();
			//.GetQueryAsNoTracking(Q => Q.RequestId == dynamic.RequestId	&& Q.InstrumentId == dynamic.InstrumentId).SingleOrDefault();
			if ((dynamic.TemplateObservationId == null) && (observationById == null))

			{
				TemplateObservation templateObservation = new TemplateObservation()
				{
					InstrumentId = dynamic.InstrumentId,
					RequestId = dynamic.RequestId,
					TempStart = dynamic.TempStart,
					Humidity = dynamic.Humidity,
					//RefWi = general.RefWi,
					InstrumentCondition = dynamic.Condition,
					Allvalues = dynamic.Units,
					CreatedBy = dynamic.CreatedBy,
					CreatedOn = DateTime.Now,
					// CalibrationReviewedBy = labTechnicalManager.Id,
					CalibrationReviewedDate = DateTime.Now
				};
				_unitOfWork.Repository<TemplateObservation>().Insert(templateObservation);
				_unitOfWork.SaveChanges();
				dynamicId = templateObservation.Id;

			}
			else
			{
				if (observationById != null)
				{

					if (dynamic.TempStart != null)
					{
						observationById.TempStart = dynamic.TempStart;
					}

					if (dynamic.Humidity != null)
					{
						observationById.Humidity = dynamic.Humidity;
					}

					if (dynamic.Condition != null)
					{
						observationById.InstrumentCondition = dynamic.Condition;
					}

					if (dynamic.Units != null)
					{
						observationById.Allvalues = dynamic.Units;
					}
					if (dynamic.RequestId != null)
					{
						observationById.RequestId = dynamic.RequestId;
					}
									
					_unitOfWork.Repository<TemplateObservation>().Update(observationById);
				}
			}
			_unitOfWork.SaveChanges();
			if (observationById != null)
			{

				dynamicId = observationById.Id;
			}

			if (dynamic.ObservationContentValuesList != null)
			{
				dynamic.ObservationContentValuesList.ForEach(x => x.ParentId = dynamicId);
				var detailData = _mapper.Map<ObservationContentValues[]>(dynamic.ObservationContentValuesList
										.Where(x => x.Id > 0).ToList());
				if (detailData.Any())
				{

					foreach (var updateData in detailData)
					{
						_unitOfWork.Repository<ObservationContentValues>().Update(updateData);
						_unitOfWork.SaveChanges();
					}
				}

				detailData = _mapper.Map<ObservationContentValues[]>(dynamic.ObservationContentValuesList
										.Where(x => x.Id == null).ToList());

				if (detailData.Any())
				{

					_unitOfWork.Repository<ObservationContentValues>().InsertRange(detailData);
					_unitOfWork.SaveChanges();
				}
			}

			if (dynamic.ObservationContentMappingList != null)
			{
				//To insert ObservationContentMapping
				//dynamic.ObservationContentMappingList.ForEach(x => x.ObservationId = dynamicId);
				//dynamic.ObservationContentMappingList.ForEach(x => x.CreatedOn = DateTime.Now);
				//dynamic.ObservationContentMappingList.ForEach(x => x.CreatedBy = dynamic.CreatedBy);

			
				//var MappingData = _mapper.Map<ObservationContentMapping[]>(dynamic.ObservationContentMappingList
				//						.Where(x => x.ContentId > 0).ToList());
				//if (MappingData.Any())
				//{
				//	foreach (var updateData in MappingData)
				//	{
				//		_unitOfWork.Repository<ObservationContentMapping>().Update(updateData);
				//		_unitOfWork.SaveChanges();
				//	}
				//}
				//MappingData = _mapper.Map<ObservationContentMapping[]>(dynamic.ObservationContentMappingList
				//						.Where(x => x.ContentId == null).ToList());
				//if (MappingData.Any())
				//{
				//	_unitOfWork.Repository<ObservationContentMapping>().InsertRange(MappingData);
				//	_unitOfWork.SaveChanges();
				//}
			}
			_unitOfWork.Commit();
			return new ResponseViewModel<DynamicViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - InsertObservation Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<DynamicViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = dynamic,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "InsertObservation"
			};
		}
	}


	public ResponseViewModel<LeverTypeDialViewModel> SubmitObservationReview(int observationId, DateTime reviewDate, int reviewStatus, int reviewedBy)
	{
		try
		{
			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
															 .GetQueryAsNoTracking(Q => Q.Id == observationId)
															 .SingleOrDefault();

			Instrument instrumentData = _unitOfWork.Repository<Instrument>()
																	.GetQueryAsNoTracking(Q => Q.Id == observationById.InstrumentId)
																	.SingleOrDefault();

			Request ReqstData = _unitOfWork.Repository<Request>()
																	.GetQueryAsNoTracking(Q => Q.Id == observationById.RequestId)
																	.SingleOrDefault();

			var numberList = GenerateULRAndCertificateNumber(instrumentData.IsNABL);


			_unitOfWork.BeginTransaction();
			observationById.CalibrationReviewedDate = reviewDate;
			observationById.ReviewStatus = reviewStatus;
			observationById.ULRNumber = numberList[0];
			observationById.CertificateNumber = numberList[1];
			observationById.CalibrationReviewedBy = reviewedBy;
			_unitOfWork.Repository<TemplateObservation>().Update(observationById);
			_unitOfWork.SaveChanges();

			RequestStatus reqestStatus = new RequestStatus();
			reqestStatus.RequestId = observationById.RequestId;
			reqestStatus.StatusId = reviewStatus == 1 ? (Int32)EnumRequestStatus.Sent : (Int32)EnumRequestStatus.Rejected;
			reqestStatus.CreatedOn = DateTime.Now;
			reqestStatus.CreatedBy = reviewedBy;
			_unitOfWork.Repository<RequestStatus>().Insert(reqestStatus);
			_unitOfWork.SaveChanges();

			//----------------------New update for listing Approved Request start---------------------------
			//Request Tempreqests = new Request();
			ReqstData.Id = observationById.RequestId;
			ReqstData.StatusId = reviewStatus == 1 ? (Int32)EnumRequestStatus.Sent : (Int32)EnumRequestStatus.Rejected;

			_unitOfWork.Repository<Request>().Update(ReqstData);
			_unitOfWork.SaveChanges();
			//----------------------New update for listing Approved Request end---------------------------

			//To Update ToolInventory Status
			if (instrumentData.ToolInventory != null && instrumentData.ToolInventory == "Yes")
			{
				if (ReqstData.TypeOfReqest == 2 || ReqstData.TypeOfReqest == 3)
				{
					if (reviewStatus == 1)
					{
						instrumentData.ToolInventoryStatus = (Int32)ToolInventoryStatus.ClosedTool;
						instrumentData.ToolRoomStatus = (Int32)ToolRoomStatus.Pending;
						instrumentData.ReplacementLabID = null;
					}
					else
					{
						instrumentData.ToolInventoryStatus = (Int32)ToolInventoryStatus.RejectedTool;

					}
				}
				else if (ReqstData.TypeOfReqest == 1)
				{
					instrumentData.ToolInventoryStatus = reviewStatus == 1 ? (Int32)ToolInventoryStatus.SentTool : (Int32)ToolInventoryStatus.RejectedTool;
				}
			}
			_unitOfWork.Repository<Instrument>().Update(instrumentData);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			return new ResponseViewModel<LeverTypeDialViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{

			ErrorViewModelTest.Log("ObservationTemplateService - SubmitObservationReview Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			_unitOfWork.RollBack();
			return new ResponseViewModel<LeverTypeDialViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "SubmitObservationReview"
			};
		}
	}
	public ResponseViewModel<ObservationContentViewModel>GetObservationContentSelectedList (List<Contentids> Contents,int InstrumentId,int TemplateObservationId)
	{
		try
		{

			StringBuilder Contentdata = new StringBuilder();
			Contentdata.Append("<Root>");
			foreach (var Contentlist in Contents)
			{
				Contentdata.Append("<ContentList>");
				Contentdata.Append(string.Format("<ContentId>{0}</ContentId>", (Int32)Contentlist.ContentId));
				//Contentdata.Append(string.Format("<InstrumentId>{0}</InstrumentId>", (Int32)Contentlist.InstrumentId));
				//Contentdata.Append(string.Format("<RequestId>{0}</RequestId>", (Int32)Contentlist.RequestId));
				
				Contentdata.Append("</ContentList>");
			}
			Contentdata.Append("</Root>");

			DataSet dsObservationContent = GetObsContentList(Contentdata.ToString(), InstrumentId, TemplateObservationId);
			List<ObservationContentViewModel> ObservationInstrument = new List<ObservationContentViewModel>();
			if (dsObservationContent != null && dsObservationContent.Tables.Count > 0 && dsObservationContent.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsObservationContent.Tables[0].Rows)
				{
					ObservationContentViewModel ContentViewModel = new ObservationContentViewModel
					{

						ObservationTemplate = Convert.ToInt32(dr["ObservationTemplate"]),
						ObservationType = Convert.ToInt32(dr["ObservationType"]),
						ContentName = dr["ContentName"].ToString(),
						ContentValue = dr["ContentValue"].ToString(),
						ContentCount = dr["ContentCount"].ToString(),
						ContentTitle1 = dr["ContentTitle1"].ToString(),
						ContentTitle2 = dr["ContentTitle2"].ToString(),
						ContentSubTitle1 = dr["ContentSubTitle1"].ToString(),
						ContentSubTitle2 = dr["ContentSubTitle2"].ToString(),
						ContentSubTitle3 = dr["ContentSubTitle3"].ToString(),
						ContentSubTitle4 = dr["ContentSubTitle4"].ToString(),
						ContentSubTitle5 = dr["ContentSubTitle5"].ToString(),
						Id = Convert.ToInt32(dr["Id"]),
						TypeOfContent = dr["TypeOfContent"].ToString(),
						//////////////////test////////////////
						ObsContentValueId = Convert.ToInt32(dr["ObsContentValueId"]),
						ParentId = Convert.ToInt32(dr["ParentId"]),
						Sno = Convert.ToInt32(dr["Sno"]),
						MeasuedValue = dr["MeasuedValue"].ToString(),
						ActualValue = dr["ActualValue"].ToString(),
						InstrumentError = dr["InstrumentError"].ToString(),
						Diff = dr["Diff"].ToString(),
						MeasuedValue1 = dr["MeasuedValue1"].ToString(),
						MeasuedValue2 = dr["MeasuedValue2"].ToString(),
						MeasuedValue3 = dr["MeasuedValue3"].ToString(),
						Average = dr["Average"].ToString(),
						Percent = dr["Percent"].ToString(),
						ContentId = Convert.ToInt32(dr["ContentId"]),
						ContentMappingId = Convert.ToInt32(dr["ContentMappingId"])

					};
					ObservationInstrument.Add(ContentViewModel);
				}
			}
			return new ResponseViewModel<ObservationContentViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = ObservationInstrument
			};

		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("InstrumentService - GetObservationContentSelectedList Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			_unitOfWork.RollBack();
			return new ResponseViewModel<ObservationContentViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "GetObservationContentSelectedList"
			};
		}
	}
	public DataSet GetObsContentList(string Contents, int InstrumentId, int TemplateObservationId)//, int deptid)
	{
		var connectionString = _configuration.GetConnectionString("CMTDatabase");
		SqlCommand cmd = new SqlCommand("GetObservationContentSelectedList");
		cmd.CommandType = CommandType.StoredProcedure;

		cmd.Parameters.AddWithValue("@Content", Contents);
		cmd.Parameters.AddWithValue("@InstrumentId", InstrumentId);
		cmd.Parameters.AddWithValue("@TemplateObservationId", TemplateObservationId);
		SqlConnection sqlConn = new SqlConnection(connectionString);
		DataSet dsResults = new DataSet();
		SqlDataAdapter sqlAdapter = new SqlDataAdapter();
		cmd.Connection = sqlConn;
		cmd.CommandTimeout = 2000;
		sqlAdapter.SelectCommand = cmd;
		sqlAdapter.Fill(dsResults);
		return dsResults;
	}

	#endregion

	public ResponseViewModel<ExternalObsViewModel> GetExternalObsById(int requestId, int instrumentId)
	{
		try
		{
			ExternalObsViewModel metalViewModel = _unitOfWork.Repository<TemplateObservation>()
			.GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)
			.Select(s => new ExternalObsViewModel()
			{
				Id = s.Id,
				TempStart = s.TempStart,
				TempEnd = s.TempEnd,
				Humidity = s.Humidity,
				RefWi = s.RefWi,
				Units = s.Allvalues,
				CalibrationPerformedDate = s.CreatedOn,
				CreatedBy = s.CreatedBy,
				CalibrationReviewedBy = s.CalibrationReviewedBy,
				CalibrationReviewedDate = s.CalibrationReviewedDate,
				ReviewStatus = s.ReviewStatus,
				ExternalObsCondition = s.InstrumentCondition,
				AdminReviewStatus = s.ExternalObsStatus,
				ULRNumber = s.ULRNumber
			}).SingleOrDefault();

			if (metalViewModel == null)
			{
				return new ResponseViewModel<ExternalObsViewModel>
				{
					ResponseCode = 200,
					ResponseMessage = "No records found",
					ResponseData = null,
					ResponseDataList = null
				};
			}
			else
			{

				List<string> performedUserData = GetUserName(metalViewModel.CreatedBy);

				if (performedUserData.Count >= 3)
				{
					metalViewModel.CalibrationPerformedBy = performedUserData[0];
					metalViewModel.PerformedBySign = performedUserData[1];
					metalViewModel.PerformedByDesignation = performedUserData[2];
				}

				List<string> reviewedUserData = GetUserName(metalViewModel.CalibrationReviewedBy);

				if (reviewedUserData.Count >= 3)
				{
					metalViewModel.ReviewedBy = reviewedUserData[0];
					metalViewModel.ReviewedBySign = reviewedUserData[1];
					metalViewModel.ReviewedByDesignation = reviewedUserData[2];
				}

				int? ulrNumber = metalViewModel.ULRNumber == null ? 0 : metalViewModel.ULRNumber;
				// int? certificateNumber = metalViewModel.CertificateNumber == null ? 0 : metalViewModel.CertificateNumber;
				//List<string> formatList = GetULRAndCertificateNumber(ulrNumber, certificateNumber);

				//if (formatList.Count >= 2)
				//{
				//    metalViewModel.ULRFormat = formatList[0];
				//    metalViewModel.CertificateFormat = formatList[1];
				//}
			}
			return new ResponseViewModel<ExternalObsViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = metalViewModel,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("ObservationTemplateService - GetMetalRulesId Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ExternalObsViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "GetMetalRulesId"
			};
		}
	}

	public ResponseViewModel<ExternalObsViewModel> InsertExternalObs(ExternalObsViewModel exObs)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			int tempobsId = 0;
			int ObjMicroData = 0;
			TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
																 .GetQueryAsNoTracking(Q => Q.RequestId == exObs.RequestId
																  && Q.InstrumentId == exObs.InstrumentId).SingleOrDefault();

			if ((exObs.TemplateObservationId == 0) && (observationById == null))

			{
				TemplateObservation templateObservation = new TemplateObservation()
				{
					InstrumentId = exObs.InstrumentId,
					RequestId = exObs.RequestId,
					TempStart = exObs.TempStart,
					TempEnd = exObs.TempEnd,
					Humidity = exObs.Humidity,
					InstrumentCondition = exObs.ExternalObsCondition,
					RefWi = exObs.RefWi,
					Allvalues = exObs.Allvalues,
					CreatedOn = DateTime.Now,
					CreatedBy = exObs.CreatedBy,
					CalibrationReviewedDate = DateTime.Now,	
					ExternalObsStatus = exObs.AdminReviewStatus,
                //Units = exObs.Allvalues,
            };
				if(exObs.AdminReviewStatus == 2)
				{
					templateObservation.Remarks = exObs.AdRemarks;
					templateObservation.CalibrationResult = "Not OK In Observation Template";
				}

				_unitOfWork.Repository<TemplateObservation>().Insert(templateObservation);
				_unitOfWork.SaveChanges();
				tempobsId = templateObservation.Id;

				if (exObs.AdminReviewStatus == 2)
				{
					RequestStatus reqestStatus = new RequestStatus();
					reqestStatus.RequestId = exObs.RequestId;
					reqestStatus.StatusId = (Int32)EnumRequestStatus.Rejected;
					reqestStatus.CreatedOn = DateTime.Now;
					reqestStatus.CreatedBy = exObs.CreatedBy;
					reqestStatus.Comment = exObs.AdRemarks;
					_unitOfWork.Repository<RequestStatus>().Insert(reqestStatus);
                    _unitOfWork.SaveChanges();

                    Request requestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == exObs.RequestId).SingleOrDefault();
					requestById.StatusId = (Int32)EnumRequestStatus.Rejected;
					requestById.ReceivedBy = exObs.CreatedBy;
					requestById.ReceivedDate = DateTime.Now;
					_unitOfWork.Repository<Request>().Update(requestById);
					_unitOfWork.SaveChanges();
				}

            }
			else
			{
				if (observationById != null)
				{


					if (exObs.TempStart != null)
					{
						observationById.TempStart = exObs.TempStart;
					}

					if (exObs.TempEnd != null)
					{
						observationById.TempEnd = exObs.TempEnd;
					}

					if (exObs.Humidity != null)
					{
						observationById.Humidity = exObs.Humidity;
					}

					if (exObs.ExternalObsCondition != null)
					{
						observationById.InstrumentCondition = exObs.ExternalObsCondition;
					}
					if (exObs.RefWi != null)
					{
						observationById.RefWi = exObs.RefWi;
					}
					if (exObs.Allvalues != null)
					{
						observationById.Allvalues = exObs.Allvalues;
					}
					if (exObs.AdminReviewStatus != null)
					{
						observationById.ExternalObsStatus = exObs.AdminReviewStatus;
					}

					_unitOfWork.Repository<TemplateObservation>().Update(observationById);
					_unitOfWork.SaveChanges();
				}
			}

			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			return new ResponseViewModel<ExternalObsViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("ObservationTemplateService - InsertMicrometer Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ExternalObsViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = exObs,
				ResponseDataList = null,
				ResponseService = "ObservationTemplateService",
				ResponseServiceMethod = "InsertMicrometer"
			};
		}
	}

    public ResponseViewModel<CertificateViewModel> GetTemplateObservationById(int requestId, int instrumentId)
    {
        try

		{  //.GetQueryAsNoTracking(Q => Q.RequestId == requestId && Q.InstrumentId == instrumentId)

			CertificateViewModel templateObservation = _unitOfWork.Repository<TemplateObservation>()
                                                                    .GetQueryAsNoTracking(Q => Q.InstrumentId == instrumentId)
																	 
                                                    .Select(s => new CertificateViewModel()
                                                    {
                                                        Id = s.Id,
                                                        TempStart = s.TempStart,
                                                        TempEnd = s.TempEnd,
                                                        Humidity = s.Humidity,
                                                        RefWi = s.RefWi,
                                                        Allvalues = s.Allvalues,
                                                        ReviewStatus = s.ReviewStatus,
                                                        CreatedBy = s.CreatedBy,
                                                        ULRNumber = s.ULRNumber,
                                                        CertificateNumber = s.CertificateNumber,
                                                        CalibrationReviewedBy = s.CalibrationReviewedBy,
                                                        //CalibrationPerformedDate = s.CreatedOn,
                                                        CalibrationReviewedDate = s.CalibrationReviewedDate,
                                                        InstrumentCondition = s.InstrumentCondition,
                                                        ExternalObsStatus = s.ExternalObsStatus,
                                                        CalibrationResult = s.CalibrationResult,
                                                        Remarks = s.Remarks,
                                                    }).SingleOrDefault();

            if (templateObservation != null)
            {
                List<string> performedUserData = GetUserName(templateObservation.CreatedBy);
                if (performedUserData.Count >= 3)
                {
                    templateObservation.CalibrationPerformedBy = performedUserData[0];
                    templateObservation.PerformedBySign = performedUserData[1];
                    templateObservation.PerformedByDesignation = performedUserData[2];
                }
                List<string> reviewedUserData = GetUserName(templateObservation.CalibrationReviewedBy);

                if (reviewedUserData.Count >= 3)
                {
                    templateObservation.ReviewedBy = reviewedUserData[0];
                    templateObservation.ReviewedBySign = reviewedUserData[1];
                    templateObservation.ReviewedByDesignation = reviewedUserData[2];
                }

                int? ulrNumber = templateObservation.ULRNumber == null ? 0 : templateObservation.ULRNumber;
                int? certificateNumber = templateObservation.CertificateNumber == null ? 0 : templateObservation.CertificateNumber;
                List<string> formatList = GetULRAndCertificateNumber(ulrNumber, certificateNumber);

                if (formatList.Count >= 2)
                {
                    templateObservation.ULRFormat = formatList[0];
                    templateObservation.CertificateFormat = formatList[1];
                }
            }

            #region
            /*
            if (plungerDialViewModel != null)
            {
                List<string> performedUserData = GetUserName(plungerDialViewModel.CreatedBy);

                if (performedUserData.Count >= 3)
                {
                    plungerDialViewModel.CalibrationPerformedBy = performedUserData[0];
                    plungerDialViewModel.PerformedBySign = performedUserData[1];
                    plungerDialViewModel.PerformedByDesignation = performedUserData[2];
                }

                List<string> reviewedUserData = GetUserName(plungerDialViewModel.CalibrationReviewedBy);

                if (reviewedUserData.Count >= 3)
                {
                    plungerDialViewModel.ReviewedBy = reviewedUserData[0];
                    plungerDialViewModel.ReviewedBySign = reviewedUserData[1];
                    plungerDialViewModel.ReviewedByDesignation = reviewedUserData[2];
                }

                int? ulrNumber = plungerDialViewModel.ULRNumber == null ? 0 : plungerDialViewModel.ULRNumber;
                int? certificateNumber = plungerDialViewModel.CertificateNumber == null ? 0 : plungerDialViewModel.CertificateNumber;
                List<string> formatList = GetULRAndCertificateNumber(ulrNumber, certificateNumber);

                if (formatList.Count >= 2)
                {
                    plungerDialViewModel.ULRFormat = formatList[0];
                    plungerDialViewModel.CertificateFormat = formatList[1];
                }
            }
			*/
            #endregion

            return new ResponseViewModel<CertificateViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = templateObservation,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            ErrorViewModelTest.Log("ObservationTemplateService - GetTemplateObservationById Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            return new ResponseViewModel<CertificateViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "ObservationTemplateService",
                ResponseServiceMethod = "GetTemplateObservationById"
            };
        }
    }

    public ResponseViewModel<CertificateViewModel> SaveCertificateTemp(int requestId, int instrumentId, string EnvironmentCondition, int userId, string exportData)
    {
        try
        {

            _unitOfWork.BeginTransaction();



            //TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
            //                                                     .GetQueryAsNoTracking(Q => Q.RequestId == requestId
            //                                                                            && Q.InstrumentId == instrumentId)
            //                                                     .SingleOrDefault();

            //observationById.CalibrationResult = CalibrationResult;
            //observationById.Remarks = Remarks;
            //_unitOfWork.Repository<TemplateObservation>().Update(observationById);
            //_unitOfWork.SaveChanges();


            //ObsTemplateLeverTypeDial leverTypeDialById = _unitOfWork.Repository<ObsTemplateLeverTypeDial>()
            //                                                        .GetQueryAsNoTracking(Q => Q.ObservationId == observationById.Id)
            //                                                        .SingleOrDefault();
            //leverTypeDialById.EnvironmentCondition = EnvironmentCondition;
            //leverTypeDialById.Uncertainity = Uncertainity;
            //leverTypeDialById.CalibrationResult = CalibrationResult;
            //leverTypeDialById.Remarks = Remarks;
            //_unitOfWork.Repository<ObsTemplateLeverTypeDial>().Update(leverTypeDialById);
            //_unitOfWork.SaveChanges();


            ResponseViewModel<QRCodeFilesViewModel> qrCodeResponseData = InsertQRCodeFiles(requestId, instrumentId, userId, exportData);

            if (qrCodeResponseData.ResponseCode == 500)
            {
                _unitOfWork.RollBack();
                return new ResponseViewModel<CertificateViewModel>
                {
                    ResponseCode = 500,
                    ResponseMessage = qrCodeResponseData.ResponseMessage,
                    ErrorMessage = qrCodeResponseData.ErrorMessage,
                    ResponseData = null,
                    ResponseDataList = null,
                    ResponseService = qrCodeResponseData.ResponseService,
                    ResponseServiceMethod = qrCodeResponseData.ResponseServiceMethod
                };
            }

            _unitOfWork.Commit();
            return new ResponseViewModel<CertificateViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            _unitOfWork.RollBack();
            ErrorViewModelTest.Log("ObservationTemplateService - SaveCertificateTemp Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            return new ResponseViewModel<CertificateViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "ObservationTemplateService",
                ResponseServiceMethod = "SaveCertificateTemp"
            };
        }
    }
}