using System;
using System.ComponentModel.DataAnnotations;

namespace reception.fitnesspro.ru.Controllers.Student.ViewModel
{
    public class StudentResultViewModel
    {
        [Required(ErrorMessage ="Не указано в которое записан студент")]
        public Guid PositionKey { get; set; }
        [Required(ErrorMessage ="Не указан принимающий преподаватель")]
        public Guid TeacherKey { get; set; }
        [Required(ErrorMessage ="Не указана полученная оценка")]
        public Guid RateKey { get; set; }
        public string Comment { get; set; }
    }
}
