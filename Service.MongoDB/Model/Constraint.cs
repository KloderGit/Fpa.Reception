using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Service.MongoDB.Model
{
    [BsonIgnoreExtraElements]
    [BsonCollection("BaseConstraints")]
    public class BaseConstraintDto : IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; }
        public Guid Key { get; set; } = Guid.NewGuid();
        public Guid ProgramKey { get; set; }
        public Guid DisciplineKey { get; set; }
        public int AllowedAttempts { get; set; }
        public IEnumerable<BaseInfo> DependsOn { get; set; }
        public bool CheckContract { get; set; }
        public int SignUpBeforeMinutes { get; set; }
        public int SignOutBeforeMinutes { get; set; }
        
        public UISettings UISettings { get; set; }
    }

    public class UISettings
    {
        public IEnumerable<PositionTypeDto> PositionTypes { get; set; }
        public IEnumerable<BaseInfo> Teachers { get; set; }
        public int OncePerMinutes { get; set; }
        public int StudentsNumber { get; set; }
    }
}
