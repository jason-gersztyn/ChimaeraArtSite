using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chimaera.Domain
{
    public interface IEntity
    {
        int Id { get; set; }
        string DisplayName { get; set; }
        string SPName { get; set; }
    }

    public class Entity : IEntity 
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string SPName { get; set; }
    }
}
