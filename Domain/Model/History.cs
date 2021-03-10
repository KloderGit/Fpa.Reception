using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class History
    {
        public Guid Object { get; set; }
        public string Action { get; set; }
        public Guid Subject { get; set; }
        public DateTime DateTime { get; set; }
    }
}
