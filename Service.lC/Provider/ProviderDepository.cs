using lc.fitnesspro.library.Interface;
using Service.lC.Dto;
using Service.lC.Interface;
using Service.lC.Model;
using Service.lC.Repository;

namespace Service.lC.Provider
{
    public class ProviderDepository
    {
        private readonly IManager manager;
        private readonly RepositoryDepository repositories;

        private ProgramProvider program;
        private IProvider<Base, BaseDto> discipline;
        private IProvider<Base, BaseDto> educationForm;
        private IProvider<ControlType, ControlTypeDto> controlType;
        private IProvider<Base, BaseDto> employee;
        private StudentProvider student;
        private GroupProvider group;
        private SubGroupProvider subGroup;
        private PersonProvider person;
        private ContractProvider contract;
        private ScoreTypeProvider scoreType;

        public ProviderDepository(RepositoryDepository repositories, IManager manager)
        {
            this.manager = manager;
            this.repositories = repositories;
        }

        public ProgramProvider Program => program ??= new ProgramProvider(repositories.Program, manager);
        public IProvider<Base, BaseDto> Discipline => discipline ??= new GenericProvider<Base, BaseDto>(repositories.Discipline);
        public IProvider<Base, BaseDto> EducationForm => educationForm ??= new GenericProvider<Base, BaseDto>(repositories.EducationForm);
        public IProvider<ControlType, ControlTypeDto> ControlType => controlType ??= new GenericProvider<ControlType, ControlTypeDto>(repositories.ControlType);
        public IProvider<Base, BaseDto> Employee => employee ??= new GenericProvider<Base, BaseDto>(repositories.Employee);
        public GroupProvider Group => @group ??= new GroupProvider(repositories.Group, manager);
        public SubGroupProvider SubGroup => subGroup ??= new SubGroupProvider(repositories.SubGroup, manager);
        public StudentProvider Student => student ??= new StudentProvider(repositories.Student, manager);
        public PersonProvider Person => person ??= new PersonProvider(repositories.Person, manager);
        public ContractProvider Contract => contract ??= new ContractProvider(repositories.Contract, manager);
        public ScoreTypeProvider ScoreType => scoreType ??= new ScoreTypeProvider(repositories.ScoreType, manager);
    }
}
