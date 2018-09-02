using System;
using System.Threading.Tasks;
using GP.Microservices.Common.Models;

namespace GP.Microservices.Common.Services
{
    public interface IOperationService
    {
        Task<Operation> CreateAsync(Guid id);

        Task<Operation> GetAsync(Guid id);

        Task<Operation> UpdateAsync(Operation operation);
    }
}