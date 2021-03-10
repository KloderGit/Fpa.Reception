using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Employee
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
        public List<Event> Event { get;set;} = new List<Event>();
    }
}
