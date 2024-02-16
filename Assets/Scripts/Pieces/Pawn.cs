using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();

        int forwardDirection = team == 0 ? 1 : -1;

        // Move one
        if (board[currentX, currentY + forwardDirection] == null)
        {
            r.Add(new Vector2Int(currentX, currentY + forwardDirection));
        }
        // Move two
        if (board[currentX, currentY + forwardDirection] == null)
        {
            if (team == 0 && currentY == 1 && board[currentX, currentY + (forwardDirection * 2)] == null)
            {
                r.Add(new Vector2Int(currentX, currentY + forwardDirection * 2));
            }
            if (team == 1 && currentY == 6 && board[currentX, currentY + (forwardDirection * 2)] == null)
            {
                r.Add(new Vector2Int(currentX, currentY + forwardDirection * 2));
            }
        }

        // Capture
        if (currentX != tileCountX - 1)
        {
            if (board[currentX + 1, currentY + forwardDirection] != null && board[currentX + 1, currentY + forwardDirection].team != team)
            {
                r.Add(new Vector2Int(currentX + 1, currentY + forwardDirection));
            }
            if (currentX != 0)
            {
                if (board[currentX - 1, currentY + forwardDirection] != null && board[currentX - 1, currentY + forwardDirection].team != team)
                {
                    r.Add(new Vector2Int(currentX - 1, currentY + forwardDirection));
                }
            }
        }
        //// Move forward
        //if (currentY + forwardDirection >= 0 && currentY + forwardDirection < tileCountY)
        //{
        //    if (board[currentX, currentY + forwardDirection] == null)
        //    {
        //        r.Add(new Vector2Int(currentX, currentY + forwardDirection));

        //        // Move forward again
        //        if (currentY + forwardDirection * 2 >= 0 && currentY + forwardDirection * 2 < tileCountY)
        //        {
        //            if (board[currentX, currentY + forwardDirection * 2] == null)
        //            {
        //                r.Add(new Vector2Int(currentX, currentY + forwardDirection * 2));
        //            }
        //        }
        //    }
        //}

        //// Capture
        //if (currentY + forwardDirection >= 0 && currentY + forwardDirection < tileCountY)
        //{
        //    if (currentX - 1 >= 0)
        //    {
        //        if (board[currentX - 1, currentY + forwardDirection] != null)
        //        {
        //            if (board[currentX - 1, currentY + forwardDirection].team != team)
        //            {
        //                r.Add(new Vector2Int(currentX - 1, currentY + forwardDirection));
        //            }
        //        }
        //    }
        //    if (currentX + 1 < tileCountX)
        //    {
        //        if (board[currentX + 1, currentY + forwardDirection] != null)
        //        {
        //            if (board[currentX + 1, currentY + forwardDirection].team != team)
        //            {
        //                r.Add(new Vector2Int(currentX + 1, currentY + forwardDirection));
        //            }
        //        }
        //    }
        //}

        return r;
    }

    public override SpecialMove GetSpecialMoves(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> availableMoves)
    {
        int direction = (team == 0) ? 1 : -1;

        if((team == 0 && currentY == 6) || (team == 1 && currentY == 1))
        {
            return SpecialMove.Promotion;
        }

        // En Passant
        if (moveList.Count > 0)
        {
            Vector2Int[] lastMove = moveList[moveList.Count - 1];
            if (board[lastMove[1].x, lastMove[1].y].type == ChessPieceType.Pawn) // If last piece moved was a pawn
            {
                if (Mathf.Abs(lastMove[0].y - lastMove[1].y) == 2) // If the last move was a +2 in either direction
                {
                    if (board[lastMove[1].x, lastMove[1].y].team != team) // If the move was from the other team
                    {
                        if (lastMove[1].y == currentY) // If both pawns are on the same Y
                        {
                            if (lastMove[1].x == currentX - 1) // Landed left
                            {
                                availableMoves.Add(new Vector2Int(currentX - 1, currentY + direction));
                                return SpecialMove.EnPassant;
                            }
                            if (lastMove[1].x == currentX + 1) // Landed right
                            {
                                availableMoves.Add(new Vector2Int(currentX + 1, currentY + direction));
                                return SpecialMove.EnPassant;
                            }
                        }
                    }
                }
            }
        }
        return SpecialMove.None;
    }
}
