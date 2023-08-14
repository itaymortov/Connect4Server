using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ServerSideConnect4.Models;
using System.Collections;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace ServerSideConnect4.Pages
{
    public class QueriesModel : PageModel
    {
        private readonly ServerSideConnect4.Data.QContext _context;

        public QueriesModel(ServerSideConnect4.Data.QContext context)
        {
            _context = context;
            this.countries = new List<SelectListItem>
            {
                new SelectListItem { Value = "Israel", Text = "Israel" },
                new SelectListItem { Value = "Mexico", Text = "Mexico" },
                new SelectListItem { Value = "Canada", Text = "Canada" },
                new SelectListItem { Value = "United Kingdom", Text = "United Kingdom" },
                new SelectListItem { Value = "Germany", Text = "Germany" },
                new SelectListItem { Value = "France", Text = "France" },
                new SelectListItem { Value = "Italy", Text = "Italy" },
                new SelectListItem { Value = "Spain", Text = "Spain" },
                new SelectListItem { Value = "Japan", Text = "Japan" },
                new SelectListItem { Value = "China", Text = "China" },
                new SelectListItem { Value = "Australia", Text = "Australia" },
                new SelectListItem { Value = "South Korea", Text = "South Korea" },
                new SelectListItem { Value = "Brazil", Text = "Brazil" },
                new SelectListItem { Value = "Argentina", Text = "Argentina" },
                new SelectListItem { Value = "India", Text = "India" },
                new SelectListItem { Value = "Russia", Text = "Russia" },
                new SelectListItem { Value = "South Africa", Text = "South Africa" },
                new SelectListItem { Value = "Nigeria", Text = "Nigeria" },
                new SelectListItem { Value = "Egypt", Text = "Egypt" },
                new SelectListItem { Value = "USA", Text = "USA" }

            };
        }

        public IList<Player> Player { get; set; } = default!;
        public IList<GameData> LastGamesDate { get; set; } = default!;
        public IList<GameData> AllGames { get; set; } = default!;
        public List<SelectListItem> players { get; set; }
        [BindProperty]
        public int selectPlayers { get; set; }
        [BindProperty]
        public string selectCountry { get; set; }
        public IList<GameData> AllSelectedPlayerGames { get; set; } = default!;
        public IList<Player> AllSelectedPlayerCountry { get; set; } = default!;
        public List<SelectListItem> countries { get; set; }
        public List<PlayerGamesCountViewModel> PlayerGamesCount { get; set; }
        public List<PlayersByAmountOfGames> playersByAmountOfGames { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Player != null)
            {
                await GetPlayers();
            }
            if (_context.GameDatas != null)
            {
                await OnGetLastGameDateAsync();
                AllGames = await _context.GameDatas.ToListAsync(); //Q19
                await OnGetPlayersCountAsync();
                await OnGetPlayersByAmountOfGamesAsync();
            }
        }

        public async Task OnGetWithoutPlayerAsync()
        {
            if (_context.GameDatas != null)
            {
                await OnGetLastGameDateAsync();
                AllGames = await _context.GameDatas.ToListAsync();
                await GetPlayers();
                await OnGetPlayersCountAsync();
                await OnGetPlayersByAmountOfGamesAsync();

            }
        }

        //Q17
        public async Task OnGetSortAsync(string sortOrder)
        {
            IQueryable<Player> playersQuery = _context.Player.AsQueryable();

            // Handle sorting
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "id_asc":
                        playersQuery = playersQuery.OrderBy(p => p.ID);
                        break;
                    case "id_desc":
                        playersQuery = playersQuery.OrderByDescending(p => p.ID);
                        break;
                    // Add more cases for other columns, if needed
                    default:
                        playersQuery = playersQuery.OrderBy(p => p.ID);
                        break;
                }
                await OnGetWithoutPlayerAsync();
            }
            else
            {
                // Default sorting by ID in ascending order
                playersQuery = playersQuery.OrderBy(p => p.ID);
            }

            Player = await playersQuery.ToListAsync();
        }
        //Q17
        public async Task<IActionResult> OnPostSortOrderAsync(string sortOrder)
        {
            await OnGetSortAsync(sortOrder);
            return Page();
        }

        //Q18
        public async Task OnGetLastGameDateAsync()
        {
            var lastGamesData = await _context.GameDatas
                .ToListAsync();

            LastGamesDate = lastGamesData
                .GroupBy(g => g.PlayerName) // Group by PlayerName
                .Select(g => new GameData
                {
                    PlayerName = g.Key,
                    BeginningGameDate = g.Max(x => x.BeginningGameDate)
                })
                .OrderBy(g => g.PlayerName, new LowercaseThenUppercaseComparer())
                .ToList();
        }

        //Q21
        public async Task<IActionResult> OnPostSelectedPlayerAsync()
        {
            await OnGetSelectedPlayerAsync();
            return Page();
        }
        //Q21
        public async Task OnGetSelectedPlayerAsync()
        {
            var filteredGames = _context.GameDatas.Where(game =>
                game.PlayerID == selectPlayers)
                .ToList();
            AllSelectedPlayerGames = filteredGames;
            await OnGetAsync();
        }
        //Q22
        public async Task OnGetPlayersCountAsync()
        {
            var playerGamesCount = _context.GameDatas
                .GroupBy(game => game.PlayerName)
                .Select(group => new PlayerGamesCountViewModel { PlayerName = group.Key, GamesPlayed = group.Count() })
                .ToList();
            PlayerGamesCount = playerGamesCount;
        }
        public List<GameData> GetGamesPlayedByPlayerID(string playerName)
        {
            var filteredGames = _context.GameDatas.Where(game =>
                game.PlayerID.Equals(playerName))
                .ToList();
            return filteredGames;
        }

        //Q24
        public async Task<IActionResult> OnPostSelectedCountryAsync()
        {
            await OnGetSelectedCountryAsync();
            return Page();
        }

        //Q24
        public async Task OnGetSelectedCountryAsync()
        {
            var filteredPlayersPerCountry = _context.Player.Where(player =>
                player.Country.Equals(selectCountry))
                .ToList();
            AllSelectedPlayerCountry = filteredPlayersPerCountry;
            await OnGetAsync();
        }

        private async Task GetPlayers()
        {
            Player = await _context.Player.ToListAsync();
            players = new List<SelectListItem> { };
            for (int i = 0; i < Player.Count; i++)
            {
                this.players.Add(new SelectListItem(Player[i].FirstName, Player[i].ID + ""));
            }
        }

        //Q23
        public async Task OnGetPlayersByAmountOfGamesAsync()
        {
            var playersGroups = await _context.GameDatas
            .GroupBy(g => g.PlayerID)
            .Select(g => new PlayersByAmountOfGames
            {
                GamesPlayed = g.Count(),
                Players = _context.Player.Where(p => g.Any(gd => gd.PlayerID == p.ID)).ToList()
            })
            .ToListAsync();
            playersByAmountOfGames = playersGroups;


            var playersWithoutGameData = Player.GroupJoin(AllGames, player => player.ID, gameData => gameData.PlayerID,
                (player, gameDataGroup) => new { Player = player, GameDataGroup = gameDataGroup })
                    .Where(x => !x.GameDataGroup.Any())
                    .Select(x => x.Player).ToList();

            PlayersByAmountOfGames playerDidntPlay = new PlayersByAmountOfGames { GamesPlayed = 0 };
            playerDidntPlay.Players = playersWithoutGameData;

            playersByAmountOfGames.Add(playerDidntPlay);

        }
    }
}

public class LowercaseThenUppercaseComparer : IComparer<string>
{
    public int Compare(string x, string y)
    {
        bool xIsLowercase = char.IsLower(x[0]);
        bool yIsLowercase = char.IsLower(y[0]);

        if (xIsLowercase && !yIsLowercase)
            return -1;
        if (!xIsLowercase && yIsLowercase)
            return 1;

        return string.Compare(x, y, StringComparison.Ordinal);
    }
}

public class PlayerGamesCountViewModel
{
    public string PlayerName { get; set; }
    public int GamesPlayed { get; set; }
}

public class PlayersByAmountOfGames
{
    public int GamesPlayed { get; set; }
    public List<Player> Players { get; set; } = default!;
}