using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Snake.Context;
using Snake.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Snake.Controllers
{
    public class HomeController : Controller
    {
        protected UserManager<UserModel> UserManager { get; }
        public EFContext Context { get; }
        public HomeController(UserManager<UserModel> userManager, EFContext context)
        {
            UserManager = userManager;
            Context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(MessageModel message)
        {
            return View(message);
        }
        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Edit(MessageModel message)
        {
            return View(message);
        }
        [HttpGet]
        public IActionResult Remove(int id)
        {
            var messageModel = Context.Messages.Where(x => x.ID == id).SingleOrDefault();
            return View(messageModel);
        }
        [HttpPost]
        public IActionResult RemoveConfirm(int id)
        {
            var messageModel = Context.Messages.Where(x => x.ID == id).SingleOrDefault();
            Context.Messages.Remove(messageModel);
            Context.SaveChanges();
            return View();
        }




    }

    
}
