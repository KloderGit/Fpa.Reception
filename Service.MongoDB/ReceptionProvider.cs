using Service.MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.MongoDB
{
    public class ReceptionProvider
    {
        public IMongoRepository<Reception> Repository { get; }

        public ReceptionProvider(IMongoDbSettings settings)
        {
            Repository = new MongoRepository<Reception>(settings);
        }

        public async Task<Reception> GetByKeyAsync(Guid key)
        {
            return await Repository.FindOneAsync(x => x.Key == key);
        }

        public async Task<IEnumerable<Reception>> GetByKeysAsync(IEnumerable<Guid> keys)
        {
            var result = await Task.Run(() => Repository.FilterByArray("Key", keys));

            return result;
        }
    }
}
