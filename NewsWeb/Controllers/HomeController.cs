using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imprima.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Mvc;
using X.PagedList;

namespace NewsWeb.Controllers
{
    public class HomeController : Controller
    {
	    private readonly INewsRepository _newsRepository;

	    public HomeController(INewsRepository newsRepository)
	    {
		    _newsRepository = newsRepository;
	    }
        public IActionResult Index(int? page, string searchString)
        {
			var news = _newsRepository.Search(searchString);
			var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
			var articles = news.ToPagedList(pageNumber, 5); // will only contain 5 products max because of the pageSize
			return View(articles);
		}
    }
}