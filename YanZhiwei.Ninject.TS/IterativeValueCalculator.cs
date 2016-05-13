using YanZhiwei.DotNet.Ninject.LearningNote.Models;

namespace YanZhiwei.DotNet.Ninject.LearningNote
{
    public class IterativeValueCalculator : IValueCalculator
    {
        public decimal ValueProducts(params Product[] products)
        {
            decimal totalValue = 0;
            foreach (Product p in products)
            {
                totalValue += p.Price;
            }
            return totalValue;
        }
    }
}