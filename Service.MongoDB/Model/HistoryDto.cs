using System;

namespace Service.MongoDB.Model
{
    public class HistoryDto
    {
        public Guid Object { get; set; }
        public string Action { get; set; }
        public Guid Subject { get; set; }
        public DateTime DateTime { get; set; }
    }
}