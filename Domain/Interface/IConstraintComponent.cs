using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interface
{
    public interface IConstraintComponent
    {
        IEnumerable<BaseConstraint> Get(IEnumerable<Guid> constraintKeys);
        BaseConstraint GetByKey(Guid key);
        IEnumerable<Domain.BaseConstraint> Find(Guid? programKey, Guid disciplineKey);
        IEnumerable<BaseConstraint> GetAll();
        Guid Store(BaseConstraint constraint);
        void Update(BaseConstraint constraint);
    }
}
