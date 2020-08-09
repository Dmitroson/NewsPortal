using Business.Interfaces;
using Business.Models;
using System.Xml;
using System.Web;
using System;

namespace NHibernate.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        string path;
        public UnitOfWork(string fileName)
        {
            
        }

        

        public void Commit()
        {
            throw new System.NotImplementedException();
        }

        public void Rollback()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
