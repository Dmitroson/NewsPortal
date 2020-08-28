﻿using Business.Interfaces;
using Business.Lucene;
using Business.Models;

namespace Business.Services
{
    public static class ServiceManager
    {
        private static IUnitOfWork unitOfWork;
        private static IArticleRepository articleRepository;
        private static ICommentRepository commentRepository;
        private static ILuceneSearcher<Article> luceneSearcher;

        public static void SetUnitOfWork(IUnitOfWork unitOfWorkInstance)
        {
            unitOfWork = unitOfWorkInstance;
        }

        public static IUnitOfWork GetUnitOfWork()
        {
            return unitOfWork;
        }

        public static void SetArticleRepository(IArticleRepository articleRepositoryInstance)
        {
            articleRepository = articleRepositoryInstance;
        }

        public static IArticleRepository GetArticleRepository()
        {
            return articleRepository;
        }

        public static void SetCommentRepository(ICommentRepository commentRepositoryInstance)
        {
            commentRepository = commentRepositoryInstance;
        }

        public static ICommentRepository GetCommentRepository()
        {
            return commentRepository;
        }

        public static void SetLuceneSearcher(ILuceneSearcher<Article> luceneSearcherInstance)
        {
            luceneSearcher = luceneSearcherInstance;
        }

        public static ILuceneSearcher<Article> GetLuceneSearcher()
        {
            return luceneSearcher;
        }
    }
}
