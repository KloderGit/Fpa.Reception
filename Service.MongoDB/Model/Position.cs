using System;
using System.Collections.Generic;

namespace Service.MongoDB.Model
{
    public class Position
    {
        public Guid Key { get; set; }
        public bool IsActive { get; set; }
        public DateTime Time { get; set; }
        public Record Record { get; set; }
        public IEnumerable<History> Histories { get; set; } = new List<History>();
    }
}