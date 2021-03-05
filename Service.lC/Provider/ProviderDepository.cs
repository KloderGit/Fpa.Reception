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
        private IProvider<Base, BaseDto> controlType;
        private IProvider<Base, BaseDto> employee;
        private GroupProvider group;
        private SubGroupProvider subGroup;

        public ProviderDepository(RepositoryDepository repositories, IManager manager)
        {
            this.manager = manager;
            this.repositories = repositories;
        }

        public ProgramProvider Program => program ?? (program = new ProgramProvider(repositories.Program, manager));
        public IProvider<Base, BaseDto> Discipline => discipline ?? (discipline = new GenericProvider<Base, BaseDto>(repositories.Discipline));
        public IProvider<Base, BaseDto> EducationForm => educationForm ?? (educationForm = new GenericProvider<Base, BaseDto>(repositories.EducationForm));
        public IProvider<Base, BaseDto> ControlType => controlType ?? (controlType = new GenericProvider<Base, BaseDto>(repositories.ControlType));
        public IProvider<Base, BaseDto> Employee => employee ?? (employee = new GenericProvider<Base, BaseDto>(repositories.Employee));
        public GroupProvider Group => group ?? (group = new GroupProvider(repositories.Group, manager));
        public SubGroupProvider SubGroup => subGroup ?? (subGroup = new SubGroupProvider(repositories.SubGroup, manager));
    }
}
