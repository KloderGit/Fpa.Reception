using Application.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.HttpClient;
using Domain;

namespace Application.Employee
{
    public class EmployeeMethods
    {
        EmployeeHttpClient employeesHttpClient;
        private readonly AssignHttpClient assignHttpClient;

        public EmployeeMethods(EmployeeHttpClient client, AssignHttpClient assignHttpClient)
        {
            employeesHttpClient = client;
            this.assignHttpClient = assignHttpClient;
        }

        public async Task<IEnumerable<BaseInfoDto>> GetByKeys(IEnumerable<Guid> keys)
        {
            return await employeesHttpClient.GetByKeys(keys);
        }

        public async Task<IEnumerable<EmployeeDto>> GetByPersonKey(IEnumerable<Guid> keys)
        {
            return await employeesHttpClient.GetByPersonKey(keys);
        }

        public async Task<IEnumerable<EmployeeDisciplineDto>> GetDisciplines(IEnumerable<Guid> keys)
        {

            return await employeesHttpClient.GetDisciplines(keys);
        }

        public async Task<IEnumerable<BaseOneToMany>> GetTeacherDisciplines(IEnumerable<Guid> keys)
        {
            var assignsArray = await assignHttpClient.GetByTeacherKeys(keys);

            var groupedByTeacher = assignsArray
                .Select(x =>
                    x.Teachers.GroupJoin(x.Disciplines,
                        t => t.Marker,
                        d => d.Marker,
                        (t, dt) => new { TeacherKey = t.TeacherKey, Disciplines = dt }))
                .SelectMany(x => x)
                .GroupBy(x => x.TeacherKey);

            var result = groupedByTeacher.Select(x =>
                new BaseOneToMany
                {
                    Key = x.Key, 
                    Children = x.SelectMany(d => d.Disciplines.Select(g => g.DisciplineKey))
                });

            return result;
        }

        
        public IEnumerable<BaseOneToMany> GetProgramWithDisciplineKey(IEnumerable<BaseOneToMany> programs, Guid key)
        {
            var progs = programs
                .Where(x => x.Children.Any(k => k == key));

            var vms = progs.Select(x => new BaseOneToMany
            {
                Key = x.Key,
                Children = x.Children.Where(d=>d == key).Distinct()
            });

            return vms;
        }

    }

}
