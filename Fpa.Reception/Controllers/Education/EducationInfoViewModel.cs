using Application.HttpClient;
using reception.fitnesspro.ru.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System;


namespace reception.fitnesspro.ru.Controllers.Education
{
    public class EducationInfoViewModel : BaseInfoViewModel
    {
        private readonly ProgramDto program;

        public BaseInfoViewModel EducationForm { get; set; }
        public IEnumerable<BaseInfoViewModel> Teachers { get; set; }
        public IEnumerable<DisciplineInfoViewModel> Disciplines { get; set; }

        public EducationInfoViewModel(ProgramDto program)
        {
            if (program == null) throw new ArgumentNullException();
            this.program = program;

            this.Key = program.Key;
            this.Title = program.Title;
        }

        public EducationInfoViewModel AddEducation(IEnumerable<BaseInfoDto> educations)
        {
            if (educations == null || educations.Any() == false) return this;

            var foundedEducation = educations.FirstOrDefault(x => x.Key == program.EducationFormKey);
            if (foundedEducation != null) this.EducationForm = new BaseInfoViewModel { Key = foundedEducation.Key, Title = foundedEducation.Title };

            return this;
        }

        public EducationInfoViewModel AddTeachers(IEnumerable<BaseInfoDto> teachers)
        {
            if (teachers == null || teachers.Any() == false) return this;

            this.Teachers = program.Teachers?
                .Select(x => teachers.FirstOrDefault(t => t.Key == x))
                .Select(x => new BaseInfoViewModel { Key = x.Key, Title = x.Title });

            return this;
        }

        public EducationInfoViewModel AddDisciplines(IEnumerable<Domain.BaseInfo> disciplines, IEnumerable<BaseInfoDto> controltypes)
        {
            if (disciplines == null || disciplines.Any() == false) return this;

            this.Disciplines = program.Disciplines?
                .Select(x =>
                    new DisciplineInfoViewModel
                    {
                        Key = x.DisciplineKey,
                        Title = GetDiscipline(x.DisciplineKey, disciplines).Title,
                        ControlType = GetControlType(x.ControlTypeKey, controltypes)
                    }
                );

            return this;

            BaseInfoViewModel GetControlType(Guid key, IEnumerable<BaseInfoDto> controltypes)
            {
                var item = controltypes?.FirstOrDefault(ct => ct.Key == key);
                if (item == null) return null;

                var vm = new BaseInfoViewModel
                {
                    Key = item.Key,
                    Title = item.Title
                };

                return vm;
            }

            BaseInfoViewModel GetDiscipline(Guid key, IEnumerable<Domain.BaseInfo> disciplines)
            {
                var item = disciplines?.FirstOrDefault(ct => ct.Key == key);
                if (item == null) return null;

                var vm = new BaseInfoViewModel
                {
                    Key = item.Key,
                    Title = item.Title
                };

                return vm;
            }
        }
    }

    public class DisciplineInfoViewModel : BaseInfoViewModel
    {
        public BaseInfoViewModel ControlType { get; set; }
    }
}
