using BlazorFullStackCrud.Client.Pages;
using BlazorFullStackCrud.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorFullStackCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;

        public SuperHeroController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetSuperHeroes()
        {
            var heroes = await _context.SuperHeroes.Include(sh => sh.Comic).ToListAsync();
            return heroes;
        }

        [HttpGet("comics")]
        public async Task<ActionResult<List<Comic>>> GetComics()
        {
            var comics = await _context.Comics.ToListAsync();
            return Ok(comics);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<SuperHero>> GetSigleHeroes(int id)
        {
            var hero = await _context.SuperHeroes
                .Include(h => h.Comic)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (hero == null)
            {
                return NotFound("Sorry, no hero here. :/");
            }
            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> CreateSupeHero(SuperHero superHero)
        {
            superHero.Comic = null;
            _context.SuperHeroes.Add(superHero);
            await _context.SaveChangesAsync();

            return Ok(await GetDbHeroes());
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(SuperHero superHero, int id)
        {
            var dbHero = await _context.SuperHeroes
                .Include(sh => sh.Comic)
                .FirstOrDefaultAsync(sh => sh.Id == id);
            if (dbHero == null)
                return NotFound("Sorry, but no hero for you. :/");

            dbHero.FirstName = superHero.FirstName;
            dbHero.LastName = superHero.LastName;
            dbHero.HeroName = superHero.HeroName;
            dbHero.ComicId = superHero.ComicId;

            await _context.SaveChangesAsync();

            return Ok(await GetDbHeroes());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int id)
        {
            var dbHero = await _context.SuperHeroes
                .Include(sh => sh.Comic)
                .FirstOrDefaultAsync(sh => sh.Id == id);
            if (dbHero == null)
                return NotFound("Sorry, but no hero for you. :/");

            _context.SuperHeroes.Remove(dbHero);
            await _context.SaveChangesAsync();

            return Ok(await GetDbHeroes());
        }

        private async Task<List<SuperHero>> GetDbHeroes()
        {
            return await _context.SuperHeroes.Include(sh => sh.Comic).ToListAsync();
        }
    }
}
