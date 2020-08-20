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

namespace LuceneSearcher
{
    public static class LuceneSearch
    {
        private static string diriectoryPath;
        private static FSDirectory directoryTemp;

        public static void SetDirectory(string path)
        {
            diriectoryPath = path;
        }

        private static FSDirectory directory
        {
            get
            {
                if (directoryTemp == null) directoryTemp = FSDirectory.Open(new DirectoryInfo(diriectoryPath));
                if (IndexWriter.IsLocked(directoryTemp)) IndexWriter.Unlock(directoryTemp);
                var lockFilePath = Path.Combine(diriectoryPath, "write.lock");
                if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                return directoryTemp;
            }
        }

        private static void AddToLuceneIndex(Article article, IndexWriter writer)
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
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var article in articleList) AddToLuceneIndex(article, writer);

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
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var searchQuery = new TermQuery(new Term("Id", articleId.ToString()));
                writer.DeleteDocuments(searchQuery);

                analyzer.Close();
                writer.Dispose();
            }
        }

        public static bool ClearLuceneIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
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

        private static Article MapLuceneDocumentToData(Document document)
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

        private static IEnumerable<Article> MapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(MapLuceneDocumentToData).ToList();
        }

        private static IEnumerable<Article> MapLuceneToDataList(IEnumerable<ScoreDoc> hits,
            IndexSearcher searcher)
        {
            return hits.Select(hit => MapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }

        private static Query ParseQuery(string searchQuery)
        {
            var query = new PrefixQuery(new Term("Description", searchQuery));
            return query;
        }

        private static IEnumerable<Article> _search(string searchQuery, string searchField = "Id")
        {
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<Article>();
            
            using (var searcher = new IndexSearcher(directory, false))
            {
                var hits_limit = 1000;
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                    var query = ParseQuery(searchQuery);
                    var hits = searcher.Search(query, hits_limit).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
                else
                {
                    var parser = new MultiFieldQueryParser
                        (Version.LUCENE_30, new[] { "Id", "Name", "Description" }, analyzer);
                    var query = ParseQuery(searchQuery);
                    var hits = searcher.Search
                    (query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }

        public static IEnumerable<Article> Search(string input, string fieldName = "")
        {
            var articles = new List<Article>();
            return string.IsNullOrEmpty(input) ? articles : _search(input, fieldName);
        }

        public static IEnumerable<Article> GetAllArticles()
        {
            if (!System.IO.Directory.EnumerateFiles(diriectoryPath).Any()) return new List<Article>();

            var searcher = new IndexSearcher(directory, false);
            var reader = IndexReader.Open(directory, false);
            var docs = new List<Document>();
            var term = reader.TermDocs();
            while (term.Next()) docs.Add(searcher.Doc(term.Doc));
            reader.Dispose();
            searcher.Dispose();
            return MapLuceneToDataList(docs);
        }

    }
}