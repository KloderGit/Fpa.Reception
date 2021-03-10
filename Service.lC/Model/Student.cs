using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Model
{
    public class Student : Base
    {
        public Guid Owner { get; set; }
        public IEnumerable<Contract> Contract { get; set; }
    }
}
