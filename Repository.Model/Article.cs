using System;

namespace Imprima.Repository.Model
{
	public class Article
	{
		public int SourceKeyId { get; set; }
		public Source Source { get; set; }
		public string Author { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Url { get; set; }
		public string UrlToImage { get; set; }
		public DateTime? PublishedAt { get; set; }
		public string Content { get; set; }
		public long ArticleId { get; set; }
	}
}