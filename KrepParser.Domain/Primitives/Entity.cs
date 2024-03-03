namespace KrepParser.Domain.Primitives
{
    /// <summary>
    /// Базовый класс сущности.
    /// </summary>
    public abstract class Entity : IEquatable<Entity>
    {
        protected Entity() { }

        protected Entity(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Устанавливает единожды значение для id во время выполнения программы.
        /// </summary>
        public Guid Id { get; private init; }

        #region Реализация операторов.

        public static bool operator ==(Entity firstValue, Entity secondValue)
        {
            return firstValue is not null && 
                secondValue is not null 
                && firstValue.Equals(secondValue);
        }

        public static bool operator !=(Entity firstValue, Entity secondValue)
        {
            return !(firstValue == secondValue);
        }

        #endregion

        public override bool Equals(object? obj)
        {
            if(obj == null) return false;
            if(obj.GetType() != GetType()) return false;
            if(obj is not Entity entity) return false;

            return entity.Id == Id;
        }

        public bool Equals(Entity? other)
        {
            if (other == null) return false;
            if (other.GetType() != GetType()) return false;

            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() * 15;
        }
    }
}
