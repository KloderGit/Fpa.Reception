using System;

namespace Service.MongoDB.Model
{
    public class Result
    {
        public Guid TeacherKey { get; set; }
        public Score Score { get; set; }
        public string Comment { get; set; }
    }
}