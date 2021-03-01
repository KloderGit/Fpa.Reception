using Service.lC.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Dto
{
    public class ProgramDto : IConvert<ProgramDto>
    {
        public Guid EducationFormKey { get; set; }
        public IEnumerable<TeacherDto> Teachers { get; set; }
        public IEnumerable<DisciplineInfoDto> Disciplines { get; set; }

        public TResult ConvertTo<TResult>(Func<ProgramDto, TResult> converter)
        {
            var result = converter(this);
            return result;
        }
    }

    public class DisciplineInfoDto
    {
        public Guid DisciplineKey { get; set; }
        public Guid ControlTypeKey { get; set; }
    }

    public class TeacherDto
    {
        public Guid TeacherKey { get; set; }
    }
}
