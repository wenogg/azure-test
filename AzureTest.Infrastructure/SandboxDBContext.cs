using AzureTest.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace AzureTest.Infrastructure {


	public class SandboxDBContext  : IdentityDbContext<ApplicationUser, ApplicationRole, string> {

        public SandboxDBContext(DbContextOptions<SandboxDBContext> options) : base(ChangeOptionsType<SandboxDBContext>(options)) {
            
        } 


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);


            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<ApplicationUser>(entity => {
                
                //entity.ToTable("Users");

                entity.Property(e => e.FirstName)
                    .HasColumnName("FirstName")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasColumnName("LastName")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

        }

        protected static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext {
            var sqlExt = options.Extensions.FirstOrDefault(e => e is SqlServerOptionsExtension);
            var inMemoryExt = (InMemoryOptionsExtension)options.Extensions.FirstOrDefault(e => e is InMemoryOptionsExtension);

            if (inMemoryExt != null) {
                return new DbContextOptionsBuilder<T>()
                        .UseInMemoryDatabase(inMemoryExt.StoreName)
                        .Options;

            } else if (sqlExt != null) {
                return new DbContextOptionsBuilder<T>()
                        .UseSqlServer(((SqlServerOptionsExtension)sqlExt).ConnectionString)
                        .Options;
            } else {
                throw new Exception("Not configured");
            }
        }
    }
}
