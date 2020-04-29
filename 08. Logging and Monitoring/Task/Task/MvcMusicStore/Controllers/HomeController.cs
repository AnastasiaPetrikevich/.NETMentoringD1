using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcMusicStore.Logger.Interfaces;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();
		private readonly ILogger logger;

		public HomeController(ILogger logger)
		{
			this.logger = logger;
			logger.Info("Home controller created.");
		}

		// GET: /Home/
		public async Task<ActionResult> Index()
        {
			logger.Info("Home controller GET: /Home/ started.");

			return View(await _storeContext.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(6)
                .ToListAsync());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}