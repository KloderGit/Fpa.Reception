using Application.Component;
using Domain.Interface;
using Domain.Model.Education;
using Microsoft.AspNetCore.Mvc;
using reception.fitnesspro.ru.Controllers.Student.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Student
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IAppContext context;

        public StudentController(IAppContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("Attestation")]
        public async Task<ActionResult<IEnumerable<Domain.Reception>>> GetAtteststion(Guid studentKey, Guid programKey)
        {
            var studentComponent = context.Student;
            var receptions = await studentComponent.GetAttestation(studentKey, programKey);

            return receptions.ToList();
        }

        [HttpGet]
        [Route("GetEducation")]
        public async Task<ActionResult<Domain.Education.Program>> GetEducatoin(Guid programKey)
        {
            var program = await context.Education.GetStudentEducation(programKey);
            return program;
        }


        [HttpGet]
        [Route("GetReceptions")]
        public async Task<ActionResult<IEnumerable<DisciplineReceptionViewModel>>> GetProgramReceptions(Guid studentKey, Guid disciplineKey)
        {
            var contract = await GetStudentContract();
            
            var disciplineReceptions = await context.Reception.GetByDisciplineKey(disciplineKey);

            var filtered = disciplineReceptions
                .Where(x => x.IsForProgram(contract.EducationProgram.Key))
                .Where(x => x.IsForGroup(contract.Group.Key))
                .Where(x => x.IsForSubGroup(contract.SubGroup.Key));

            var viewModel = filtered.Select(x => new DisciplineReceptionViewModel(x)).ToList();

            viewModel.ForEach(x => x.CheckContractExpired(contract));
            viewModel.ForEach(x => x.CheckEmptyPlaces());
            viewModel.ForEach(x => x.CheckIsNotInPast());
            viewModel.ForEach(x => x.CheckAllowedDisciplinePeriod(contract));
            viewModel.ForEach(x => x.CheckAttemptsCount(disciplineKey, studentKey, contract, context.Reception));
            viewModel.ForEach(x => x.CheckDependencies(disciplineKey, studentKey, context.Reception));
            viewModel.ForEach(x => x.CheckSignUpBefore());
            viewModel.ForEach(x => x.CheckSignUpDoubles(disciplineKey, studentKey, context.Reception));

            return viewModel;

            async Task<Contract> GetStudentContract()
            {
                var allStudentContract = await context.Student.GetContracts(studentKey);
                var contract = allStudentContract//.Where(x => x.ExpiredDate > DateTime.Now.Date)
                    .FirstOrDefault(x => x.ExpiredDate == allStudentContract.Max(d => d.ExpiredDate));

                return contract;
            }
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<ActionResult> SignUp([FromBody] SignUpViewModel model)
        {
            var getting = await context.Reception.GetByPosition(model.PositionKey);
            var reception = getting.FirstOrDefault();

            var position = reception?.PositionManager.Positions.FirstOrDefault(x => x.Key == model.PositionKey);

            if (position == default) return NotFound(nameof(model.PositionKey));

            position.Record = new Domain.Record { DisciplineKey = model.DisciplineKey, ProgramKey = model.ProgramKey, StudentKey = model.StudentKey };

            context.Reception.ReplaceReception(reception);

            return Ok();
        }


        [HttpGet]
        [Route("GetReceptions2")]
        public async Task<ActionResult<IEnumerable<GetProgramReceptionsViewModel>>> GetProgramReceptions2(Guid studentKey, Guid programKey)
        {
            var programReceptions = await context.Reception.GetProgramReceptions(programKey);

            var allStudentContract = await context.Student.GetContracts(studentKey);
            var programContracts = allStudentContract.Where(x => x.EducationProgram.Key == programKey);
            var contract = programContracts.Where(x => x.ExpiredDate > DateTime.Now.Date)
                .FirstOrDefault(x => x.ExpiredDate == programContracts.Max(d => d.ExpiredDate));

            var viewModelList = programReceptions.Select(x => GetProgramReceptionsViewModel.Create(x)).ToList();

            foreach (var item in viewModelList)
            {
                if ((contract.ExpiredDate != default && contract.ExpiredDate < DateTime.Now) == false) 
                    item.CommonRejectReasons.Add("Contract Expired");
                if ((item.Date < DateTime.Now) == false) 
                    item.CommonRejectReasons.Add("Date is in the past");
                if (item.Positions == default || item.Positions.Any() == false )
                    item.CommonRejectReasons.Add("There are not free places");
            }

            viewModelList.ForEach(x => x.Events.ForEach(e => e.EventRejectReasons = null));

            return null;
        }
    }
}
