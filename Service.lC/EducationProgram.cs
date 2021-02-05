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
                .Select(x => x.Key)
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

        public async Task<IEnumerable<ProgramInfoDto>> FindSiblings(Guid key)
        {
            var query = await manager.Program
                            .Filter(x => x.DeletionMark == false).And()
                            .Filter(x => x.Status == "Активный").And()
                            .Filter(x => x.Disciplines.Any(t => t.DisciplineKey == key))
                            .Select(x => x.Key)
                            .Select(x => x.Title)
                            .GetByFilter();

            query.ToArray().Where(x => x != null);

            var siblings = query.Select(x => new ProgramInfoDto { Key = x.Key, Title = x.Title });

            return siblings ?? Enumerable.Empty<ProgramInfoDto>();
        }

        public async Task<IEnumerable<GroupDto>> FindProgramGroup(IEnumerable<Guid> keys)
        {
            var today = DateTime.Now;

            var query = manager.Group
                        .Filter(x => x.DeletionMark == false).And()
                        .Filter(x => x.Finish > today).AndAlso();

            var nodeList = new LinkedList<Guid>(keys);
            for (var node = nodeList.First; node != null; node = node.Next)
            {
                var value = node.Value;
                query.Filter(x => x.ProgramKey == value);
                if (node != nodeList.Last) query.Or();
            }

            var result = await query.GetByFilter();

            if (result == null || result.Any() == false) return Enumerable.Empty<GroupDto>();

            var groups = result.Select(x => new GroupDto { 
                Key = x.Key, 
                Title = x.Title, 
                ProgramKey = x.ProgramKey, 
                Start = x.Start, 
                Finish = x.Finish });

            return groups ?? Enumerable.Empty<GroupDto>();
        }

        public async Task<IEnumerable<SubGroupDto>> FindSubgroups(IEnumerable<Guid> keys)
        {
            var query = manager.SubGroup
                        .Filter(x => x.DeletionMark == false).AndAlso();

            var nodeList = new LinkedList<Guid>(keys);
            for (var node = nodeList.First; node != null; node = node.Next)
            {
                var value = node.Value;
                query.Filter(x => x.GroupKey == value);
                if (node != nodeList.Last) query.Or();
            }

            var result = await query.GetByFilter();

            if (result == null || result.Any() == false) return Enumerable.Empty<SubGroupDto>();

            var subGroups = result.Select(x => new SubGroupDto
            {
                Key = x.Key,
                Title = x.Title,
                GroupKey = x.GroupKey
            });

            return subGroups ?? Enumerable.Empty<SubGroupDto>();
        }
    }

    public class ProgramInfoDto
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
    }

    public class GroupDto
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
        public Guid ProgramKey { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
    }

    public class SubGroupDto
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
        public Guid GroupKey { get; set; }
    }
}
