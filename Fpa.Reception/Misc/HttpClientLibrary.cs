using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Application.HttpClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace reception.fitnesspro.ru.Misc
{
    public static class HttpClientLibrary
    {
        public static void AddHttpClients(IServiceCollection servicesCollection, IConfiguration configuration)
        {
            servicesCollection.AddHttpClient<PersonHttpClient>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("lc:EndPoints:Person").Value);
            });

            servicesCollection.AddHttpClient<EmployeeHttpClient>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("lc:EndPoints:Employee").Value);
            });

            servicesCollection.AddHttpClient<ProgramHttpClient>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("lc:EndPoints:Program").Value);
            });
            
            servicesCollection.AddHttpClient<IdentityHttpClient>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("lc:EndPoints:Identity").Value);
            });

            servicesCollection.AddHttpClient<AssignHttpClient>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("lc:EndPoints:Assign").Value);
            });

            servicesCollection.AddHttpClient<DisciplineHttpClient>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("lc:EndPoints:Discipline").Value);
            });

            servicesCollection.AddHttpClient<EducationFormHttpClient>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("lc:EndPoints:EducationForm").Value);
            });

            servicesCollection.AddHttpClient<ControlTypeHttpClient>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("lc:EndPoints:ControlType").Value);
            });
        }
    }
}
