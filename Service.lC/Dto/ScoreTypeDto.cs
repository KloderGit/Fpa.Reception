using Service.lC.Interface;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Dto
{
    public class ScoreTypeDto : BaseDto, IConvert<ScoreTypeDto>
    {
        public int MinRate { get; set; }
        public int MaxRate { get; set; }

        public string Rate { get; set; }

        static ScoreTypeDto()
        {
            Converter.Register<ScoreTypeDto, ScoreType>(Convert);
        }

        public TResult ConvertTo<TResult>(Func<ScoreTypeDto, TResult> converter)
        {
            var result = converter(this);
            return result;
        }

        public static ScoreType Convert(ScoreTypeDto dto)
        {
            var group = new ScoreType
            {
                Key = dto.Key,
                Title = dto.Title,
                MaxRate = dto.MaxRate,
                MinRate = dto.MinRate,
                Rate = dto.Rate
            };

            return group;
        }
    }
}
