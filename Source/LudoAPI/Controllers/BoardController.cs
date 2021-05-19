using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ludo.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LudoAPI;
using LudoAPI.Models;

namespace Ludo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardRepo _boardRepo;

        public BoardController(IBoardRepo boardRepo)
        {
            _boardRepo = boardRepo;
        }

        // GET: api/Board
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Board>>> GetAllBoards()
        {
            var boardList = await _boardRepo.GetAllBoards();

            if (boardList.Count == 0)
                return NoContent();
            return Ok(boardList);
        }

        // GET: api/Board/ludoBoard
        [HttpGet("{boardName}")]
        public async Task<ActionResult<Board>> GetBoardByName(string boardName)
        {
            var board = await _boardRepo.GetBoardByName(boardName);

            if (board == null)
                return NotFound();
            return Ok(board);
        }

        // PUT: api/Board/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBoard(int id, Board board)
        {
            var status = await _boardRepo.UpdateBoard(id, board);

            if (status.Exception != null)
            {
                switch (status.Exception.InnerException.Message)
                {
                    case "Id does not match":
                        return BadRequest();
                    case "Not found":
                        return NotFound();
                }
            }

            if (status.IsCompleted)
                return Ok();

            return NoContent();
        }

        // POST: api/Board
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Board>> AddBoard(Board board)
        {
            var result = await _boardRepo.AddBoard(board);

            if(result.Exception != null)
                return BadRequest();
            return Ok(board);
        }

        // DELETE: api/Board/5
        [HttpDelete("{boardName}")]
        public async Task<IActionResult> DeleteBoard(string boardName)
        {
            var status = await _boardRepo.DeleteBoard(boardName);

            if (status.Exception != null)
                return NotFound();
            return Ok();
        }
    }
}
