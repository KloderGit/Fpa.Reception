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
    public class StudentProvider : GenericProvider<Student, StudentDto>
    {
        private readonly IManager manager;

        public StudentProvider(
            IRepositoryAsync<Student, StudentDto> repository,
            IManager manager)
            : base(repository)
        {
            this.manager = manager;
        }

        public async Task<IEnumerable<Student>> FilterByPerson(IEnumerable<Guid> personKeys)
        {
            if (personKeys.IsNullOrEmpty()) return new List<Student>();

            var query = manager.Student
                        .Filter(x => x.DeletionMark == false).AndAlso();

            var nodeList = new LinkedList<Guid>(personKeys);
            for (var node = nodeList.First; node != null; node = node.Next)
            {
                var value = node.Value;
                query.Filter(x => x.PersonKey == value);
                if (node != nodeList.Last) query.Or();
            };

            var result = await query.GetByFilter();

            var keys = result?.Select(x => x.Key) ?? Enumerable.Empty<Guid>();

            var students = await Repository.GetAsync(keys);

            return students;
        }
    }
}
