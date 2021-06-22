using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Reception.ViewModel
{
    public class ChangeDateViewModel
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public Guid ReceptionKey { get; set; }
    }
}
