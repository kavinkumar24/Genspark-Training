using HospitalAppointments.Interfaces;
namespace HospitalAppointments.Repositories
{
    public class Repository<K, T> : IRepository<K, T> where T : class
    {
        private readonly List<T> _entities = new List<T>();

        public T Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity cannot be null");
            }
            _entities.Add(entity);
            return entity;
        }

        public T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity cannot be null");
            }
            var existingEntity = GetById(entity.GetHashCode());
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {entity.GetHashCode()} not found.");
            }
            _entities.Remove(existingEntity);
            _entities.Add(entity);
            return entity;
        }
        public T Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }
            _entities.Remove(entity);
            return entity;
        }
        public T GetById(int id)
        {
            var entity = _entities.FirstOrDefault(e => e.GetHashCode() == id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }
            return entity;
        }
        public IEnumerable<T> GetAll()
        {
            return _entities;
        }


    }
}