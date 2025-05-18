using DomainTables;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LibraryClient.Services
{
    public class BookService
    {
        private readonly HttpClient _http;

        public BookService(HttpClient http)
        {
            _http = http;
        }

        public Task<List<Book>> GetAllAsync() =>
            _http.GetFromJsonAsync<List<Book>>("api/books")
            ?? Task.FromResult(new List<Book>());

        public Task<Book?> GetAsync(int id) =>
            _http.GetFromJsonAsync<Book?>($"api/books/{id}");

        public Task<HttpResponseMessage> CreateAsync(Book b) =>
            _http.PostAsJsonAsync("api/books", b);

        public Task<HttpResponseMessage> UpdateAsync(Book b) =>
            _http.PutAsJsonAsync($"api/books/{b.Id}", b);

        public Task<HttpResponseMessage> DeleteAsync(int id) =>
            _http.DeleteAsync($"api/books/{id}");
    }
}