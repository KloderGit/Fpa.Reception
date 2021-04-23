using Mapster;
using reception.fitnesspro.ru.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace reception.fitnesspro.ru.Controllers.Teacher.ViewModel
{
    public class TableRow
    {
        private Domain.Position position;

        public TableRow(Domain.Position position)
        {
            this.position = position;
            this.PositionKey = position.Key;
            this.Time = position.Time;
        }

        public Guid PositionKey { get; set; }
        public DateTime Time { get; set; }
        public BaseInfoViewModel Student { get; set; }
        public BaseInfoViewModel Program { get; set; }
        public BaseInfoViewModel Discipline { get; set; }

        public ResultVm Result { get; set; }

        public IEnumerable<BaseInfoViewModel> RateTypes { get; set; }

        public TableRow IncludeStudent(IEnumerable<Domain.BaseInfo> students)
        { 
            if(position.Record == default || position.Record.StudentKey == default || students == default) return this;

            var student = students.FirstOrDefault(x=>x.Key == position.Record.StudentKey);

            if (student != null) this.Student = new BaseInfoViewModel {Key = student.Key, Title = student.Title};

            return this;
        }

        public TableRow IncludeProgram(IEnumerable<Domain.BaseInfo> programs)
        { 
            if(position.Record == default || position.Record.ProgramKey == default || programs ==default) return this;

            var program = programs.FirstOrDefault(x=>x.Key == position.Record.ProgramKey);

            if (program != null) this.Program = new BaseInfoViewModel {Key = program.Key, Title = program.Title};

            return this;
        }

        public TableRow IncludeDiscipline(IEnumerable<Domain.BaseInfo> disciplines)
        { 
            if(position.Record == default || position.Record.DisciplineKey == default || disciplines == default) return this;

            var discipline = disciplines.FirstOrDefault(x=>x.Key == position.Record.DisciplineKey);

            if (discipline != null)
                this.Discipline = new BaseInfoViewModel {Key = discipline.Key, Title = discipline.Title};

            return this;
        }

        public TableRow IncludeControlType(Domain.Model.Education.ControlType control)
        { 
            if(position.Record == default || control == default) return this;

            var rates = control.RateType.SelectMany(x=>x.RateKey.ScoreVariants).ToList();

            var didntShowUp = new Domain.BaseInfo{ Key = new Guid("736563b7-3111-4cd6-81d7-539ad92eb568"), Title = "Не явился" };

            rates.Add(didntShowUp);

            this.RateTypes = rates.Adapt<IEnumerable<BaseInfoViewModel>>();

            return this;
        }

        public TableRow IncludeRate(IEnumerable<Domain.BaseInfo> rates)
        { 
            if(position?.Record?.Result?.RateKey == default) return this;

            var rate = rates.FirstOrDefault(x=>x.Key == position.Record.Result.RateKey);

            if (rate != null)
                this.Result = new ResultVm
                    {RateKey = rate.Key, Title = rate.Title, Comment = position?.Record?.Result?.Comment};

            return this;
        }

        public class ResultVm
        {
            public Guid RateKey { get; set; }
            public string Title { get; set; }
            public string Comment { get; set; }
        }
    }
}