using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PieceScript : MonoBehaviour
{
    [Header("General Information")]
    // position of this piece in grid
    public Vector2Int Position;
    // game manager script
    GameManagerScript gameManager;
    // positions script
    PositionScript gameobjectPositions;
    // positions script
    PositionScript gameobjectPositions2;
    // tile piece is on
    public GameObject TilePieceIsOn;

    [Header ("Pawn Information")]
    // check to see if pawn path blocked
    public bool PawnIsBlocked = false;

    [Header ("Bishop Information")]
    // check to see if Bishop path blocked
    public bool BishopFrontLeftBlocked = false;
    public bool BishopFrontRightBlocked = false;
    public bool BishopBackLeftBlocked = false;
    public bool BishopBackRightBlocked = false;

    [Header("Rook Information")]
    // Check to see if rook path is blocked
    public bool RookFrontBlocked = false;
    public bool RookBackBlocked = false;
    public bool RookLeftBlocked = false;
    public bool RookRightBlocked = false;

    [Header("Queen Information")]
    // Check to see if queen path is blocked
    public bool QueenFrontLeftBlocked = false;
    public bool QueenFrontBlocked = false;
    public bool QueenFrontRightBlocked = false;
    public bool QueenLeftBlocked = false;
    public bool QueenRightBlocked = false;
    public bool QueenBackLeftBlocked = false;
    public bool QueenBackBlocked = false;
    public bool QueenBackRightBlocked = false;
    public PhotonView pv;
    int pvid;
    
    private void Awake()
    {
        // Finds Game Manager
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        if (gameManager.Connectivity == "Online")
        {
            this.transform.SetParent(GameObject.Find(tag).transform);
        }
    }
    private void Start()
    {
        Debug.Log("Start function in PieceScript called;");
        // Finds Game Manager
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        // Gets board position Script
        gameobjectPositions2 = GameObject.Find("Board").GetComponent<PositionScript>();
        // Checks for Online Status and sets photon view
        if(gameManager.Connectivity == "Online")
        {
            pv = this.transform.GetComponent<PhotonView>();
        }
    }

    private void Update()
    {
        // Do Once Per Turn
        // get tile piece is on
        for(int i = 0; i < gameobjectPositions2.GetComponent<PositionScript>().tiles.Length; i++)
        {
            // if this pieces position matches a tiles position set tile is on to that tile
            if(this.Position == gameobjectPositions2.GetComponent<PositionScript>().tiles[i].GetComponent<TileScript>().Position)
            {
                TilePieceIsOn = gameobjectPositions2.GetComponent<PositionScript>().tiles[i];
            }
        }
        // Do Once Per Turn
        // sets position of this gameobject to the tile that shares its Position
        for(int j = 0; j <gameobjectPositions2.tiles.Length; j++)
        {
            if(Position == gameobjectPositions2.tiles[j].GetComponent<TileScript>().Position)
            {
                this.gameObject.transform.position = gameobjectPositions2.tiles[j].gameObject.transform.position;
            }
        }
        
    }

    // Functionality for Offline
    #region
    // Sets Pieces new position and moves object
    public void EndOfTurn()
    {
        // Do Once Per Turn
        // get tile piece is on
        for (int i = 0; i < gameobjectPositions2.GetComponent<PositionScript>().tiles.Length; i++)
        {
            // if this pieces position matches a tiles position set tile is on to that tile
            if (this.Position == gameobjectPositions2.GetComponent<PositionScript>().tiles[i].GetComponent<TileScript>().Position)
            {
                TilePieceIsOn = gameobjectPositions2.GetComponent<PositionScript>().tiles[i];
            }
        }
        // Do Once Per Turn
        // sets position of this gameobject to the tile that shares its Position
        for (int j = 0; j < gameobjectPositions2.tiles.Length; j++)
        {
            if (Position == gameobjectPositions2.tiles[j].GetComponent<TileScript>().Position)
            {
                this.gameObject.transform.position = gameobjectPositions2.tiles[j].gameObject.transform.position;
            }
        }
    }

    // checks for possible moves of this piece
    public void CheckPossibleMoves(string name, string tag)
    {
        gameobjectPositions = GameObject.Find("Board").GetComponent<PositionScript>();
        // if piece is white
        if(tag == "White")
        {
            // check moves if piece is a pawn
            if(name == "Pawn")
            {
                // since pawn will always start in row infront of king, and white side will always be the base of grid we can use that row as base

                // if pawn is in start row
                if(this.Position.y == 1)
                {
                    // check tiles in front of pawn for any possible kill moves
                    for(int i = 0 ; i < gameobjectPositions.blackPieces.Length ; i++)
                    {
                        // if possible right diagonal move shares place with a black piece
                        if(this.Position.x + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                        {
                            // set tile which black piece is on to a possible move tile
                            gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                        // if possible left diagonal move shares place with a black piece
                        if (this.Position.x - 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                        {
                            // set tile which black piece is on to a possible move tile
                            gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // search for tile in front of piece for possible moves
                    for (int k = 0; k < gameobjectPositions.tiles.Length; k++)
                    {
                        // if tile in front exists
                        if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // set piece to being blocked
                                PawnIsBlocked = true;
                            }
                        }
                        // if tile two moves infront exists
                        if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y && PawnIsBlocked == false)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                        }
                    }
                }
                // if pawn is not in front row
                if (this.Position.y != 1)
                {
                    // check tiles in front of pawn for any possible kill moves
                    for (int i = 0; i < gameobjectPositions.blackPieces.Length; i++)
                    {
                        // if possible right diagonal move shares place with a black piece
                        if (this.Position.x + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                        {
                            // set tile which black piece is on to a possible move tile
                            gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                        // if possible left diagonal move shares place with a black piece
                        if (this.Position.x - 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                        {
                            // set tile which black piece is on to a possible move tile
                            gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // search for tile in front of piece for possible moves
                    for (int k = 0; k < gameobjectPositions.tiles.Length; k++)
                    {
                        // if tile in front exists
                        if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                        }
                    }
                }
            }
            // check moves if piece is a knight
            if (name == "Knight")
            {
                // check tiles in knight movements for any possible kill moves
                for (int i = 0; i < gameobjectPositions.blackPieces.Length; i++)
                {
                    //               o
                    // if possible Xoo
                    if (this.Position.x + 2 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    //              o
                    //              o
                    // if possible Xo
                    if (this.Position.x + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 2 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    //             o
                    // if possible ooX
                    if (this.Position.x - 2 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    //             o
                    //             o
                    // if possible oX
                    if (this.Position.x - 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 2 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    //             ooX
                    // if possible o
                    if (this.Position.x - 2 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    //             oX
                    //             o
                    // if possible o
                    if (this.Position.x - 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 2 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    //             Xoo
                    // if possible   o
                    if (this.Position.x + 2 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    //             Xo
                    //              o
                    // if possible  o
                    if (this.Position.x + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 2 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }

                }
                // search for possible unoccupied tiles for knight move
                for (int k = 0; k < gameobjectPositions.tiles.Length; k++)
                {
                    //               o
                    // if possible Xoo
                    if (this.Position.x + 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    //              o
                    //              o
                    // if possible Xo
                    if (this.Position.x + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    //             o
                    // if possible ooX
                    if (this.Position.x - 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    //             o
                    //             o
                    // if possible oX
                    if (this.Position.x - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    //             ooX
                    // if possible o
                    if (this.Position.x - 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    //             oX
                    //             o
                    // if possible o
                    if (this.Position.x - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    //             Xoo
                    // if possible   o
                    if (this.Position.x + 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    //             Xo
                    //              o
                    // if possible  o
                    if (this.Position.x + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                }
            }
            // check moves if piece is a King
            if (name == "King")
            {
                // check tiles in front of pawn for any possible kill moves
                for (int i = 0; i < gameobjectPositions.blackPieces.Length; i++)
                {
                    // if possible INFRONT
                    if (this.Position.x == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    // if possible BEHIND
                    if (this.Position.x == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    // if possible LEFT
                    if (this.Position.x - 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    // if possible RIGHT
                    if (this.Position.x - 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    // if possible front right diagonal
                    if (this.Position.x + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    // if possible front left diagonal
                    if (this.Position.x - 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    // if possible back right diagonal
                    if (this.Position.x + 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    // if possible back left diagonal
                    if (this.Position.x - 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 1 == gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.blackPieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }

                }
                // search for tile in front of piece for possible moves
                for (int k = 0; k < gameobjectPositions.tiles.Length; k++)
                {
                    // if tile in front exists
                    if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // if tile behind exists
                    if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // if tile right exists
                    if (this.Position.x + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // if tile left exists
                    if (this.Position.x - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // if tile front left exists
                    if (this.Position.x - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // if tile front right exists
                    if (this.Position.x + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // if tile back left exists
                    if (this.Position.x - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // if tile back right exists
                    if (this.Position.x + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                }
            }
            // check moves if piece is a Bishop
            if (name == "Bishop")
            {
                // loop length of board
                for (int l = 1; l <= gameManager.boardSize; l++)
                {
                    // loop the tiles
                    for(int k = 0; k < gameobjectPositions.tiles.Length; k++)
                    {
                        // if tile in front left exists
                        if (this.Position.x-l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y+l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && BishopFrontLeftBlocked == false)
                            { 
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    BishopFrontLeftBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && BishopFrontLeftBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    BishopFrontLeftBlocked = true;
                                }
                            }
                        }
                        // if tile in front right exists
                        if (this.Position.x + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && BishopFrontRightBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    BishopFrontRightBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && BishopFrontRightBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    BishopFrontRightBlocked = true;
                                }
                            }
                        }
                        // if tile in back left exists
                        if (this.Position.x - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && BishopBackLeftBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    BishopBackLeftBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && BishopBackLeftBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    BishopBackLeftBlocked = true;
                                }
                            }
                        }
                        // if tile in back right exists
                        if (this.Position.x + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && BishopBackRightBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    BishopBackRightBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && BishopBackRightBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    BishopBackRightBlocked = true;
                                }
                            }
                        }
                    }
                }
            }          
            // check moves if piece is a rook
            if (name == "Rook")
            {
                // loop length of board
                for (int l = 1; l <= gameManager.boardSize; l++)
                {
                    // loop the tiles
                    for (int k = 0; k < gameobjectPositions.tiles.Length; k++)
                    {
                        // if tile in front exists
                        if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && RookFrontBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    RookFrontBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && RookFrontBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    RookFrontBlocked = true;
                                }
                            }
                        }
                        // if tile in Back exists
                        if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && RookBackBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    RookBackBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && RookBackBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    RookBackBlocked = true;
                                }
                            }
                        }
                        // if tile in Left exists
                        if (this.Position.x - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && RookLeftBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    RookLeftBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && RookLeftBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    RookLeftBlocked = true;
                                }
                            }
                        }
                        // if tile in Right exists
                        if (this.Position.x + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && RookRightBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    RookRightBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && RookRightBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    RookRightBlocked = true;
                                }
                            }
                        }
                    }
                }
            }
            // check moves if piece is a queen
            if (name == "Queen")
            {
                // loop length of board
                for (int l = 1; l <= gameManager.boardSize; l++)
                {
                    // loop the tiles
                    for (int k = 0; k < gameobjectPositions.tiles.Length; k++)
                    {
                        // if tile in front exists
                        if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenFrontBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenFrontBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenFrontBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenFrontBlocked = true;
                                }
                            }
                        }
                        // if tile in Back exists
                        if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenBackBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenBackBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenBackBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenBackBlocked = true;
                                }
                            }
                        }
                        // if tile in Left exists
                        if (this.Position.x - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenLeftBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenLeftBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenLeftBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenLeftBlocked = true;
                                }
                            }
                        }
                        // if tile in Right exists
                        if (this.Position.x + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenRightBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenRightBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenRightBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenRightBlocked = true;
                                }
                            }
                        }
                        // if tile in front left exists
                        if (this.Position.x - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenFrontLeftBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenFrontLeftBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenFrontLeftBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenFrontLeftBlocked = true;
                                }
                            }
                        }
                        // if tile in front right exists
                        if (this.Position.x + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenFrontRightBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenFrontRightBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenFrontRightBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenFrontRightBlocked = true;
                                }
                            }
                        }
                        // if tile in back left exists
                        if (this.Position.x - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenBackLeftBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenBackLeftBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenBackLeftBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenBackLeftBlocked = true;
                                }
                            }
                        }
                        // if tile in back right exists
                        if (this.Position.x + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenBackRightBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenBackRightBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenBackRightBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenBackRightBlocked = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        if (tag == "Black")
        {
            // check moves if piece is a pawn
            if (name == "Pawn")
            {
                // since pawn will always start in row infront of king, and white side will always be the base of grid we can use that row as base

                // if pawn is in start row
                if (this.Position.y == gameManager.boardSize - 2)
                {
                    // check tiles in front of pawn for any possible kill moves
                    for (int i = 0; i < gameobjectPositions.whitePieces.Length; i++)
                    {
                        // if possible right diagonal move shares place with a black piece
                        if (this.Position.x + 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                        {
                            // set tile which black piece is on to a possible move tile
                            gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                        // if possible left diagonal move shares place with a black piece
                        if (this.Position.x - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                        {
                            // set tile which black piece is on to a possible move tile
                            gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // search for tile in front of piece for possible moves
                    for (int k = 0; k < gameobjectPositions.tiles.Length; k++)
                    {
                        // if tile in front exists
                        if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // set piece to being blocked
                                PawnIsBlocked = true;
                            }

                        }
                        // if tile two moves infront exists
                        if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y && PawnIsBlocked == false)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                        }
                    }
                }
                // if pawn is not in front row
                if (this.Position.y != 1)
                {
                    // check tiles in front of pawn for any possible kill moves
                    for (int i = 0; i < gameobjectPositions.whitePieces.Length; i++)
                    {
                        // if possible right diagonal move shares place with a black piece
                        if (this.Position.x + 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                        {
                            // set tile which black piece is on to a possible move tile
                            gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                        // if possible left diagonal move shares place with a black piece
                        if (this.Position.x - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                        {
                            // set tile which black piece is on to a possible move tile
                            gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // search for tile in front of piece for possible moves
                    for (int k = 0; k < gameobjectPositions.tiles.Length; k++)
                    {
                        // if tile in front exists
                        if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                        }
                    }
                }
            }
            // check moves if piece is a knight
            if (name == "Knight")
            {
                // check tiles in knight movements for any possible kill moves
                for (int i = 0; i < gameobjectPositions.whitePieces.Length; i++)
                {
                    //               o
                    // if possible Xoo
                    if (this.Position.x + 2 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    //              o
                    //              o
                    // if possible Xo
                    if (this.Position.x + 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 2 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    //             o
                    // if possible ooX
                    if (this.Position.x - 2 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    //             o
                    //             o
                    // if possible oX
                    if (this.Position.x - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 2 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    //             ooX
                    // if possible o
                    if (this.Position.x - 2 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    //             oX
                    //             o
                    // if possible o
                    if (this.Position.x - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 2 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    //             Xoo
                    // if possible   o
                    if (this.Position.x + 2 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    //             Xo
                    //              o
                    // if possible  o
                    if (this.Position.x + 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 2 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }

                }
                // search for possible unoccupied tiles for knight move
                for (int k = 0; k < gameobjectPositions.tiles.Length; k++)
                {
                    //               o
                    // if possible Xoo
                    if (this.Position.x + 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    //              o
                    //              o
                    // if possible Xo
                    if (this.Position.x + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    //             o
                    // if possible ooX
                    if (this.Position.x - 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    //             o
                    //             o
                    // if possible oX
                    if (this.Position.x - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    //             ooX
                    // if possible o
                    if (this.Position.x - 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    //             oX
                    //             o
                    // if possible o
                    if (this.Position.x - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    //             Xoo
                    // if possible   o
                    if (this.Position.x + 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    //             Xo
                    //              o
                    // if possible  o
                    if (this.Position.x + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 2 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                }
            }
            // check moves if piece is a King
            if (name == "King")
            {
                // check tiles in front of pawn for any possible kill moves
                for (int i = 0; i < gameobjectPositions.whitePieces.Length; i++)
                {
                    // if possible INFRONT
                    if (this.Position.x == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    // if possible BEHIND
                    if (this.Position.x == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    // if possible LEFT
                    if (this.Position.x - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    // if possible RIGHT
                    if (this.Position.x - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    // if possible front right diagonal
                    if (this.Position.x + 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    // if possible front left diagonal
                    if (this.Position.x - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y + 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    // if possible back right diagonal
                    if (this.Position.x + 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }
                    // if possible back left diagonal
                    if (this.Position.x - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.x && this.Position.y - 1 == gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().Position.y)
                    {
                        // set tile which black piece is on to a possible move tile
                        gameobjectPositions.whitePieces[i].GetComponent<PieceScript>().TilePieceIsOn.GetComponent<TileScript>().tileIsPossibleMove = true;
                    }

                }
                // search for tile in front of piece for possible moves
                for (int k = 0; k < gameobjectPositions.tiles.Length; k++)
                {
                    // if tile in front exists
                    if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // if tile behind exists
                    if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // if tile right exists
                    if (this.Position.x + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // if tile left exists
                    if (this.Position.x - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // if tile front left exists
                    if (this.Position.x - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // if tile front right exists
                    if (this.Position.x + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // if tile back left exists
                    if (this.Position.x - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                    // if tile back right exists
                    if (this.Position.x + 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - 1 == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                    {
                        // if tile is unoccupied
                        if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false)
                        {
                            // set tile to possible move
                            gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                        }
                    }
                }
            }
            // check moves if piece is a Bishop
            if (name == "Bishop")
            {
                // loop length of board
                for (int l = 1; l <= gameManager.boardSize; l++)
                {
                    // loop the tiles
                    for (int k = 0; k < gameobjectPositions.tiles.Length; k++)
                    {
                        // if tile in front left exists
                        if (this.Position.x - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && BishopFrontLeftBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    BishopFrontLeftBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && BishopFrontLeftBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    BishopFrontLeftBlocked = true;
                                }
                            }
                        }
                        // if tile in front right exists
                        if (this.Position.x + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && BishopFrontRightBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    BishopFrontRightBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && BishopFrontRightBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    BishopFrontRightBlocked = true;
                                }
                            }
                        }
                        // if tile in back left exists
                        if (this.Position.x - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && BishopBackLeftBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    BishopBackLeftBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && BishopBackLeftBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    BishopBackLeftBlocked = true;
                                }
                            }
                        }
                        // if tile in back right exists
                        if (this.Position.x + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && BishopBackRightBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    BishopBackRightBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && BishopBackRightBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    BishopBackRightBlocked = true;
                                }
                            }
                        }
                    }
                }
            }
            // check moves if piece is a rook
            if (name == "Rook")
            {
                // loop length of board
                for (int l = 1; l <= gameManager.boardSize; l++)
                {
                    // loop the tiles
                    for (int k = 0; k < gameobjectPositions.tiles.Length; k++)
                    {
                        // if tile in front exists
                        if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && RookFrontBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    RookFrontBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && RookFrontBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    RookFrontBlocked = true;
                                }
                            }
                        }
                        // if tile in Back exists
                        if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && RookBackBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    RookBackBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && RookBackBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    RookBackBlocked = true;
                                }
                            }
                        }
                        // if tile in Left exists
                        if (this.Position.x - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && RookLeftBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    RookLeftBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && RookLeftBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    RookLeftBlocked = true;
                                }
                            }
                        }
                        // if tile in Right exists
                        if (this.Position.x + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && RookRightBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    RookRightBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && RookRightBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    RookRightBlocked = true;
                                }
                            }
                        }
                    }
                }
            }
            // check moves if piece is a queen
            if (name == "Queen")
            {
                // loop length of board
                for (int l = 1; l <= gameManager.boardSize; l++)
                {
                    // loop the tiles
                    for (int k = 0; k < gameobjectPositions.tiles.Length; k++)
                    {
                        // if tile in front exists
                        if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenFrontBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenFrontBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenFrontBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenFrontBlocked = true;
                                }
                            }
                        }
                        // if tile in Back exists
                        if (this.Position.x == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenBackBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenBackBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenBackBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenBackBlocked = true;
                                }
                            }
                        }
                        // if tile in Left exists
                        if (this.Position.x - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenLeftBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenLeftBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenLeftBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenLeftBlocked = true;
                                }
                            }
                        }
                        // if tile in Right exists
                        if (this.Position.x + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenRightBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenRightBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenRightBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenRightBlocked = true;
                                }
                            }
                        }
                        // if tile in front left exists
                        if (this.Position.x - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenFrontLeftBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenFrontLeftBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenFrontLeftBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenFrontLeftBlocked = true;
                                }
                            }
                        }
                        // if tile in front right exists
                        if (this.Position.x + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenFrontRightBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenFrontRightBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenFrontRightBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenFrontRightBlocked = true;
                                }
                            }
                        }
                        // if tile in back left exists
                        if (this.Position.x - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenBackLeftBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenBackLeftBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenBackLeftBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenBackLeftBlocked = true;
                                }
                            }
                        }
                        // if tile in back right exists
                        if (this.Position.x + l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.x && this.Position.y - l == gameobjectPositions.tiles[k].GetComponent<TileScript>().Position.y)
                        {
                            // if tile is unoccupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == false && QueenBackRightBlocked == false)
                            {
                                // set tile to possible move
                                gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                            }
                            // if tile is occupied
                            if (gameobjectPositions.tiles[k].GetComponent<TileScript>().occupied == true)
                            {
                                // Check to see if its a white piece
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag == this.tag)
                                {
                                    // set piece to being blocked
                                    QueenBackRightBlocked = true;
                                }
                                // Check to see if its the first black piece in path
                                if (gameobjectPositions.tiles[k].GetComponent<TileScript>().pieceOnTile.tag != this.tag && QueenBackRightBlocked == false)
                                {
                                    // set to possible move
                                    gameobjectPositions.tiles[k].GetComponent<TileScript>().tileIsPossibleMove = true;
                                    // set path to blocked
                                    QueenBackRightBlocked = true;
                                }
                            }
                        }
                    }
                }
            }
        
        }
    }

    // Resets Checks on piece for possible movements
    public void ResetPiece()
    {
        // Pawn Blocks
        PawnIsBlocked = false;
        // Bishop Blocks
        BishopFrontLeftBlocked = false;
        BishopFrontRightBlocked = false;
        BishopBackLeftBlocked = false;
        BishopBackRightBlocked = false;
        // Rook Blocks
        RookFrontBlocked = false;
        RookBackBlocked = false;
        RookLeftBlocked = false;
        RookRightBlocked = false;
        // Queen Blocks
        QueenFrontBlocked = false;
        QueenBackBlocked = false;
        QueenLeftBlocked = false;
        QueenRightBlocked = false;
        QueenFrontLeftBlocked = false;
        QueenFrontRightBlocked = false;
        QueenBackLeftBlocked = false;
        QueenBackRightBlocked = false;
    }

    #endregion

    // Functionality for Offline
    #region
    // Sets Pieces new position and moves object
    [PunRPC]
    public void EndOfTurnOnline()
    {
        // Do Once Per Turn
        // get tile piece is on
        for (int i = 0; i < gameobjectPositions2.GetComponent<PositionScript>().tiles.Length; i++)
        {
            // if this pieces position matches a tiles position set tile is on to that tile
            if (this.Position == gameobjectPositions2.GetComponent<PositionScript>().tiles[i].GetComponent<TileScript>().Position)
            {
                TilePieceIsOn = gameobjectPositions2.GetComponent<PositionScript>().tiles[i];
            }
        }
        // Do Once Per Turn
        // sets position of this gameobject to the tile that shares its Position
        for (int j = 0; j < gameobjectPositions2.tiles.Length; j++)
        {
            if (Position == gameobjectPositions2.tiles[j].GetComponent<TileScript>().Position)
            {
                this.gameObject.transform.position = gameobjectPositions2.tiles[j].gameObject.transform.position;
            }
        }
    }

    // Resets Checks on piece for possible movements
    [PunRPC]
    public void ResetPieceOnline()
    {
        // Pawn Blocks
        PawnIsBlocked = false;
        // Bishop Blocks
        BishopFrontLeftBlocked = false;
        BishopFrontRightBlocked = false;
        BishopBackLeftBlocked = false;
        BishopBackRightBlocked = false;
        // Rook Blocks
        RookFrontBlocked = false;
        RookBackBlocked = false;
        RookLeftBlocked = false;
        RookRightBlocked = false;
        // Queen Blocks
        QueenFrontBlocked = false;
        QueenBackBlocked = false;
        QueenLeftBlocked = false;
        QueenRightBlocked = false;
        QueenFrontLeftBlocked = false;
        QueenFrontRightBlocked = false;
        QueenBackLeftBlocked = false;
        QueenBackRightBlocked = false;
    }

    [PunRPC]
    public void PieceNewPositionOnline(int x, int y)
    {
        Position.x = x;
        Position.y = y;
    }
    #endregion
}
