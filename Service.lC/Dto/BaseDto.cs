using Service.lC.Interface;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Dto
{
    public class BaseDto : IConvert<BaseDto>
    {
        public Guid Key { get; set; }
        public string Title { get; set; }

        static BaseDto()
        {
            Converter.Register<BaseDto, Base>(Convert);
        }

        public TResult ConvertTo<TResult>(Func<BaseDto, TResult> converter)
        {
            var result = converter(this);
            return result;
        }

        public static Base Convert(BaseDto dto)
        {
            var item = new Base
            {
                Key = dto.Key,
                Title = dto.Title
            };

            return item;
        }
    }
}
