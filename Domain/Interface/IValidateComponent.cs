using Domain.Model.Education;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IValidateComponent
    {
        Task<bool> CheckContractExpired(Contract contract);
        bool CheckIsNotInPast(DateTime date);
        Task<bool> CheckSignUpDoubles(Guid disciplineKey, Guid studentKey);
    }
}
