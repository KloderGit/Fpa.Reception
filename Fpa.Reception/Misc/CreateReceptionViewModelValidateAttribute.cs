using Domain;
using reception.fitnesspro.ru.Controllers.Reception.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Misc
{
    public class CreateReceptionViewModelValidateAttribute : ValidationAttribute
    {
        public CreateReceptionViewModelValidateAttribute()
        {
            ErrorMessage = "Временные промежутки для этого типа записи не заполнены!";
        }
        public override bool IsValid(object value)
        {
            CreateReceptionViewModel p = value as CreateReceptionViewModel;

            var isValid = true;

            if ((p.PositionType != PositionType.Free) && (p.Times == null || p.Times.Any() == false))
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
