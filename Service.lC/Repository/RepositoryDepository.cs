using Service.lC.Dto;
using Service.lC.Interface;
using Service.lC.Model;

namespace Service.lC.Repository
{
    public class RepositoryDepository
    {
        private readonly BaseHttpClient client;

        private IRepositoryAsync<Program, ProgramDto> program;
        private IRepositoryAsync<Base, BaseDto> discipline;
        private IRepositoryAsync<Base, BaseDto> educationForm;
        private IRepositoryAsync<Base, BaseDto> controlType;
        private IRepositoryAsync<Base, BaseDto> employee;
        private IRepositoryAsync<Base, BaseDto> group;
        private IRepositoryAsync<Base, BaseDto> subGroup;

        public RepositoryDepository(BaseHttpClient client)
        {
            this.client = client;
        }

        public IRepositoryAsync<Program, ProgramDto> Program => program ?? (program = new GenericRepository<Program, ProgramDto>(client, "lc/Program"));
        public IRepositoryAsync<Base, BaseDto> Discipline => discipline ?? (discipline = new GenericRepository<Base, BaseDto>(client, "lc/Discipline"));
        public IRepositoryAsync<Base, BaseDto> EducationForm => educationForm ?? (educationForm = new GenericRepository<Base, BaseDto>(client, "lc/EducationForm"));
        public IRepositoryAsync<Base, BaseDto> ControlType => controlType ?? (controlType = new GenericRepository<Base, BaseDto>(client, "lc/Control"));
        public IRepositoryAsync<Base, BaseDto> Employee => employee ?? (employee = new GenericRepository<Base, BaseDto>(client, "lc/Employee"));
        public IRepositoryAsync<Base, BaseDto> Group => group ?? (group = new GenericRepository<Base, BaseDto>(client, "lc/Group"));
        public IRepositoryAsync<Base, BaseDto> SubGroup => subGroup ?? (subGroup = new GenericRepository<Base, BaseDto>(client, "lc/SubGroup"));

        //public IRepositoryAsync<T> GetRepository<T>()
        //{
        //    var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //    var withInterface = properties
        //        .FirstOrDefault(x =>
        //            x.PropertyType.GetInterfaces()
        //                .FirstOrDefault(i => i.GenericTypeArguments.Any(t => t == typeof(T)))
        //            != null);

        //    IRepositoryAsync<T> result = null;

        //    result = withInterface.GetValue(manager) as IRepository<T>;

        //    return result;
        //}
    }
}
