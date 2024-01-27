using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chimaera.Domain
{
    public interface IAggregate
    {
        Guid Id { get; set; }
        int Version { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
    }

    public class Aggregate : IAggregate
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public Aggregate()
        {
            Id = Guid.NewGuid();
            Version = 1;
            DateCreated = DateTime.Now;
            DateModified = DateCreated;
        }
    }
}
