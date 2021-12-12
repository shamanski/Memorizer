using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ReversoConsole.DbModel;

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
        public void Delete<T>(T item) where T : LearningModelBase
        {
            using (var db = new AppContext())
            {
                db.Entry(item).State = EntityState.Deleted;
                db.SaveChanges();
            }         
        }

        public void LoadElement<T> (ref T item, string collection) where T: LearningModelBase
        {
            using (var db = new AppContext())
            {
                db.Attach<T>(item);
                db.Entry(item).Collection(collection).Load();
            }
        }
    }
}
