using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Component;
using Application.Employee;
using Application.HttpClient;
using Application.Program;
using Domain.Interface;
using lc.fitnesspro.library;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reception.fitnesspro.ru.Controllers.Education.ViewModel;
using reception.fitnesspro.ru.Controllers.Teacher;
using Service.lC;
using Service.lC.Provider;
using Service.MongoDB;

namespace reception.fitnesspro.ru.Controllers.Education
{
    [Route("[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IAppContext context;

        public EducationController(IAppContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("GetProgramSiblings")]
        public async Task<ActionResult<IEnumerable<Domain.Education.Program>>> GetSiblings(Guid daisciplineKey) // EducationStructureViewModel
        {
            var programs = await context.Education.GetProgramsByDiscipline(daisciplineKey);

            return programs.ToList();
        }
    }
}
