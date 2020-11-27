using System;

namespace Services.Dto.Shared
{
    public class BaseDto
    {
        public string CreatedBy { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }
    }

    public interface IBaseDto
    {
        string CreatedBy { set; get; }

        DateTime? CreatedDate { set; get; }

        string UpdatedBy { set; get; }

        DateTime? UpdatedDate { set; get; }
    }
}
