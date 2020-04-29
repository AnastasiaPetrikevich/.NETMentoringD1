using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcMusicStore.Infastructure;
using MvcMusicStore.Models;
using PerformanceCounterHelper;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();
		private readonly CounterHelper<PerfomanceCounters> counterHelper;

		public StoreController(CounterHelper<PerfomanceCounters> counterHelper)
		{
			this.counterHelper = counterHelper;
		}

		// GET: /Store/
		public async Task<ActionResult> Index()
        {
            return View(await _storeContext.Genres.ToListAsync());
        }

        // GET: /Store/Browse?genre=Disco
        public async Task<ActionResult> Browse(string genre)
        {
			var stopwatch = Stopwatch.StartNew();
			var resultView = View(await _storeContext.Genres.Include("Albums").SingleAsync(g => g.Name == genre));
			stopwatch.Stop();

			counterHelper.IncrementBy(PerfomanceCounters.ProcessingBrowseRequestInStoreController, stopwatch.ElapsedTicks);

			return resultView;
		}

        public async Task<ActionResult> Details(int id)
        {
            var album = await _storeContext.Albums.FindAsync(id);

            return album != null ? View(album) : (ActionResult)HttpNotFound();
        }

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            return PartialView(
                _storeContext.Genres.OrderByDescending(
                    g => g.Albums.Sum(a => a.OrderDetails.Sum(od => od.Quantity))).Take(9).ToList());
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