using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Employee : BaseInfo
    {
        public List<Event> Event { get;set;} = new List<Event>();
    }
}
