using LudoAPI.Models;
using System.Threading.Tasks;

namespace Ludo.API.Data
{
    public interface IGameRepo
    {
        Task<Board> GetGameByName(string name);
    }
}
