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
        private readonly RepositoryDepository depository;

        private ProgramProvider program;
        private IProvider<Base, BaseDto> discipline;
        private IProvider<Base, BaseDto> educationForm;
        private IProvider<Base, BaseDto> controlType;
        private IProvider<Base, BaseDto> employee;
        private GroupProvider group;
        private IProvider<Base, BaseDto> subGroup;

        public ProviderDepository(BaseHttpClient client, IManager manager)
        {
            this.manager = manager;
            this.depository = new RepositoryDepository(client);
        }

        public ProgramProvider Program => program ?? (program = new ProgramProvider(depository, manager));
        public IProvider<Base, BaseDto> Discipline => discipline ?? (discipline = new GenericProvider<Base, BaseDto>(depository.Discipline, depository));
        public IProvider<Base, BaseDto> EducationForm => educationForm ?? (educationForm = new GenericProvider<Base, BaseDto>(depository.EducationForm, depository));
        public IProvider<Base, BaseDto> ControlType => controlType ?? (controlType = new GenericProvider<Base, BaseDto>(depository.ControlType, depository));
        public IProvider<Base, BaseDto> Employee => employee ?? (employee = new GenericProvider<Base, BaseDto>(depository.Employee, depository));
        public GroupProvider Group => group ?? (group = new GroupProvider(depository, manager));
        public IProvider<Base, BaseDto> SubGroup => subGroup ?? (subGroup = new GenericProvider<Base, BaseDto>(depository.SubGroup, depository));
    }
}
