namespace Extenso.Data.Entity
{
    public abstract class BaseEntity<T> : IEntity
    {
        public T Id { get; set; }

        #region IEntity Members

        public object[] KeyValues => new object[] { Id };

        #endregion IEntity Members
    }
}