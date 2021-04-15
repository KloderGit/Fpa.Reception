using Service.lC.Extensions;
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
        private readonly ScoreTypeProvider scoreTypeProvider;

        public ControlTypeManager(
            ScoreTypeProvider scoreTypeProvider
            )
        {
            this.scoreTypeProvider = scoreTypeProvider;
        }

        public async Task IncludeScoreType(IEnumerable<ControlType> controlTypes)
        {
            if (controlTypes.IsNullOrEmpty()) return;

            var controlTypeKeys = ReduceArray(controlTypes.Select(t => t.Key));

            var scoreTypes = await scoreTypeProvider.FilterByControlType(controlTypeKeys);

            controlTypes.ToList()
                .ForEach(x => x.ScoreTypes = scoreTypes.Where(g => g.ParentKey == x.Key));
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
