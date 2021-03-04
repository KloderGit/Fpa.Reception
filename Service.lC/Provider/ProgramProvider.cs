using lc.fitnesspro.library.Interface;
using Service.lC.Dto;
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

        public ProgramProvider (RepositoryDepository depository, IManager manager)
            : base(depository.Program, depository)
        {
            this.manager = manager;
        }

        public async Task<IEnumerable<Program>> FilterByTeacher(Guid teacherKey)
        {
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
    }
}