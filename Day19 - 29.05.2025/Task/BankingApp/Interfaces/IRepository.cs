using BankingApp.Models;
namespace BankingApp.Interfaces
{
   public interface IRepository<K, T> where T : class
    {
        public Task<T> Add(T item);
        public Task<T> Get(K key);
        public Task<IEnumerable<T>> GetAll();
        public Task<T> Update(K key, T item);
        public Task<T> Delete(K key);
        Task<IEnumerable<Customer>> IncludeAllAsync(Func<object, object> value);
    }
}