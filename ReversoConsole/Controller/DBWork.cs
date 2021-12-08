using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ReversoConsole.Controller
{
    class DBWork : IDataSaver
    {
        public List<T> Load<T>() where T : class
        {
            using (var db = new AppContext())
            {
                var result = db.Set<T>().Where(t => true).ToList();
                return result;
            }
        }

        public void Save<T>(List<T> item) where T : class
        {
            using (var db = new AppContext())
            {
                foreach (var i in item) 
                {
                    db.Entry(i).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }
        public void Update<T>(T item) where T : class
        {
            using (var db = new AppContext())
            {
                db.Set<T>().Attach(item);
                db.SaveChanges();
            }
        }
    }
}
