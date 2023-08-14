using Microsoft.EntityFrameworkCore;

namespace ServerSideConnect4.Data
{
    public class QContext : DbContext
    {
        public DbSet<Models.Player> Player { get; set; } = default!;
        public DbSet<Models.GameData> GameDatas { get; set; } = default!;

        public QContext (DbContextOptions<QContext> options) : base(options) { }

    }
}
