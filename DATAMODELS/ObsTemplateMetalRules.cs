using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMT.DATAMODELS
{
	public class ObsTemplateMetalRules
	{
		public int Id { get; set; }
		public int? ParentId { get; set; }

        public int? TemplateObservationId { get; set; }

        public int? SquarenessMeasued { get; set; }

        public int? SquarenessActuals { get; set; }

        public int? SquarenessInstrumentError { get; set; }
        public int? CalibrationReviewedBy { get; set; }
		public DateTime? CalibrationReviewedDate { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }

		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }

	}
}
