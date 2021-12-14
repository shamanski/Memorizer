using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole.Controller
{
    public abstract class BaseController
    {
        private readonly IDataSaver manager = new DBWork(new AppContext());

        protected void Save<T>(List<T> item) where T : class
        {
            manager.Save(item);
        }
        protected void Update<T>(T item) where T : class
        {
            manager.Update(item);
        }

        protected List<T> Load<T>() where T : class
        {
            return manager.Load<T>();
        }

        protected T LoadElement<T>(T item, string collection) where T: LearningModelBase
        {
           return manager.LoadElement<T>(item, collection);
        }

        protected void Delete<T>(T item) where T : LearningModelBase
        {
            manager.Delete<T>(item);
        }


    }
}
