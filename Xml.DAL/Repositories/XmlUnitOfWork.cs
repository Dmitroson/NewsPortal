using Business.Interfaces;
using System.Xml.Linq;

namespace Xml.DAL.Repositories
{
    public class XmlUnitOfWork : IUnitOfWork
    {
        private XElement snapshot;
        public XElement Document { get; set; }

        public void OpenSession()
        {
            if (Document != null)
                Dispose();

            Document = XmlHelper.OpenDocument();
            snapshot = new XDocument(Document).Root;
        }

        public void Commit()
        {
            try
            {
                if(Document != null)
                {
                    XmlHelper.SaveDocument();
                    snapshot = new XDocument(Document).Root;
                }
            }
            catch
            {
                Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            if (Document != null)
                Document = new XDocument(snapshot).Root;
        }

        public void Dispose()
        {
            Document = null;
            snapshot = null;
        }
    }
}
