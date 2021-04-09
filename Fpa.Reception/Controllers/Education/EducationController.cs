using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
