using Service.lC.Interface;
using System;

namespace Service.lC.Model
{
    public class Base : IConvert<Base>
    {
        public Guid Key { get; set; }
        public string Title { get; set; }

        public TResult ConvertTo<TResult>(Func<Base, TResult> converter)
        {
            var result = converter(this);
            return result;
        }
    }
}