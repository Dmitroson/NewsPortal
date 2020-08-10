using Business.Interfaces;
using Business.Models;
using Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Xml.DAL.Repositories
{
    public class XmlCommentRepository : ICommentRepository
    {
        private const string ISOFormat = "yyyy-MM-dd\\THH:mm:ss";

        private readonly XmlUnitOfWork unitOfWork;

        public XmlCommentRepository()
        {
            unitOfWork = ServiceManager.GetUnitOfWork() as XmlUnitOfWork;
        }

        public IEnumerable<Comment> GetAll()
        {
            unitOfWork.OpenDocument();

            XElement root = unitOfWork.Document.Element("comments");
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
            unitOfWork.OpenDocument();

            XElement xComment = null;
            foreach (var item in unitOfWork.Document.Element("comments").Elements("comment"))
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
            unitOfWork.OpenDocument();

            var comments = new List<Comment>();

            foreach(var item in unitOfWork.Document.Element("comments").Elements("comment"))
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
            unitOfWork.OpenDocument();
            
            XElement root = unitOfWork.Document.Element("comments");
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
            unitOfWork.OpenDocument();

            XElement xComment = null;
            foreach (var item in unitOfWork.Document.Element("comments").Elements("comment"))
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
            unitOfWork.OpenDocument();

            XElement xComment = null;
            foreach (var item in unitOfWork.Document.Element("comments").Elements("comment"))
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