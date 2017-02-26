using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckerBoardScript : MonoBehaviour {

    public Piece[,] pieces = new Piece[8,8];
    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;
    private List<Piece> forcedPieces = new List<Piece>();

    private bool justJumped = false;

    public GameObject winMenu;
    public Text winText;
    public bool gameOver = false;

    private Vector3 boardOffset = new Vector3(-4.0f, 0, -4.0f);
    private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);
    public bool isWhiteTurn;
    public bool isWhite;
    private bool hasKilled;

    private Vector2 mouseOver;
    private Piece selectedPiece;
    private Vector2 startDrag;
    private Vector2 endDrag;

    private void Start()
    {
        winMenu.SetActive(false);
        forcedPieces = new List<Piece>();
        isWhiteTurn = true;
        isWhite = true;
        generateBoard();
    }

    private void Update()
    {

        //if it is my turn
        {
            UpdateMouseOver();

            int x = (int)mouseOver.x;
            int y = (int)mouseOver.y;

            if(selectedPiece != null)
            {
                updatePieceDrag(selectedPiece);
            }

            if (Input.GetMouseButtonDown(0))
            {
                selectPiece(x, y);
            }

            if (Input.GetMouseButtonUp(0))
            {
                tryMove((int)startDrag.x, (int)startDrag.y, x, y);
            }
        }
    }

    private void updatePieceDrag(Piece p)
    {
        //if it's my turn
        if (!Camera.main)
        {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            p.transform.position = hit.point + Vector3.up; 

        }
    }

    private void tryMove(int startX, int startY, int x, int y)
    {
        forcedPieces = ScanForPossibleMoves();

        startDrag = new Vector2(startX, startY);
        endDrag = new Vector2(x, y);
        selectedPiece = pieces[startX, startY]; //multiplayer support

        //check if out of bounds
        if(x < 0 || x > 8 || y < 0 || y > 8)
        {
            if(selectedPiece != null)
            {
                movePiece(selectedPiece, startX, startY);
            }

            startDrag = Vector2.zero;
            selectedPiece = null;
            return;
        }

        //is there a selected piece?
        if (selectedPiece != null)
        {
           // Debug.Log("There is a selected piece");
            //If it didn't go anywhere?
            if (endDrag == startDrag)
            {
               // Debug.Log("Didnt go anywhere");
                movePiece(selectedPiece, startX, startY);
                startDrag = Vector2.zero;
                selectedPiece = null;
                return;
            }

            //check if valid move
            bool isValid = selectedPiece.isValidMove(pieces, startX, startY, x, y);
            //Debug.Log("Is Valid move? " + isValid);

            if (selectedPiece.isValidMove(pieces, startX, startY, x, y))
            {
              //  Debug.Log("SelectedPiece is valid move");
                //did we jump anyone?
                if (Mathf.Abs(x - startX) == 2)
                {
                    Debug.Log("We jumped someone!");
                    Piece p = pieces[(startX + x) / 2, (startY + y) / 2];
                    if (p != null)
                    {
                        justJumped = true;
                        pieces[(startX + x) / 2, (startY + y) / 2] = null;
                        Destroy(p.gameObject);
                        hasKilled = true;
                    }
                }
                else
                {
                    justJumped = false;
                }

                //Were we supposed to kill anything?
                if(forcedPieces.Count != 0 && !hasKilled)
                {
                    movePiece(selectedPiece, startX, startY);
                    startDrag = Vector2.zero;
                    selectedPiece = null;
                    return;
                }

                //Debug.Log("Lets move the piece");
                pieces[x, y] = selectedPiece;
                pieces[startX, startY] = null;
                movePiece(selectedPiece, x, y);
                EndTurn();
            }
            else
            {
                movePiece(selectedPiece, startX, startY);
                startDrag = Vector2.zero;
                selectedPiece = null;
                return;
            }
        }
    }

    private void EndTurn()
    {
        int x = (int)endDrag.x;
        int y = (int)endDrag.y;

        if(selectedPiece != null)
        {
            if(selectedPiece.isWhite && !selectedPiece.isKing && y == 7)    //HERE!! : May need to change "isWhite" to "!isWhite"
            {
                selectedPiece.isKing = true;
                selectedPiece.transform.Rotate(Vector3.right * 180);
            }
            else if (!selectedPiece.isWhite && !selectedPiece.isKing && y == 0)    //HERE!! : May need to change "isWhite" to "!isWhite"
            {
                selectedPiece.isKing = true;
                selectedPiece.transform.Rotate(Vector3.right * 180);
            }
        }

        selectedPiece = null;
        startDrag = Vector2.zero;

        if(ScanForPossibleMoves(selectedPiece, x, y).Count != 0 && hasKilled)
        {
            return;
        }

        hasKilled = false;
        isWhite = !isWhite;
        isWhiteTurn = !isWhiteTurn;
        CheckVictory();
    }

    private void CheckVictory()
    {
        var piecesHere = FindObjectsOfType<Piece>();
        bool hasWhite = false;
        bool hasBlack = false;
        int whites = 0;
        int blacks = 0;
        for (int i = 0; i < piecesHere.Length; i++)
        {
            if (piecesHere[i].isWhite)
            {
                whites++;
                hasWhite = true;
            }
            else if (!piecesHere[i].isWhite)
            {
                blacks++;
                hasBlack = true;
            } 
        }

        Debug.Log("Whites left: " + whites);
        Debug.Log("Blacks left: " + blacks);
        Debug.Log("hasWhite: " + hasWhite);
        Debug.Log("hasBlack: " + hasBlack);

        if (whites == 1 && isWhiteTurn && justJumped == true) Victory(true);
        else if (blacks == 1 && !isWhiteTurn && justJumped == true) Victory(true);
        if (hasWhite == true && hasBlack == false) Victory(true);
        else if (hasWhite == false && hasBlack == true) Victory(false);

        //if (hasWhite) Victory(true);
        //else if (hasBlack) Victory(false);
    }

    private void Victory(bool isWhite)
    {
        if (isWhite)
        {
            Debug.Log("White Team Wins");
            winMenu.SetActive(true);
            winText.text = "White Team Wins!";
            //pop up menu with text "White Team Wins!" 
            //two buttons for "Quit" and for "Play Again"

        }
        else if (!isWhite)
        {
            Debug.Log("Black Team Wins");
            winMenu.SetActive(true);
            winText.text = "Black Team Wins!";
            //pop up menu with text "Black Team Wins!" 
            //two buttons for "Quit" and for "Play Again"
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    private List<Piece> ScanForPossibleMoves(Piece p, int x, int y)
    {
        forcedPieces = new List<Piece>();

        if(pieces[x,y].isForcedToMove(pieces, x, y))
        {
            forcedPieces.Add(p);
        }


        return forcedPieces;
    }

    private List<Piece> ScanForPossibleMoves()
    {
        forcedPieces = new List<Piece>();

        //check all the pieces
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if(pieces[i,j] != null && pieces[i,j].isWhite == isWhiteTurn)
                {
                    if (pieces[i, j].isForcedToMove(pieces, i, j))
                    {
                        forcedPieces.Add(pieces[i, j]);
                    }
                }
            }
        }
        Debug.Log("ForcedPieces.Count= " + forcedPieces.Count);

        return forcedPieces;
    }

    private void selectPiece(int x, int y)
    {
        //out of bounds?
        if(x < 0 || x >= 8 || y < 0 || y > 8)
        {
            return;
        }

        //is there a piece here?
        Piece p = pieces[x, y];

        if(p != null && p.isWhite == isWhite)
        {
            if(forcedPieces.Count == 0)
            {
                selectedPiece = p;
                startDrag = mouseOver;
                //Debug.Log(selectedPiece.name);
            }
            else
            {
                //look for the piece under our forcedpieces list
                if(forcedPieces.Find(fp => fp == p) == null)
                {
                    return;
                }

                selectedPiece = p;
                startDrag = mouseOver;
            }

        }
    }

    private void UpdateMouseOver()
    {
        //if it's my turn
        if (!Camera.main)
        {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }


    }

    private void generateBoard()
    {
        //generate white team
        for(int y = 0; y < 3; y++)
        {
            bool oddRow = (y % 2 == 0);

            for(int x = 0; x < 8; x += 2)
            {
                //generate piece
                generatePiece((oddRow) ? x : x+1, y);
            }
        }

        //generate black team
        for(int y = 7; y > 4; y--)
        {
            bool oddRow = (y % 2 == 0);

            for(int x = 0; x < 8; x += 2)
            {
                // generate piece
                 generatePiece((oddRow) ? x : x + 1, y);
            }
        }
    }

    private void generatePiece(int x, int y)
    {
        bool isPieceWhite = (y > 3) ? false : true;
        GameObject go = Instantiate((isPieceWhite)?whitePiecePrefab:blackPiecePrefab) as GameObject;
        go.transform.SetParent(transform);
        Piece p = go.GetComponent<Piece>();
        pieces[x, y] = p;
        movePiece(p, x, y);
    }

    private void movePiece(Piece p, int x, int y)
    {
        p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
    }

}
