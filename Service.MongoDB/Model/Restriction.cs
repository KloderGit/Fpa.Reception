using System;

namespace Service.MongoDB.Model
{
    public class Restriction
    {
        public BaseInfo Program { get; set; }
        public BaseInfo Group { get; set; }
        public BaseInfo SubGroup { get; set; }
        public Option Option { get; set; }
    }
}