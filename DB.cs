using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEx
{
    public class DB
    {
        private demoexEntities2 _context;

        public DB()
        {
            _context = demoexEntities2.GetContext();
        }

        public demoexEntities2 GetContext()
        {
            return _context;
        }

        public int getLastID()
        {
            // Получение последнего идентификатора из таблицы заявок
            var lastId = _context.Order.OrderByDescending(o => o.ID_order).FirstOrDefault()?.ID_order;

            // Если lastId равен null, получу 0, иначе значение lastId
            return lastId ?? 0;
        }

        public void addToTable(Order order)
        {
            _context.Order.Add(order);
            _context.SaveChanges();
        }
    }
}
