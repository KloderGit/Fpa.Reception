using System.Collections.Generic;

namespace Service.MongoDB.Model
{
    public class ReceptionPayloadDto
    {
        public IEnumerable<BaseInfoDto> Teachers { get; set; } = new List<BaseInfoDto>();
        public BaseInfoDto Discipline { get; set; }
        public IEnumerable<PayloadRestrictionDto> Restrictions { get; set; } = new List<PayloadRestrictionDto>();
        public PayloadRequirementDto Requirement { get; set; }
    }
}
