using Service.lC.Interface;
using Service.lC.Model;

namespace Service.lC.Repository
{
    public class RepositoryDepository
    {
        private readonly BaseHttpClient client;

        private IRepositoryAsync<Program> program;
        private IRepositoryAsync<Base> discipline;
        private IRepositoryAsync<Base> educationForm;
        private IRepositoryAsync<Base> controlType;
        private IRepositoryAsync<Base> employee;
        private IRepositoryAsync<Base> group;
        private IRepositoryAsync<Base> subGroup;

        public RepositoryDepository(BaseHttpClient client)
        {
            this.client = client;
        }

        public IRepositoryAsync<Program> Program => program ?? (program = new GenericRepository<Program>(client, "lc/Program"));
        public IRepositoryAsync<Base> Discipline => discipline ?? (discipline = new GenericRepository<Base>(client, "Discipline"));
        public IRepositoryAsync<Base> EducationForm => educationForm ?? (educationForm = new GenericRepository<Base>(client, "EducationForm"));
        public IRepositoryAsync<Base> ControlType => controlType ?? (controlType = new GenericRepository<Base>(client, "Control"));
        public IRepositoryAsync<Base> Employee => employee ?? (employee = new GenericRepository<Base>(client, "Employee"));
        public IRepositoryAsync<Base> Group => group ?? (group = new GenericRepository<Base>(client, "Group"));
        public IRepositoryAsync<Base> SubGroup => subGroup ?? (subGroup = new GenericRepository<Base>(client, "SubGroup"));

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
