using HospitalAppointments.Models;
namespace HospitalAppointments.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        T Add(T entity);
        T Update(T entity);
        T Delete(int id);
        T GetById(int id);
        IEnumerable<T> GetAll();
    }
   
}