using Domain;
using System;

namespace reception.fitnesspro.ru.Controllers.Reception.ViewModel
{
    public class ResultViewModel
    {
        public Guid TeacherKey { get; set; }
        public AttestationScoreType ScoreType { get; set; }
        public Object ScoreValue { get; set; }
        public string Comment { get; set; }
    }
}