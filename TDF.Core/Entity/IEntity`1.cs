namespace TDF.Core.Entity
{
    public interface IEntity<T> where T : class
    {
        T Id { get; set; }
    }
}
