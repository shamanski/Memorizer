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
        DbContext db;
        public DBWork(DbContext context)
        {
            db = context;
        }

        public List<T> Load<T>() where T : class
        {

                var result = db.Set<T>().Where(t => true).ToList();
                return result;

        }

        public void Save<T>(List<T> item) where T : class
        {
                foreach (var i in item) 
                {
                    db.Entry(i).State = EntityState.Modified;
                }
                db.SaveChanges();
        }

        public void Update<T>(T item) where T : class
        {
                db.Set<T>().Attach(item);
                db.SaveChanges();
        }

        public void Delete<T>(T item) where T : LearningModelBase
        {

                db.Entry(item).State = EntityState.Deleted;
                db.SaveChanges();     
        }

        public T LoadElement<T> (T item, string collection) where T: LearningModelBase
        {

                db.Attach<T>(item);
            var memberEntry = db.Entry(item).Member(collection);

            // if (memberEntry is DbCollectionEntry collectionMember)
            //     collectionMember.Load();

            // if (memberEntry is DbReferenceEntry referenceMember)
            //     referenceMember.Load();
            try
            {
                db.Entry(item).Collection(collection).Load();
            }
            catch
            {
                db.Entry(item).Reference(collection).Load();
            }
            db.SaveChanges();
            return db.Entry(item).Entity;
        }
    }
}
