using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Service.MongoDB.Model
{
    [BsonIgnoreExtraElements]
    [BsonCollection("TeacherSetting")]
    public class TeacherSetting : IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; } = DateTime.Now;
        public Guid Key { get; set; } = Guid.NewGuid();
        public Guid ServiceTeacherKey { get; set; }
        public int ScheduleTeacherId { get; set; }
        public bool IsEntireAreaShown { get; set; }
    }
}
