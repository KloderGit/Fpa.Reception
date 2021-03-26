using lc.fitnesspro.library.Interface;
using Service.lC.Dto;
using Service.lC.Interface;
using Service.lC.Model;
using Service.lC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Provider
{
    public class ProgramProvider: GenericProvider<Program, ProgramDto>
    {
        private readonly IManager manager;

        public ProgramProvider (
            IRepositoryAsync<Program, ProgramDto> repository, 
            IManager manager)
            : base(repository)
        {
            this.manager = manager;
        }

        public async Task<IEnumerable<Program>> GetAll()
        {
            var query = await manager.Program
                .Filter(x => x.DeletionMark == false).And()
                .Filter(x => x.Status == "Активный")
                .Select(x => x.Key)
                .GetByFilter().ConfigureAwait(false);

            var keys = query?.Select(x => x.Key) ?? Enumerable.Empty<Guid>();

            var programs = await Repository.GetAsync(keys);

            return programs;
        }

        public async Task<IEnumerable<Program>> FilterByTeacher(Guid teacherKey)
        {
            if (teacherKey == default) return new List<Program>();

            var query = await manager.Program
                .Filter(x => x.DeletionMark == false).And()
                .Filter(x => x.Status == "Активный").And()
                .Filter(x => x.Teachers.Any(t => t.TeacherKey == teacherKey))
                .Select(x => x.Key)
                .GetByFilter().ConfigureAwait(false);

            var keys = query?.Select(x => x.Key) ?? Enumerable.Empty<Guid>();

            var programs = await Repository.GetAsync(keys);

            return programs;
        }

        public async Task<IEnumerable<Program>> FilterByDiscipline(Guid disciplineKey)
        {
            if (disciplineKey == default) return new List<Program>();

            var query = await manager.Program
                    .Filter(x => x.DeletionMark == false).And()
                    .Filter(x => x.Status == "Активный").And()
                    .Filter(x => x.Disciplines.Any(t => t.DisciplineKey == disciplineKey))
                    .Select(x => x.Key)
                    .GetByFilter();

            var keys = query?.Select(x => x.Key) ?? Enumerable.Empty<Guid>();

            var programs = await Repository.GetAsync(keys);

            return programs;
        }
    }
}