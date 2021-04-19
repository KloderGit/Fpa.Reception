using System;
using System.Collections.Generic;
using System.Linq;

namespace reception.fitnesspro.ru.Controllers.Teacher.ViewModel
{
    public class TableViewModel
    {
        private Domain.Reception reception;

        public TableViewModel(Domain.Reception reception)
        {
            this.reception = reception;
            this.ReceptionKey = reception.Key;
        }

        public Guid ReceptionKey { get; set; }
        public IEnumerable<TableRow> Positions { get; set; }

        public TableViewModel IncludePositions(
            IEnumerable<Domain.BaseInfo> students,
            IEnumerable<Domain.BaseInfo> programs,
            IEnumerable<Domain.BaseInfo> disciplines,
            IEnumerable<Domain.BaseInfo> controlTypes
            )
        { 
            Positions = reception?.PositionManager.Positions?
                .Select(
                    x=> new TableRow(x)
                    .IncludeStudent(students)
                    .IncludeProgram(programs)
                    .IncludeDiscipline(disciplines)
                    //.IncludeControlType(controlTypes)
                );

            return this;
        }
    }
}
