using Business.Interfaces;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Xml.DAL.Repositories
{
    public class XmlCommentRepository : ICommentRepository
    {
        private const string ISOFormat = "yyyy-MM-dd\\THH:mm:ss";

        private XElement Document
        {
            get
            {
                return XmlHelper.GetDocument();
            }
        }

        public IEnumerable<Comment> GetAll()
        {
            XElement root = Document.Element("comments");
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
            XElement xComment = null;
            foreach (var item in Document.Element("comments").Elements("comment"))
            {
                if (item.Element("id").Value == id.ToString())
                {
                    xComment = item;
                    break;
                }
            }

            var comment = new Comment
            {
                Id = int.Parse(xComment.Element("id").Value),
                Text = xComment.Element("text").Value,
                UserName = xComment.Element("userName").Value,
                ArticleId = int.Parse(xComment.Element("articleId").Value),
                PubDate = DateTime.Parse(xComment.Element("pubDate").Value),
            };
            return comment;
        }

        public IEnumerable<Comment> GetCommentsBy(int articleId)
        {
            var comments = new List<Comment>();

            foreach(var item in Document.Element("comments").Elements("comment"))
            {
                if (item.Element("articleId").Value == articleId.ToString())
                {
                    var comment =  new Comment
                    {
                        Id = int.Parse(item.Element("id").Value),
                        Text = item.Element("text").Value,
                        UserName = item.Element("userName").Value,
                        ArticleId = int.Parse(item.Element("articleId").Value),
                        PubDate = DateTime.Parse(item.Element("pubDate").Value),
                    };
                    comments.Add(comment);
                }
            }

            return comments;
        }

        public void Create(Comment comment)
        {
            XElement root = Document.Element("comments");
            if (root.Attribute("lastId") == null)
            {
                root.Add(new XAttribute("lastId", 0));
            }

            int lastId = int.Parse(root.Attribute("lastId").Value);

            root.Attribute("lastId").Value = (++lastId).ToString();

            root.Add(new XElement("comment",
                new XElement("id", lastId),
                new XElement("text", comment.Text),
                new XElement("userName", comment.UserName),
                new XElement("pubDate", comment.PubDate.Value.ToString(ISOFormat)),
                new XElement("articleId", comment.ArticleId)));
        }

        public void Update(Comment comment)
        {
            XElement xComment = null;
            foreach (var item in Document.Element("comments").Elements("comment"))
            {
                if (item.Element("id").Value == comment.Id.ToString())
                {
                    xComment = item;
                    break;
                }
            }

            xComment.Element("text").Value = comment.Text;
            xComment.Element("userName").Value = comment.UserName;
        }

        public void Delete(int id)
        {
            XElement xComment = null;
            foreach (var item in Document.Element("comments").Elements("comment"))
            {
                if (item.Element("id").Value == id.ToString())
                {
                    xComment = item;
                    break;
                }
            }

            xComment.Remove();
        }
    }
}