using System;
using DATAMODELS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CMT.DATAMODELS
{
    public partial class CMTDatabaseContext : DbContext
    {
        public CMTDatabaseContext()
        {
        }

        public CMTDatabaseContext(DbContextOptions<CMTDatabaseContext> options)
            : base(options)
        {
        }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Lovs> Lovs { get; set; }
		public virtual DbSet<Location> Location { get; set; }
		public virtual DbSet<Instrument> Instrument { get; set; }
        public virtual DbSet<Master> Master { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<ExternalRequest> ExternalRequest { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<MasterQuarantine> MasterQuarantine { get; set; }
        public virtual DbSet<Uploads> Uploads { get; set; }
        public virtual DbSet<MasterFileUpload> MasterFileUpload { get; set; }
        public virtual DbSet<ExternalRequestStatus> ExternalRequestStatus { get; set; }
        public virtual DbSet<InstrumentQuarantine> InstrumentQuarantine { get; set; }
        public virtual DbSet<InstrumentFileUpload> InstrumentFileUpload { get; set; }
        public virtual DbSet<RequestStatus> RequestStatus { get; set; }
        public virtual DbSet<FeedbackData> FeedbackData { get; set; }
        public virtual DbSet<FeedbackInvite> FeedbackInvite { get; set; }
        public virtual DbSet<ObsTemplateMicrometer> ObsTemplateMicrometer { get; set; }
        public virtual DbSet<ObsTemplateLeverTypeDial> ObsTemplateLeverTypeDial { get; set; }
        public virtual DbSet<ObsTemplateVernierCaliper> ObsTemplateVernierCaliper { get; set; }
        public virtual DbSet<ObsTemplatePlungerDial> ObsTemplatePlungerDial { get; set; }
        public virtual DbSet<ObsTemplateThreadGauges> ObsTemplateThreadGauges { get; set; }
        public virtual DbSet<ObsTemplateTWobs> ObsTemplateTWobs { get; set; }
        public virtual DbSet<ObsTemplateGeneral> ObsTemplateGeneral { get; set; }
        public virtual DbSet<QCAlternateMethodTemplate> QCAlternateMethodTemplate { get; set; }
        public virtual DbSet<QCAlternateMethodTemplateData> QCAlternateMethodTemplateData { get; set; }
        public virtual DbSet<QCReplicateTestTemplate> QCReplicateTestTemplate { get; set; }
        public virtual DbSet<QCReplicateTestTemplateData> QCReplicateTestTemplateData { get; set; }
        public virtual DbSet<QCReTestTemplate> QCReTestTemplate { get; set; }
        public virtual DbSet<QCReTestTemplateData> QCReTestTemplateData { get; set; }
        public virtual DbSet<TemplateObservation> TemplateObservation { get; set; }
        public virtual DbSet<QCIntermediateTemplate> QCIntermediateTemplate { get; set; }
        public virtual DbSet<QCIntermediateTemplateResult> QCIntermediateTemplateResult { get; set; }
        public virtual DbSet<ObsGeneralMeasuredValues> ObsGeneralMeasuredValues { get; set; }
        public virtual DbSet<ObsGeneralDynamicValues> ObsGeneralDynamicValues { get; set; }
        public virtual DbSet<MasterEquipmentHistory> MasterEquipmentHistory { get; set; }
        public virtual DbSet<ObsTemplateGeneralNew> ObsTemplateGeneralNew { get; set; }
		public virtual DbSet<ObsMicrometerValues> ObsMicrometerValues { get; set; }
		public virtual DbSet<QRCodeFiles> QRCodeFiles {get;set;}
		public virtual DbSet<ObsTemplateMetalRules> ObsTemplateMetalRules { get;set;}
		public virtual DbSet<ObsTemplateValues> ObsTemplateValues { get;set;}
		public virtual DbSet<UserDepartmentMapping> UserDepartmentMapping { get;set;}
		public virtual DbSet<UserRoleMapping> UserRoleMapping { get;set;}
		public virtual DbSet<UserRoles> UserRoles { get;set;}
		public virtual DbSet<ObservationContentValues> ObservationContentValues { get; set; }
		public virtual DbSet<ObservationContentMapping> ObservationContentMapping { get; set; }
		public virtual DbSet<EmailServiceStatus> EmailServiceStatus { get; set; }
		public virtual DbSet<ToolRoomMaster> ToolRoomMaster { get; set; }

		public virtual DbSet<ToolRoomHistory> ToolRoomHistory { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Master>()
            .HasOne(p => p.SupplierModel)
            .WithMany(b => b.Masters)
            .HasForeignKey(p => p.SupplierId);

            modelBuilder.Entity<MasterQuarantine>()
            .HasOne(p => p.Masters)
            .WithMany(b => b.QuarantineModel)
            .HasForeignKey(p => p.MasterId);


            modelBuilder.Entity<MasterFileUpload>()
            .HasOne(p => p.Masters)
            .WithMany(b => b.FileUploadModel)
            .HasForeignKey(p => p.MasterId);

            modelBuilder.Entity<MasterFileUpload>()
            .HasOne(p => p.Upload)
            .WithMany(b => b.MasterUpload)
            .HasForeignKey(p => p.UploadId);


            modelBuilder.Entity<ExternalRequest>()
            .HasOne(p => p.MasterModel)
            .WithMany(b => b.ExternalRequestModel)
            .HasForeignKey(p => p.MasterId);

            modelBuilder.Entity<ExternalRequestStatus>()
            .HasOne(p => p.ExternalRequestModel)
            .WithMany(b => b.ExternalRequestStatusModal)
            .HasForeignKey(p => p.ExternalRequestId);

            modelBuilder.Entity<ExternalRequestStatus>()
            .HasOne(p => p.UserModel)
            .WithMany(b => b.ExtStatus)
            .HasForeignKey(p => p.CreatedBy);

            modelBuilder.Entity<User>()
            .HasOne(p => p.Department)
            .WithMany(b => b.User)
            .HasForeignKey(p => p.DepartmentId);

			modelBuilder.Entity<Department>()
			.HasOne(p => p.Location)
			.WithMany(b => b.Department)
			.HasForeignKey(p => p.PlantId);

			modelBuilder.Entity<Request>()
            .HasOne(p => p.InstrumentModel)
            .WithMany(b => b.RequestModel)
            .HasForeignKey(p => p.InstrumentId);

			modelBuilder.Entity<RequestStatus>()
           .HasOne(p => p.RequestModel)
           .WithMany(b => b.RequestStatusModel)
           .HasForeignKey(p => p.RequestId);

            modelBuilder.Entity<RequestStatus>()
            .HasOne(p => p.UserModel)
            .WithMany(b => b.ReqStatus)
            .HasForeignKey(p => p.CreatedBy);

            modelBuilder.Entity<FeedbackInvite>()
            .HasOne(p => p.UserModel)
            .WithMany(b => b.FeedbackUsers)
            .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<FeedbackInvite>()
           .HasOne(p => p.InviteUserModel)
           .WithMany(b => b.FeedbackInviters)
           .HasForeignKey(p => p.InvitedBy);

            modelBuilder.Entity<FeedbackData>()
           .HasOne(p => p.FeedbackInviteModel)
           .WithMany(b => b.FeedbackUsers)
           .HasForeignKey(p => p.FeedbackInviteId);

            modelBuilder.Entity<FeedbackData>()
            .HasOne(p => p.UserModel)
            .WithMany(b => b.FeedbackDataReviewer)
            .HasForeignKey(p => p.ReviewedBy);

            modelBuilder.Entity<Instrument>()
            .HasOne(p => p.DepartmenttModel)
            .WithMany(b => b.Instrument)
            .HasForeignKey(p => p.UserDept);

            modelBuilder.Entity<Instrument>()
            .HasOne(p => p.UserModel)
            .WithMany(b => b.Instrument)
            .HasForeignKey(p => p.CreatedBy);

            modelBuilder.Entity<ObsTemplateLeverTypeDial>()
            .HasOne(p => p.Observation)
            .WithMany(b => b.LeverTypeDialModel)
            .HasForeignKey(p => p.ObservationId);

          modelBuilder.Entity<TemplateObservation>()
             .HasOne(p => p.CalibrationCreatedModel)
             .WithMany(b => b.TemplateObservatinoCreateData)
             .HasForeignKey(p => p.CreatedBy);

            modelBuilder.Entity<TemplateObservation>()
            .HasOne(p => p.CalibrationReviewedModel)
            .WithMany(b => b.TemplateObservationsREviewData)
            .HasForeignKey(p => p.CalibrationReviewedBy);

            modelBuilder.Entity<ObsTemplateMicrometer>()
            .HasOne(p => p.Observation)
            .WithMany(b => b.MicromerterModel)
            .HasForeignKey(p => p.ObservationId);

             modelBuilder.Entity<ObsTemplateThreadGauges>()
            .HasOne(p => p.Observation)
            .WithMany(b => b.ThreadGaugesDialModel)
            .HasForeignKey(p => p.ObservationId);

           modelBuilder.Entity<ObsTemplatePlungerDial>()
            .HasOne(p => p.Observation)
            .WithMany(b => b. PlungerDialModel)
            .HasForeignKey(p => p.ObservationId);

           modelBuilder.Entity<ObsTemplateTWobs>()
            .HasOne(p => p.Observation)
            .WithMany(b => b.TwosModel)
            .HasForeignKey(p => p.ObservationId);

           modelBuilder.Entity<ObsTemplateGeneral>()
            .HasOne(p => p.Observation)
            .WithMany(b => b.GeneralModel)
            .HasForeignKey(p => p.ObservationId);

            modelBuilder.Entity<ObsTemplateVernierCaliper>()
            .HasOne(p => p.Observation)
            .WithMany(b => b.VerniercaliperModel)
            .HasForeignKey(p => p.ObservationId);

            modelBuilder.Entity<ObsTemplateGeneralNew>()
            .HasOne(p => p.Observation)
            .WithMany(b => b.GeneralNewModel)
            .HasForeignKey(p => p.ObservationId);
            
             modelBuilder.Entity<Master>()
            .HasOne(p => p.Lovs)
            .WithMany(b => b.Master)
            .HasForeignKey(p => p.CalibFreqId);

			modelBuilder.Entity<Master>()
		   .HasOne(p => p.DepartmentModel)
		   .WithMany(b => b.MasterModel)
		   .HasForeignKey(p => p.DepartmentId);

			modelBuilder.Entity<ObsGeneralMeasuredValues>().Property(x => x.MeasuedValue).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<ObsGeneralMeasuredValues>().Property(x => x.Trial1).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<ObsGeneralMeasuredValues>().Property(x => x.Trial2).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<ObsGeneralMeasuredValues>().Property(x => x.Trial3).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<ObsGeneralMeasuredValues>().Property(x => x.Average).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<ObsGeneralMeasuredValues>().Property(x => x.Average).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCIntermediateTemplateResult>().Property(x => x.CalibrationResult).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCIntermediateTemplateResult>().Property(x => x.CurrentInternalCheck).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCIntermediateTemplateResult>().Property(x => x.DifferenceResult).HasColumnType("decimal(18, 6)");            
            modelBuilder.Entity<QCReplicateTestTemplate>().Property(x => x.EnValue).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReTestTemplate>().Property(x => x.EnValue).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplate>().Property(x => x.EnValue).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReplicateTestTemplateData>().Property(x => x.Average).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReplicateTestTemplateData>().Property(x => x.MU).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReTestTemplateData>().Property(x => x.Average).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReTestTemplateData>().Property(x => x.MU).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplate>().Property(x => x.Mux1AvgValue).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplate>().Property(x => x.Mux2AvgValue).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplate>().Property(x => x.Mux1).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplate>().Property(x => x.Mux2).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplate>().Property(x => x.Mux1SqrValue).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplate>().Property(x => x.Mux2SqrValue).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReplicateTestTemplateData>().Property(x => x.Observation1).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReplicateTestTemplateData>().Property(x => x.Observation2).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReplicateTestTemplateData>().Property(x => x.Observation3).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReplicateTestTemplateData>().Property(x => x.Observation4).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReplicateTestTemplateData>().Property(x => x.Observation5).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReplicateTestTemplateData>().Property(x => x.Observation6).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReplicateTestTemplateData>().Property(x => x.Observation7).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReplicateTestTemplateData>().Property(x => x.Observation8).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReplicateTestTemplateData>().Property(x => x.Observation9).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReplicateTestTemplateData>().Property(x => x.Observation10).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReplicateTestTemplateData>().Property(x => x.U2).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReTestTemplateData>().Property(x => x.Observation1).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReTestTemplateData>().Property(x => x.Observation2).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReTestTemplateData>().Property(x => x.Observation3).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReTestTemplateData>().Property(x => x.Observation4).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReTestTemplateData>().Property(x => x.Observation5).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReTestTemplateData>().Property(x => x.Observation6).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReTestTemplateData>().Property(x => x.Observation7).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReTestTemplateData>().Property(x => x.Observation8).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReTestTemplateData>().Property(x => x.Observation9).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReTestTemplateData>().Property(x => x.Observation10).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCReTestTemplateData>().Property(x => x.U2).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplateData>().Property(x => x.Val1).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplateData>().Property(x => x.Val2).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplateData>().Property(x => x.Val3).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplateData>().Property(x => x.Val4).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplateData>().Property(x => x.Val5).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplateData>().Property(x => x.Val6).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplateData>().Property(x => x.Val7).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplateData>().Property(x => x.Val8).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplateData>().Property(x => x.Val9).HasColumnType("decimal(18, 6)");
            modelBuilder.Entity<QCAlternateMethodTemplateData>().Property(x => x.Val10).HasColumnType("decimal(18, 6)");
        }
    }
}