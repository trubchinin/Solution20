using DomainTables;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LibraryClient.Services
{
    public class ReaderService
    {
        private readonly HttpClient _http;

        public ReaderService(HttpClient http)
        {
            _http = http;
        }

        public Task<List<Reader>> GetAllAsync() =>
            _http.GetFromJsonAsync<List<Reader>>("api/readers")
            ?? Task.FromResult(new List<Reader>());

        public Task<Reader?> GetAsync(int id) =>
            _http.GetFromJsonAsync<Reader?>($"api/readers/{id}");

        public Task<HttpResponseMessage> CreateAsync(Reader r) =>
            _http.PostAsJsonAsync("api/readers", r);

        public Task<HttpResponseMessage> UpdateAsync(Reader r) =>
            _http.PutAsJsonAsync($"api/readers/{r.Id}", r);

        public Task<HttpResponseMessage> DeleteAsync(int id) =>
            _http.DeleteAsync($"api/readers/{id}");
    }
}