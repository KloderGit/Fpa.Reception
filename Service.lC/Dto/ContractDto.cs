using Service.lC.Interface;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.lC.Dto
{
    public class ContractDto : BaseDto, IConvert<ContractDto>
    {
        public string RegisterTitle { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime StartEducationDate { get; set; }
        public DateTime FinisEducationhDate { get; set; }
        public Guid EducationProgramKey { get; set; }
        public Guid GroupKey { get; set; }
        public Guid SubGroupKey { get; set; }

        public IEnumerable<RegisterStudent> Registry { get; set; }

        static ContractDto()
        {
            Converter.Register<ContractDto, Contract>(Convert);
        }

        public TResult ConvertTo<TResult>(Func<ContractDto, TResult> converter)
        {
            var result = converter(this);
            return result;
        }

        public static Contract Convert(ContractDto dto)
        {
            var contract = new Contract
            {
                Key = dto.Key,
                Title = dto.RegisterTitle,
                Students = dto.Registry.Select(x => x.StudentKey),
                EducationProgram = new Base { Key = dto.EducationProgramKey },
                Group = new Base { Key = dto.GroupKey },
                SubGroup = new Base { Key = dto.SubGroupKey },
                ExpiredDate = dto.ExpiredDate,
                StartEducationDate = dto.StartEducationDate,
                FinishEducationhDate = dto.FinisEducationhDate
            };

            return contract;
        }

        public class RegisterStudent
        {
            public Guid StudentKey { get; set; }
        }
    }
}
