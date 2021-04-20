using reception.fitnesspro.ru.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Teacher.ViewModel
{
    public class TableControlTypeViewModel : BaseInfoViewModel
    { 
        public IEnumerable<ScoreInfo> RateType { get; set; }

        public class ScoreInfo
        {
            public string LineNumber { get; set; }
            public ScoreTypeViewModel RateKey { get; set; }
        }
    }

    public class ScoreTypeViewModel : BaseInfoViewModel
    {
        public Guid ParentKey { get; set; }
        public int MaxGrade { get; set; }
        public int MinGrade { get; set; }
        public string Grade { get; set; }

        public IEnumerable<BaseInfoViewModel> ScoreVariants { get; set; }
    }
}
