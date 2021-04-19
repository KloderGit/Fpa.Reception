using Service.lC.Interface;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.lC.Dto
{
    public class ControlTypeDto : BaseDto, IConvert<ControlTypeDto>
    {
        public IEnumerable<ScoreInfoDto> RateType { get; set; } = new List<ScoreInfoDto>();

        static ControlTypeDto()
        {
            Converter.Register<ControlTypeDto, ControlType>(Convert);
        }

        public TResult ConvertTo<TResult>(Func<ControlTypeDto, TResult> converter)
        {
            var result = converter(this);
            return result;
        }

        public static ControlType Convert(ControlTypeDto dto)
        {
            var controlType = new ControlType
            {
                Key = dto.Key,
                Title = dto.Title,
                RateType = dto.RateType.Select(
                     x => new ControlType.ScoreInfo
                     {
                         LineNumber = x.LineNumber,
                         RateKey = new ScoreType { Key = x.RateKey }
                     })
            };

            return controlType;
        }

        public class ScoreInfoDto
        {
            public string LineNumber { get; set; }
            public Guid RateKey { get; set; }
        }
    }
}
