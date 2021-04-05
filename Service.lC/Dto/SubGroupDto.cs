using Service.lC.Interface;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Dto
{
    public class SubGroupDto : BaseDto, IConvert<SubGroupDto>
    {
        public Guid GroupKey { get; set; }

        static SubGroupDto()
        {
            Converter.Register<SubGroupDto, SubGroup>(Convert);
        }

        public TResult ConvertTo<TResult>(Func<SubGroupDto, TResult> converter)
        {
            var result = converter(this);
            return result;
        }

        public static SubGroup Convert(SubGroupDto dto)
        {
            var subGroup = new SubGroup
            {
                Key = dto.Key,
                Title = dto.Title,
                Owner = dto.GroupKey
            };

            return subGroup;
        }
    }
}
