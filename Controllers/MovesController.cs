using Microsoft.AspNetCore.Mvc;
using ChessAPI.Models;
using ChessAPI.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http; // Potrzebne dla IHttpContextAccessor
using Microsoft.Extensions.Configuration; // Potrzebne dla IConfiguration
using ChessAPI.Logic;
using System; // Potrzebne dla ArgumentException

namespace ChessAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovesController : ControllerBase
    {
        private readonly ChessDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MovesController(ChessDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> PostMove(Move move)
        {
            if (move.From != "start")
            {
            // Walidacja ruchu
            var validMoves = ChessLogic.GetValidMovesForPiece(move.Piece, move.From);
            
            if (move.To == null || !validMoves.Contains(move.To))
            {
                return BadRequest("Nieprawidłowy ruch!");
            }
            if (!validMoves.Contains(move.To))
            {
                return BadRequest("Nieprawidłowy ruch!");
            }
            }
            _context.Moves.Add(move);
            await _context.SaveChangesAsync();
            return Ok(move);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Move>>> GetLast10Moves()
        {
            var last10Moves = await _context.Moves
            .OrderByDescending(move => move.Id) // Sortuj malejąco po ID
            .Take(10) // Wybierz 10 pierwszych ruchów
            .ToListAsync();

            return last10Moves;
        }

        [HttpGet("available-pieces")]
        public IActionResult GetAvailablePieces()
        {   
            if (_httpContextAccessor.HttpContext?.Request == null)
            {
                return BadRequest("Nie można uzyskać dostępu do kontekstu HTTP.");
            }
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var pieces = new List<object>
            {
                new { Name = "Wieża", ImagePath = $"{baseUrl}/images/wieża.png" },
                new { Name = "Skoczek", ImagePath = $"{baseUrl}/images/skoczek.png" },
                new { Name = "Goniec", ImagePath = $"{baseUrl}/images/goniec.png" },
                new { Name = "Pionek", ImagePath = $"{baseUrl}/images/pionek.png" },
                new { Name = "Król", ImagePath = $"{baseUrl}/images/król.png" },
                new { Name = "Królowa", ImagePath = $"{baseUrl}/images/królowa.png" }
            };

            return Ok(pieces);
        }

        [HttpGet("valid-moves")]
        public IActionResult GetValidMoves(string pieceType, string currentPosition)
        {
            try
            {   
                if (pieceType != null && currentPosition != null)
                {  
                    var validMoves = ChessLogic.GetValidMovesForPiece(pieceType!, currentPosition!);
                    return Ok(validMoves);
                }
                else
                {
                    return BadRequest("Nieprawidłowe dane wejściowe: wymagane są pieceType i currentPosition.");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}