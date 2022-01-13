using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace ClevarWeb.Data.Models.Internals
{
    public abstract class DefaultDbContext : DbContext
    {
        public DbConnection Connection
        {
            get => this.Database.GetDbConnection();
        }

        public DefaultDbContext(DbContextOptions options) : base(options)
        {
            this.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));
            //this.Database.EnsureCreated();
            // Automatic migration scripts go here
        }

        public override int SaveChanges()
        {
            this.SetCreatedModifiedDates();

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.SetCreatedModifiedDates();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            this.SetCreatedModifiedDates();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.SetCreatedModifiedDates();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetCreatedModifiedDates()
        {
            DateTime now = DateTime.Now;

            foreach (EntityEntry changeEntry in this.ChangeTracker.Entries())
            {
                if (changeEntry.State == EntityState.Deleted || changeEntry.State == EntityState.Detached || changeEntry.State == EntityState.Unchanged)
                    continue;

                if (changeEntry.Entity is Project projectTable)
                {
                    if (changeEntry.State == EntityState.Added)
                        projectTable.CreatedDateTime = now;
                    projectTable.ModifiedDateTime = now;
                }

                if (changeEntry.Entity is Person personTable)
                {
                    if (changeEntry.State == EntityState.Added)
                        personTable.CreatedDateTime = now;
                    personTable.ModifiedDateTime = now;
                }

                if (changeEntry.Entity is Document documentTable)
                {
                    if (changeEntry.State == EntityState.Added)
                        documentTable.CreatedDateTime = now;
                    documentTable.ModifiedDateTime = now;
                }


            }
        }
    }
}
