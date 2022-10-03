using BlazorFullStackCrud.Client.Pages;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace BlazorFullStackCrud.Client.Services.SuperHeroServices
{
    public class SuperHeroServices : ISuperHeroServices
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;

        public SuperHeroServices(HttpClient httpClient, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }

        public List<SuperHero> Heroes { get; set; } = new();
        public List<Comic> Comics { get; set; } = new();

        public async Task CreateHero(SuperHero superHero)
        {
            var result = await _httpClient.PostAsJsonAsync("api/superhero", superHero);
            var response = await result.Content.ReadFromJsonAsync<List<SuperHero>>();
            Heroes = response;
        }

        public async Task DeleteSuperHero(int id)
        {
            var result = await _httpClient.DeleteAsync($"api/superhero/{id}");
            await SetHeroes(result);
        }

        public async Task GetComics()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Comic>>("api/superhero");
            if (result != null)
                Comics = result;
        }

        public async Task<SuperHero> GetSingleHero(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<SuperHero>($"api/superhero/{id}");
            if (result != null)
                return result;
            throw new Exception("Hero not found !");
        }

        public async Task GetSuperHeroes()
        {
            var result = await _httpClient.GetFromJsonAsync<List<SuperHero>>("api/superhero");
            if (result != null)
                Heroes = result;
        }
        public async Task UpdateSuperHero(SuperHero superHero)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/superhero/{superHero.Id}", superHero);
            await SetHeroes(result);
        }

        private async Task SetHeroes(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<SuperHero>>();
            Heroes = response;
            _navigationManager.NavigateTo("superheroes");
        }
    }
}
