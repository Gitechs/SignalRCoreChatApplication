using System;
using System.Collections.Generic;
using ChatApplication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Infrastructures.Stores {
    public class ChatDbContext : IdentityDbContext<User, Role, string> {
        public ChatDbContext (DbContextOptions options) : base (options) { }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating (ModelBuilder builder) {
            var hasher = new PasswordHasher<User> ();
            var users = new List<User> {
                new User {
                UserName = "M.Sheykhveysi4680@Gmail.com",
                Email = "M.Sheykhveysi4680@Gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = "09307514680",
                Id = Guid.NewGuid ().ToString (),
                NormalizedEmail = "M.Sheykhveysi4680@Gmail.com".ToUpper (),
                NormalizedUserName = "M.Sheykhveysi4680@Gmail.com".ToUpper (),
                PasswordHash = hasher.HashPassword (null, "123456789"),
                SecurityStamp = Guid.NewGuid ().ToString ()
                },
                new User {
                UserName = "a.alavi1234@Gmail.com",
                Email = "a.alavi1234@Gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = "09368560182",
                Id = Guid.NewGuid ().ToString (),
                NormalizedEmail = "a.alavi1234@Gmail.com".ToUpper (),
                NormalizedUserName = "a.alavi1234@Gmail.com".ToUpper (),
                PasswordHash = hasher.HashPassword (null, "123456789"),
                SecurityStamp = Guid.NewGuid ().ToString ()
                },
                new User {
                UserName = "t.taheri1234@gmail.com",
                Email = "t.taheri1234@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PhoneNumber = "09309303030",
                Id = Guid.NewGuid ().ToString (),
                NormalizedEmail = "t.taheri1234@gmail.com".ToUpper (),
                NormalizedUserName = "t.taheri1234@gmail.com".ToUpper (),
                PasswordHash = hasher.HashPassword (null, "123456789"),
                SecurityStamp = Guid.NewGuid ().ToString ()
                }
            };
            builder.Entity<User> (buildAction => {
                buildAction.HasData (users);
            });

            base.OnModelCreating (builder);
        }
    }
}