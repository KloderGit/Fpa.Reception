using lc.fitnesspro.library.Interface;
using lc.fitnesspro.library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.lC
{
    public class EducationProgram
    {
        private readonly IManager manager;

        public EducationProgram(IManager imanager)
        {
            this.manager = imanager;
        }

        public async Task<IEnumerable<Program>> GetByTeacher(Guid key)
        {
            var query = await manager.Program
                .Filter(x => x.DeletionMark == false).And()
                .Filter(x => x.Status == "Активный").And()
                .Filter(x => x.Teachers.Any(t => t.TeacherKey == key))
                .GetByFilter().ConfigureAwait(false);

            return query ?? Enumerable.Empty<Program>();
        }

        public async Task<IEnumerable<Guid>> GetProgramGuidByTeacher(Guid key)
        {
            var query = await manager.Program
                .Filter(x => x.DeletionMark == false).And()
                .Filter(x => x.Status == "Активный").And()
                .Filter(x => x.Teachers.Any(t => t.TeacherKey == key))
                .Select(x=>x.Key)
                .GetByFilter();

            query.ToArray();

            var keys = query.Select(x => x.Key);

            return keys ?? Enumerable.Empty<Guid>();
        }

        public async Task<IEnumerable<Program>> Find(IEnumerable<Guid> keys)
        {
            var query = manager.Program
                .Filter(x => x.DeletionMark == false).And()
                .Filter(x => x.Status == "Активный").AndAlso();

            var nodeList = new LinkedList<Guid>(keys);
            for (var node = nodeList.First; node != null; node = node.Next)
            {
                var value = node.Value;
                query.Filter(x => x.Key == value);
                if (node != nodeList.Last) query.Or();
            }

            var result = await query.GetByFilter();

            if (result == null || result.Any() == false) return Enumerable.Empty<Program>();

            return result;
        }
    }
}
