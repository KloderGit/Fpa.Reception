using Service.lC.Interface;
using Service.lC.Model;
using Service.lC.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.lC.Provider
{
    public class ProviderDepository
    {
        private readonly BaseHttpClient client;
        private readonly RepositoryDepository depository;

        private ProgramProvider program;
        private IProvider<Base> discipline;
        private IProvider<Base> educationForm;
        private IProvider<Base> controlType;
        private IProvider<Base> employee;
        private IProvider<Base> group;
        private IProvider<Base> subGroup;

        public ProviderDepository(BaseHttpClient client)
        {
            this.client = client;
            this.depository = new RepositoryDepository(client);
        }

        public ProgramProvider Program => program ?? (program = new ProgramProvider(depository));
        public IProvider<Base> Discipline => discipline ?? (discipline = new GenericProvider<Base>(depository.Discipline, depository));
        public IProvider<Base> EducationForm => educationForm ?? (educationForm = new GenericProvider<Base>(depository.EducationForm, depository));
        public IProvider<Base> ControlType => controlType ?? (controlType = new GenericProvider<Base>(depository.ControlType, depository));
        public IProvider<Base> Employee => employee ?? (employee = new GenericProvider<Base>(depository.Employee, depository));
        public IProvider<Base> Group => group ?? (group = new GenericProvider<Base>(depository.Group, depository));
        public IProvider<Base> SubGroup => subGroup ?? (subGroup = new GenericProvider<Base>(depository.SubGroup, depository));
    }
}
