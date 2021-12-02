using System;
using System.Threading.Tasks;
using MyRest.Business.Models;

namespace MyRest.Business.Intefaces
{
    public interface IProductService : IDisposable
    {
        Task Add(Product product);
        Task Update(Product product);
        Task Delete(Guid id);
    }
}