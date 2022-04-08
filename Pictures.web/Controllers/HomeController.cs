using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pictures.data;
using Pictures.web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;



namespace Pictures.web.Controllers
{
    public class HomeController : Controller
    {
     
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            ImagesRepository repo = new(connectionString);
            var home = new HomeViewModel
            { 
                Images = repo.GetAll()
            };
          
            return View(home);
        }
      
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(Image image, IFormFile imageFile)
        {
            string fileName = $"{Guid.NewGuid()}-{imageFile.FileName}";

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            imageFile.CopyTo(fs);
            image.ImageLocation = fileName;


            var connectionString = _configuration.GetConnectionString("ConStr");
            ImagesRepository repo = new(connectionString);
            image.Date = DateTime.Now;
            repo.Add(image);
            return Redirect("/");
        }
        public IActionResult ViewImage(int id, bool liked)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            ImagesRepository repo = new(connectionString);
            Image image = repo.GetById(id);
            if (image == null)
            {
                return Redirect("/");
            }
            ViewImageViewModel vm = new ViewImageViewModel
            {
                Image = image
            };
           

            List<int> ids = HttpContext.Session.Get<List<int>>("ListingIds");
           
           
            if (ids == null)
            {
                ids = new List<int>();
            }
            if (liked)
            {
                ids.Add(id);
                repo.AddLike(image);
            }
           
            HttpContext.Session.Set("ListingIds", ids);
            vm.DisabledIds = ids;
            return View(vm);
        }
        public IActionResult GetLikes(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            ImagesRepository repo = new(connectionString);
            Image image = repo.GetById(id);
            return Json(image.Likes);
        }

    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
