using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.MongoDB.Model
{
    [BsonIgnoreExtraElements]
    [BsonCollection("Receptions")]
    public class ReceptionDto : IDocument
    {
        public ObjectId Id { get; set; }
        public Guid Key { get; set; }
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<ReceptionPayloadDto> Events { get; set; } = new List<ReceptionPayloadDto>();
        public PositionManagerDto PositionManager { get; set; }
        public IEnumerable<HistoryDto> Histories { get; set; } = new List<HistoryDto>();
        public DateTime CreatedAt { get; }
    }
}
