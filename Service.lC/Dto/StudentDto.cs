using Service.lC.Interface;
using Service.lC.Model;
using System;

namespace Service.lC.Dto
{
    public class StudentDto : Base, IConvert<StudentDto>
    {
        public Guid PersonKey { get; set; }

        static StudentDto()
        {
            Converter.Register<StudentDto, Student>(Convert);
        }

        public TResult ConvertTo<TResult>(Func<StudentDto, TResult> converter)
        {
            var result = converter(this);
            return result;
        }

        public static Student Convert(StudentDto dto)
        {
            var student = new Student
            {
                Key = dto.Key,
                Title = dto.Title,
                Owner = dto.PersonKey
            };

            return student;
        }
    }
}
