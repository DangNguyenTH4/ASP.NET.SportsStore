using System.Collections.Generic;
using System.Linq;
using Domain.Abstract;
using Domain.Entities;
namespace Domain.Concrete
{
    public class EFProductRepostory : IProductRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<Product> Productss
        {
            get { return context.Products; }
        }
    }
}
