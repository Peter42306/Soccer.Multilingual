using Microsoft.AspNetCore.Mvc;
using Soccer.Multilingual.Filters;
using Soccer.Multilingual.Models;

namespace MultilingualSite.Controllers
{
    [Culture]
    public class ClubController : Controller
    {
        private readonly ClubContext cc;

        public ClubController(ClubContext context)
        {
            cc = context;
        }

        public ActionResult Index()
        {
            HttpContext.Session.SetString("path", Request.Path);
            return View(cc.Clubs);
        }

        [HttpGet]
        public ActionResult CreateClub()
        {
            HttpContext.Session.SetString("path", Request.Path);
            return View();
        }

        [HttpPost]
        public ActionResult CreateClub(Club club)
        {
            if (ModelState.IsValid)
            {
                cc.Clubs.Add(club);
                cc.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(club);
        }

        public ActionResult ChangeCulture(string lang)
        {
            string? returnUrl = HttpContext.Session.GetString("path") ?? "/Club/Index";

            // Список культур
            List<string> cultures = new List<string>() { "ru", "en", "uk", "de", "fr" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(10); // срок хранения куки - 10 дней
            Response.Cookies.Append("lang", lang, option); // создание куки
            return Redirect(returnUrl);
        }

    }
}
