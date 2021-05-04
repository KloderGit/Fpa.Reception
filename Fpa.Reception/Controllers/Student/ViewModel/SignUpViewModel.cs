using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Student.ViewModel
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Не указан студент")]
        public Guid StudentKey { get; set; }
        [Required(ErrorMessage = "Не указана дисциплина")]
        public Guid DisciplineKey { get; set; }
        //[Required(ErrorMessage = "Не указана программа")]
        public Guid ProgramKey { get; set; }
        [Required(ErrorMessage = "Не указана позиция")]
        public Guid PositionKey { get; set; }
    }
}
