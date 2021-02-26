using Domain;
using reception.fitnesspro.ru.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace reception.fitnesspro.ru.Controllers.Reception.ViewModel
{
    [CreateReceptionViewModelValidateAttribute]
    public class CreateReceptionViewModel 
    {
        [Required]
        [Range(typeof(DateTime), "01/01/1900", "01/01/2100")]        
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Мероприятие не определенно")]
        public IEnumerable<EventViewModel> Events { get; set; }

        [Required(ErrorMessage = "Вариант записи не определен")]
        public PositionType Type { get; set; }

        public IEnumerable<DateTime> Times { get; set; }
    }
}
