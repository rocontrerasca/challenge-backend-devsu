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
            e.Navigation(c => c.Accounts).AutoInclude();
            e.ToTable("tbl_client");
            e.HasKey(x => x.ClientId);
            e.HasIndex(x => x.IdentificationNumber).IsUnique();
            e.Property(x => x.IdentificationNumber).HasColumnName("identification_number").HasMaxLength(20).IsRequired();
            e.Property(x => x.Gender).HasMaxLength(2).IsRequired();
            e.Property(x => x.PhoneNumber).HasColumnName("phone_number").HasMaxLength(15).IsRequired();
            e.Property(x => x.Password).HasMaxLength(20).IsRequired();
            e.Property(x => x.Active).IsRequired();
            e.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(120).IsRequired();
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.Property(x => x.ClientId).HasColumnName("client_id");
            e.HasMany(c => c.Accounts)
             .WithOne(a => a.Client)
             .HasForeignKey(a => a.ClientRefId);
        });

        b.Entity<Account>(e =>
        {
            e.HasKey(e => e.AccountId).HasName("tbl_account_pkey");

            e.ToTable("tbl_account");
            e.Navigation(c => c.Client).AutoInclude();
            e.Navigation(c => c.Movements).AutoInclude();
            e.HasIndex(x => x.AccountNumber).IsUnique();
            e.Property(x => x.AccountType).HasColumnName("account_type").IsRequired();
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.Property(x => x.AccountId).HasColumnName("account_id");
            e.Property(x => x.AccountNumber).HasColumnName("account_number");
            e.Property(x => x.ClientRefId).HasColumnName("client_ref_id");
            e.Property(x => x.InitialBalance).HasColumnName("initial_balance");
            e.HasOne(x => x.Client).WithMany(c => c.Accounts)
                .HasForeignKey(x => x.ClientRefId);
        });

        b.Entity<Move>(e =>
        {
            e.HasKey(e => e.MoveId).HasName("tbl_move_pkey");
            e.Navigation(c => c.Account).AutoInclude();
            e.ToTable("tbl_move");
            e.Property(x => x.MoveId).HasColumnName("move_id");
            e.Property(x => x.TransactionDate).HasColumnName("transaction_date");
            e.Property(x => x.MoveType).HasColumnName("move_type");
            e.Property(x => x.AccountRefId).HasColumnName("account_ref_id");
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
