using System.Runtime.Serialization;

namespace Extenso.Data.Entity
{
    public abstract class BaseEntity<T> : IEntity
    {
        public T Id { get; set; }

        #region IEntity Members

        [IgnoreDataMember] // OData v8 does not like this property and will break if we don't use [IgnoreDataMember] here.
        public object[] KeyValues => new object[] { Id };

        #endregion IEntity Members
    }
}