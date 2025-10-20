using Challenge.Devsu.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Challenge.Devsu.Infrastructure.Persistence.Contexts;

public partial class DbDataContext : DbContext
{
    public DbDataContext(DbContextOptions<DbDataContext> options)
       : base(options)
    {

    }
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Move> Movements => Set<Move>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Client>(e =>
        {
            e.HasKey(e => e.ClientId).HasName("tbl_client_pkey");

            e.ToTable("tbl_client");
            e.HasKey(x => x.ClientId);
            e.HasIndex(x => x.IdentificationNumber).IsUnique();
            e.Property(x => x.IdentificationNumber).HasMaxLength(20).IsRequired();
            e.Property(x => x.Gender).HasMaxLength(2).IsRequired();
            e.Property(x => x.PhoneNumber).HasMaxLength(15).IsRequired();
            e.Property(x => x.Password).HasMaxLength(20).IsRequired();
            e.Property(x => x.Active).IsRequired();
            e.Property(x => x.FullName).HasMaxLength(120).IsRequired();
        });

        b.Entity<Account>(e =>
        {
            e.HasKey(e => e.AccountId).HasName("tbl_account_pkey");

            e.ToTable("tbl_account");

            e.HasIndex(x => x.AccountNumber).IsUnique();
            e.Property(x => x.AccountType).IsRequired();
            e.HasOne(x => x.Client).WithMany(c => c.Accounts)
                .HasForeignKey(x => x.ClientRefId);
        });

        b.Entity<Move>(e =>
        {
            e.HasKey(e => e.MoveId).HasName("tbl_move_pkey");

            e.ToTable("tbl_move");

            e.HasOne(m => m.Account).WithMany(c => c.Movements)
                .HasForeignKey(m => m.AccountRefId);
        });

        b.Entity<Log>(e =>
        {
            e.HasKey(e => e.LogId).HasName("tbl_log_pkey");

            e.ToTable("tbl_log");
        });
    }
}
