using System;
using System.ComponentModel.DataAnnotations;

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
        public bool IsEntireAreaShown { get; set; }
    }

    public class UpdateTeacherSettingViewModel
    {
        [Required]
        public Guid Key { get; set; }
        [Required]
        public Guid ServiceTeacherKey { get; set; }
        [Required]
        public int ScheduleTeacherId { get; set; }
        public bool IsEntireAreaShown { get; set; }
    }
}
