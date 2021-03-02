using Service.lC.Interface;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.lC.Dto
{
    public class ProgramDto : BaseDto, IConvert<ProgramDto>
    {
        public Guid EducationFormKey { get; set; }
        public IEnumerable<TeacherDto> Teachers { get; set; }
        public IEnumerable<DisciplineInfoDto> Disciplines { get; set; }

        static ProgramDto()
        {
            Converter.Register<ProgramDto, Program>(Convert);
        }

        public TResult ConvertTo<TResult>(Func<ProgramDto, TResult> converter)
        {
            var result = converter(this);
            return result;
        }

        public static Program Convert(ProgramDto dto)
        {
            var program = new Program
            {
                Key = dto.Key,
                Title = dto.Title,
                EducationForm = new Base { Key = dto.EducationFormKey },
                Teachers = dto.Teachers?.Select(t => new Base { Key = t.TeacherKey }),
                Educations = dto.Disciplines?.Select(
                         d => new Education
                         {
                             Order = d.LineNumber,
                             Discipline = new Base { Key = d.DisciplineKey },
                             ControlType = new Base { Key = d.ControlTypeKey }
                         })
            };

            return program;
        }
    }

    public class DisciplineInfoDto
    {
        public int LineNumber { get; set; }
        public Guid DisciplineKey { get; set; }
        public Guid ControlTypeKey { get; set; }
    }

    public class TeacherDto
    {
        public Guid TeacherKey { get; set; }
    }
}
