using LudoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ludo.API.Data
{
    public interface IPlayerRepo
    {
        Task<Task> AddPlayer(string playerName, string boardName, string color);
    }
}
