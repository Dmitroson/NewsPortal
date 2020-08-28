using Business.Lucene;
using Business.Models;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System.Collections.Generic;
using System.IO;
using Version = Lucene.Net.Util.Version;

namespace Lucene
{
    public class LuceneSearcher : ILuceneSearcher<Article>
    {
        private FSDirectory directory;
        private string directoryPath;

        public LuceneSearcher(string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        private FSDirectory Directory
        {
            get
            {
                if (directory == null)
                    directory = FSDirectory.Open(new DirectoryInfo(directoryPath));

                if (IndexWriter.IsLocked(directory))
                    IndexWriter.Unlock(directory);

                var lockFilePath = Path.Combine(directoryPath, "write.lock");
                if (File.Exists(lockFilePath))
                    File.Delete(lockFilePath);

                return directory;
            }
        }

        public void Delete(int articleId)
        {
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var searchQuery = new TermQuery(new Term("Id", articleId.ToString()));
                    writer.DeleteDocuments(searchQuery);
                }
            }
        }

        public void UpdateLuceneIndex(IEnumerable<Article> articles)
        {
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(Directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.DeleteAll();
                    writer.Optimize();
                }
            }
            foreach (var article in articles)
            {
                Save(article);
            }
        }

        public void Save(Article article)
        {
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var searchQuery = new TermQuery(new Term("Id", article.Id.ToString()));
                    writer.DeleteDocuments(searchQuery);

                    var document = new Document();
                    document.Fill(article);
                    writer.AddDocument(document);
                }
            }
        }

        public ArticleCollection GetArticlesBy(Criteria criteria, bool onlyVisible)
        {
            using (var analyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                using (var searcher = new IndexSearcher(Directory, true))
                {
                    var hitsLimit = 1000;
                    var fields = "Title,Description".Split(',');

                    var parser = new MultiFieldQueryParser(Version.LUCENE_30, fields, analyzer);

                    Query query;
                    if (!string.IsNullOrEmpty(criteria.SearchString))
                    {
                        query = parser.Parse(criteria.SearchString.Trim());
                    }
                    else
                    {
                        query = new MatchAllDocsQuery();
                    }

                    var filter = GetFilter(criteria.FilterRange);
                    var sort = GetSort(criteria.SortOrder);
                    var hits = searcher.Search(query, filter, hitsLimit, sort).ScoreDocs;

                    return hits.ToArticleCollection(searcher, criteria, onlyVisible);
                }
            }
        }

        private Filter GetFilter(DateRange dateRange)
        {
            string start = DateTools.DateToString(dateRange.Start, DateTools.Resolution.SECOND);
            string end = DateTools.DateToString(dateRange.End, DateTools.Resolution.SECOND);

            var filterRange = FieldCacheRangeFilter.NewStringRange("PubDate", start, end, true, true);
            return filterRange;
        }

        private Sort GetSort(SortOrder order)
        {
            switch (order)
            {
                case SortOrder.DateAscending:
                    return new Sort(new SortField("PubDate", SortField.STRING, false));

                case SortOrder.DateDescending:
                    return new Sort(new SortField("PubDate", SortField.STRING, true));

                case SortOrder.TitleAscending:
                    return new Sort(new SortField("Title", SortField.STRING, false));

                case SortOrder.TitleDescending:
                    return new Sort(new SortField("Title", SortField.STRING, true));

                case SortOrder.DescriptionAscending:
                    return new Sort(new SortField("Description", SortField.STRING, false));

                case SortOrder.DescriptionDescending:
                    return new Sort(new SortField("Description", SortField.STRING, true));

                default:
                    return new Sort(new SortField("PubDate", SortField.STRING, true));
            }
        }
    }
}