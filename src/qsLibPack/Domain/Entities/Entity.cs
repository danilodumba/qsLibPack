using System;
using System.Collections.Generic;

namespace qsLibPack.Domain.Entities
{
    public abstract class Entity<TId>
    {
        public TId Id
        {
            get; protected set;
        } = default!;

        public override bool Equals(object? obj)
        {
            var compareTo = obj as Entity<TId>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (compareTo is null || GetType() != compareTo.GetType()) return false;

            return EqualityComparer<TId>.Default.Equals(Id, compareTo.Id);
        }

        public static bool operator ==(Entity<TId> a, Entity<TId> b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity<TId> a, Entity<TId> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() ^ 93) + EqualityComparer<TId>.Default.GetHashCode(Id!);
        }

        public abstract void Validate();
    }
}
