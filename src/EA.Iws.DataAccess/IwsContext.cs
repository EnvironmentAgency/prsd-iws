﻿namespace EA.Iws.DataAccess
{
    using System.Data.Entity;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.DataAccess.Extensions;
    using Core.Domain;
    using Core.Domain.Auditing;
    
    public class IwsContext : DbContext
    {
        private readonly IUserContext userContext;

        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        public IwsContext(IUserContext userContext)
            : base("name=Iws.DefaultConnection")
        {
            this.userContext = userContext;
            Database.SetInitializer<IwsContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var assembly = typeof(IwsContext).Assembly;
            var coreAssembly = typeof(AuditorExtensions).Assembly;

            modelBuilder.Conventions.AddFromAssembly(assembly);
            modelBuilder.Configurations.AddFromAssembly(assembly);

            modelBuilder.Conventions.AddFromAssembly(coreAssembly);
            modelBuilder.Configurations.AddFromAssembly(coreAssembly);
        }

        public override int SaveChanges()
        {
            this.SetEntityId();
            this.AuditChanges(userContext.UserId);

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.SetEntityId();
            this.AuditChanges(userContext.UserId);

            return base.SaveChangesAsync(cancellationToken);
        }

        public void DeleteOnCommit(Entity entity)
        {
            Entry(entity).State = EntityState.Deleted;
        }
    }
}