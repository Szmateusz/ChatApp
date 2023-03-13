using Blog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Blog
{
    public class DBcontext : IdentityDbContext<UserModel>
    {
        public DBcontext(DbContextOptions<DBcontext> options) : base(options) { }

       
        public DbSet<GroupMessage> GroupMessages {get; set;}

        public DbSet<PrivateMessage> PrivateMessages { get; set; }

        public DbSet<ConnectingToGroups> ConnectingToRooms { get; set; }


        public DbSet<Room> Rooms { get; set;}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

              builder.Entity<GroupMessage>()
                .HasOne<UserModel>(a => a.Sender)
                .WithMany(d => d.Messages)
                .HasForeignKey(d => d.UserID);

            builder.Entity<GroupMessage>()
               .HasOne<Room>(a => a.RoomSender)
               .WithMany(d => d.Messages)
               .HasForeignKey(d => d.RoomId);

           


            builder.Entity<ConnectingToGroups>()
              .HasOne<Room>(a => a.Roomsender);

            builder.Entity<ConnectingToGroups>()
             .HasOne<UserModel>(a => a.UserSender);


        }
        
    }
}
