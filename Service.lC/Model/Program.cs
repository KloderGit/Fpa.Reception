
using Service.lC.Interface;
using System;
using System.Collections.Generic;

namespace Service.lC.Model
{
    public class Program : Base, IConvert<Program>
    {
        public Base EducationForm { get; set; }
        public IEnumerable<Base> Teachers { get; set; }
        public IEnumerable<Education> Educations { get; set; }

        public TResult ConvertTo<TResult>(Func<Program, TResult> converter)
        {
            var result = converter(this);
            return result;
        }
    }

    public class Education
    {
        public int Order { get; set; }
        public Base Discipline { get; set; }
        public Base ControlType { get; set; }
    }
}