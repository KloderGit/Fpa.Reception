using Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace reception.fitnesspro.ru.Controllers.Teacher.ViewModel
{
    public class TableViewModel
    {
        private Domain.Reception reception;

        private IEnumerable<Domain.Education.Program> programs;

        public TableViewModel(Domain.Reception reception)
        {
            this.reception = reception;
            this.ReceptionKey = reception.Key;
        }

        public Guid ReceptionKey { get; set; }
        public IEnumerable<TableRow> Positions { get; set; }

        public TableViewModel IncludePositions(
            IEnumerable<Domain.BaseInfo> students,
            IEnumerable<Domain.Education.Program> programs,
            IEnumerable<Domain.BaseInfo> disciplines,
            IEnumerable<Domain.Model.Education.ControlType> controlTypes,
            IEnumerable<BaseInfo> rates)
        { 
            this.programs = programs;

            Positions = reception?.PositionManager.Positions?
                .Where(x=> x != default)
                .Select(
                    x=> new TableRow(x)
                    .IncludeStudent(students)
                    .IncludeProgram(programs)
                    .IncludeDiscipline(disciplines)
                    .IncludeRate(rates)
                    .IncludeControlType(GetControl(x, controlTypes))
                );

            return this;

            Domain.Model.Education.ControlType GetControl(Position position, IEnumerable<Domain.Model.Education.ControlType> controlTypes)
            { 
                if(position == default || position.Record == default || position.Record.DisciplineKey == default) return null;

                var prg = programs.FirstOrDefault(x=>x.Key == position.Record.ProgramKey);

                var edu = prg.Educations.FirstOrDefault(x=>x.Discipline.Key == position.Record.DisciplineKey);

                var controlKey = programs.FirstOrDefault(x=>x.Key == position.Record.ProgramKey)
                    .Educations.FirstOrDefault(x=>x.Discipline.Key == position.Record.DisciplineKey).ControlType.Key;

                if(controlKey == default) return null;

                var control = controlTypes.FirstOrDefault(x=>x.Key == controlKey);

                //var scores= controlTypes.FirstOrDefault(x=>x.Key == controlKey).RateType.SelectMany(x=>x.RateKey.ScoreVariants);

                return control;
            }
        }

    }
}
