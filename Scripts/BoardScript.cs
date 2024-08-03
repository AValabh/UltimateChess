using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BoardScript : MonoBehaviourPunCallbacks
{
    public GameManagerScript gameManager;
    public GameObject PawnPrefab;
    public GameObject KnightPrefab;
    public GameObject KingPrefab;
    public GameObject BishopPrefab;
    public GameObject RookPrefab;
    public GameObject QueenPrefab;
    public Material WhiteMaterial;
    public Material BlackMaterial;
    public PhotonView pv;

    // Functionality for Offline Play
    #region
    // Called from GameManager to populate board
    public void PopulateBoard()
    {
        // Sets Game Manager
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        if(gameManager.Connectivity == "Offline")
        {
            // Populate board for normal match
            if (gameManager.gameType == "NormalMatch")
            {
                AddPawns();
                AddRook("White", new Vector2Int(0, 0));
                AddRook("White", new Vector2Int(7, 0));
                AddKnight("White", new Vector2Int(1, 0));
                AddKnight("White", new Vector2Int(6, 0));
                AddBishop("White", new Vector2Int(2, 0));
                AddBishop("White", new Vector2Int(5, 0));
                AddKing("White", new Vector2Int(3, 0));
                AddQueen("White", new Vector2Int(4, 0));

                AddRook("Black", new Vector2Int(0, gameManager.boardSize - 1));
                AddRook("Black", new Vector2Int(7, gameManager.boardSize - 1));
                AddKnight("Black", new Vector2Int(1, gameManager.boardSize - 1));
                AddKnight("Black", new Vector2Int(6, gameManager.boardSize - 1));
                AddBishop("Black", new Vector2Int(2, gameManager.boardSize - 1));
                AddBishop("Black", new Vector2Int(5, gameManager.boardSize - 1));
                AddKing("Black", new Vector2Int(4, gameManager.boardSize - 1));
                AddQueen("Black", new Vector2Int(3, gameManager.boardSize - 1));
            }
            // Populate board for speed match
            if (gameManager.gameType == "SpeedMatch")
            {
                AddPawns();
                AddRook("White", new Vector2Int(0, 0));
                AddRook("White", new Vector2Int(5, 0));
                AddKnight("White", new Vector2Int(1, 0));
                AddKnight("White", new Vector2Int(4, 0));
                AddKing("White", new Vector2Int(3, 0));
                AddQueen("White", new Vector2Int(2, 0));

                AddRook("Black", new Vector2Int(0, gameManager.boardSize - 1));
                AddRook("Black", new Vector2Int(5, gameManager.boardSize - 1));
                AddKnight("Black", new Vector2Int(1, gameManager.boardSize - 1));
                AddKnight("Black", new Vector2Int(4, gameManager.boardSize - 1));
                AddKing("Black", new Vector2Int(2, gameManager.boardSize - 1));
                AddQueen("Black", new Vector2Int(3, gameManager.boardSize - 1));
            }
        }
    }

    void AddPawns()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        // Adds White Pawns
        for (int i = 0; i < gameManager.boardSize; i++)
        {
            GameObject Pawn = Instantiate(PawnPrefab) as GameObject;
            Pawn.transform.parent = GameObject.Find("White").transform;
            Pawn.name = "Pawn";
            Pawn.tag = "White";
            Pawn.GetComponentInChildren<MeshRenderer>().material = WhiteMaterial;
            Pawn.GetComponent<PieceScript>().Position = new Vector2Int(i, 1);
        }

        // Adds Black Pawns
        for (int j = 0; j < gameManager.boardSize; j++)
        {
            GameObject Pawn = Instantiate(PawnPrefab) as GameObject;
            Pawn.transform.parent = GameObject.Find("Black").transform;
            Pawn.name = "Pawn";
            Pawn.tag = "Black";
            Pawn.GetComponentInChildren<MeshRenderer>().material = BlackMaterial;
            Pawn.GetComponent<PieceScript>().Position = new Vector2Int(j, gameManager.boardSize - 2);
        }
    }
    void AddKnight(string tag, Vector2Int Position)
    {
        if (tag == "White")
        {
            GameObject Knight = Instantiate(KnightPrefab) as GameObject;
            Knight.transform.parent = GameObject.Find("White").transform;
            Knight.name = "Knight";
            Knight.tag = tag;
            Knight.GetComponentInChildren<MeshRenderer>().material = WhiteMaterial;
            Knight.GetComponent<PieceScript>().Position = Position;
        }
        if (tag == "Black")
        {
            GameObject Knight = Instantiate(KnightPrefab) as GameObject;
            Knight.transform.parent = GameObject.Find("Black").transform;
            Knight.name = "Knight";
            Knight.tag = tag;
            Knight.gameObject.transform.eulerAngles = new Vector3(Knight.transform.eulerAngles.x, Knight.transform.eulerAngles.y - 180f, Knight.transform.eulerAngles.z);
            Knight.GetComponentInChildren<MeshRenderer>().material = BlackMaterial;
            Knight.GetComponent<PieceScript>().Position = Position;
        }
    }
    void AddKing(string tag, Vector2Int Position)
    {
        if (tag == "White")
        {
            GameObject King = Instantiate(KingPrefab) as GameObject;
            King.transform.parent = GameObject.Find("White").transform;
            King.name = "King";
            King.tag = tag;
            King.GetComponentInChildren<MeshRenderer>().material = WhiteMaterial;
            King.GetComponent<PieceScript>().Position = Position;
        }
        if (tag == "Black")
        {
            GameObject King = Instantiate(KingPrefab) as GameObject;
            King.transform.parent = GameObject.Find("Black").transform;
            King.name = "King";
            King.tag = tag;
            King.GetComponentInChildren<MeshRenderer>().material = BlackMaterial;
            King.GetComponent<PieceScript>().Position = Position;
        }
    }
    void AddBishop(string tag, Vector2Int Position)
    {
        if (tag == "White")
        {
            GameObject Bishop = Instantiate(BishopPrefab) as GameObject;
            Bishop.transform.parent = GameObject.Find("White").transform;
            Bishop.name = "Bishop";
            Bishop.tag = tag;
            Bishop.GetComponentInChildren<MeshRenderer>().material = WhiteMaterial;
            Bishop.GetComponent<PieceScript>().Position = Position;
        }
        if (tag == "Black")
        {
            GameObject Bishop = Instantiate(BishopPrefab) as GameObject;
            Bishop.transform.parent = GameObject.Find("Black").transform;
            Bishop.name = "Bishop";
            Bishop.tag = tag;
            Bishop.GetComponentInChildren<MeshRenderer>().material = BlackMaterial;
            Bishop.GetComponent<PieceScript>().Position = Position;
        }
    }
    void AddRook(string tag, Vector2Int Position)
    {
        if (tag == "White")
        {
            GameObject Rook = Instantiate(RookPrefab) as GameObject;
            Rook.transform.parent = GameObject.Find("White").transform;
            Rook.name = "Rook";
            Rook.tag = tag;
            Rook.GetComponentInChildren<MeshRenderer>().material = WhiteMaterial;
            Rook.GetComponent<PieceScript>().Position = Position;
        }
        if (tag == "Black")
        {
            GameObject Rook = Instantiate(RookPrefab) as GameObject;
            Rook.transform.parent = GameObject.Find("Black").transform;
            Rook.name = "Rook";
            Rook.tag = tag;
            Rook.GetComponentInChildren<MeshRenderer>().material = BlackMaterial;
            Rook.GetComponent<PieceScript>().Position = Position;
        }
    }
    void AddQueen(string tag, Vector2Int Position)
    {
        if (tag == "White")
        {
            GameObject Queen = Instantiate(QueenPrefab) as GameObject;
            Queen.transform.parent = GameObject.Find("White").transform;
            Queen.name = "Queen";
            Queen.tag = tag;
            Queen.GetComponentInChildren<MeshRenderer>().material = WhiteMaterial;
            Queen.GetComponent<PieceScript>().Position = Position;
        }
        if (tag == "Black")
        {
            GameObject Queen = Instantiate(QueenPrefab) as GameObject;
            Queen.transform.parent = GameObject.Find("Black").transform;
            Queen.name = "Queen";
            Queen.tag = tag;
            Queen.GetComponentInChildren<MeshRenderer>().material = BlackMaterial;
            Queen.GetComponent<PieceScript>().Position = Position;
        }
    }
    #endregion

    //Functionality for Online Play
    #region
    [PunRPC]
    public void PopulateBoardOnline()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        if (gameManager.Connectivity == "Online")
        {
            Debug.Log("In online Mode");

            if (PhotonNetwork.IsMasterClient)
            {
                if (gameManager.gameType == "NormalMatch")
                {
                    //pv.RPC("AddPawnsOnline", RpcTarget.All);
                   /* pv.RPC("AddRookOnline", RpcTarget.All, ("White", new Vector2Int(0, 0)));
                    pv.RPC("AddRookOnline", RpcTarget.All, ("White", new Vector2Int(7, 0)));
                    pv.RPC("AddKnightOnline", RpcTarget.All, ("White", new Vector2Int(1, 0)));
                    pv.RPC("AddKnightOnline", RpcTarget.All, ("White", new Vector2Int(6, 0)));
                    pv.RPC("AddBishopOnline", RpcTarget.All, ("White", new Vector2Int(2, 0)));
                    pv.RPC("AddBishopOnline", RpcTarget.All, ("White", new Vector2Int(5, 0)));
                    pv.RPC("AddKingOnline", RpcTarget.All, ("White", new Vector2Int(3, 0)));
                    pv.RPC("AddQueenOnline", RpcTarget.All, ("White", new Vector2Int(4, 0)));

                    pv.RPC("AddRookOnline", RpcTarget.All, ("Black", new Vector2Int(0, gameManager.boardSize - 1)));
                    pv.RPC("AddRookOnline", RpcTarget.All, ("Black", new Vector2Int(7, gameManager.boardSize - 1)));
                    pv.RPC("AddKnightOnline", RpcTarget.All, ("Black", new Vector2Int(1, gameManager.boardSize - 1)));
                    pv.RPC("AddKnightOnline", RpcTarget.All, ("Black", new Vector2Int(6, gameManager.boardSize - 1)));
                    pv.RPC("AddBishopOnline", RpcTarget.All, ("Black", new Vector2Int(2, gameManager.boardSize - 1)));
                    pv.RPC("AddBishopOnline", RpcTarget.All, ("Black", new Vector2Int(5, gameManager.boardSize - 1)));
                    pv.RPC("AddKingOnline", RpcTarget.All, ("Black", new Vector2Int(4, gameManager.boardSize - 1)));
                    pv.RPC("AddQueenOnline", RpcTarget.All, ("Black", new Vector2Int(3, gameManager.boardSize - 1)));*/
                }

                if (gameManager.gameType == "SpeedMatch")
                {
                    Debug.Log("Attempting to instantiate");
                    //pv.RPC("AddPawnsOnline", RpcTarget.All);
                    /*pv.RPC("AddRook", RpcTarget.All, ("White", new Vector2Int(0, 0)));
                    pv.RPC("AddRook", RpcTarget.All, ("White", new Vector2Int(5, 0)));
                    pv.RPC("AddKnight", RpcTarget.All, ("White", new Vector2Int(1, 0)));
                    pv.RPC("AddKnight", RpcTarget.All, ("White", new Vector2Int(4, 0)));
                    pv.RPC("AddKing", RpcTarget.All, ("White", new Vector2Int(3, 0)));
                    pv.RPC("AddQueen", RpcTarget.All, ("White", new Vector2Int(2, 0)));

                    pv.RPC("AddRook", RpcTarget.All, ("Black", new Vector2Int(0, gameManager.boardSize - 1)));
                    pv.RPC("AddRook", RpcTarget.All, ("Black", new Vector2Int(5, gameManager.boardSize - 1)));
                    pv.RPC("AddKnight", RpcTarget.All, ("Black", new Vector2Int(1, gameManager.boardSize - 1)));
                    pv.RPC("AddKnight", RpcTarget.All, ("Black", new Vector2Int(4, gameManager.boardSize - 1)));
                    pv.RPC("AddKing", RpcTarget.All, ("Black", new Vector2Int(2, gameManager.boardSize - 1)));
                    pv.RPC("AddQueen", RpcTarget.All, ("Black", new Vector2Int(3, gameManager.boardSize - 1)));*/
                }
            }
        }
    }
    
    [PunRPC]
    void AddPawnsOnline()
    {
        Debug.Log("Calling add pawns");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        if (gameManager.Connectivity == "Online")
        {
            Debug.Log("Trying to instantiate pieces, Online");
        }
        if (PhotonNetwork.IsMasterClient)
        {
            // Adds White Pawns
            for (int i = 0; i < gameManager.boardSize; i++)
            {
                GameObject Pawn = PhotonNetwork.Instantiate(PawnPrefab.name, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                Pawn.transform.parent = GameObject.Find("White").transform;
                Pawn.name = "Pawn";
                Pawn.tag = "White";
                Pawn.GetComponentInChildren<MeshRenderer>().material = WhiteMaterial;
                Pawn.GetComponent<PieceScript>().Position = new Vector2Int(i, 1);
            }

            // Adds Black Pawns
            for (int j = 0; j < gameManager.boardSize; j++)
            {
                GameObject Pawn = PhotonNetwork.Instantiate(PawnPrefab.name, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                Pawn.transform.parent = GameObject.Find("Black").transform;
                Pawn.name = "Pawn";
                Pawn.tag = "Black";
                Pawn.GetComponentInChildren<MeshRenderer>().material = BlackMaterial;
                Pawn.GetComponent<PieceScript>().Position = new Vector2Int(j, gameManager.boardSize - 2);
            }
        }
    }
    [PunRPC]
    void AddKnightOnline(string tag, Vector2Int Position)
    {
        if(tag == "White")
        {
            GameObject Knight = Instantiate(KnightPrefab) as GameObject;
            Knight.transform.parent = GameObject.Find("White").transform;
            Knight.name = "Knight";
            Knight.tag = tag;
            Knight.GetComponentInChildren<MeshRenderer>().material = WhiteMaterial;
            Knight.GetComponent<PieceScript>().Position = Position;
        }
        if(tag == "Black")
        {
            GameObject Knight = Instantiate(KnightPrefab) as GameObject;
            Knight.transform.parent = GameObject.Find("Black").transform;
            Knight.name = "Knight";
            Knight.tag = tag;
            Knight.gameObject.transform.eulerAngles = new Vector3(Knight.transform.eulerAngles.x, Knight.transform.eulerAngles.y - 180f, Knight.transform.eulerAngles.z);
            Knight.GetComponentInChildren<MeshRenderer>().material = BlackMaterial;
            Knight.GetComponent<PieceScript>().Position = Position;
        }
    }
    [PunRPC]
    void AddKingOnline(string tag, Vector2Int Position)
    {
        if(tag == "White")
        {
            GameObject King = Instantiate(KingPrefab) as GameObject;
            King.transform.parent = GameObject.Find("White").transform;
            King.name = "King";
            King.tag = tag;
            King.GetComponentInChildren<MeshRenderer>().material = WhiteMaterial;
            King.GetComponent<PieceScript>().Position = Position;
        }
        if (tag == "Black")
        {
            GameObject King = Instantiate(KingPrefab) as GameObject;
            King.transform.parent = GameObject.Find("Black").transform;
            King.name = "King";
            King.tag = tag;
            King.GetComponentInChildren<MeshRenderer>().material = BlackMaterial;
            King.GetComponent<PieceScript>().Position = Position;
        }
    }
    [PunRPC]
    void AddBishopOnline(string tag, Vector2Int Position)
    {
        if(tag == "White")
        {
            GameObject Bishop = Instantiate(BishopPrefab) as GameObject;
            Bishop.transform.parent = GameObject.Find("White").transform;
            Bishop.name = "Bishop";
            Bishop.tag = tag;
            Bishop.GetComponentInChildren<MeshRenderer>().material = WhiteMaterial;
            Bishop.GetComponent<PieceScript>().Position = Position;
        }
        if(tag == "Black")
        {
            GameObject Bishop = Instantiate(BishopPrefab) as GameObject;
            Bishop.transform.parent = GameObject.Find("Black").transform;
            Bishop.name = "Bishop";
            Bishop.tag = tag;
            Bishop.GetComponentInChildren<MeshRenderer>().material = BlackMaterial;
            Bishop.GetComponent<PieceScript>().Position = Position;
        }
    }
    [PunRPC]
    void AddRookOnline(string tag, Vector2Int Position)
    {
        if (tag == "White")
        {
            GameObject Rook = Instantiate(RookPrefab) as GameObject;
            Rook.transform.parent = GameObject.Find("White").transform;
            Rook.name = "Rook";
            Rook.tag = tag;
            Rook.GetComponentInChildren<MeshRenderer>().material = WhiteMaterial;
            Rook.GetComponent<PieceScript>().Position = Position;
        }
        if (tag == "Black")
        {
            GameObject Rook = Instantiate(RookPrefab) as GameObject;
            Rook.transform.parent = GameObject.Find("Black").transform;
            Rook.name = "Rook";
            Rook.tag = tag;
            Rook.GetComponentInChildren<MeshRenderer>().material = BlackMaterial;
            Rook.GetComponent<PieceScript>().Position = Position;
        }
    }
    [PunRPC]
    void AddQueenOnline(string tag, Vector2Int Position)
    {
        if (tag == "White")
        {
            GameObject Queen = Instantiate(QueenPrefab) as GameObject;
            Queen.transform.parent = GameObject.Find("White").transform;
            Queen.name = "Queen";
            Queen.tag = tag;
            Queen.GetComponentInChildren<MeshRenderer>().material = WhiteMaterial;
            Queen.GetComponent<PieceScript>().Position = Position;
        }
        if (tag == "Black")
        {
            GameObject Queen = Instantiate(QueenPrefab) as GameObject;
            Queen.transform.parent = GameObject.Find("Black").transform;
            Queen.name = "Queen";
            Queen.tag = tag;
            Queen.GetComponentInChildren<MeshRenderer>().material = BlackMaterial;
            Queen.GetComponent<PieceScript>().Position = Position;
        }
    }
    #endregion
}
