using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Snake.Context;
using Snake.Models;

namespace Snake.Controllers
{
    public class ChannelController : Controller
    {
        protected UserManager<UserModel> UserManager { get; }
        protected SignInManager<UserModel> SignInManager { get; }
        protected RoleManager<IdentityRole<int>> RoleManager { get; }
        private readonly EFContext _context;

        public ChannelController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, RoleManager<IdentityRole<int>> roleManager, EFContext context)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            _context = context;
        }

        // GET: Channel
        public async Task<IActionResult> Index()
        {
            UserModel user = await UserManager.GetUserAsync(User);

            var list = _context.Channels.Include(c => c.UserAuthor).Where(c => c.UserAuthor.Id == user.Id).ToList();

            if (list.Count == 0)
            {
                ChannelModel channelModel = new ChannelModel();
                channelModel.Color = "red";
                channelModel.Name = $"{user.UserName}";
                channelModel.UserAuthor = user;

                _context.Channels.Add(channelModel);
                _context.SaveChanges();

                int newChanneID = _context.Channels.Where(c => c.Name == user.UserName).Single().ID;

                UserChannelModel userChannel = new UserChannelModel();
                userChannel.UserID = user.Id;
                userChannel.ChannelID = newChanneID;

                _context.UserChannels.Add(userChannel);
                _context.SaveChanges();
            }

            var userOutput = _context.Users.Include(u => u.Channels).Where(u => u.Id == user.Id).Single();
            return View(userOutput);
        }

        // GET: Channel/Details/5
        public async Task<IActionResult> Details(int? id, int? all)
        {

            if (id == null)
            {
                return NotFound();
            }

            ViewBag.channel = _context.Channels.Where(c => c.ID == id).Include(c => c.UserAuthor).Single();
            ViewBag.messages = _context.Messages.Include(m => m.Channel).Where(m => m.Channel.ID == id).Include(m => m.User);
            if (ViewBag.channel == null)
            {
                return NotFound();
            }

            MessageModel message = new MessageModel();

            if (all != null) ViewBag.All = "1";

            return View(message);
        }

        // GET: Channel/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.listColor = new List<SelectListItem>() {
                 new SelectListItem {Text = "zielony", Value = "green"},
                 new SelectListItem {Text = "różowy", Value = "pink"},
                 new SelectListItem {Text = "czerwony", Value = "red"},
                 new SelectListItem {Text = "zielony", Value = "green"},
                 new SelectListItem {Text = "żółty", Value = "yellow"},
                 new SelectListItem {Text = "purpurowy", Value = "purple"}
            };

            return View();
        }

        // POST: Channel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Color")] ChannelModel channelModel)
        {
            UserModel user = await UserManager.GetUserAsync(User);
            var existChannel = _context.Channels.Where(c => c.Name == channelModel.Name).Include(c => c.UserAuthor).SingleOrDefault();

            if (ModelState.IsValid && (existChannel == null || existChannel.UserAuthor.UserName != user.UserName))
            {
                channelModel.UserAuthor = user;

                _context.Channels.Add(channelModel);
                await _context.SaveChangesAsync();

                channelModel = _context.Channels.Where(c => c.Name == channelModel.Name).Include(c => c.UserAuthor).Where(c => c.UserAuthor.UserName == user.UserName).Single();

                UserChannelModel userChannel = new UserChannelModel();
                userChannel.UserID = user.Id;
                userChannel.ChannelID = channelModel.ID;

                _context.UserChannels.Add(userChannel);
                await _context.SaveChangesAsync();

                return Redirect("/Channel/Details/" + channelModel.ID);
            }
            else
            {
                ViewBag.info = "kanał już istnieje!";
            }
            return View(channelModel);
        }

        // GET: Channel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channelModel = await _context.Channels.SingleOrDefaultAsync(m => m.ID == id);
            if (channelModel == null)
            {
                return NotFound();
            }
            return View(channelModel);
        }

        // POST: Channel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Color")] ChannelModel channelModel)
        {
            if (id != channelModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(channelModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChannelModelExists(channelModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(channelModel);
        }

        // GET: Channel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channelModel = await _context.Channels
                .SingleOrDefaultAsync(m => m.ID == id);

            if (channelModel == null)
            {
                return NotFound();
            }

            return View(channelModel);
        }

        // POST: Channel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var channelModel = await _context.Channels.Include(c => c.Messages).SingleOrDefaultAsync(m => m.ID == id);

            _context.Channels.Remove(channelModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ShowAllChannel()
        {
            UserModel user = await UserManager.GetUserAsync(User);

            var list = _context.Channels.Include(c => c.UserAuthor).Where(c => c.UserAuthor.Id == user.Id).ToList();

            if (list.Count == 0)
            {
                ChannelModel channelModel = new ChannelModel();
                channelModel.Color = "red";
                channelModel.Name = $"{user.UserName}";
                channelModel.UserAuthor = user;

                _context.Channels.Add(channelModel);
                _context.SaveChanges();

                int newChanneID = _context.Channels.Where(c => c.Name == user.UserName).Single().ID;

                UserChannelModel userChannel = new UserChannelModel();
                userChannel.UserID = user.Id;
                userChannel.ChannelID = newChanneID;

                _context.UserChannels.Add(userChannel);
                _context.SaveChanges();
            }


            var listChnnel = _context.Channels.Include(c => c.UserAuthor).ToList();

            if (listChnnel.Count == 0)
            {
                ViewBag.Info = "Brak kanałów";
                return View();
            }

            return View(listChnnel);
        }


        private bool ChannelModelExists(int id)
        {
            return _context.Channels.Any(e => e.ID == id);
        }
    }
}
