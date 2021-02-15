using System;
using System.ComponentModel.DataAnnotations;

namespace reception.fitnesspro.ru.ViewModel
{
    public class BaseInfoViewModel
    {
        [Required]
        public Guid Key { get; set; }
        public string Title { get; set; }
    }
}
