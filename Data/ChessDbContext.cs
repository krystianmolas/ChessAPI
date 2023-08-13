using Microsoft.EntityFrameworkCore;
using ChessAPI.Models;

namespace ChessAPI.Data
{
    public class ChessDbContext : DbContext
    {
        public ChessDbContext(DbContextOptions<ChessDbContext> options)
            : base(options)
        { }

        public DbSet<Move> Moves { get; set; }
    }
}
