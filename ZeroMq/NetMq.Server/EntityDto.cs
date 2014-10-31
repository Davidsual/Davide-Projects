using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMq.Server
{
    [Serializable]
    public class EntityDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Age { get; set; }
        public int Port { get; set; }
        public EntityDto RelatedEntity { get; set; }
    }
}
