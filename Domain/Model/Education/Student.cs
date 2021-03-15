﻿using System;
using System.Collections.Generic;

namespace Domain.Model.Education
{
    public class Student : BaseInfo
    {
        public List<StudentEducation> Educations { get; set; }

        public class StudentEducation
        {
            public BaseInfo EducationProgram { get; set; }
            public BaseInfo Group { get; set; }
            public BaseInfo SubGroup { get; set; }
            public BaseInfo EducationForm { get; set; }
            public StudentContract Contract { get; set; }
        }

        public class StudentContract
        {
            public Guid Key { get; set; }
            public DateTime ExpiredDate { get; set; }
            public DateTime StartEducationDate { get; set; }
            public DateTime FinishEducationDate { get; set; }
        }
    }
}