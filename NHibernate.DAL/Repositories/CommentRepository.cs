using Business.Interfaces;
using Business.Models;
using Business.Services;
using System.Collections.Generic;
using System.Linq;

namespace NHibernate.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly NHUnitOfWork unitOfWork;

        public CommentRepository()
        {
            unitOfWork = ServiceManager.GetUnitOfWork() as NHUnitOfWork;
        }

        public IEnumerable<Comment> GetAll()
        {
            unitOfWork.OpenSession();

            var comments = unitOfWork.Session.Query<Comment>();
            return comments;
        }

        public Comment Get(int id)
        {
            unitOfWork.OpenSession();

            var comment = unitOfWork.Session.Get<Comment>(id);
            return comment;
        }

        public IEnumerable<Comment> GetCommentsBy(int articleId)
        {
            unitOfWork.OpenSession();

            var comments = unitOfWork.Session.Query<Comment>().Where(c => c.ArticleId == articleId).ToList();
            return comments;
        }
        
        public void Create(Comment comment)
        {
            unitOfWork.OpenSession();
            unitOfWork.BeginTransaction();

            unitOfWork.Session.Save(comment);  
        }

        public void Update(Comment comment)
        {
            unitOfWork.OpenSession();
            unitOfWork.BeginTransaction();

            unitOfWork.Session.Update(comment);
        }

        public void Delete(int id)
        {
            unitOfWork.OpenSession();
            unitOfWork.BeginTransaction();

            var comment = unitOfWork.Session.Get<Comment>(id);
            unitOfWork.Session.Delete(comment);
        }
    }
}