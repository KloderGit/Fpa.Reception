using Service.lC.Interface;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Dto
{
    public class ScoreTypeDto : BaseDto, IConvert<ScoreTypeDto>
    {
        public Guid ParentKey { get; set; }
        public int MinGrade { get; set; }
        public int MaxGrade { get; set; }

        public string Grade { get; set; }

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
                ParentKey = dto.ParentKey,
                Title = dto.Title,
                MaxGrade = dto.MaxGrade,
                MinGrade = dto.MinGrade,
                Grade = dto.Grade
            };

            return group;
        }
    }
}
