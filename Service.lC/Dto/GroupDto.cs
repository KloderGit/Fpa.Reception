using Service.lC.Interface;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Dto
{
    public class GroupDto : BaseDto, IConvert<GroupDto>
    {
        public Guid ProgramKey { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }

        static GroupDto()
        {
            Converter.Register<GroupDto, Group>(Convert);
        }

        public TResult ConvertTo<TResult>(Func<GroupDto, TResult> converter)
        {
            var result = converter(this);
            return result;
        }

        public static Group Convert(GroupDto dto)
        {
            var group = new Group
            {
                Key = dto.Key,
                Title = dto.Title,
                Owner = dto.ProgramKey,
                Start = dto.Start,
                Finish = dto.Finish
            };

            return group;
        }
    }
}
