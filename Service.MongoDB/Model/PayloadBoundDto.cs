using System;

namespace Service.MongoDB.Model
{
    public class PayloadConstraintDto
    {
        public Guid Program { get; set; }
        public Guid Group { get; set; }
        public Guid SubGroup { get; set; }
        public PayloadOptionDto Options { get; set; }
    }
}