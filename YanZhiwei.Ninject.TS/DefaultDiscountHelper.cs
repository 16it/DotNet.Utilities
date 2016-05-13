namespace YanZhiwei.DotNet.Ninject.LearningNote
{
    public class DefaultDiscountHelper : IDiscountHelper
    {
        public decimal DiscountSize { get; set; }

        public decimal ApplyDiscount(decimal totalParam)
        {
            return (totalParam - (DiscountSize / 10m * totalParam));
        }
    }
}