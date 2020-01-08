using System;
using System.Text;
using System.Threading.Tasks;

namespace AIMS.SharedKernel.Interfaces
{

    public interface IHandle<in T> where T : BaseDomainEvent
    {
        Task Handle(T domainEvent);
    }
}
