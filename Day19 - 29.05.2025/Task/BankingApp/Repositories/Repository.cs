using BankingApp.Interfaces;
using BankingApp.Contexts;
using BankingApp.Models;

namespace BankingApp.Repositories
{
    public  abstract class Repository<K, T> : IRepository<K, T> where T:class
    {
        protected readonly BankingContext _bankingContext;

        public Repository(BankingContext clinicContext)
        {
            _bankingContext = clinicContext;
        }
        public async Task<T> Add(T item)
        {
            _bankingContext.Add(item);
            await _bankingContext.SaveChangesAsync();//generate and execute the DML queries for the objects where state is in ['added','modified','deleted']
            return item;
        }

        public async Task<T> Delete(K key)
        {
            var item = await Get(key);
            if (item != null)
            {
                _bankingContext.Remove(item);
                await _bankingContext.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for deleting");
        }

        public abstract Task<T> Get(K key);


        public abstract Task<IEnumerable<T>> GetAll();

        public Task<IEnumerable<Customer>> IncludeAllAsync(Func<object, object> value)
        {
            throw new NotImplementedException();
        }

        public async Task<T> Update(K key, T item)
        {
            var myItem = await Get(key);
            if (myItem != null)
            {
                _bankingContext.Entry(myItem).CurrentValues.SetValues(item);
                await _bankingContext.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for updation");
        }
    }
}