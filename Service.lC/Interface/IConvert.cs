using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Interface
{
    public interface IConvert<T>
    {
        TResult ConvertTo<TResult>(Func<T, TResult> converter);
    }
}
