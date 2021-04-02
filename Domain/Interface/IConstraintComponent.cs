using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interface
{
    public interface IConstraintComponent
    {
        IEnumerable<Constraint> Get(IEnumerable<Guid> constraintKeys);
        IEnumerable<Constraint> GetAll();
        void Store(Constraint constraint);
    }
}
