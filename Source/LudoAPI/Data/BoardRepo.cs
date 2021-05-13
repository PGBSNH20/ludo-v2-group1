using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoAPI;
using LudoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ludo.API.Data
{
    public class BoardRepo : IBoardRepo
    {
        private readonly LudoContext _context;

        public BoardRepo(LudoContext context)
        {
            _context = context;
        }

        public async Task<List<Board>> GetAllBoards()
        {
            return await _context.Board.ToListAsync();
        }

        public async Task<Board> GetBoardByName(string name)
        {
            try
            {
                return await _context.Board.Where(b => b.BoardName == name).FirstAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<Task> UpdateBoard(int id, Board board)
        {
            if (id != board.Id)
            {
                return Task.FromException(new ArgumentException("Id does not match"));
            }

            _context.Entry(board).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoardExists(id))
                {
                    return Task.FromException(new ArgumentException("Not found"));
                }

                throw;
            }

            return Task.CompletedTask;
        }

        public async Task<Task> AddBoard(Board board)
        {
            try
            {
                _context.Board.Add(board);
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch
            {
                return Task.FromException(new ArgumentException("Bad request"));
            }

        }

        public async Task<Task> DeleteBoard(string name)
        {
            try
            {
                var board = await _context.Board.Where(b => b.BoardName == name).FirstAsync();
                _context.Board.Remove(board);
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch
            {
               return Task.FromException(new ArgumentException("Not found"));
            }
        }

        private bool BoardExists(int id)
        {
            return _context.Board.Any(e => e.Id == id);
        }
    }
}
