using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DawidKrolikiewiczProj.Data;
using DawidKrolikiewiczProj.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace DawidKrolikiewiczProj.Controllers
{
    [Authorize]
    public class PoselsController : Controller
    {
        private readonly DawidKrolikiewiczProjContext _context;

        public PoselsController(DawidKrolikiewiczProjContext context)
        {
            _context = context;
        }

        // GET: Posels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Posel.ToListAsync());
        }

        // GET: Posels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posel = await _context.Posel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (posel == null)
            {
                return NotFound();
            }

            return View(posel);
        }

        // GET: Posels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Club,Profession")] Posel posel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(posel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(posel);
        }

        // GET: Posels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posel = await _context.Posel.FindAsync(id);
            if (posel == null)
            {
                return NotFound();
            }
            return View(posel);
        }


        // [Custom: Wypełnianie formularza danymi losowego posła z api.sejm.gov]
        public async Task<IActionResult> Get()
        {
            Posel posel = new Posel();

            Random random = new Random();
            int num = random.Next(1, 495);

            using (var client = new HttpClient())
            {
                var endpoint = new Uri("https://api.sejm.gov.pl/sejm/term10/MP/" + num);
                var result = client.GetAsync(endpoint).Result;
                var json = await result.Content.ReadAsStringAsync();

                posel = parsePosel(json);
            }

            return View(posel);
        }

        private Posel parsePosel(string json)
        {
            dynamic data = JObject.Parse(json);

            Posel posel = new Posel();
            posel.FirstName = data.firstName;
            posel.Profession = data.profession;
            posel.LastName = data.lastName;
            posel.Club = data.club;

            return posel;
        }

        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            // Return view only with objects that match the pattern
            return View("Index", await _context.Posel.Where(j => j.FirstName.Contains(SearchPhrase) || j.LastName.Contains(SearchPhrase)).ToListAsync());
        }

        // POST: Posels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Club,Profession")] Posel posel)
        {
            if (id != posel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PoselExists(posel.Id))
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
            return View(posel);
        }

        // GET: Posels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posel = await _context.Posel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (posel == null)
            {
                return NotFound();
            }

            return View(posel);
        }

        // POST: Posels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var posel = await _context.Posel.FindAsync(id);
            if (posel != null)
            {
                _context.Posel.Remove(posel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PoselExists(int id)
        {
            return _context.Posel.Any(e => e.Id == id);
        }
    }
}
