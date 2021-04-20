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
        public TableControlTypeViewModel RateTypes { get; set; }

        public TableRow IncludeStudent(IEnumerable<Domain.BaseInfo> students)
        { 
            if(position.Record == default || position.Record.StudentKey == default || students == default) return this;

            var student = students.FirstOrDefault(x=>x.Key == position.Record.StudentKey);

            this.Student = new BaseInfoViewModel{ Key = student.Key, Title = student.Title};

            return this;
        }

        public TableRow IncludeProgram(IEnumerable<Domain.BaseInfo> programs)
        { 
            if(position.Record == default || position.Record.ProgramKey == default || programs ==default) return this;

            var program = programs.FirstOrDefault(x=>x.Key == position.Record.ProgramKey);

            this.Program = new BaseInfoViewModel{ Key = program.Key, Title = program.Title};

            return this;
        }

        public TableRow IncludeDiscipline(IEnumerable<Domain.BaseInfo> disciplines)
        { 
            if(position.Record == default || position.Record.DisciplineKey == default || disciplines == default) return this;

            var discipline = disciplines.FirstOrDefault(x=>x.Key == position.Record.DisciplineKey);

            this.Discipline = new BaseInfoViewModel{ Key = discipline.Key, Title = discipline.Title};

            return this;
        }

        public TableRow IncludeControlType(Domain.Model.Education.ControlType control)
        { 
            if(position.Record == default || control == default) return this;

            var rates = control.Adapt<TableControlTypeViewModel>();

            this.RateTypes = rates;

            //this.RateTypes = rates.Select(x=> new BaseInfoViewModel{ Key = x.Key, Title = x.Title});

            return this;
        }
    }
}