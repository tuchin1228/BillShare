using BillShare.Models;
using Microsoft.EntityFrameworkCore;

namespace BillShare.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Group_User> Group_Users { get; set; }
        public DbSet<Expend> Expends { get; set; }
        public DbSet<ExpendDetail> ExpendDetails { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }


    }
}
