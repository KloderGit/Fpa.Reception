using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Schedule.MySql.Model;

namespace Service.Schedule.MySql
{
    public interface IScheduleService
    {
        Task<IEnumerable<EventInfo>> TeacherSchedule(int teacherId);
    }
}