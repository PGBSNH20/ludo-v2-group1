using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Ludo.API.Logic;
using LudoAPI.Models;
using RestSharp;

namespace Ludo.Web
{
    public class LudoDataAccess : ILudoDataAccess
    {
        public async Task<IRestResponse> AddBoard(string boardName)
        {
            var board = GameFactory.CreateBoard(boardName);
            var client = new RestClient("https://localhost/api/"); 
            var request = new RestRequest("board/", Method.POST);
            request.AddJsonBody(board);
            var s= await client.ExecuteAsync(request);
            return s;
        }
        public async Task<Board> GetBoard(string boardName)
        {
            var client = new RestClient("https://localhost/api/");
            var request = new RestRequest($"board/{boardName}", DataFormat.Json);
            var response = await client.GetAsync<Board>(request);
            return response;
        }
    }

    public interface ILudoDataAccess
    {
        Task<IRestResponse> AddBoard(string boardName);
        Task<Board> GetBoard(string boardName);
    }
}
