using Business.Interfaces;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace NHibernate.DAL.Repositories
{
    public class CommentRepository
    {
        private string path;

        public CommentRepository()
        {
            path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
        }

        public IEnumerable<Comment> GetAll()
        {
            XDocument xdoc = XDocument.Load(path);
            XElement root = xdoc.Element("commrnts");
            IEnumerable<XElement> xComments = root.Elements();

            var comments = new List<Comment>();
            foreach (var item in xComments)
            {
                var comment = new Comment
                {
                    Id = int.Parse(item.Element("id").Value),
                    Text = item.Element("text").Value,
                    UserName = item.Element("userName").Value,
                    ArticleId = int.Parse(item.Element("articleId").Value),
                    PubDate = DateTime.Parse(item.Element("pubDate").Value),
                };
                comments.Add(comment);
            }
            return comments;
        }

        public Comment Get(int id)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(path);
            XmlNode node = xdoc.SelectSingleNode($@"/comments/comment[id={id}]");
            var comment = new Comment
            {
                Id = int.Parse(node["id"].InnerText),
                Text = node["text"].InnerText,
                UserName = node["userName"].InnerText,
                ArticleId = int.Parse(node["articleId"].InnerText),
                PubDate = DateTime.Parse(node["pubDate"].InnerText),
            };
            return comment;
        }

        public IEnumerable<Comment> GetCommentsBy(int articleId)
        {
            XDocument xdoc = XDocument.Load(path);
            var comments = from item in xdoc.Element("comments").Elements("comment")
                        where int.Parse(item.Element("articleId").Value) == articleId
                        select new Comment
                        {
                            Id = int.Parse(item.Attribute("id").Value),
                            Text = item.Attribute("text").Value,
                            UserName = item.Attribute("userName").Value,
                            ArticleId = int.Parse(item.Attribute("articleId").Value),
                            PubDate = DateTime.Parse(item.Attribute("pubDate").Value),
                        };
            return comments;
        }

        public void Create(Comment comment)
        {
            XDocument xdoc = XDocument.Load(path);
            XElement root = xdoc.Element("comments");
            if (root.Attribute("lastCommentId") == null)
            {
                root.Add(new XAttribute("lastId", 0));
            }

            int lastId = int.Parse(root.Attribute("lastCommentId").Value);

            root.Attribute("lastCommentId").Value = (++lastId).ToString();

            root.Add(new XElement("comment",
                new XElement("id", lastId),
                new XElement("text", comment.Text),
                new XElement("userName", comment.UserName),
                new XElement("articleId", comment.ArticleId),
                new XElement("pubDate", comment.PubDate)));

            xdoc.Save(path);
        }

        public void Update(Comment comment)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(path);
            XmlNode node = xdoc.SelectSingleNode($@"/comments/comment[id={comment.Id}]");
            node["title"].InnerText = comment.Text;
            node["userName"].InnerText = comment.UserName;
            xdoc.Save(path);
        }

        public void Delete(int id)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(path);
            XmlNode node = xdoc.SelectSingleNode($@"/comments/comment[id={id}]");
            node.ParentNode.RemoveChild(node);
            xdoc.Save(path);
        }
    }
}