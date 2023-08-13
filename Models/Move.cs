namespace ChessAPI.Models
{
    public class Move
    {   
        public int Id { get; set; }
        public string? Piece { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
    }
}