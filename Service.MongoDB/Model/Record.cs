using System;

namespace Service.MongoDB.Model
{
    public class Record
    {
        public Guid StudentKey { get; set; }
        public Guid ProgramKey { get; set; }
        public Guid DisciplineKey { get; set; }
        public Result Result { get; set; }
    }
}