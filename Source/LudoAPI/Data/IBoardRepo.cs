using System.Collections.Generic;
using System.Threading.Tasks;
using LudoAPI.Models;

namespace Ludo.API.Data
{
    public interface IBoardRepo
    {
        Task<List<Board>> GetAllBoards();
        Task<Board> GetBoardByName(string name);
        Task<Task> UpdateBoard(int id, Board board);
        Task<Task> AddBoard(string boardName);
        Task<Task> DeleteBoard(string name);
    }
}