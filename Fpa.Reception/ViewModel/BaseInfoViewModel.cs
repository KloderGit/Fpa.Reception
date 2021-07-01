using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace reception.fitnesspro.ru.ViewModel
{
    public class BaseInfoViewModel
    {
        [Required]
        public Guid Key { get; set; }
        public string Title { get; set; }

        public void FillTitle(IEnumerable<BaseInfo> items)
        {
            var item = items.FirstOrDefault(x => x.Key == Key);
            Title = item.Title ?? "";
        }
    }
}
