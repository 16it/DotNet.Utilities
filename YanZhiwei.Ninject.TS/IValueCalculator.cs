using YanZhiwei.DotNet.Ninject.LearningNote.Models;
namespace YanZhiwei.DotNet.Ninject.LearningNote
{
    public interface IValueCalculator
    {
        decimal ValueProducts(params Product[] products);
    }
}