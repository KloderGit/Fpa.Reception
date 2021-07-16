using Service.lC.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Model
{
   public class AttestationTable : Base, IConvert<AttestationTable>
    {
        public bool DeletionMark { get; set; }
        public DateTime Date { get; set; }
        public DateTime AttestationDate { get; set; }
        public Guid DisciplineKey { get; set; }
        public Guid TeacherKey { get; set; }
        public Guid ProgramKey { get; set; }
        public Guid ControlTypeKey { get; set; }
        public Guid GroupKey { get; set; }
        public IEnumerable<AttestationStudent> Registry { get; set; }

        public TResult ConvertTo<TResult>(Func<AttestationTable, TResult> converter)
        {
            var result = converter(this);
            return result;
        }
    }

    public class AttestationStudent
    {
        public string LineNumber { get; set; }
        public Guid StudentKey { get; set; }
        public Guid Rate { get; set; }
        public string Comment { get; set; }
        public bool IsExcelentStudent { get; set; }
    }
}
