using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Business.Models;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;

namespace NewsPortal
{
    public static class LuceneSearch
    {
        private static string _luceneDir = "D:/ASP.Net/NewsPortalAlfa/NewsPortal/lucene_index";
        private static FSDirectory _directoryTemp;

        private static FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
                if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
                var lockFilePath = Path.Combine(_luceneDir, "write.lock");
                if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                return _directoryTemp;
            }
        }

        private static void _addToLuceneIndex(Article article, IndexWriter writer)
        {
            var searchQuery = new TermQuery(new Term("Id", article.Id.ToString()));
            writer.DeleteDocuments(searchQuery);

            var doc = new Document();

            doc.Add(new Field("Id", article.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Title", article.Title, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Description", article.Description, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("PubDate", article.PubDate.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("ImageUrl", article.ImageUrl, Field.Store.YES, Field.Index.ANALYZED));

            writer.AddDocument(doc);
        }

        public static void AddUpdateLuceneIndex(IEnumerable<Article> articleList)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // add data to lucene search index (replaces older entry if any)
                foreach (var article in articleList) _addToLuceneIndex(article, writer);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public static void AddUpdateLuceneIndex(Article article)
        {
            AddUpdateLuceneIndex(new List<Article> { article });
        }

        public static void DeleteArticle(int articleId)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entry
                var searchQuery = new TermQuery(new Term("Id", articleId.ToString()));
                writer.DeleteDocuments(searchQuery);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public static bool ClearLuceneIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.DeleteAll();

                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static Article _mapLuceneDocumentToData(Document document)
        {
            return new Article
            {
                Id = Convert.ToInt32(document.Get("Id")),
                Title = document.Get("Title"),
                Description = document.Get("Description"),
                PubDate = Convert.ToDateTime(document.Get("PubDate")),
                ImageUrl = document.Get("ImageUrl"),
            };
        }

        private static IEnumerable<Article> _mapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(_mapLuceneDocumentToData).ToList();
        }

        private static IEnumerable<Article> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits,
            IndexSearcher searcher)
        {
            return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        private static FuzzyQuery fparseQuery(string searchQuery)
        {
            var query = new FuzzyQuery(new Term("Content", searchQuery), 0.5f);
            return query;
        }

        private static IEnumerable<Article> _search(string searchQuery, string searchField = "")
        {
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<Article>();

            using (var searcher = new IndexSearcher(_directory, false))
            {
                var hits_limit = 1000;
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                    var query = fparseQuery(searchQuery);
                    var hits = searcher.Search(query, hits_limit).ScoreDocs;
                    var results = _mapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
                else
                {
                    var parser = new MultiFieldQueryParser
                        (Version.LUCENE_30, new[] { "Id", "Name", "Description" }, analyzer);
                    var query = fparseQuery(searchQuery);
                    var hits = searcher.Search
                    (query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                    var results = _mapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }

        public static IEnumerable<Article> Search(string input, string fieldName = "")
        {
            return string.IsNullOrEmpty(input) ? new List<Article>() : _search(input, fieldName);
        }

        public static IEnumerable<Article> GetAllArticles()
        {
            if (!System.IO.Directory.EnumerateFiles(_luceneDir).Any()) return new List<Article>();

            var searcher = new IndexSearcher(_directory, false);
            var reader = IndexReader.Open(_directory, false);
            var docs = new List<Document>();
            var term = reader.TermDocs();
            while (term.Next()) docs.Add(searcher.Doc(term.Doc));
            reader.Dispose();
            searcher.Dispose();
            return _mapLuceneToDataList(docs);
        }

    }
}