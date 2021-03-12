using System;
namespace qsLibPack.Domain.Entities
{
    public abstract class AggregateRoot<TId> : Entity
    {
        public TId Id
        {
            get; protected set;
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as AggregateRoot<TId>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (compareTo is null) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(AggregateRoot<TId> a, AggregateRoot<TId> b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(AggregateRoot<TId> a, AggregateRoot<TId> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() ^ 93) + Id.GetHashCode();
        }
    }
}