using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Interface
{
    public interface IProvider<T>
    {
        IRepositoryAsync<T> Repository { get; }
    }
}
