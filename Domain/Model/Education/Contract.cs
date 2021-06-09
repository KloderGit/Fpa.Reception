using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Model.Education
{
    public class Contract
    {
        public IEnumerable<Guid> Students { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime StartEducationDate { get; set; }
        public DateTime FinishEducationhDate { get; set; }
        public BaseInfo EducationProgram { get; set; }
        public BaseInfo Group { get; set; }
        public BaseInfo SubGroup { get; set; }

        public bool IsContractForStudent(Guid studentKey)
        {
            if(Students == default || Students.Any() == false) return false;

            return Students.Any(x=>x == studentKey);
        }

        public bool IsContractExpiredForDay(DateTime date)
        {
            if (ExpiredDate != default && ExpiredDate.Date < date.Date) return true;
            return false;
        }

        public bool IsDateInPeriodFromStart(DateTime date, int daysFromStart)
        {
            var limitDate = StartEducationDate.AddDays(daysFromStart);
            var isLowerThenDate = limitDate < date.Date;

            if (isLowerThenDate) return false;

            return true;
        }
    }
}
