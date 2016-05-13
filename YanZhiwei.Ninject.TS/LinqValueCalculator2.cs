using System.Linq;
using YanZhiwei.DotNet.Ninject.LearningNote.Models;

namespace YanZhiwei.DotNet.Ninject.LearningNote
{
    public class LinqValueCalculator2 : IValueCalculator
    {
        private IDiscountHelper discounter;

        public LinqValueCalculator2(IDiscountHelper discountParam)
        {
            discounter = discountParam;
        }

        public decimal ValueProducts(params Product[] products)
        {
            return discounter.ApplyDiscount(products.Sum(p => p.Price));
        }
    }
}