using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using ServerSideConnect4.Data;
using ServerSideConnect4.Models;

namespace ServerSideConnect4.Api
{
    public class PlayersController : Controller
    {
        private readonly QContext _context;
        private Random random = new Random();


        public PlayersController(QContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
              return _context.Player != null ? 
                          View(await _context.Player.ToListAsync()) :
                          Problem("Entity set 'QContext.Player'  is null.");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Player == null)
            {
                return NotFound();
            }

            var player = await _context.Player
                .FirstOrDefaultAsync(m => m.ID == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        [HttpGet("DetailsPlayers")]
        public async Task<ActionResult<IEnumerable<Player>>> DetailsPlayers()
        {
            if (_context.Player == null)
            {
                return NotFound();
            }
            return await _context.Player.ToListAsync();
        }

        [HttpGet("DetailsPlayers/{id}")]
        public async Task<ActionResult<Player>> DetailsPlayers(int id)
        {
            var player = await _context.Player.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            return player;
        }


        // GET: Players/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult list()
        {
            return View();
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Player == null)
            {
                return NotFound();
            }

            var player = await _context.Player.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,Phone,Country")] Player player)
        {
            if (id != player.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.ID))
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
            return View(player);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Player == null)
            {
                return NotFound();
            }

            var player = await _context.Player
                .FirstOrDefaultAsync(m => m.ID == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Player == null)
            {
                return Problem("Entity set 'QContext.Player'  is null.");
            }
            var player = await _context.Player.FindAsync(id);
            if (player != null)
            {
                _context.Player.Remove(player);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
          return (_context.Player?.Any(e => e.ID == id)).GetValueOrDefault();
        }
        public class ID
        {
            public int Value { get; set; }
        }
        [HttpPost("StartGame/")]
        public async Task<ActionResult> StartGame([FromBody] ID myID)
        {
            
            if (!PlayerExists(myID.Value))
                return BadRequest("Player ID is req");

            GameData gameData = new GameData
            {
                PlayerID = (myID.Value),
                PlayerName = _context.Player.FindAsync(myID.Value).Result.FirstName
            };
            DateTime currentUtcDateTime = DateTime.UtcNow;
            // Define the Israel Standard Time zone
            TimeZoneInfo israelTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
            // Convert the UTC time to Israel's local time
            DateTime israelDateTime = TimeZoneInfo.ConvertTimeFromUtc(currentUtcDateTime, israelTimeZone);
            gameData.BeginningGameDate = israelDateTime;
            _context.GameDatas.Add(gameData);
            await _context.SaveChangesAsync();
            return Ok(gameData.ID);

        }
        [HttpGet("step/")]
        public async Task<ActionResult> step()
        {
            int column = random.Next(0, 7);
            return Ok(column);
        }

        public class EndData
        {
            public int GameID { get; set; }
            public bool IsPlayerWin { get; set; }
        }
        [HttpPost("EndGame/")]
        public async Task<ActionResult> EndGame([FromBody] EndData end_data)
        {            
            DateTime currentUtcDateTime = DateTime.UtcNow;
            // Define the Israel Standard Time zone
            TimeZoneInfo israelTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
            // Convert the UTC time to Israel's local time
            DateTime israelDateTime = TimeZoneInfo.ConvertTimeFromUtc(currentUtcDateTime, israelTimeZone);
            GameData gameData = _context.GameDatas.Find(end_data.GameID);
            gameData.TimeDurationGame = israelDateTime - gameData.BeginningGameDate;
            gameData.isPlayerWin = end_data.IsPlayerWin;
            await _context.SaveChangesAsync();
            return Ok("Game end successfuly");

        }
    }
}
