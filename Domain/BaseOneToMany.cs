using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class BaseOneToMany
    {
        public Guid Key { get; set; }
        public IEnumerable<Guid> Children { get; set; }
    }
}
