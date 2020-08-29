using Business.Services;
using System.Xml.Linq;
using Xml.DAL.Repositories;

namespace Xml.DAL
{
    public static class XmlHelper
    {
        public static XmlUnitOfWork UnitOfWork { get; set; }
        public static string ConnectionString { get; set; }

        public static XElement OpenDocument()
        {
            return XDocument.Load(ConnectionString).Root;
        }

        public static XElement GetDocument()
        {
            UnitOfWork = ServiceManager.GetUnitOfWork() as XmlUnitOfWork;
            if (UnitOfWork.Document == null)
            {
                return OpenDocument();
            }
            return UnitOfWork.Document;
        }

        public static void SaveDocument()
        {
            UnitOfWork.Document.Save(ConnectionString);
        }
    }
}
