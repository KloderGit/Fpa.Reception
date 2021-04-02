using Service.MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MongoDB
{
    public class PositionProvider
    {
        public IMongoRepository<Reception> Repository { get; }

        public PositionProvider(IMongoDbSettings settings)
        {
            Repository = new MongoRepository<Reception>(settings);
        }

        public async Task<Position> GetByKeyAsync(Guid key)
        {
            var ownedReception = await Task.Run(() => Repository.FilterByPath("PositionManager.Positions.Key", key));

            var result = ownedReception.SelectMany(x => x.PositionManager.Positions).FirstOrDefault(x => x.Key == key);

            return result;
        }

        public async Task<IEnumerable<Position>> GetByKeysAsync(IEnumerable<Guid> keys)
        {
            var ownedReceptions = await Task.Run(() => Repository.FilterByArray("PositionManager.Positions.Key", keys));

            var result = ownedReceptions.SelectMany(x => x.PositionManager.Positions).Where(x => keys.Contains(x.Key));

            return result;
        }
    }
}
