using Domain;
using Domain.Model;
using Domain.Model.Education;
using Mapster;
using reception.fitnesspro.ru.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Student.ViewModel
{
    public class StudentRecordInfoViewModel
    {
        public List<StudentInfo> Students { get; set; } = new List<StudentInfo>();


        public StudentRecordInfoViewModel(IEnumerable<BaseInfo> students, IEnumerable<Domain.Reception> receptions, IEnumerable<StudentSetting> settings)
        {
            foreach (var student in students)
            {
                Students.AddRange(CreateStudentInfo(student, receptions, settings));
            }
        }

        public void FillPrograms(IEnumerable<BaseInfo> programs) => Students.ForEach(x => x.FillPrograms(programs));

        public void FillContract(IEnumerable<Contract> contracts) => Students.ForEach(x => x.FillContract(contracts));

        public void FillDisciplines(IEnumerable<BaseInfo> disciplines) => Students.ForEach(x => x.FillDisciplines(disciplines));

        public void FillControlType(IEnumerable<Domain.Education.Program> programs, IEnumerable<ControlType> controlTypes)
        {
            Students.ToList().ForEach(x => x.FillControlTypes(programs, controlTypes));
        }

        IEnumerable<StudentInfo> CreateStudentInfo(BaseInfo student, IEnumerable<Domain.Reception> receptions, IEnumerable<StudentSetting> settings)
        {

            var studentReception = receptions.Where(x => x.PositionManager.GetSignedUpStudentPosition(student.Key).Any());
            var studentSetting = settings.FirstOrDefault(x => x.StudentKey == student.Key);

            var allStudentPositions = studentReception.SelectMany(x => x.PositionManager.GetSignedUpStudentPosition(student.Key));

            var listOfPrograms = allStudentPositions.Select(x => x.Record.ProgramKey).Distinct();

            var infos = new List<StudentInfo>();

            foreach (var program in listOfPrograms)
            {
                var studentInfo = new StudentInfo
                {
                    Key = student.Key,
                    Title = student.Title,
                    Contract = new ContratInfo { Program = new BaseInfoViewModel { Key = program } },
                    Disciplines = CreateEventInfo(student, program, studentReception, studentSetting)
                };

                infos.Add(studentInfo);
            }

            return infos;
        }

        IEnumerable<DisciplineInfo> CreateEventInfo(BaseInfo student, Guid programKey, IEnumerable<Domain.Reception> receptions, StudentSetting studentSetting)
        {
            var eventInfos = new List<DisciplineInfo>();

            var allStudentPositionsByProgram = receptions.SelectMany(x => x.PositionManager.GetSignedUpStudentPosition(student.Key))
                .Where(x => x.Record.ProgramKey == programKey);

            var listOfDiscipline = allStudentPositionsByProgram.Select(x => x.Record.DisciplineKey).Distinct();

            foreach (var disciplineKey in listOfDiscipline)
            {
                var disciplineInfo = new DisciplineInfo { Key = disciplineKey };

                var positions = allStudentPositionsByProgram.Where(x => x.Record.DisciplineKey == disciplineKey);

                disciplineInfo.Records = positions.Select(x => GetRecord(x, studentSetting)).ToList();

                eventInfos.Add(disciplineInfo);
            }

            return eventInfos;
        }

        RecordInfo GetRecord(Position position, StudentSetting setting)
        {
            if (position.Record == default) return null;
            var currentSetting = setting?.DisciplineSettings?.FirstOrDefault(x => x.disciplineKey == position.Record.DisciplineKey);

            var record = new RecordInfo
            {
                PositionKey = position.Key,
                DateTime = position.Time,
                Result = position?.Record?.Result == default ? null : new ResultInfo(position),
                SignUpAttempt = currentSetting?.GetRestSignUpCount().Value,
                SignOutAttempt = currentSetting?.GetRestSignUpCount().Value,
            };

            return record;
        }


        public class StudentInfo : BaseInfoViewModel
        {
            public ContratInfo Contract { get; set; }
            public IEnumerable<DisciplineInfo> Disciplines { get; set; } = new List<DisciplineInfo>();

            public void FillContract(IEnumerable<Contract> contracts)
            {
                if (contracts == default) return;

                var currentContract = contracts.FirstOrDefault(c => c.EducationProgram.Key == Contract.Program.Key && c.IsContractForStudent(this.Key));

                if (currentContract != default)
                {
                    this.Contract.ExpirationDate = currentContract.ExpiredDate;
                    this.Contract.Group = currentContract.Group.Adapt<BaseInfoViewModel>();
                }
            }

            public void FillDisciplines(IEnumerable<BaseInfo> disciplines)
            {
                if (disciplines == default) return;
                Disciplines.ToList().ForEach(x => x.FillDisciplines(disciplines));
            }

            public void FillPrograms(IEnumerable<BaseInfo> programs)
            {
                Contract.Program = programs.FirstOrDefault(p => p.Key == Contract.Program.Key).Adapt<BaseInfoViewModel>();
            }

            public void FillControlTypes(IEnumerable<Domain.Education.Program> programs, IEnumerable<ControlType> controlTypes)
            {
                var program = programs.FirstOrDefault(x => x.Key == Contract.Program.Key);
                Disciplines.ToList().ForEach(x => x.FillControlTypes(program, controlTypes));
            }
        }

        public class DisciplineInfo : BaseInfoViewModel
        {
            public List<RecordInfo> Records { get; set; } = new List<RecordInfo>();

            public void FillDisciplines(IEnumerable<BaseInfo> disciplines)
            {
                if (disciplines == default) return;
                var discipline = disciplines.FirstOrDefault(e => e.Key == this.Key);
                this.Title = discipline.Title;
            }

            public void FillControlTypes(Domain.Education.Program program, IEnumerable<ControlType> controlTypes)
            {
                var controlKey = program.Educations.FirstOrDefault(x => x.Discipline.Key == this.Key)?.ControlType?.Key;
                var controlType = controlTypes.FirstOrDefault(ct => ct.Key == controlKey);

                Records.ToList().ForEach(x => x.FillControlType(controlType));
            }
        }

        public class RecordInfo
        {
            public Guid PositionKey { get; set; }
            public DateTime DateTime { get; set; }
            public ResultInfo Result { get; set; }
            public int? SignUpAttempt { get; set; }
            public int? SignOutAttempt { get; set; }
            public IEnumerable<BaseInfoViewModel> PossibleRates { get; set; }

            public void FillControlType(ControlType controlType)
            {
                var rates = new List<BaseInfo>();

                if (controlType != default)
                {
                    rates = controlType.RateType.SelectMany(x => x.RateKey.ScoreVariants).ToList();
                }

                var didntShowUp = new Domain.BaseInfo { Key = new Guid("736563b7-3111-4cd6-81d7-539ad92eb568"), Title = "Не явился" };
                rates.Add(didntShowUp);

                if (Result != default)
                {
                    var currentResult = rates.FirstOrDefault(rt => rt.Key == Result.Key);

                    if (currentResult != default)
                    {
                        Result.Title = currentResult.Title;
                    }
                }

                this.PossibleRates = rates.Adapt<IEnumerable<BaseInfoViewModel>>();
            }
        }

        public class ResultInfo : BaseInfoViewModel
        {
            public ResultInfo(Position position)
            {
                if (position?.Record?.Result == default) return;
                this.Key = position.Record.Result.RateKey;
                this.Comment = position.Record.Result.Comment;
            }
            public string Comment { get; set; }
        }

        public class ContratInfo
        {
            public BaseInfoViewModel Program { get; set; }
            public BaseInfoViewModel Group { get; set; }
            public DateTime ExpirationDate { get; set; }
        }
    }
}
