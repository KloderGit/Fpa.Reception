using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Schedule.MySql.Model
{
    public class ProgramInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<GroupInfo> Groups { get;set; }
    }
}
