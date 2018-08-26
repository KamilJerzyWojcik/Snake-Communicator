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
    public class MessageController : Controller
    {
        protected UserManager<UserModel> UserManager { get; }
        private readonly EFContext _context;

        public MessageController(EFContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            UserManager = userManager;
        }

        // GET: Message
        public async Task<IActionResult> Index()
        {
            return View(await _context.Messages.ToListAsync());
        }

        // GET: Message/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageModel = await _context.Messages
                .SingleOrDefaultAsync(m => m.ID == id);
            if (messageModel == null)
            {
                return NotFound();
            }

            return View(messageModel);
        }

        // GET: Message/Create
        public IActionResult Create(int id)
        {
            MessageModel message = new MessageModel();
            message.Channel = _context.Channels.SingleOrDefault(c => c.ID == id);
            return View(message);
        }

        // POST: Message/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Message")] MessageModel messageModel, int id)
        {
            messageModel.Channel = _context.Channels.SingleOrDefault(c => c.ID == id);
            messageModel.Created = DateTime.Now;
            messageModel.User = await UserManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                _context.Add(messageModel);
                await _context.SaveChangesAsync();
            }
            return Redirect("/Channel/Details/" + id);
        }

        // GET: Message/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageModel = await _context.Messages.SingleOrDefaultAsync(m => m.ID == id);
            ViewBag.channelId = _context.Messages.Where(m => m.ID == id).Include(m => m.Channel).Single().Channel.ID;

            if (messageModel == null)
            {
                return NotFound();
            }
            return View(messageModel);
        }

        // POST: Message/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ID, Message, Created")] MessageModel messageModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var editedMessage = _context.Messages.Where(m => m.ID == messageModel.ID).Include(m => m.User).Include(m => m.Channel).Single();
                    editedMessage.Message = messageModel.Message;

                    _context.Update(editedMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageModelExists(messageModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var channelID = _context.Messages.Where(m => m.ID == messageModel.ID).Include(m => m.Channel).Single().Channel.ID;

                return RedirectToAction($"Details/{channelID}", "Channel");
            }
            return View(messageModel);
        }

        // GET: Message/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageModel = await _context.Messages.SingleOrDefaultAsync(m => m.ID == id);

            if (messageModel == null)
            {
                return NotFound();
            }

            ViewBag.channel = _context.Messages.Where(m => m.ID == id).Include(m => m.Channel).Single().Channel;

            return View(messageModel);
        }

        // POST: Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var messageModel = await _context.Messages.Include(m => m.Channel).SingleOrDefaultAsync(m => m.ID == id);
            int channelID = messageModel.Channel.ID;
            _context.Messages.Remove(messageModel);
            await _context.SaveChangesAsync();

            return RedirectToAction($"Details/{channelID}", "Channel");
        }

        private bool MessageModelExists(int id)
        {
            return _context.Messages.Any(e => e.ID == id);
        }
    }
}
