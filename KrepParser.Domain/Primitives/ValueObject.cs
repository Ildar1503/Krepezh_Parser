namespace KrepParser.Domain.Primitives
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        /// <summary>
        /// Получение атомпрных значений объекта.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object? obj)
        {
            return obj is ValueObject other && ValuesAreEqual(other);
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Aggregate(default(int), HashCode.Combine);
        }

        public bool Equals(ValueObject? other)
        {
            return other is not null && ValuesAreEqual(other);
        }

        /// <summary>
        /// Проверка атомарных значений.
        /// </summary>
        /// <param name="other">Проверяемое значение</param>
        /// <returns>Булевое значение проверки атомарных коллекций объекта</returns>
        private bool ValuesAreEqual(ValueObject other)
        {
            return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
        }
    }
}
