using reception.fitnesspro.ru.Controllers.Person;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Misc
{
    public class PhoneOrEmailRequireAttribute : ValidationAttribute
    {
        public PhoneOrEmailRequireAttribute()
        {
            ErrorMessage = "At least, the phone or email array should be filled in!";
        }
        public override bool IsValid(object value)
        {
            ContactsViewModel p = value as ContactsViewModel;

            return (p.Phones == null || p.Phones.Any() == false) && (p.Emails == null || p.Emails.Any() == false)
                ? false : true;
        }
    }
}
