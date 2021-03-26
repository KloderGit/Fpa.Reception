using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Service.MongoDB.Model
{
    [BsonIgnoreExtraElements]
    [BsonCollection("Constraints")]

    public class Constraint : IDocument
    {
        public ObjectId Id { get; set; }
        public Guid Key { get; set; } = Guid.NewGuid();
        public Guid ProgramKey { get; set; }
        public Guid DisciplineKey { get; set; }
        public int ActiveForDays { get; set; }
        public int AllowedAttemptCount { get; set; }
        public IEnumerable<Guid> DependsOnOtherDisciplines { get; set; }
        public DateTime CreatedAt { get; } = DateTime.Now;
    }
}
