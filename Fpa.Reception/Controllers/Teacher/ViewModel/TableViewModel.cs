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
            this.Events = reception.Events?.Select(x=>x.Discipline);
        }

        public Guid ReceptionKey { get; set; }
        public IEnumerable<BaseInfo> Events { get; set; }
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
                .Where(x => x != default)
                .Select(
                    x => new TableRow(x)
                        .IncludeStudent(students)
                        .IncludeProgram(programs)
                        .IncludeDiscipline(disciplines)
                        .IncludeRate(rates)
                    //.IncludeControlType(GetControl(x, controlTypes))
                ).ToList();
            
            Positions.Where(x=>x.Student != default).ToList()
                .ForEach(x=>x.IncludeControlType( GetControl(x.Discipline.Key, x.Program.Key, controlTypes)) );

            return this;

            Domain.Model.Education.ControlType GetControl(Guid disciplineKey, Guid programKey,
                IEnumerable<Domain.Model.Education.ControlType> controlTypes)
            { 
                if(disciplineKey == default || programKey == default) return null;

                var prg = programs.FirstOrDefault(x=>x.Key == programKey);

                var edu = prg.Educations.FirstOrDefault(x=>x.Discipline.Key == disciplineKey);

                var program = programs.ToList().FirstOrDefault(x=>x.Key == programKey);
                if(program == default) return null;
                var discipline = program.Educations.FirstOrDefault(x=>x.Discipline.Key == disciplineKey);
                if(discipline == default) return null;
                var controlKey = discipline.ControlType.Key;



                //var controlKey = programs.ToList().FirstOrDefault(x=>x.Key == programKey)
                //    .Educations.FirstOrDefault(x=>x.Discipline.Key == disciplineKey).ControlType?.Key;

                if(controlKey == default) return null;

                var control = controlTypes.FirstOrDefault(x=>x.Key == controlKey);

                //var scores= controlTypes.FirstOrDefault(x=>x.Key == controlKey).RateType.SelectMany(x=>x.RateKey.ScoreVariants);

                return control;
            }
        }

    }
}
