using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {

    public bool isWhite;
    public bool isKing;

	public bool isValidMove(Piece[,] board, int startX, int startY, int x, int y)
    {
        //if are moving on top of another piece
        if(board[x, y] != null)
        {
           // Debug.Log("Moving on top of another piece.");
            return false;
        }

        isWhite = board[startX, startY].isWhite;

        int deltaMove = Mathf.Abs(startX - x);        
        int deltaMoveY = Mathf.Abs(startY - y);
        //Debug.Log("Delta Move = " + deltaMove);
        //Debug.Log("isKing= " + isKing);
       // Debug.Log("isWhite= " + isWhite);
        //Debug.Log("Delta Move Y= " + deltaMoveY);


        //check if going backwards?
        if (isWhite && !isKing)
        {
            //Debug.Log("Here 1!");
            if (startY > y)
            {
                return false;
            }
        }
        if(!isWhite && !isKing)
        {
            //Debug.Log("Here 2!");
            if (startY < y)
            {
                return false;
            }
        }

        if (isWhite || isKing)
        {
          //  Debug.Log("It is white or it is a king.");
            if (deltaMove == 1)
            {
                if (deltaMoveY == 1)
                {
                    return true;
                }
            }
            else if(deltaMove == 2)
            {
                if(deltaMoveY == 2)
                {
                    Piece p = board[(startX + x) / 2, (startY + y) / 2];
                    if (p != null && p.isWhite != isWhite)
                    {
                        return true;
                    }
                }
            }
        }

        //black team
        if (!isWhite || isKing)
        {
          //  Debug.Log("Delta Move = " + deltaMove);
          //  Debug.Log("Delta Move Y= " + deltaMoveY);
          //  Debug.Log("It is black or it is a king");
            if (deltaMove == 1)
            {
                if (deltaMoveY == 1)
                {
                    return true;
                }
            }
            else if (deltaMove == 2)
            {
                if (deltaMoveY == 2)
                {
                    Piece p = board[(startX + x) / 2, (startY + y) / 2];
                    if (p != null && p.isWhite != isWhite)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool isForcedToMove(Piece[,] board, int x, int y)
    {
        if (isWhite || isKing)
        {
            //top left
            if(x >= 2 && y <= 5)
            {
                Piece p = board[x - 1, y + 1];
                //if there is a piece there and not same color, have to kill it
                if(p != null && p.isWhite != isWhite)
                {
                    //check if possible to land after jump
                    if(board[x-2, y+2] == null)
                    {
                        return true;
                    }
                }
            }

            //top right
            if (x <= 5 && y <= 5)
            {
                Piece p = board[x + 1, y + 1];
                //if there is a piece there and not same color, have to kill it
                if (p != null && p.isWhite != isWhite)
                {
                    //check if possible to land after jump
                    if (board[x + 2, y + 2] == null)
                    {
                        return true;
                    }
                }
            }
        }

        if(!isWhite || isKing)
        {
            //bottom left
            if (x >= 2 && y >= 2)
            {
                Piece p = board[x - 1, y - 1];
                //if there is a piece there and not same color, have to kill it
                if (p != null && p.isWhite != isWhite)
                {
                    //check if possible to land after jump
                    if (board[x - 2, y - 2] == null)
                    {
                        return true;
                    }
                }
            }

            //bottom right
            if (x <= 5 && y >= 2)
            {
                Piece p = board[x + 1, y - 1];
                //if there is a piece there and not same color, have to kill it
                if (p != null && p.isWhite != isWhite)
                {
                    //check if possible to land after jump
                    if (board[x + 2, y - 2] == null)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
