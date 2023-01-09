using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Samples.Domain;

namespace Samples.Database
{
    public class MoviesContext : DbContext
    {
        private readonly DbProviderType _providerType;

        public MoviesContext() => _providerType = DbProviderType.InMemory;

        public MoviesContext(DbProviderType providerType) => _providerType = providerType;

        public MoviesContext(DbContextOptions<MoviesContext> options) : base(options) { }

        public DbSet<Actor> Actors { get; set; }

        public DbSet<Director> Directors { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Trivia> Trivia { get; set; }

        public DbSet<Writer> Writers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (_providerType == DbProviderType.InMemory)
                    optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

                if (_providerType == DbProviderType.LocalDb)
                    optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = PublisherDatabase");

                // Output the logs to the console
                optionsBuilder.LogTo(Console.WriteLine, new[]
                {
                    DbLoggerCategory.Database.Command.Name
                }, LogLevel.Information);

                // Include query parameters in the logs
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var actors = new List<Actor>
            {
                new Actor{ Id = 1, FirstName = "Daniel", LastName = "Radcliffe" },
                new Actor{ Id = 2, FirstName = "Rupert", LastName = "Grint" },
                new Actor{ Id = 3, FirstName = "Emma", LastName = "Watson" },
            };

            var directors = new List<Director>
            {
                new Director{ Id = 1, FirstName = "Chris", LastName = "Columbus" },
                new Director{ Id = 2, FirstName = "Alfonso", LastName = "Cuarón" },
                new Director{ Id = 3, FirstName = "Mike", LastName = "Newell" },
                new Director{ Id = 4, FirstName = "David", LastName = "Yates" },
            };

            var writers = new List<Writer>
            {
                new Writer{ Id = 1, FirstName = "J.K.", LastName = "Rowling" }
            };

            var movies = new List<Movie>
            {
                new Movie{ Id = 1, ReleaseYear = 2001, DirectorId = 1, WriterId = 1, Title = "Harry Potter and the Sorcerer's Stone" },
                new Movie{ Id = 2, ReleaseYear = 2002, DirectorId = 1, WriterId = 1, Title = "Harry Potter and the Chamber of Secrets" },
                new Movie{ Id = 3, ReleaseYear = 2004, DirectorId = 2, WriterId = 1, Title = "Harry Potter and the Prisoner of Azkaban" },
                new Movie{ Id = 4, ReleaseYear = 2005, DirectorId = 3, WriterId = 1, Title = "Harry Potter and the Goblet of Fire" },
                new Movie{ Id = 5, ReleaseYear = 2007, DirectorId = 4, WriterId = 1, Title = "Harry Potter and the Order of the Phoenix" },
                new Movie{ Id = 6, ReleaseYear = 2009, DirectorId = 4, WriterId = 1, Title = "Harry Potter and the Half-Blood Prince" },
                new Movie{ Id = 7, ReleaseYear = 2010, DirectorId = 4, WriterId = 1, Title = "Harry Potter and the Deathly Hallows: Part 1" },
                new Movie{ Id = 8, ReleaseYear = 2011, DirectorId = 4, WriterId = 1, Title = "Harry Potter and the Deathly Hallows: Part 2" },
            };

            var trivia = new List<Trivia>
            {
                new Trivia{ Id = 1, MovieId = 1, Description = "Alan Rickman was hand picked to play Snape by J.K. Rowling, and received special instructions from her about character. Rowling even provided him with vital details of Snape's backstory, not revealed until the final novel." },
                new Trivia{ Id = 2, MovieId = 2, Description = "Tom Felton forgot his line when Draco sees Harry disguised as Goyle, so he improvised \"I didn't know you could read\"." },
                new Trivia{ Id = 3, MovieId = 3, Description = "In order to acquaint himself with his two lead actors and actress, director Alfonso Cuarón had each of them write an essay about their characters, from a first-person point of view. Emma Watson, in true Hermione fashion, went a little overboard and wrote a sixteen-page essay. Daniel Radcliffe, like Harry, wrote a simple one-page summary, and Rupert Grint, like Ron, never even turned his in." },
                new Trivia{ Id = 4, MovieId = 4, Description = "Director Mike Newell was not aware that Alan Rickman wore black contact lenses for the role of Snape until one day when he was complimenting him on the amazing shade of his eyes. Rickman leaned over and popped one of the lenses out." },
                new Trivia{ Id = 5, MovieId = 5, Description = "Dumbledore's line \"Don't fight him, Harry, you can't win.\", was featured prominently in just about every trailer and television spot, yet it is nowhere in the final version of this movie, nor in the DVD's extended scenes." },
                new Trivia{ Id = 6, MovieId = 6, Description = "Dame Maggie Smith completed filming this movie while undergoing radio-therapy as treatment for breast cancer." },
                new Trivia{ Id = 7, MovieId = 7, Description = "Jason Isaacs (Lucius Malfoy) originally considered not returning for this movie, fearing that his character's arrest and imprisonment at the end of Harry Potter and the Order of the Phoenix (2007) would mean very little, if any, screentime in the finale. Upon meeting J.K. Rowling, he begged to be let out of prison. She told him \"You're out. Chapter one.\" This immediately convinced him to sign on for the movie." },
                new Trivia{ Id = 8, MovieId = 8, Description = "According to Tom Felton, Voldemort awkwardly hugging Draco was not scripted, but an improvisation by Ralph Fiennes. Felton's reactions of stopping dead in his tracks, not knowing what to do, is genuine." },
                new Trivia{ Id = 9, MovieId = 1, Description = "The child actors and actresses would do their actual schoolwork in the movie, to make the school setting more real." }
            };

            modelBuilder.Entity<Actor>().HasData(actors);
            modelBuilder.Entity<Director>().HasData(directors);
            modelBuilder.Entity<Writer>().HasData(writers);
            modelBuilder.Entity<Movie>().HasData(movies);
            modelBuilder.Entity<Trivia>().HasData(trivia);
        }
    }

    public enum DbProviderType
    {
        InMemory,

        LocalDb
    }
}
