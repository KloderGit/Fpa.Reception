using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Settings
{
    public class GetAllTeacherSettingViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class AddTeacherSettingViewModel
    {
        [Required]
        public Guid ServiceTeacherKey { get; set; }
        [Required]
        public int ScheduleTeacherId { get; set; }
    }

    public class UpdateTeacherSettingViewModel
    {
        [Required]
        public Guid Key { get; set; }
        [Required]
        public Guid ServiceTeacherKey { get; set; }
        [Required]
        public int ScheduleTeacherId { get; set; }
    }
}
