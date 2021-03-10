using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Education
{
    public class Group : BaseInfo
    {
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }

        public IEnumerable<BaseInfo> SubGroups { get; set; }
    }
}