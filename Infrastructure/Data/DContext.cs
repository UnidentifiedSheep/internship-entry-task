using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class DContext : DbContext
{
    public DContext()
    {
    }

    public DContext(DbContextOptions<DContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Move> Moves { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("games_pk");

            entity.ToTable("games");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.BoardSize).HasColumnName("board_size");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CurrentTurn)
                .HasMaxLength(24)
                .HasColumnName("current_turn");
            entity.Property(e => e.Etag)
                .HasMaxLength(64)
                .HasColumnName("etag");
            entity.Property(e => e.FirstPlayer)
                .HasMaxLength(24)
                .HasColumnName("first_player");
            entity.Property(e => e.IsFinished)
                .HasDefaultValue(false)
                .HasColumnName("is_finished");
            entity.Property(e => e.SecondPlayer)
                .HasMaxLength(24)
                .HasColumnName("second_player");
            entity.Property(e => e.WhoWon)
                .HasMaxLength(24)
                .HasColumnName("who_won");
            entity.Property(e => e.WinLength).HasColumnName("win_length");
        });

        modelBuilder.Entity<Move>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("moves_pk");

            entity.ToTable("moves");

            entity.HasIndex(e => new { e.GameId, e.X, e.Y }, "moves_game_id_x_y_uindex").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Etag)
                .HasMaxLength(64)
                .HasColumnName("etag");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.MovedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("moved_at");
            entity.Property(e => e.Player)
                .HasMaxLength(24)
                .HasColumnName("player");
            entity.Property(e => e.X).HasColumnName("x");
            entity.Property(e => e.Y).HasColumnName("y");

            entity.HasOne(d => d.Game).WithMany(p => p.Moves)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("moves_games_id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
