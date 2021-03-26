using Mapster;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Application.Mappings
{
    public class RegisterMaps
    {
        public RegisterMaps()
        {
            TypeAdapterConfig.GlobalSettings.Compiler = exp => exp.CompileWithDebugInfo();

            TypeAdapterConfig.GlobalSettings.AllowImplicitDestinationInheritance = true;

            new BaseInfoMaps();
            new ContractMaps();
            new EventMaps();
            new PersonMaps();
            new PositionMaps();
            new ReceptionMaps();
            new RecordMaps();
            new ResultMaps();
            new ScoreMaps();
            new StudentMaps();
        }
    }
}
