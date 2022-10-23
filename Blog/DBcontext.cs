using Blog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Blog
{
    public class DBcontext : IdentityDbContext<UserModel>
    {
        public DBcontext(DbContextOptions<DBcontext> options) : base(options) { }

       
        public DbSet<Message> Messages {get; set;}

        public DbSet<Room> Rooms { get; set;}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

              builder.Entity<Message>()
                .HasOne<UserModel>(a => a.Sender)
                .WithMany(d => d.Messages)
                .HasForeignKey(d => d.UserID);

            builder.Entity<Message>()
               .HasOne<Room>(a => a.RoomSender)
               .WithMany(d => d.Messages)
               .HasForeignKey(d => d.RoomId);
        }
        
    }
}
