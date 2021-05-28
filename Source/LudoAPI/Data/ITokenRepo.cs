using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ludo.API.Data
{
    public interface ITokenRepo
    {
        Task<string> MoveToken(int tokenId, int diceNumber);
    }
}
