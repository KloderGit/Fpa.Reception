using lc.fitnesspro.library.Interface;
using Service.lC.Dto;
using Service.lC.Extensions;
using Service.lC.Interface;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Provider
{
    public class ScoreTypeProvider : GenericProvider<ScoreType, ScoreTypeDto>
    {
        private readonly IManager manager;

        public ScoreTypeProvider(
            IRepositoryAsync<ScoreType, ScoreTypeDto> repository,
            IManager manager)
            : base(repository)
        {
            this.manager = manager;
        }

        [Obsolete]
        public async Task<IEnumerable<ScoreType>> FilterByControlType(IEnumerable<Guid> controlTypeKeys)
        {
            if (controlTypeKeys.IsNullOrEmpty()) return new List<ScoreType>();

            var query = manager.Rate
                        .Filter(x => x.DeletionMark == false).AndAlso();

            var nodeList = new LinkedList<Guid>(controlTypeKeys);
            for (var node = nodeList.First; node != null; node = node.Next)
            {
                var value = node.Value;
                query.Filter(x => x.ParentKey == value);
                if (node != nodeList.Last) query.Or();
            };

            var quyr = query.DebugViewQuery();

            var result = await query.GetByFilter();

            var keys = result?.Select(x => x.Key) ?? Enumerable.Empty<Guid>();

            var scoreTypes = await Repository.GetAsync(keys);

            return scoreTypes;
        }

        public async Task<IEnumerable<ScoreType>> FilterByScoreType(IEnumerable<Guid> scoreTypeKeys)
        {
            if (scoreTypeKeys.IsNullOrEmpty()) return new List<ScoreType>();

            var query = manager.Rate
                        .Filter(x => x.DeletionMark == false).AndAlso();

            var nodeList = new LinkedList<Guid>(scoreTypeKeys);
            for (var node = nodeList.First; node != null; node = node.Next)
            {
                var value = node.Value;
                query.Filter(x => x.ParentKey == value);
                if (node != nodeList.Last) query.Or();
            };

            var result = await query.GetByFilter();

            var keys = result?.Select(x => x.Key) ?? Enumerable.Empty<Guid>();

            var scoreTypes = await Repository.GetAsync(keys);

            return scoreTypes;
        }

        public async Task<IEnumerable<ScoreType>> GetAllRates()
        {
            var query = manager.Rate
                        .Filter(x => x.DeletionMark == false);

            var result = await query.GetByFilter();

            var keys = result?.Select(x => x.Key) ?? Enumerable.Empty<Guid>();

            var scoreTypes = await Repository.GetAsync(keys);

            return scoreTypes;
        }
    }
}
