using Business.Interfaces;

namespace NHibernate.DAL.Repositories
{
    public class NHUnitOfWork : IUnitOfWork
    {
        private ITransaction transaction;
        public ISession Session { get; private set; }

        public void OpenSession()
        {
            if (Session != null)
                Dispose();

            Session = NHibernateHelper.OpenSession();
        }
        
        public void BeginTransaction()
        {
            transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        public void Rollback()
        {
            if (transaction.IsActive)
                transaction.Rollback();
        }

        public void Dispose()
        {
            Session.Close();
            Session = null;
        }
    }
}
