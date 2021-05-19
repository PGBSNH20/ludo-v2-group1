using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        private readonly HttpClient _client = new();

        public async Task<HttpResponseMessage> AddBoard(string boardName)
        {
            var board = GameFactory.CreateBoard(boardName);
            var json = JsonConvert.SerializeObject(board);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("https://localhost/api/board/", data);
            return response;
        }
        public async Task<Board> GetBoardAsync(string boardName)
        {
            Board board = null;
            HttpResponseMessage response = await _client.GetAsync($"https://localhost/api/board/{boardName}");
            
            if (response.IsSuccessStatusCode)
                board = await response.Content.ReadFromJsonAsync<Board>();
            return board;
        }
        public async Task<Board> GetGameAsync(string gameName)
        {
            Board board = null;
            HttpResponseMessage response = await _client.GetAsync($"https://localhost/api/game/{gameName}");

            if (response.IsSuccessStatusCode)
                board = await response.Content.ReadFromJsonAsync<Board>();
            return board;
        }

        public async Task<string> GetPlayerTurn(string gameName)
        {
            string msg = string.Empty;
            HttpResponseMessage response = await _client.GetAsync($"https://localhost/api/players/turn/{gameName}");

            if (response.IsSuccessStatusCode)
                msg = await response.Content.ReadAsStringAsync();
            return msg;
        }
        public async Task<HttpResponseMessage> AddPlayerTurn(string gameName, string playerName)
        {
            var json = JsonConvert.SerializeObject(playerName);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync($"https://localhost/api/players/turn/{gameName}", data);
            return response;
        }
        public async Task<RestResponse> PostPlayer(string gameName, PlayerTokenColor nameColor)
        {
            var json = JsonConvert.SerializeObject(nameColor);
            
            var client = new RestClient("https://localhost/api/players/");
            var request = new RestRequest($"{gameName}", Method.POST).AddJsonBody(json);
            var response = await client.ExecuteAsync(request);
           return (RestResponse)response;
        }
    }

    public interface ILudoDataAccess
    {
        Task<HttpResponseMessage> AddBoard(string boardName);
        Task<Board> GetBoardAsync(string path);
        Task<Board> GetGameAsync(string boardName);
        Task<RestResponse> PostPlayer(string gameName, PlayerTokenColor nameColor);
        Task<string> GetPlayerTurn(string gameName);
        Task<HttpResponseMessage> AddPlayerTurn(string gameName, string playerName);
    }
}
