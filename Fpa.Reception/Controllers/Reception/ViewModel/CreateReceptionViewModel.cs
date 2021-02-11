using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace reception.fitnesspro.ru.Controllers.Reception.ViewModel
{

    public class CreateReceptionViewModel : IValidatableObject
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public IEnumerable<EventViewModel> Events { get; set; }

        [Required]
        public PositionType PositionType { get; set; }

        public IEnumerable<DateTime> Times { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if ( (PositionType != PositionType.Free) && (Times == null || Times.Any() == false) )
                errors.Add(new ValidationResult("Временные промежутки для этого типа записи не заполнены", new[] { nameof(PositionType), nameof(Times) }));

            return errors;
        }
    }
}
