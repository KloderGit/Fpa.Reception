using Service.MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MongoDB
{
    public class ReceptionProvider
    {
        public IMongoRepository<Reception> Repository { get; }

        public ReceptionProvider(IMongoRepository<Reception> repository)
        {
            Repository = repository;
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

        public async Task<IEnumerable<Reception>> GetByPosition(Guid positionKey)
        {
            var query = await Task.Run(() => Repository.FilterByPath("PositionManager.Positions.Key", positionKey));

            //var sdff = Repository.FilterByPath("PositionManager.Positions.Key", positionKey).ToList();

            var result = query.ToList();

            return result;
        }

        public async Task<IEnumerable<Reception>> GetByStudent(Guid studentKey)
        {
            var result = await Task.Run(() => Repository.FilterByPath("PositionManager.Positions.Record.StudentKey", studentKey));

            return result;
        }

        //public async Task<IEnumerable<Reception>> GetByStudentEducation(Guid studentKey, Guid programKey, Guid groupKey, Guid subGroupKey)
        //{


        //    var result = await Task.Run(() => Repository.FilterByPath("PositionManager.Positions.Record.StudentKey", studentKey));

        //    return result;
        //}

        public async Task<IEnumerable<Reception>> GetByDiscipline(Guid disciplineKey)
        {
            var result = await Task.Run(() => Repository.FilterByPath("Events.Discipline.Key", disciplineKey));

            return result;
        }

        public async Task<IEnumerable<Reception>> GetByTeacher(Guid teacherKey)
        {
            var result = await Task.Run(() => Repository.FilterByPath("Event.Teachers.Key", teacherKey));

            return result;
        }
    }
}
