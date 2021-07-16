using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IAttestationComponent
    {
        Task Store(Reception reception);
    }
}
