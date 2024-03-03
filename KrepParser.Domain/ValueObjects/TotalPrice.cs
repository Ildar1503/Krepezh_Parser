using KrepParser.Domain.Primitives;
using KrepParser.Domain.Shared;

namespace KrepParser.Domain.ValueObjects
{
    /// <summary>
    /// Класс для проверки общей стоимости товара с учетом и без учета скидки.
    /// </summary>
    //public sealed class TotalPrice : ValueObject
    //{
    //    public const double MinDiscount = 0;

    //    public double Value { get; }

    //    private TotalPrice(double discount)
    //    {
    //        Value = discount;
    //    }

    //    public static Result Create(double discount)
    //    {
    //        if (discount < MinDiscount)
    //        {
    //            return new InvalidResult("Скидка на товар не может быть ниже 0%");
    //        }

    //        return new SuccesResult();
    //    }

    //    public override IEnumerable<object> GetAtomicValues()
    //    {
    //        yield return Value;
    //    }
    //}
}
