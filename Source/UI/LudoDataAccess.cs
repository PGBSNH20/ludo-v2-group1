using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Ludo.API.Logic;
using LudoAPI.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Ludo.Web
{
    public class LudoDataAccess : ILudoDataAccess
    {
        static readonly HttpClient client = new HttpClient();

        public async Task<HttpResponseMessage> AddBoard(string boardName)
        {
            var board = GameFactory.CreateBoard(boardName);
            var json = JsonConvert.SerializeObject(board);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost/api/board/", data);
            return response;
        }
        public async Task<Board> GetBoardAsync(string boardName)
        {
            Board board = null;
            HttpResponseMessage response = await client.GetAsync($"https://localhost/api/board/{boardName}");
            if (response.IsSuccessStatusCode)
            {
                board = await response.Content.ReadFromJsonAsync<Board>();
            }
            return board;
        }
    }

    public interface ILudoDataAccess
    {
        Task<HttpResponseMessage> AddBoard(string boardName);
        Task<Board> GetBoardAsync(string path);
    }
}
