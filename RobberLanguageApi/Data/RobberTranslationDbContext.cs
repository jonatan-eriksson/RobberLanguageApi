using Microsoft.EntityFrameworkCore;
using RobberLanguageApi.Models;

namespace RobberLanguageApi.Data
{
    public class RobberTranslationDbContext : DbContext
    {
        public RobberTranslationDbContext(DbContextOptions<RobberTranslationDbContext> options) : base(options)
        {

        }

        public DbSet<Translation> Translations { get; set; }
    }
}
