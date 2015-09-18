using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Infrastructure.DataAccess
{
    public class Entity : IObjectWithState, ISoftDeletable
    {
        // Private fields

        //Id until entity has been written to the database
        //Needed to provide correct hashcode
        private readonly Guid _TransientId;

        // Class initializers
        protected Entity()
        {
            _TransientId = Guid.NewGuid();
        }

        // Properties
        public long Id { get; set; }

        public bool IsDeleted { get; set; }

        public State State { get; set; }

        // Static methods
        public static bool operator ==(Entity e1, Entity e2)
        {
            if (ReferenceEquals(e1, e2)) return true;

            if ((object)e1 == null || (object)e2 == null) return false;

            return e1.GetHashCode() == e2.GetHashCode();
        }

        public static bool operator !=(Entity e1, Entity e2)
        {
            return !(e1 == e2);
        }


        // Public methods
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            var _other = obj as Entity;
            if (_other == null) return false;

            return obj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return Id != 0
                ? 0 ^ (GetType().Name + "_" + Id).GetHashCode()
                : _TransientId.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", GetType(), Id);
        }
    }
}