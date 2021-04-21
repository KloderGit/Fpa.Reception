using Service.lC.Dto;
using Service.lC.Extensions;
using Service.lC.Interface;
using Service.lC.Model;
using Service.lC.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Manager
{
    public class ControlTypeManager
    {
        private readonly IProvider<ControlType, ControlTypeDto> controlTypeProvider;
        private readonly ScoreTypeProvider scoreTypeProvider;

        public ControlTypeManager(
            IProvider<ControlType, ControlTypeDto> controlTypeProvider,
            ScoreTypeProvider scoreTypeProvider
            )
        {
            this.controlTypeProvider = controlTypeProvider;
            this.scoreTypeProvider = scoreTypeProvider;
        }

        public async Task<IEnumerable<ControlType>> GetControlTypesByKeys(IEnumerable<Guid> controlTypeKeys)
        {
            var controlTypes = await controlTypeProvider.Repository.GetAsync(controlTypeKeys);

            return controlTypes;
        }

        public async Task IncludeScoreType(IEnumerable<ControlType> controlTypes)
        {
            if (controlTypes.IsNullOrEmpty()) return;

            var scoreTypeKeys = controlTypes.Where(z => z.RateType != default).SelectMany(t => t.RateType.Select(k => k.RateKey.Key));

            scoreTypeKeys = ReduceArray(scoreTypeKeys);

            var scoreTypes = await scoreTypeProvider.Repository.GetAsync(scoreTypeKeys);

            controlTypes.ToList()
                .ForEach(x => x.RateType = x.RateType.Select(a=>GetScoreInfo(a)));


            ControlType.ScoreInfo GetScoreInfo(ControlType.ScoreInfo info)
            {
                var scoreinfo = new ControlType.ScoreInfo();
                scoreinfo.LineNumber = info.LineNumber;
                scoreinfo.RateKey = scoreTypes.FirstOrDefault(x => x.Key == info.RateKey.Key);

                return scoreinfo;
            }
        }

        public async Task IncludeRateType(IEnumerable<ScoreType> scoreTypes)
        {
            if (scoreTypes.IsNullOrEmpty()) return;

            var scoreTypeKeys = ReduceArray(scoreTypes.Select(t => t.Key));

            var rateTypes = await scoreTypeProvider.FilterByScoreType(scoreTypeKeys);

            scoreTypes.ToList()
                .ForEach(x => x.ScoreVariants = GetRate(x));
        
            IEnumerable<Base> GetRate(ScoreType score)
            {
                var items = rateTypes.Where(x => x.ParentKey == score.Key)
                    .Select(x => new Base { Key = x.Key, Title = x.Title });

                return items;
            }
        }

        public async Task<IEnumerable<Base>> GetAllRates()
        {
            var rates = await scoreTypeProvider.GetAllRates();

            return rates;
        }

        private List<Guid> ReduceArray(IEnumerable<Guid> keys)
        {
            return keys
                .Distinct()
                .Where(x => x != default)
                .ToList();
        }
    }


}
