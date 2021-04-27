using reception.fitnesspro.ru.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Student.ViewModel
{
    public class StudentHistoryViewModel
    {
        public DateTime DateTime { get; set; }
        public BaseInfoViewModel Program { get; set; }
        public BaseInfoViewModel Discipline { get; set; }
        public BaseInfoViewModel Teacher { get; set; }
        public BaseInfoViewModel Rate { get; set; }
        public string Comment { get; set; }
    }
}
