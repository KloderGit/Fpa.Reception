using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Model
{
    public class Group : Base
    {
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }

        public IEnumerable<Base> SubGroup { get; set; }
    }
}
