using ReversoConsole.DbModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoConsole.Controller
{
    public interface IDataSaver
    {
        void Save<T>(List<T> item) where T : class;
        void Update<T>(T item) where T : class;
        List<T> Load<T>() where T : class;
        T LoadElement<T>(T item, string collection) where T: LearningModelBase;
        void Delete<T>(T item) where T : LearningModelBase;

    }
}
