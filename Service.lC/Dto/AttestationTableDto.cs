using Service.lC.Interface;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Dto
{
    public class AttestationTableDto : BaseDto, IConvert<AttestationTableDto>
    {
        public bool DeletionMark { get; set; }
        public DateTime Date { get; set; }
        public DateTime AttestationDate { get; set; }
        public Guid DisciplineKey { get; set; }
        public Guid TeacherKey { get; set; }
        public Guid ProgramKey { get; set; }
        public Guid ControlTypeKey { get; set; }
        public Guid GroupKey { get; set; }
        public ICollection<AttestationStudentDto> Registry { get; }

        public TResult ConvertTo<TResult>(Func<AttestationTableDto, TResult> converter)
        {
            var result = converter(this);
            return result;
        }
    }

    public class AttestationStudentDto
    {
        public string LineNumber { get; set; }
        public Guid StudentKey { get; set; }
        public Guid Rate { get; set; }
        public string Comment { get; set; }
        public bool IsExcelentStudent { get; set; }
    }
}
