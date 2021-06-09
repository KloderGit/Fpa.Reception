using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Service.MongoDB.Model
{
    [BsonIgnoreExtraElements]
    [BsonCollection("StudentSetting")]
    public class StudentSetting : IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; } = DateTime.Now;
        public Guid Key { get; set; } = Guid.NewGuid();
        public Guid StudentKey { get; set; }
        public IEnumerable<DisciplineSetting> DisciplineSettings { get; set; }
    }

    public class DisciplineSetting
    {
        public Guid DisciplineKey { get; set; }
        public int? SignUpCount;
        public int? SignOutCount;
        public DateTime? LastDaySetting;
    }
}
