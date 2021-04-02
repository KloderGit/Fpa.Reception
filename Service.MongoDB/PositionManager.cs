using Service.MongoDB.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.MongoDB
{
    public class PositionManager
    {
        public ReceptionProvider Provider;

        public PositionManager(ReceptionProvider provider)
        {
            this.Provider = provider;
        }

    }
}
