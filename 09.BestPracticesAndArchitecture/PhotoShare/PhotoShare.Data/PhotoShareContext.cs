namespace PhotoShare.Data
{
    using EntityConfiguration;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class PhotoShareContext : DbContext
    {
        public PhotoShareContext()
        {
        }

        public PhotoShareContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<AlbumRole> AlbumRoles { get; set; }

        public DbSet<Town> Towns { get; set; }
		
		public DbSet<AlbumTag> AlbumTags { get; set; }

        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer(Configuration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AlbumConfiguration());

            modelBuilder.ApplyConfiguration(new AlbumRoleConfiguration());

            modelBuilder.ApplyConfiguration(new AlbumTagConfiguration());

            modelBuilder.ApplyConfiguration(new FriendshipConfiguration());

            modelBuilder.ApplyConfiguration(new PictureConfiguration());

            modelBuilder.ApplyConfiguration(new TagConfiguration());

            modelBuilder.ApplyConfiguration(new TownConfiguration());

            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}