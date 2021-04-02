using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Student.ViewModel
{
    public class GetProgramReceptionsViewModel
    {
        public Domain.Reception Reception { get; private set; }

        public List<string> CommonRejectReasons { get; set; } = new List<string>();

        public DateTime Date { get; set; }
        public List<PositionViewModel> Positions { get; set; } = new List<PositionViewModel>();

        public List<EventViewModel> Events { get; set; } = new List<EventViewModel>();

        public static GetProgramReceptionsViewModel Create(Domain.Reception reception)
        {
            return new GetProgramReceptionsViewModel
            {
                Reception = reception,
                Date = reception.Date,
                Positions = reception.PositionManager.Positions
                .Where(x=>x.Record == default)
                .Select(pos =>
                    new PositionViewModel
                    {
                        Key = pos.Key,
                        Time = pos.Time
                    }).ToList(),
                Events = reception.Events.Select(ev =>
                    new EventViewModel
                    {
                        Discipline = new KeyValuePair<Guid, string>(ev.Discipline.Key, ev.Discipline.Title),
                    }).ToList()
            };
        }


        public class EventViewModel
        {
            public List<string> EventRejectReasons { get; set; } = new List<string>();

            public KeyValuePair<Guid, string> Discipline { get; set; }
        }

        public class PositionViewModel
        {
            public Guid Key { get; set; }
            public DateTime Time { get; set; }
        }
    }
}
