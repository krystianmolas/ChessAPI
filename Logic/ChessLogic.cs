using System;
using System.Collections.Generic;

namespace ChessAPI.Logic
{
    public static class ChessLogic
    {
        public static List<string> GetValidMovesForPiece(string? pieceType, string? currentPosition)
        {   

            pieceType = pieceType ?? throw new ArgumentNullException(nameof(pieceType), "Typ figury nie może być null.");
            currentPosition = currentPosition ?? throw new ArgumentNullException(nameof(currentPosition), "Pozycja nie może być null.");

            switch (pieceType.ToLower())
               {
                case "wieża":
                    return GetValidMovesForRook(currentPosition);
                case "goniec":
                    return GetValidMovesForBishop(currentPosition);
                case "królowa":
                    return GetValidMovesForQueen(currentPosition);
                case "skoczek":
                    return GetValidMovesForKnight(currentPosition);
                case "król":
                    return GetValidMovesForKing(currentPosition);
                case "pionek":
                    return GetValidMovesForPawn(currentPosition);
                default:
                    throw new ArgumentException("Nieprawidłowy typ figury");
            }
        }

        private static List<string> GetValidMovesForRook(string currentPosition)
        {
            var validMoves = new List<string>();
            int currentFile = currentPosition[0] - 'a';
            int currentRank = currentPosition[1] - '1';

            // Ruchy poziome
            for (int i = 0; i < 8; i++)
            {
                if (i != currentFile)
                {
                    validMoves.Add($"{(char)('a' + i)}{currentRank + 1}");
                }
            }

            // Ruchy pionowe
            for (int i = 0; i < 8; i++)
            {
                if (i != currentRank)
                {
                    validMoves.Add($"{(char)('a' + currentFile)}{i + 1}");
                }
            }

            return validMoves;
        }

        private static List<string> GetValidMovesForBishop(string currentPosition)
        {
            var validMoves = new List<string>();
            int currentFile = currentPosition[0] - 'a';
            int currentRank = currentPosition[1] - '1';

            // Ruchy po przekątnych
            for (int i = -7; i <= 7; i++)
            {
                if (i != 0 && IsValidSquare(currentFile + i, currentRank + i))
                {
                    validMoves.Add($"{(char)('a' + currentFile + i)}{currentRank + i + 1}");
                }

                if (i != 0 && IsValidSquare(currentFile + i, currentRank - i))
                {
                    validMoves.Add($"{(char)('a' + currentFile + i)}{currentRank - i + 1}");
                }
            }

            return validMoves;
        }

        private static List<string> GetValidMovesForQueen(string currentPosition)
        {
            var validMoves = new List<string>();
            validMoves.AddRange(GetValidMovesForRook(currentPosition));
            validMoves.AddRange(GetValidMovesForBishop(currentPosition));
            return validMoves;
        }

        private static List<string> GetValidMovesForKnight(string currentPosition)
        {
            var validMoves = new List<string>();
            int currentFile = currentPosition[0] - 'a';
            int currentRank = currentPosition[1] - '1';

            int[] fileOffsets = { -2, -2, -1, -1, 1, 1, 2, 2 };
            int[] rankOffsets = { -1, 1, -2, 2, -2, 2, -1, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newFile = currentFile + fileOffsets[i];
                int newRank = currentRank + rankOffsets[i];

                if (IsValidSquare(newFile, newRank))
                {
                    validMoves.Add($"{(char)('a' + newFile)}{newRank + 1}");
                }
            }

            return validMoves;
        }

        private static List<string> GetValidMovesForKing(string currentPosition)
        {
            var validMoves = new List<string>();
            int currentFile = currentPosition[0] - 'a';
            int currentRank = currentPosition[1] - '1';

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        int newFile = currentFile + i;
                        int newRank = currentRank + j;

                        if (IsValidSquare(newFile, newRank))
                        {
                            validMoves.Add($"{(char)('a' + newFile)}{newRank + 1}");
                        }
                    }
                }
            }

            return validMoves;
        }

        private static List<string> GetValidMovesForPawn(string currentPosition)
        {
            var validMoves = new List<string>();
            int currentFile = currentPosition[0] - 'a';
            int currentRank = currentPosition[1] - '1';

            // Zakładamy, że pionek jest biały dla uproszczenia. Dla czarnego piona musielibyśmy odejmować od rank.
            if (IsValidSquare(currentFile, currentRank + 1))
            {
                validMoves.Add($"{(char)('a' + currentFile)}{currentRank + 2}");
            }

            return validMoves;
        }

        private static bool IsValidSquare(int file, int rank)
        {
            return file >= 0 && file < 8 && rank >= 0 && rank < 8;
        }
    }
}
