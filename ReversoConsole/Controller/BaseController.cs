using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole.Controller
{
    public abstract class BaseController
    {
        private readonly IDataSaver manager = new DBWork();

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

        protected void LoadElement<T>(ref T item, string collection) where T: LearningModelBase
        {
            manager.LoadElement<T>(ref item, collection);
        }

        protected void Delete<T>(T item) where T : LearningModelBase
        {
            manager.Delete<T>(item);
        }


    }
}
