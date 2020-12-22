using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DAL.Context
{
    public class MyContext : IdentityDbContext<AppUser, AppRole,int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=.;database=Mazaka;Integrated Security=True;");
            base.OnConfiguring(optionsBuilder);
        }
       
        public DbSet<TaskDetail> TaskDetails { get; set; }
    }
}
