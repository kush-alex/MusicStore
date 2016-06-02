using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MusicStore.Web.DataContexts
{
    public class Db:DbContext
    {   
        public DbSet<Artist> Artists { get; set; }

        public DbSet<Producer> Producers { get; set; }
        
        public DbSet<Album> Albums { get; set; }
        
        public DbSet<Song> Songs { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Occupation> Occupations { get; set; }
        

        public Db() : base("DefaultConnection") { }

    }
}
