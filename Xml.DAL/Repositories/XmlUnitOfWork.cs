using Business.Interfaces;
using System.Xml.Linq;

namespace Xml.DAL.Repositories
{
    public class XmlUnitOfWork : IUnitOfWork
    {
        private string path;
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
        }

        public void Commit()
        {
            Document.Save(path);
        }

        public void Rollback()
        {
            
        }

        public void Dispose()
        {
            Document = null;
        }
    }
}
