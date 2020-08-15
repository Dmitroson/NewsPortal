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
            transaction = Session.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                if (transaction != null)
                    transaction.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
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
            transaction = null;
        }
    }
}
