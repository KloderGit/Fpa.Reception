using Application.Employee;
using Application.Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.HttpClient;

namespace reception.fitnesspro.ru.Controllers.Teacher
{
    public class GetDisciplinesViewModel
    {
        public List<TeacherViewModel> Teachers { get; set; } = new List<TeacherViewModel>();

        private IEnumerable<EmployeeDisciplineDto> orders;
        private IEnumerable<ProgramDto> programs;

        public GetDisciplinesViewModel(IEnumerable<EmployeeDisciplineDto> orders, IEnumerable<ProgramDto> programs)
        {
            this.orders = orders;
            this.programs = programs;
        }

        public GetDisciplinesViewModel Create()
        {
            foreach (var order in orders)
            {
                var teacher = new TeacherViewModel
                {
                    Key = order.TeacherKey,
                    Programs = Reduce(order.Disciplines)
                };

                Teachers.Add(teacher);
            }

            return this;
        }

        private IEnumerable<ProgramViewModel> Reduce(IEnumerable<Guid> keys)
        {
            var array = keys.SelectMany(x => GetProgram(x));

            var group = array.GroupBy(x => x.Key);

            var result = group.Select(x => new ProgramViewModel
            {
                Key = x.Key,
                Title = x.First().Title,
                Education = x.First().Education,
                Disciplines = x.SelectMany(d => d.Disciplines)
            });

            return result;
        }

        private IEnumerable<ProgramViewModel> GetProgram(Guid key)
        {
            var item = programs.Where(x => x.Disciplines.Any(d => d.Key == key))
                                .Select(p => new ProgramViewModel
                                {
                                    Key = p.Key,
                                    Title = p.Title,
                                    Education = new EducationViewModel { Key = p.EducationForm.Key, Title = p.EducationForm.Title},
                                    Disciplines = p.Disciplines.Where(d => d.Key == key)
                                                               .Select(v => new DisciplineViewModel 
                                                                            { 
                                                                                Key = v.Key, 
                                                                                Title = v.Item.Title, 
                                                                                ControlType = new ControlTypeViewModel { Key = v.ControlTypeKey } 
                                                                             })});
            return item;
        }

    }

    public class TeacherViewModel
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
        public IEnumerable<ProgramViewModel> Programs { get; set; } = new List<ProgramViewModel>();
    }

    public class ProgramViewModel
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
        public EducationViewModel Education { get; set; }
        public IEnumerable<DisciplineViewModel> Disciplines { get; set; } = new List<DisciplineViewModel>();
    }

    public class EducationViewModel
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
    }


    public class DisciplineViewModel
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
        public ControlTypeViewModel ControlType { get; set; }
    }

    public class ControlTypeViewModel
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
    }
}
