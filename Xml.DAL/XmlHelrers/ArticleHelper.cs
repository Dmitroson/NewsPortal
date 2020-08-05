using Business.Models;
using System.IO;
using System.Xml.Serialization;

namespace Xml.DAL.XmlHelrers
{
    class ArticleHelper
    {
        private void WriteArticleToFile(string filename, Article article)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Article));
            TextWriter writer = new StreamWriter(filename);
            serializer.Serialize(writer, article);
            writer.Close();
        }

        private Article ReadArticleFromFile(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Article));
            FileStream fs = new FileStream(filename, FileMode.Open);
            var article = new Article();
            article = (Article)serializer.Deserialize(fs);
            return article;
        }
    }
}
