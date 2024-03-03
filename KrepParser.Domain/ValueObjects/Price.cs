using KrepParser.Domain.Entites;
using KrepParser.Domain.Primitives;
using KrepParser.Domain.Shared;

namespace KrepParser.Domain.ValueObjects
{
    /// <summary>
    /// Класс валидации цены.
    /// </summary>
    public sealed class Price : ValueObject
    {
        //Минимально допустимая цена.
        public const double MinPrice = 0; 

        private Price(double value)
        {
            Value = value;
        }

        //public static Result Create(double price)
        //{
        //    if (price < MinPrice)
        //    {
        //        return new (price);
        //    }

        //    return new SuccesResult();
        //} 

        public double Value { get; }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
