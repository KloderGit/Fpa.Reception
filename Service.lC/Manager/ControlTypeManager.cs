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
        private readonly IProvider<Base, BaseDto> controlTypeProvider;
        private readonly ScoreTypeProvider scoreTypeProvider;

        public ControlTypeManager(
            IProvider<Base, BaseDto> controlTypeProvider,
            ScoreTypeProvider scoreTypeProvider
            )
        {
            this.controlTypeProvider = controlTypeProvider;
            this.scoreTypeProvider = scoreTypeProvider;
        }

        public async Task<IEnumerable<Base>> GetControlTypesByKeys(IEnumerable<Guid> controlTypeKeys)
        {
            var controlTypes = await controlTypeProvider.Repository.GetAsync(controlTypeKeys);

            return controlTypes;
        }

        public async Task IncludeScoreType(IEnumerable<ControlType> controlTypes)
        {
            if (controlTypes.IsNullOrEmpty()) return;

            var controlTypeKeys = ReduceArray(controlTypes.Select(t => t.Key));

            var scoreTypes = await scoreTypeProvider.FilterByControlType(controlTypeKeys);

            controlTypes.ToList()
                .ForEach(x => x.ScoreTypes = scoreTypes.Where(g => g.ParentKey == x.Key));
        }

        public async Task IncludeRateType(IEnumerable<ScoreType> scoreTypes)
        {
            if (scoreTypes.IsNullOrEmpty()) return;

            var scoreTypeKeys = ReduceArray(scoreTypes.Select(t => t.Key));

            var rateTypes = await scoreTypeProvider.FilterByScoreType(scoreTypeKeys);

            scoreTypes.ToList()
                .ForEach(x => x.ScoreVariants = rateTypes.Where(g => g.ParentKey == x.Key));
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
