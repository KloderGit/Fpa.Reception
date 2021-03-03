using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.lC.Manager
{
    public partial class EducationManager
    {


        private async Task<IEnumerable<Guid>> FindDisciplinePrograms(Guid disciplineKey)
        {
            var query = await lcManager.Program
                    .Filter(x => x.DeletionMark == false).And()
                    .Filter(x => x.Status == "Активный").And()
                    .Filter(x => x.Disciplines.Any(t => t.DisciplineKey == disciplineKey))
                    .Select(x => x.Key)
                    .GetByFilter();

            var result = query == null ? Enumerable.Empty<Guid>() : query.Select(x => x.Key);

            return result;
        }

        private async Task<IEnumerable<Guid>> FindProgramGroups(Guid programKey)
        {
            var today = DateTime.Now;

            var query = await lcManager.Group
                        .Filter(x => x.DeletionMark == false).And()
                        .Filter(x => x.Finish > today).And()
                        .Filter(x => x.ProgramKey == programKey)
                        .Select(x => x.Key)
                        .GetByFilter();

            var result = query == null ? Enumerable.Empty<Guid>() : query.Select(x => x.Key);

            return result;
        }

        private async Task<IEnumerable<Guid>> FindGroupSubGroup(Guid groupKey)
        {
            var query = await lcManager.SubGroup
                        .Filter(x => x.DeletionMark == false).And()
                        .Filter(x => x.GroupKey == groupKey)
                        .Select(x => x.Key)
                        .GetByFilter();

            var result = query == null ? Enumerable.Empty<Guid>() : query.Select(x => x.Key);

            return result;
        }

        private async Task<IEnumerable<Program>> GetPrograms(IEnumerable<Guid> keys)
        {
            var programs = await programProvider.Repository.GetAsync(keys);

            programs = await programProvider.IncludeEducationForm(programs);
            programs = await programProvider.IncludeEducations(programs);
            programs = await programProvider.IncludeTeachers(programs);

            return programs;
        }


    }
}
