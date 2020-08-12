using Business.Interfaces;
using System.Xml.Linq;

namespace Xml.DAL.Repositories
{
    public class XmlUnitOfWork : IUnitOfWork
    {
        private string path;
        private XElement snapshot;
        public XElement Document { get; set; }

        public XmlUnitOfWork(string path)
        {
            this.path = path;
        }

        public void OpenDocument()
        {
            if (Document != null)
                Dispose();

            Document = XDocument.Load(path).Element("database");
            snapshot = Document;
        }

        public void Commit()
        {
            try
            {
                Document.Save(path);
            }
            catch
            {
                Rollback();
            }
            finally
            {
                Dispose();
            }
        }

        public void Rollback()
        {
            Document = snapshot;
        }

        public void Dispose()
        {
            Document = null;
            snapshot = null;
        } 
    }
}
