using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookStore.DataAccess.Repository.IRepository.IRepository;

namespace BookStore.DataAccess.Repository.IRepository
{

    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
