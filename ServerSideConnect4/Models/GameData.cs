using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerSideConnect4.Models
{
    public class GameData
    {
        [Key]
        public int ID { get; set; } = default!;
        public int PlayerID { get; set; } = default!;
        public string PlayerName { get; set; } = default!;
        public DateTime BeginningGameDate { get; set; } = default!;
        public TimeSpan TimeDurationGame { get; set; } = default!;
        public bool isPlayerWin { get; set; } = default!;

    }
}
