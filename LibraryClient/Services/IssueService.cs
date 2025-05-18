using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using DomainTables;

namespace LibraryClient.Services
{
    public class IssueService
    {
        private readonly HttpClient _http;

        public IssueService(HttpClient http)
        {
            _http = http;
        }

        public Task<List<Issue>?> GetAllAsync() =>
            _http.GetFromJsonAsync<List<Issue>>("api/issues");

        public Task<Issue?> GetAsync(int readerId, int bookId) =>
            _http.GetFromJsonAsync<Issue>($"api/issues/{readerId}/{bookId}");

        public Task<HttpResponseMessage> CreateAsync(Issue i) =>
            _http.PostAsJsonAsync("api/issues", i);

        public Task<HttpResponseMessage> UpdateAsync(Issue i) =>
            _http.PutAsJsonAsync($"api/issues/{i.ReaderId}/{i.BookId}", i);

        public Task<HttpResponseMessage> DeleteAsync(int readerId, int bookId) =>
            _http.DeleteAsync($"api/issues/{readerId}/{bookId}");
    }
}