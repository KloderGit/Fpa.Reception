using Domain;
using Domain.Interface;
using Service.lC;
using Service.lC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Component
{
    public class AttestationTableComponent : IAttestationComponent
    {
        private readonly Context lcService;

        public AttestationTableComponent(Context lcService)
        {
            this.lcService = lcService;
        }

        public async Task Store(Reception reception)
        {
            var dataGroups = reception.PositionManager.GetGroupedRecords();

            var tables = new List<AttestationTable>();

            foreach (var group in dataGroups)
            {
                var table = new AttestationTable
                {
                    AttestationDate = reception.Date,
                    ProgramKey = group.Key.Item1,
                    DisciplineKey = group.Key.Item2,
                    Date = reception.Date,
                    TeacherKey = group.Select(x => x.Result.TeacherKey).Where(x => x != default).FirstOrDefault(),
                    Title = reception.Date.Date.ToString(),
                    Registry = group.Select(x =>
                        new AttestationStudent
                        {
                            StudentKey = x.StudentKey,
                            Comment = x.Result.Comment,
                            Rate = x.Result.RateKey
                        }
                    )
                };

                tables.Add(table);
            }

            var tasks = new List<Task>();

            foreach(var table in tables)
            { 
                tasks.Add(lcService.AttestationTable.Create(table));
            }

            await Task.WhenAll(tasks);
        }
    }
}
