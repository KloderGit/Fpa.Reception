using Microsoft.Extensions.Configuration;
using Service.lC.Dto;
using Service.lC.Interface;
using Service.lC.Model;

namespace Service.lC.Repository
{
    public class RepositoryDepository
    {
        private readonly BaseHttpClient client;
        private readonly IConfiguration configuration;

        private IRepositoryAsync<Program, ProgramDto> program;
        private IRepositoryAsync<Base, BaseDto> discipline;
        private IRepositoryAsync<Base, BaseDto> educationForm;
        private IRepositoryAsync<ControlType, ControlTypeDto> controlType;
        private IRepositoryAsync<Base, BaseDto> employee;
        private IRepositoryAsync<Group, GroupDto> group;
        private IRepositoryAsync<SubGroup, SubGroupDto> subGroup;
        private IRepositoryAsync<Student, StudentDto> student;
        private PersonRepository person;
        private IRepositoryAsync<Contract, ContractDto> contract;
        private IRepositoryAsync<ScoreType, ScoreTypeDto> scoreType;

        public RepositoryDepository(BaseHttpClient client, IConfiguration configuration)
        {
            this.client = client;
            this.configuration = configuration;
        }

        public IRepositoryAsync<Program, ProgramDto> Program => program ??= new GenericRepository<Program, ProgramDto>(client, "lc/Program");
        public IRepositoryAsync<Base, BaseDto> Discipline => discipline ??= new GenericRepository<Base, BaseDto>(client, "lc/Discipline");
        public IRepositoryAsync<Base, BaseDto> EducationForm => educationForm ??= new GenericRepository<Base, BaseDto>(client, "lc/EducationForm");
        public IRepositoryAsync<ControlType, ControlTypeDto> ControlType => controlType ??= new GenericRepository<ControlType, ControlTypeDto>(client, "lc/Control");
        public IRepositoryAsync<Base, BaseDto> Employee => employee ??= new GenericRepository<Base, BaseDto>(client, "lc/Employee");
        public IRepositoryAsync<Group, GroupDto> Group => @group ??= new GenericRepository<Group, GroupDto>(client, "lc/Group");
        public IRepositoryAsync<SubGroup, SubGroupDto> SubGroup => subGroup ??= new GenericRepository<SubGroup, SubGroupDto>(client, "lc/SubGroup");
        public IRepositoryAsync<Student, StudentDto> Student => student ??= new GenericRepository<Student, StudentDto>(client, "lc/Student");
        public PersonRepository Person => person ??= new PersonRepository(client, "lc/Person");
        public IRepositoryAsync<Contract, ContractDto> Contract => contract ??= new GenericRepository<Contract, ContractDto>(client, "lc/v1/Contract");
        public IRepositoryAsync<ScoreType, ScoreTypeDto> ScoreType => scoreType ??= new GenericRepository<ScoreType, ScoreTypeDto>(client, "lc/v1/Rate");

    }
}
