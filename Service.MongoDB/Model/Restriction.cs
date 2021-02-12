using System;

namespace Service.MongoDB.Model
{
    public class Restriction
    {
        public Guid Program { get; set; }
        public Guid Group { get; set; }
        public Guid SubGroup { get; set; }
        public Option Option { get; set; }
    }
}