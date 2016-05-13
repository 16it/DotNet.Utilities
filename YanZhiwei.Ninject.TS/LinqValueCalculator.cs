using System.Linq;
using YanZhiwei.DotNet.Ninject.LearningNote.Models;

namespace YanZhiwei.DotNet.Ninject.LearningNote
{
    public class LinqValueCalculator : IValueCalculator
    {
        public decimal ValueProducts(params Product[] products)
        {
            return products.Sum(p => p.Price);
        }
    }
}