using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Service.MongoDB.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.MongoDB.Model
{
    [BsonIgnoreExtraElements]
    [BsonCollection("Receptions")]
    public class Reception : IDocument
    {
        private DateTime date;

        public ObjectId Id { get; set; }
        public Guid Key { get; set; } = Guid.NewGuid();
        public bool IsActive { get; set; }
        public DateTime Date { 
            get => date.SetKind(DateTimeKind.Utc);
            set => date = value;                
         }
        public IEnumerable<Event> Events { get; set; } = new List<Event>();
        public PositionManager PositionManager { get; set; }
        public IEnumerable<History> Histories { get; set; } = new List<History>();
        public DateTime CreatedAt { get; } = DateTime.Now;
    }
}
