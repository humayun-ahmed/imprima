using System.Collections.Generic;

namespace Imprima.ExternalService.Dto
{
	public class NewsRoot
	{
		public string Status { get; set; }
		public int TotalResults { get; set; }
		public List<ArticleDto> Articles { get; set; }
	}
}