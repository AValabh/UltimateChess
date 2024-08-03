using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviourPunCallbacks
{
    // Camera 
    public GameObject CameraRotator;
    public float CameraRotateSpeed;
    public float CameraRotateTime;
    // game type holder
    public string gameType;
    // current players turn
    public string playerTurn;
    // Holder for selected piece
    public GameObject selectedPiece;
    // Holder for board size
    public int boardSize;
    // Player variables
    public string Player1 = "White";
    public string Player2 = "Black";
    public string turn;
    public int turnCounter;
    public bool makingMove;
    public GameObject SelectedPiece;
    // Scripts accessed
    PositionScript positionScript;
    BoardScript board;
    // Online gameplay variables
    public string Connectivity;
    public string PlayerColour;
    public GameObject PlayerObject;
    public GameObject PlayerContainer;
    // Tile Affected In Turn
    [HideInInspector] public GameObject TileAffected;
    // Photon View
    public PhotonView pv;
    // Variable for random skill generation
    int skillNo;
    // Container for players in room
    public Player[] players;
    // Check if game is special match
    public bool isSpecialMatch = false;
    // Online Player Variables
    public GameObject Player1Info;
    public GameObject Player2Info;
    public TMP_Text Player1Name;
    public TMP_Text Player2Name;
    // End of game variables
    public bool isGameover = false;
    public GameObject GameOverScreen;
    public TMP_Text WinnerName;

    // Start is called before the first frame update
    void Start()
    {
        // Check if game is Online or Offline
        if (PhotonNetwork.IsConnected)
        {
            Connectivity = "Online";
            pv = this.transform.GetComponent<PhotonView>();
            CameraRotator.transform.GetComponent<RotateCam>().RotateCamera = true;
        }
        else { Connectivity = "Offline"; };

        // Distinguishes between if game is online or offline and starts process
        // Online
        #region
        if (Connectivity == "Online")
        {
            turnCounter = 0;
            Debug.Log("Calling Update Positions, from game manager");
            Debug.Log("Start function in Gamemanager called;");

            // Set Scripts that will be used
            positionScript = GameObject.Find("Board").GetComponent<PositionScript>();
            board = GameObject.Find("Board").GetComponent<BoardScript>();

            // populate board
            board.pv.RPC("PopulateBoardOnline", RpcTarget.All);

            // Update board positions
            positionScript.pv.RPC("UpdatePositionsOnline", RpcTarget.All);
            positionScript.pv.RPC("SetSpecialSkillsOfOnline", RpcTarget.All);

            // Check to see which player is master client and which is second player, then assign all required variables and information
            if (PhotonNetwork.IsMasterClient)
            {
                PlayerColour = "White";
                GameObject PlayerSpawned1 = PhotonNetwork.Instantiate(this.PlayerObject.name, new Vector3(0, 0, 0), Quaternion.identity);
                PlayerSpawned1.transform.parent = PlayerContainer.transform;
                PlayerSpawned1.GetComponent<PlayerController>().PlayerNumber = "Player 1";
                PlayerSpawned1.GetComponent<PlayerController>().PlayerColour = "White";
                Player1Name.text = PhotonNetwork.MasterClient.NickName;
                Player2Name.text = PhotonNetwork.CurrentRoom.GetPlayer(2).NickName;
                Debug.LogError("Player 1 Name : " + Player1Name.text);
                Debug.LogError("Player 2 Name : " + Player2Name.text);
                Player2Info.SetActive(false);
            }
            else
            {
                PlayerColour = "Black";
                GameObject PlayerSpawned2 = PhotonNetwork.Instantiate(this.PlayerObject.name, new Vector3(0, 0, 0), Quaternion.identity);
                PlayerSpawned2.transform.parent = PlayerContainer.transform;
                PlayerSpawned2.GetComponent<PlayerController>().PlayerNumber = "Player 2";
                PlayerSpawned2.GetComponent<PlayerController>().PlayerColour = "Black";
                Player1Name.text = PhotonNetwork.MasterClient.NickName;
                Player2Name.text = PhotonNetwork.CurrentRoom.GetPlayer(2).NickName;
                Debug.LogError("Player 1 Name : " + Player1Name.text);
                Debug.LogError("Player 2 Name : " + Player2Name.text);
                Player2Info.SetActive(false);
            }
            // Get list of players
            players = PhotonNetwork.PlayerList;
            // Start turn controller
            pv.RPC("TurnControllerOnline", RpcTarget.All);
            // Set tiles to position script
            for (int i = 0; i < positionScript.tiles.Length; i++)
            {
                positionScript.tiles[i].GetComponent<TileScript>().pv.RPC("UpdateTileInfoOnline", RpcTarget.All);
            }
            // Set Gameover notification off
            GameOverScreen.SetActive(false);
        }
        #endregion
        // Offline
        #region
        if (Connectivity == "Offline")
        {
            turnCounter = 0;
            Debug.Log("Calling Update Positions, from game manager");
            Debug.Log("Start function in Gamemanager called;");

            // Set Scripts that will be used
            positionScript = GameObject.Find("Board").GetComponent<PositionScript>();
            board = GameObject.Find("Board").GetComponent<BoardScript>();
            // Populates Board
            board.PopulateBoard();
            // Update board positions
            positionScript.UpdatePositions();
            positionScript.SetSpecialSkillsOff();
            // Start turn controller
            TurnController();
            // Set tiles to position script
            for (int i = 0; i < positionScript.tiles.Length; i++)
            {
                positionScript.tiles[i].GetComponent<TileScript>().UpdateTileInfo();
            }
            GameOverScreen.SetActive(false);
        }
        #endregion
    }

    // Update
    void Update()
    {
        // Controls What player can do in their turn
        // check to see if raycast hits tile
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Controls player moves in offline state
        if(Connectivity == "Offline")
        {
            if (Physics.Raycast(ray, out var hit))
            {
                // if tile hit
                if (hit.transform.tag == "Tile")
                {
                    // if tiles hovered on, while occupied, with a piece who's turn it is.
                    if (hit.transform.GetComponent<TileScript>().tileHoveredOn == true)
                    {
                        // check for mouse click
                        if (Input.GetMouseButtonDown(0))
                        {
                            // Update tiles info
                            hit.transform.GetComponent<TileScript>().UpdateTileInfo();
                            Debug.Log("Clicked " + hit.transform.name);
                            // check if tiles occupied and if the turn colour matches piece colour
                            if (hit.transform.GetComponent<TileScript>().occupied == true && hit.transform.GetComponent<TileScript>().pieceOnTile.gameObject.tag == turn && makingMove == false)
                            {
                                hit.transform.GetComponent<TileScript>().isSelected();
                                hit.transform.GetComponent<TileScript>().pieceOnTile.transform.GetComponent<PieceScript>().CheckPossibleMoves(hit.transform.GetComponent<TileScript>().pieceOnTile.name, turn);
                                selectedPiece = hit.transform.GetComponent<TileScript>().pieceOnTile;
                            }
                        }
                    }
                    if (hit.transform.GetComponent<TileScript>().tileIsPossibleMove == true && makingMove == true)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            // Check to see if theres a piece on tile
                            if (hit.transform.GetComponent<TileScript>().occupied == true && hit.transform.GetComponent<TileScript>().pieceOnTile.tag != turn)
                            {
                                // Destroy piece occupying tile which is a possible move
                                Debug.Log("Destroying " + hit.transform.GetComponent<TileScript>().pieceOnTile.tag + " " + hit.transform.GetComponent<TileScript>().pieceOnTile.name);
                                if (hit.transform.GetComponent<TileScript>().pieceOnTile.name == "King")
                                {
                                    GameOver(turn);
                                }
                                Destroy(hit.transform.GetComponent<TileScript>().pieceOnTile);
                            }
                            Debug.Log("Clicked Possible Move" + hit.transform.name);
                            selectedPiece.GetComponent<PieceScript>().Position = hit.transform.GetComponent<TileScript>().Position;
                            positionScript.UpdatePositions();
                            selectedPiece.GetComponent<PieceScript>().EndOfTurn();
                            TileAffected = hit.transform.gameObject;
                            //selectedPiece.GetComponent<PieceScript>().EndOfTurn();
                        }
                        if (Input.GetMouseButtonUp(0))
                        {
                            TurnEnd();
                        }
                    }
                }
            }
        }

        // Controls player moves in online state
        if (Connectivity == "Online" && isGameover == false)
        {
            if (Physics.Raycast(ray, out var hit))
            {
                // if tile hit
                if (hit.transform.tag == "Tile" && (turn == PlayerColour))
                {
                    // if tiles hovered on, while occupied, with a piece who's turn it is.
                    if (hit.transform.GetComponent<TileScript>().tileHoveredOn == true)
                    {
                        // check for mouse click
                        if (Input.GetMouseButtonDown(0))
                        {
                            // Update tiles info
                            hit.transform.GetComponent<TileScript>().UpdateTileInfo();
                            Debug.Log("Clicked " + hit.transform.name);
                            // check if tiles occupied and if the turn colour matches piece colour
                            if (hit.transform.GetComponent<TileScript>().occupied == true && hit.transform.GetComponent<TileScript>().pieceOnTile.gameObject.tag == turn && makingMove == false)
                            {
                                hit.transform.GetComponent<TileScript>().isSelected();
                                hit.transform.GetComponent<TileScript>().pieceOnTile.transform.GetComponent<PieceScript>().CheckPossibleMoves(hit.transform.GetComponent<TileScript>().pieceOnTile.name, turn);
                                selectedPiece = hit.transform.GetComponent<TileScript>().pieceOnTile;
                            }
                        }
                    }
                    if (hit.transform.GetComponent<TileScript>().tileIsPossibleMove == true && makingMove == true)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            // Update tiles info
                            //hit.transform.GetComponent<TileScript>().UpdateTileInfo();
                            // Check to see if theres a piece on tile
                            if (hit.transform.GetComponent<TileScript>().occupied == true && hit.transform.GetComponent<TileScript>().pieceOnTile.tag != turn)
                            {
                                if(hit.transform.GetComponent<TileScript>().pieceOnTile.name == "King")
                                {
                                    // destroy object, pause game, set win screen active
                                    // Destroy piece occupying tile which is a possible move
                                    Debug.Log("Destroying " + hit.transform.GetComponent<TileScript>().pieceOnTile.tag + " " + hit.transform.GetComponent<TileScript>().pieceOnTile.name);
                                    pv.RPC("DestroyObject", RpcTarget.All, hit.transform.GetComponent<TileScript>().pieceOnTile.transform.GetComponent<PhotonView>().ViewID);
                                    pv.RPC("MatchEnd", RpcTarget.All, turn);
                                }
                                // Destroy piece occupying tile which is a possible move
                                Debug.Log("Destroying " + hit.transform.GetComponent<TileScript>().pieceOnTile.tag + " " + hit.transform.GetComponent<TileScript>().pieceOnTile.name);
                                pv.RPC("DestroyObject", RpcTarget.All, hit.transform.GetComponent<TileScript>().pieceOnTile.transform.GetComponent<PhotonView>().ViewID);
                                //Destroy(hit.transform.GetComponent<TileScript>().pieceOnTile.gameObject);
                            }
                            Debug.Log("Clicked Possible Move" + hit.transform.name);
                            selectedPiece.GetComponent<PieceScript>().Position = hit.transform.GetComponent<TileScript>().Position;
                            selectedPiece.GetComponent<PieceScript>().pv.RPC("PieceNewPositionOnline", RpcTarget.All, hit.transform.GetComponent<TileScript>().Position.x, hit.transform.GetComponent<TileScript>().Position.y);
                            positionScript.pv.RPC("UpdatePositionsOnline", RpcTarget.All);
                            selectedPiece.GetComponent<PieceScript>().pv.RPC("EndOfTurnOnline",RpcTarget.All);
                            TileAffected = hit.transform.gameObject;
                        }
                        if (Input.GetMouseButtonUp(0))
                        {
                            skillNo = Random.Range(1, 3);
                            pv.RPC("TurnEndOnline", RpcTarget.All, skillNo);
                        }
                    }
                }
            }
        }
    }
    // Functions for Game Over Buttons
    public void OnReturnToMainMenu()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        SceneManager.LoadScene("HomeScene");
    }
    public void OnReturnToLobby()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }
    // Functions for Online Play
    #region
    [PunRPC]
    void MatchEnd(string playerColour)
    {
        // functionality for end of match
        isGameover = true;
        GameOverScreen.SetActive(true);
        if(playerColour == "White")
        {
            WinnerName.text = Player1Name.text;
        }
        if(playerColour == "Black")
        {
            WinnerName.text = Player2Name.text;
        }
    }
    [PunRPC]
    void TurnEndOnline(int skillNo)
    {
        if(Connectivity == "Online")
        {
            positionScript.pv.RPC("UpdatePositionsOnline", RpcTarget.All);
            SelectedPiece = null;
            makingMove = false;
            // Reset all tiles
            for (int i = 0; i < positionScript.tiles.Length; i++)
            {
                positionScript.tiles[i].GetComponent<TileScript>().pv.RPC("ResetTileOnline", RpcTarget.All);
                positionScript.tiles[i].GetComponent<TileScript>().pv.RPC("UpdateTileInfoOnline", RpcTarget.All);
            }
            for (int i = 0; i < positionScript.blackPieces.Length; i++)
            {
                positionScript.blackPieces[i].GetComponent<PieceScript>().pv.RPC("ResetPieceOnline", RpcTarget.All);
            }
            for (int i = 0; i < positionScript.whitePieces.Length; i++)
            {
                positionScript.whitePieces[i].GetComponent<PieceScript>().pv.RPC("ResetPieceOnline", RpcTarget.All);
            }
            turnCounter++;

            // Activate a special tile with a 33% chance
            if (isSpecialMatch == true)
            {
                int chance = Random.Range(0,3);
                if (chance == 2)
                {

                    Debug.Log("Calling Special Tile");
                    positionScript.pv.RPC("ActivateSpecialTileOnline", RpcTarget.All, skillNo);
                }

            }
            pv.RPC("TurnControllerOnline",RpcTarget.All);
            CameraRotator.transform.GetComponent<RotateCam>().pv.RPC("TurnColour", RpcTarget.All,turn);
        }

    }
    [PunRPC]
    void TurnControllerOnline()
    {
        if (turnCounter % 2 == 0)
        {
            turn = "White";
            Player1Info.SetActive(true);
            Player2Info.SetActive(false);
            PhotonNetwork.SetMasterClient(players[0]);
        }
        if (turnCounter % 2 != 0)
        {
            turn = "Black";
            Player2Info.SetActive(true);
            Player1Info.SetActive(false);
            PhotonNetwork.SetMasterClient(players[1]);
        }
    }
    [PunRPC]
    void DestroyObject(int viewID)
    {
        // Check if King is Destroyed
        if(PhotonView.Find(viewID).gameObject.name == "King")
        {
            if(PhotonView.Find(viewID).gameObject.tag == "White")
            {
                // Game winner is black
            }
            if (PhotonView.Find(viewID).gameObject.tag == "Black")
            {
                // Game winner is white
            }
        }
        Destroy(PhotonView.Find(viewID).gameObject);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(isGameover == false)
        {
            if (otherPlayer.NickName == Player1Name.text)
            {
                pv.RPC("MatchEnd", RpcTarget.All, "Black");
            }
            if (otherPlayer.NickName == Player2Name.text)
            {
                pv.RPC("MatchEnd", RpcTarget.All, "White");
            }
        }
    }
    #endregion

    // Functions for Offline Play
    #region
    void TurnEnd()
    {
        positionScript.UpdatePositions();
        SelectedPiece = null;
        makingMove = false;
        // Reset all tiles
        for (int i = 0; i < positionScript.tiles.Length; i++)
        {
            positionScript.tiles[i].GetComponent<TileScript>().ResetTile();
            positionScript.tiles[i].GetComponent<TileScript>().UpdateTileInfo();
        }
        for (int i = 0; i < positionScript.blackPieces.Length; i++)
        {
            positionScript.blackPieces[i].GetComponent<PieceScript>().ResetPiece();
        }
        for (int i = 0; i < positionScript.whitePieces.Length; i++)
        {
            positionScript.whitePieces[i].GetComponent<PieceScript>().ResetPiece();
        }
        turnCounter++;
        TurnController();
        CameraRotator.transform.GetComponent<RotateCam>().RotateCamera = true;
        CameraRotator.transform.GetComponent<RotateCam>().turn = turn;
        // Activate a special tile with a 33% chance
        if(isSpecialMatch == true)
        {
            int chance = Random.Range(1, 3);
            if (chance == 2)
            {
                Debug.Log("Calling Special Tile");
                positionScript.ActivateSpecialTile();
            }

        }
    }

    void TurnController()
    {
        if (turnCounter % 2 == 0)
        {
            turn = "White";
        }
        if (turnCounter % 2 != 0)
        {
            turn = "Black";
        }
    }

    void GameOver(string turn)
    {
        if(turn == "White")
        {
            GameOverScreen.SetActive(true);
            WinnerName.text = "White";
        }
        if (turn == "Black")
        {
            GameOverScreen.SetActive(true);
            WinnerName.text = "Black";
        }
    }
    #endregion
}
