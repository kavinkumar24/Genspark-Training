using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageAppoinmentsApp.Models;
using ManageAppoinmentsApp.Interface;
using ManageAppoinmentsApp.Exceptions;

namespace ManageAppoinmentsApp.Repositories
{
    public class AppointmentRepository : Repository<int, Appointment>
    {
        public override ICollection<Appointment> GetAll()
        {
            if (_items.Count == 0)
            {
                throw new CollectionEmptyException("No Appointment found");
            }
            return _items;
        }

        public override Appointment GetById(int id)
        {
            var appointment = _items.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with ID {id} not found.");
            }
            return appointment;
        }

        protected override int GenerateID()
        {
            return _items.Count == 0 ? 1 : _items.Max(a => a.Id) + 1;
        }
    }
}
