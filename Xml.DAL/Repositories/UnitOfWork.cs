using Business.Interfaces;
using Business.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace NHibernate.DAL.Repositories
{
    public class UnitOfWork
    {
        private string fileName;
        private ArticleRepository articleRepository;
        private CommentRepository commentRepository;
        private bool disposed;
        private TextWriter writer;
        XmlSerializer articleSerializer;
        XmlSerializer commentSerializer;
        FileStream stream;


        public UnitOfWork(string fileName)
        {
            articleSerializer = new XmlSerializer(typeof(IEnumerable<Article>));
            commentSerializer = new XmlSerializer(typeof(IEnumerable<Comment>));

            this.fileName = fileName;

            stream = new FileStream(fileName, FileMode.Open);
            writer = new StreamWriter(fileName);
        }

        public void Save()
        {
            writer.Close();
        }

        public void Dispose()
        {
            if (!disposed)
            {
                writer.Close();
                disposed = true;
            }
        }
    }
}
