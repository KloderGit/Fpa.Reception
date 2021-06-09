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

        Task<IEnumerable<ScheduleProgramInfo>> GetAllScheduleGroups();
        Task<IEnumerable<Domain.Education.Program>> GetAllServiceGroups();
        Task<IEnumerable<GroupSettings>> GetAllGroupSettings();
        Task<GroupSettings> GetGroupSettings(Guid groupSettingKey);
        Task<GroupSettings> FindGroupSettings(Guid serviceGroupKey);
        Task<Guid> AddGroupSettings(GroupSettings model);
        Task UpdateGroupSettings(GroupSettings model);
        Task DeleteGroupSettings(Guid key);
        Task<StudentSetting> GetStudentSetting(Guid studentKey);
        Task<Guid> AddStudentSetting(StudentSetting model);
        Task UpdateStudentSetting(StudentSetting model);
    }
}
