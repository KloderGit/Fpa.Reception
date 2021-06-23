using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Schedule.MySql.Model;

namespace Service.Schedule.MySql
{
    public interface IScheduleService
    {
        Task<IEnumerable<TeacherInfo>> GetTeachers();
        Task<IEnumerable<TeacherEventInfo>> TeacherSchedule(int teacherId);

        Task<IEnumerable<ProgramInfo>> GetGroups();
        Task<IEnumerable<GroupEventInfo>> GroupSchedule(int groupId);
    }
}