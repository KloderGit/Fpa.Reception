using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ISettingComponent
    {
        Task<IEnumerable<Tuple<int, string>>> GetAllScheduleTeachers();
        Task<IEnumerable<BaseInfo>> GetAllServiceTeachers();
        Task<IEnumerable<TeacherSetting>> GetAllTeacherSettings();
        Task<TeacherSetting> GetTeacherSettings(Guid serviceTeacherKey);
        Task<Guid> AddTeacherSettings(TeacherSetting model);
        Task UpdateTeacherSettings(TeacherSetting model);
        Task DeleteTeacherSettings(Guid key);

        IEnumerable<BaseConstraint> Get(IEnumerable<Guid> constraintKeys);
        BaseConstraint GetByKey(Guid key);
        IEnumerable<Domain.BaseConstraint> Find(Guid? programKey, Guid disciplineKey);
        IEnumerable<BaseConstraint> GetAll();
        Guid Store(BaseConstraint constraint);
        void Update(BaseConstraint constraint);
    }
}
