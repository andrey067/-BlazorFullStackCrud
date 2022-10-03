namespace BlazorFullStackCrud.Client.Services.SuperHeroServices
{
    public interface ISuperHeroServices
    {
        List<SuperHero> Heroes { get; set; }
        List<Comic> Comics { get; set; }
        Task GetComics();
        Task GetSuperHeroes();
        Task<SuperHero> GetSingleHero(int id);
        Task CreateHero(SuperHero superHero);
        Task UpdateSuperHero(SuperHero superHero);
        Task DeleteSuperHero(int id);
    }
}
