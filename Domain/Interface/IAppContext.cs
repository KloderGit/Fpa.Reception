using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interface
{
    public interface IAppContext
    {
        IEducationComponent Education { get; }
        IPersonComponent Person { get; }
        IStudentComponent Student { get; }
        ITeacherComponent Teacher { get; }
        IReceptionComponent Reception { get; }
        IConstraintComponent Constraint { get; }
        IValidateComponent Validator { get; }
    }
}
