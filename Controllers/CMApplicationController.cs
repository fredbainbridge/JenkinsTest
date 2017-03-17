using Microsoft.AspNetCore.Mvc;
using CMWebAPI.Models;

namespace CMWebAPI.Controllers
{
    public class CMApplicationController : Controller
    {
        private CMContext _context;
        private ICMApplicationRepository _CMApplicationRepository;
        private IApplicationFromDBViewRepository _ApplicationFromDBViewRepository;

        public CMApplicationController(ICMApplicationRepository CMAppRepository, IApplicationFromDBViewRepository AppFromViewRepository) 
        {
            _CMApplicationRepository = CMAppRepository;
            _ApplicationFromDBViewRepository = AppFromViewRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_CMApplicationRepository.GetAll());
        }

        public JsonResult Name(string id)
        {
            return Json(_CMApplicationRepository.GetByName(id));
        }

        public IActionResult DP(string id, string appName)
        {
            return View(_CMApplicationRepository.GetAppOnDP(id, appName));
        }

        public JsonResult DPJSON(string id, string appName)
        {
            return Json(_CMApplicationRepository.GetAppOnDP(id, appName));
        }
        public IActionResult AppFromView()
        {
            return View(_ApplicationFromDBViewRepository.GetAll());
        }
       /* public IActionResult Technology(string id)
        {
            //this should happen in the repository class
            var apps =  from app in _context.Application
                        where app.Technology == id
                        select app;
            return View(apps);
        }
        
        public IActionResult CI(int id)
        {
            var app = from x in _context.Application
                      where x.CI_ID == id
                      select x;
            return View(app);
        }
        */
        /*
        public  IActionResult AdminCategories(string id)
        {
            string query = @"
                    SELECT
                    App.CI_ID as CI_ID, App.DisplayName as Name,
                    stuff((select '; '+ LC.CategoryInstanceName
                        from v_LocalizedCategories LC,
                        vAdminCategoryMemberships ACM
                        where ACM.CategoryInstanceID = LC.CategoryInstanceID AND App.CI_UniqueID = ACM.ObjectKey
                        for xml path(''), type).value('.', 'varchar(max)'), 1, 1, '') As 'AdministrativeCategories'
                    FROM
                    fn_ListLatestApplicationCIs(1033) App
                    ORDER BY App.DisplayName
            ";
            //var app =  _context.Database
            //    .ExecuteSqlCommand(query);
            var app = _context.ApplicationWithAdminCategory
                .FromSql(query);
            //var app = _context.Database.ExecuteSqlCommand(query);
            return View(app);
        }
        */
        
    }
}
