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

        public string Convert(string s, int numRows)
        {
            s = "PAYPALISHIRING"; numRows = 3;
            int addLetter = numRows, addLetterOld = 0;
            string finalstring = "", stringsplit;
            int lastofString = 3, lastitem, counter = 0, oldindex = -1;
            bool maxreached = false;

            if (s.Length <= numRows) return s;
            string[] array = new string[numRows];
            //Console.WriteLine("s.Lenght : " + s.Length + "s.IndexOf(stringsplit)" + s.IndexOf(stringsplit));

            while (addLetterOld <= s.Length)
            {
                stringsplit = s.Substring(addLetterOld, lastofString);
                Console.WriteLine("stringsplit.Length :" + stringsplit.Length + ", stringsplit : " + stringsplit + ", s.Substring(0, addLetter) : " + s.Substring(addLetterOld, lastofString));
                for (int i = 0; i <= stringsplit.Length - 1; i++)
                {
                    if (Int32.Equals(oldindex, -1))
                    {
                        array[i] += stringsplit[i];
                    }
                    else
                    {

                        if (oldindex == (stringsplit.Length - 1))
                        {
                            array[oldindex] += stringsplit[i];
                            oldindex--;
                            i++;
                            //maxreached = true;

                        }

                        else if (oldindex == 0)
                        {
                            oldindex++;
                            //maxreached = true;
                        }
                        //else if (oldindex >= stringsplit.Length)
                        //{
                        //    oldindex = 0;
                        //    maxreached = true;
                        //} 
                        array[oldindex] += stringsplit[i];
                        oldindex++;
                        Console.WriteLine("oldindex :" + oldindex);

                    }
                    Console.WriteLine("1)" + " i : " + i + ", array[i] : " + array[i] + ", stringsplit[i] :" + stringsplit[i]);
                    Console.WriteLine("Int32.Equals(i, (stringsplit.Length - 1))" + Int32.Equals(i, (stringsplit.Length - 1)));
                    if (Int32.Equals(i, (stringsplit.Length - 1)))
                    {
                        counter = 0;
                        maxreached = true;
                        for (int j = i; j >= 0;)
                        {
                            if (addLetterOld >= s.Length) break;
                            stringsplit = s.Substring(addLetterOld, lastofString);
                            if (maxreached == true)
                            {
                                i = 0;
                                maxreached = false;
                                addLetterOld += numRows;
                            }
                            if (counter <= stringsplit.Length)
                            {
                                j--;
                                if (j == stringsplit.Length - 1)
                                {
                                    array[j] += stringsplit[i];
                                    j--;
                                    i++;
                                }
                                else if (j >= 0)
                                {
                                    array[j] += stringsplit[i];
                                    i++;

                                }
                                else if (j == -1)
                                {
                                    j = 1;
                                    array[j] += stringsplit[i];
                                    j++;
                                    oldindex = j;
                                    Console.WriteLine("2)" + " i : " + i + ", j : " + j + ", array[j] : " + array[j] + ", stringsplit[i] :" + stringsplit[j]);
                                    j = -1;

                                }


                                counter++;
                            }

                            Console.WriteLine("i : " + i + ", j : " + j + ", stringsplit[i] : " + stringsplit[i] + " stringsplit : " + stringsplit);
                        }
                        Console.WriteLine(" maxreached : " + maxreached + ", i : " + i + ", (stringsplit.Length - 1) : " + (stringsplit.Length - 1));
                    }
                    //Console.WriteLine(Int32.Equals(i, (stringsplit.Length)));

                }
                addLetterOld += numRows;

                //Console.WriteLine("s.Length " + s.Length + ", addLetterOld : " + addLetterOld);
                if ((s.Length - addLetterOld) < 3)//calcola quanti elementi rimangono della stringa 
                {
                    lastofString = s.Length - addLetterOld;
                    //Console.WriteLine("2 s.Length " + s.Length + ", addLetterOld : " + addLetterOld + ", lastofString :" + lastofString);
                }
            }

            foreach (var item in array)
            {
                finalstring += item;
            }

            return finalstring;
        }

        public string Convert2(string s, int numRows)
        {
            s = "PAYPALISHIRING"; numRows = 3;
            int addLetter = numRows, addLetterOld = 0;
            string finalstring = "", stringsplit;
            int lastofString = 3, times = 0, oldIndex = -1, increaseIndex = 0;
            bool maxSize = false;

            int counveterDebug = 0;
            if (s.Length <= numRows) return s;
            string[] array = new string[numRows];
            //Console.WriteLine("s.Lenght : " + s.Length + "s.IndexOf(stringsplit)" + s.IndexOf(stringsplit));

            while (addLetterOld <= s.Length)
            {
                stringsplit = s.Substring(addLetterOld, lastofString);
                if ((times % 2) == 0)
                {
                    oldIndex = covertPositive(stringsplit, oldIndex, array, increaseIndex, maxSize);
                    Console.WriteLine("stringsplit.Length :" + stringsplit.Length + ", stringsplit : " + stringsplit + ", s.Substring(0, addLetter) : " + s.Substring(addLetterOld, lastofString));
                }
                else if ((times % 2) == 1)
                {
                    oldIndex = covertNegative(stringsplit, oldIndex, array, increaseIndex, maxSize);
                }
                addLetterOld += numRows;

                //Console.WriteLine("s.Length " + s.Length + ", addLetterOld : " + addLetterOld);
                if ((s.Length - addLetterOld) < 3)//calcola quanti elementi rimangono della stringa 
                {
                    lastofString = s.Length - addLetterOld;
                    //Console.WriteLine("2 s.Length " + s.Length + ", addLetterOld : " + addLetterOld + ", lastofString :" + lastofString);
                }
                times++;
            }
            foreach (var item in array)
            {
                finalstring += item;
                Console.WriteLine("counveterDebug : " + counveterDebug + ", item : " + item);
                counveterDebug++;
            }

            return finalstring;
        }

        public int covertPositive(string stringsplit, int oldIndex, string[] array, int increaseIndex, bool maxSize)
        {
            int stringIndex = 0, arrayIndex = 0, counterIteration = 0;
            bool maxIsOver = false, oneRunReset = true;
            for (int i = 0; i <= stringsplit.Length - 1; i++)
            {
                arrayIndex = (oldIndex != -1) ? oldIndex : arrayIndex;

                if (((stringIndex <= stringsplit.Length - 1) && (arrayIndex > stringsplit.Length - 1)) || (oldIndex != -1))
                {
                    if (oneRunReset && arrayIndex > stringsplit.Length - 1)
                    {
                        arrayIndex = ((stringsplit.Length - 1) - 1);
                        oneRunReset = false;
                        oldIndex = -1;
                        Console.WriteLine("1) if reset");
                    }
                    array[arrayIndex] += stringsplit[stringIndex];
                    Console.WriteLine("1) if" + ", oldIndex : " + oldIndex + ", arrayIndex : " + arrayIndex + ", stringIndex : " + stringIndex + " i : " + i + ", array[arrayIndex] : " + array[arrayIndex] + ", stringsplit[stringIndex] :" + stringsplit[stringIndex]);
                    arrayIndex--;
                    stringIndex++;

                }
                else
                {
                    array[arrayIndex] += stringsplit[stringIndex];
                    Console.WriteLine("1) else" + ", oldIndex : " + oldIndex + ", arrayIndex : " + arrayIndex + ", stringIndex : " + stringIndex + " i : " + i + ", array[arrayIndex] : " + array[arrayIndex] + ", stringsplit[stringIndex] :" + stringsplit[stringIndex]);
                    arrayIndex++;
                    stringIndex++;
                }
                if (arrayIndex != -1 && arrayIndex != 2) oldIndex = arrayIndex;

                counterIteration++;
                Console.WriteLine("oldIndex : " + oldIndex, ", counterIteration :" + counterIteration);

            }
            return ((arrayIndex - 1) == (stringsplit.Length - 1)) ? -1 : arrayIndex - 1;

        }




        public int covertNegative(string stringsplit, int oldIndex, string[] array, int increaseIndex, bool maxSize)
        {
            int stringIndex = 0, arrayIndex = 0, counterIteration = 0;
            bool maxIsOver = false, oneRunReset = true;
            for (int i = 2; i >= 0; i--)
            {
                arrayIndex = (oldIndex != -1) ? oldIndex : arrayIndex;

                if (((stringIndex <= stringsplit.Length - 1) && (arrayIndex < 0)) || ((oldIndex != -1)))
                {
                    if (oneRunReset)
                    {
                        array[arrayIndex] += stringsplit[stringIndex];
                        oneRunReset = false;
                    }
                    //Console.WriteLine("2) if " + ", oldIndex : " + oldIndex +  " , i :" + i + ", array[arrayIndex] : " + array[arrayIndex] + ", stringsplit[stringIndex] :" + stringsplit[stringIndex]);
                    Console.WriteLine("2) if" + ", oldIndex : " + oldIndex + ", arrayIndex : " + arrayIndex + ", stringIndex : " + stringIndex + " i : " + i + ", array[arrayIndex] : " + array[arrayIndex] + ", stringsplit[stringIndex] :" + stringsplit[stringIndex]);
                    arrayIndex++;
                    stringIndex++;
                    oldIndex = arrayIndex;
                    Console.WriteLine("oldIndex : " + oldIndex);
                }
                else
                {
                    arrayIndex--;
                    arrayIndex = (arrayIndex == -1) ? 1 : arrayIndex;
                    array[arrayIndex] += stringsplit[stringIndex];
                    Console.WriteLine("2) else" + ", oldIndex : " + oldIndex + ", arrayIndex : " + arrayIndex + ", stringIndex : " + stringIndex + " i : " + i + ", array[arrayIndex] : " + array[arrayIndex] + ", stringsplit[stringIndex] :" + stringsplit[stringIndex]);

                    stringIndex++;
                }
                if (i <= 0 && arrayIndex != 0) oldIndex = arrayIndex;
                //TODO : DA COMPELTARE
                counterIteration++;
                Console.WriteLine("oldIndex : " + oldIndex, ", counterIteration :" + counterIteration);



            }
            return ((arrayIndex + 1) == 0) ? -1 : arrayIndex + 1;

        }



        // TODO ; da gestire gli indici dopo che pal , la l si trova su 1 , gestire le condizioni degli if   

        public string Convert3(string s, int numRows)
        {
            s = "PAYPALISHIRING"; numRows = 3;
            int addLetter = numRows, addLetterOld = 0;
            string finalstring = "", stringsplit , stringsplit2;
            int lastofString = 3, times = 0, oldIndex = -1, increaseIndex = 0;
            bool maxSize = false;
            //merge variabili 
            int stringIndex = 0, arrayIndex = 0, counterIteration1 = 0, counterIteration2 = 0;
            bool maxIsOver = false, oneRunReset = true;

            int counveterDebug = 0;
            if (s.Length <= numRows) return s;
            string[] array = new string[numRows];
            //Console.WriteLine("s.Lenght : " + s.Length + "s.IndexOf(stringsplit)" + s.IndexOf(stringsplit));

            while (addLetterOld <= s.Length)
            {
                stringsplit = s.Substring(addLetterOld, lastofString);

                //arrayIndex = (arrayIndex >= (stringsplit.Length - 1)) ? arrayIndex - 1 : arrayIndex;
                //array[arrayIndex] += stringsplit[stringIndex];
                if ((times % 2) == 0)
                {
                    counterIteration1 = 0;
                    for (int i = 0; i <= (stringsplit.Length - 1); i++)
                    {
                        array[arrayIndex] += stringsplit[stringIndex];
                        Console.WriteLine("1) positive if " + ", oldIndex : " + oldIndex + ", arrayIndex : " + arrayIndex + ", stringIndex : " + stringIndex + " i : " + i + ", array[arrayIndex] : " + array[arrayIndex] + ", stringsplit[stringIndex] :" + stringsplit[stringIndex]);
                        if (i < (stringsplit.Length - 1))
                        {
                            arrayIndex++;
                            stringIndex++;
                        }

                        counterIteration1++;

                        if (arrayIndex > (stringsplit.Length - 1))
                        {
                            stringsplit2 = stringsplit.Substring(((counterIteration2 - 1) -1 ), ((lastofString - (counterIteration2 - 1) - 1)));
                            arrayIndex = ((stringsplit2.Length - 1) - 1);
                            for (int j1 = 0; j1 < ((stringsplit2.Length )); j1++)
                            {
                                arrayIndex--;
                                arrayIndex = (arrayIndex == -1) ? 1 : arrayIndex;
                                array[arrayIndex] += stringsplit2[j1];
                                Console.WriteLine("1) positive else" + ", oldIndex : " + oldIndex + ", arrayIndex : " + arrayIndex + ", stringIndex : " + stringIndex + " j1 : " + j1 + ", array[arrayIndex] : " + array[arrayIndex] + ", stringsplit2[j1] :" + stringsplit2[j1]);
                            }

                        }

                    }
                    stringIndex = 0;
                }
                else if ((times % 2) == 1)
                {
                    counterIteration2 = 0;
                    for (int j = (stringsplit.Length - 1); j >= 0; j--)
                    {
                        //arrayIndex = (arrayIndex == -1) ? 1 : arrayIndex;
                        if (arrayIndex <= 0)
                        {
                            Console.WriteLine("ciao");
                            stringsplit2 = stringsplit.Substring(((counterIteration2 )), ((lastofString - (counterIteration2 - 1) - 1)));
                            Console.WriteLine("stringsplit2 : " + stringsplit2);
                            arrayIndex = 0 + 1;
                            for (int i1 = 0; i1 < ((stringsplit2.Length)); i1++)
                            {
                                Console.WriteLine(" stringsplit2[i1] : " + stringsplit2[i1]);
                                //arrayIndex = (arrayIndex == -1) ? 1 : arrayIndex;
                                array[arrayIndex] += stringsplit2[i1];
                                //arrayIndex++;
                                Console.WriteLine("2) negative if" + ", oldIndex : " + oldIndex + ", arrayIndex : " + arrayIndex + ", stringIndex : " + stringIndex + " i1 : " + i1 + ", array[arrayIndex] : " + array[i1] + ", stringsplit[i1] :" + stringsplit2[i1]);
                            }

                        }
                        else
                        {
                            arrayIndex--;
                            array[arrayIndex] += stringsplit[stringIndex];
                            Console.WriteLine("2) negative else" + ", oldIndex : " + oldIndex + ", arrayIndex : " + arrayIndex + ", stringIndex : " + stringIndex + " j : " + j + ", array[arrayIndex] : " + array[arrayIndex] + ", stringsplit[stringIndex] :" + stringsplit[stringIndex]);
                            stringIndex++;
                        }



                        counterIteration2++;
                    }
                    stringIndex = 0;

                }
                //Console.WriteLine("cycle " + ", oldIndex : " + oldIndex + ", arrayIndex : " + arrayIndex + ", stringIndex : " + stringIndex + " i : " + i + ", array[arrayIndex] : " + array[arrayIndex] + ", stringsplit[stringIndex] :" + stringsplit[stringIndex]);

                addLetterOld += numRows;

                //Console.WriteLine("s.Length " + s.Length + ", addLetterOld : " + addLetterOld);
                if ((s.Length - addLetterOld) < 3)//calcola quanti elementi rimangono della stringa 
                {
                    lastofString = s.Length - addLetterOld;
                    //Console.WriteLine("2 s.Length " + s.Length + ", addLetterOld : " + addLetterOld + ", lastofString :" + lastofString);
                }
                times++;
            }
            foreach (var item in array)
            {
                finalstring += item;
                Console.WriteLine("counveterDebug : " + counveterDebug + ", item : " + item);
                counveterDebug++;
            }

            return finalstring;

        }
    }
}

  
    