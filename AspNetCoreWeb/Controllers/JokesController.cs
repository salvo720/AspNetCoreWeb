using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWeb.Data;
using AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using System.Diagnostics.Metrics;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http.Features;
using System.Text.RegularExpressions;
using System.Numerics;

namespace AspNetCoreWeb.Controllers
{
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JokesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jokes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Joke.ToListAsync());
        }

        public async Task<IActionResult> JokesSearchForm()
        {
            return View();
        }

        // il nome della variabile deve essere uguale al name del campo input o andra in errore la raccolta dei dati 
        public async Task<IActionResult> JokesFormResult(string searchPhrase)
        {
            // se alla funzione view si mette tra le virgolette il primo parametro indichera una view da richiamare
            // il secondo parametro e l'orm che raccoglie i dati 
            return View("Index", await _context.Joke.Where(where => where.jokeQuestion.Contains(searchPhrase)).ToListAsync());
        }

        // GET: Jokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Joke == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke
                .FirstOrDefaultAsync(m => m.id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        // GET: Jokes/Create

        [Authorize] // [Authorize] : indica che l'utente deve essere loggato per poter proseguire con la funzione altrimenti
                    // sara reindirizzato alla pagina di log-in 
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("id,jokeQuestion,jokeAnswer")] Joke joke)
        {
            if (ModelState.IsValid)
            {
                _context.Add(joke);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(joke);
        }

        // GET: Jokes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Joke == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }
            return View(joke);
        }

        // POST: Jokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("id,jokeQuestion,jokeAnswer")] Joke joke)
        {
            if (id != joke.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(joke);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JokeExists(joke.id))
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
            return View(joke);
        }

        // GET: Jokes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Joke == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke
                .FirstOrDefaultAsync(m => m.id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        // POST: Jokes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Joke == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Joke'  is null.");
            }
            var joke = await _context.Joke.FindAsync(id);
            if (joke != null)
            {
                _context.Joke.Remove(joke);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JokeExists(int id)
        {
            return _context.Joke.Any(e => e.id == id);
        }

        //leetCode question : 14. Longest Common Prefix
        public string LongestCommonPrefix()
        {
            char letter = ' ';
            string textshared = "";
            string[] strs = { "car", "ci" };
            int counter = -1;
            int subElement = 2; // e 2 perche : 1) -1 perche lenght inzia a contare da 1 , 2) -1 perche counter parte da -1 come valore inizale 

            if (strs.Length == 1) return strs[0];

            for (int elemento = 0; elemento < 1; elemento++)
            {
                for (int lettera = 0; lettera < strs[elemento].Length; lettera++)
                {
                    letter = strs[elemento][lettera];
                    counter = -1;
                    for (int elemento2 = 1; elemento2 < strs.Length; elemento2++)
                    {
                        Console.WriteLine("lettera : " + lettera + ",  strs[elemento].Length : " + strs[elemento].Length + ", strs[elemento].Length : " + strs[elemento2].Length);
                        if (lettera >= strs[elemento].Length || lettera >= strs[elemento2].Length)
                        {
                            continue;
                        }
                        Console.WriteLine("counter : " + counter + ", strs[elemento][lettera]  : " + strs[elemento][lettera] + ", strs[elemento2][lettera] " + strs[elemento2][lettera] + ", textshared : " + textshared);

                        if (strs[elemento][lettera] == strs[elemento2][lettera])
                        {
                            counter++;
                            if (Int32.Equals(counter, (strs.Length - subElement)))
                            {
                                textshared += letter;
                                //Console.WriteLine("counter : " + counter + ", subElement : " + subElement + ", strs.Length - subElement : " + (strs.Length - subElement) + ", textshared : " + textshared);
                            }
                            Console.WriteLine("counter : " + counter + ", subElement : " + subElement + ", strs.Length - subElement : " + (strs.Length - subElement) + ", textshared : " + textshared);
                        }
                        else if (strs[elemento][lettera] != strs[elemento2][lettera] || counter == -1)
                        {
                            return textshared;
                        }
                    }
                }
            }

            return textshared;
        }


       // soluzione presa da leet code , riscrivere cosi resta impressa 
        public string ConvertSolution(string s, int numRows)
        {
            s = "PAYPALISHIRING"; numRows = 3;
            if (numRows == 1) return s;

            // creo un array prendendo il valore minimo tra le righe e la lunghezza di s 
            string[] rows = new string[Math.Min(numRows, s.Length)];
            int curRow = 0;
            bool goingDown = false;
            string ret = "";


            foreach (char c in s)
            {
                rows[curRow] += c;
                // cerco i casi in cui la direzione deve essere invertita , e lo faccio assegnando a goingDown il valore opposto 
                if (curRow == 0 || curRow == numRows - 1) goingDown = !goingDown;
                // il prossimo indice dipende dal valore di goingDown , se e true sara +1 se e false sara -1 
                curRow += goingDown ? 1 : -1;
            }

            foreach (string row in rows) ret += row;
            return ret;
        }
    }
}

  
    