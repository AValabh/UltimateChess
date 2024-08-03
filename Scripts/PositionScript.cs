using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PositionScript : MonoBehaviourPunCallbacks
{
    // holds all white pieces active
    public GameObject[] whitePieces;
    // holds all black pieces active
    public GameObject[] blackPieces;
    // holds all active tiles
    public GameObject[] tiles;
    // game manager
    [HideInInspector] public GameManagerScript gameManager;
    // photon view
    public PhotonView pv;
    // unoccupied tiles
    public List<GameObject> unOccupiedTiles;
    int RandomNumber;

    private void Start()
    {
        Debug.Log("Start function in PositionScript called;");

        // Gets game manager object and script
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();

        // Checks for online status and sets pv
        if (gameManager.Connectivity == "Online")
        {
            pv = this.transform.GetComponent<PhotonView>();
        }
    }

    // Functionality for Online Play
    #region
    // Updates list of all objects in scene
    [PunRPC]
    public void UpdatePositionsOnline()
    {
        if (gameManager.Connectivity == "Online")
        {
            Debug.Log("Updating Posiitions from Position Script");
            // adds all white pieces to array
            whitePieces = GameObject.FindGameObjectsWithTag("White");
            // adds all black pieces to array
            blackPieces = GameObject.FindGameObjectsWithTag("Black");
            // adds all tiles to array
            tiles = GameObject.FindGameObjectsWithTag("Tile");
            // Remove Nulls
            pv.RPC("RemoveNullsWhiteOnline", RpcTarget.All);
            pv.RPC("RemoveNullsBlackOnline", RpcTarget.All);
        }
    }
    // Removes nulls from list
    [PunRPC]
    public void RemoveNullsWhiteOnline()
    {
        // Swap Pieces from array to list
        List<GameObject> whitePiecesList = new List<GameObject>(whitePieces);
        // Remove the nulls from the list
        whitePiecesList.RemoveAll(x => x == null);
        // Convert list back into a array
        whitePieces = whitePiecesList.ToArray();
    }
    [PunRPC]
    public void RemoveNullsBlackOnline()
    {
        // Swap Pieces from array to list
        List<GameObject> blackPiecesList = new List<GameObject>(blackPieces);
        // Remove the nulls from the list
        blackPiecesList.RemoveAll(x => x == null);
        // Convert list back into a array
        blackPieces = blackPiecesList.ToArray();
    }
    [PunRPC]
    public void SetSpecialSkillsOfOnline()
    {
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<TileScript>().pv.RPC("SetSpecialSkillOffOnline",RpcTarget.All);
        }
    }

    [PunRPC]
    public void ActivateSpecialTileOnline(int skillNo)
    {
        int i = 0;
        // for each tile
        foreach (GameObject tilez in tiles)
        {
            // if tile is not occuplied
            if (tilez.GetComponent<TileScript>().occupied == false)
            {
                // add tile to the array
                unOccupiedTiles.Add(tilez);
                i++;
            }
        }
        int specialTileNo = Random.Range(0, i);
        unOccupiedTiles[specialTileNo].GetComponent<TileScript>().pv.RPC("SetSpecialSkillONnOnline", RpcTarget.All);
        // Get random between 1 and 3
        unOccupiedTiles[specialTileNo].GetComponent<TileScript>().SpecialSkill.GetComponent<SpecialSkillController>().pv.RPC("SetSkillOnline", RpcTarget.All, skillNo);
    }

    #endregion

    // Functionality for Offline Play
    #region
    // Updates list of all objects in scene
    public void UpdatePositions()
    {
        Debug.Log("Updating Posiitions from Position Script");
        // adds all white pieces to array
        whitePieces = GameObject.FindGameObjectsWithTag("White");
        // adds all black pieces to array
        blackPieces = GameObject.FindGameObjectsWithTag("Black");
        // adds all tiles to array
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        // Remove Nulls
        RemoveNulls(whitePieces, blackPieces);
    }

    // Removes destroyed object holders in list
    public void RemoveNulls(GameObject[] whitepieces, GameObject[] blackpieces)
    {
        // Swap Pieces from array to list
        List<GameObject> whitePiecesList = new List<GameObject>(whitePieces);
        // Remove the nulls from the list
        whitePiecesList.RemoveAll(x => x == null);
        // Convert list back into a array
        whitePieces = whitePiecesList.ToArray();

        // Swap Pieces from array to list
        List<GameObject> blackPiecesList = new List<GameObject>(blackPieces);
        // Remove the nulls from the list
        blackPiecesList.RemoveAll(x => x == null);
        // Convert list back into a array
        blackPieces = blackPiecesList.ToArray();
    }

    public void SetSpecialSkillsOff()
    {
        foreach(GameObject tile in tiles)
        {
            tile.GetComponent<TileScript>().SetSpecialSkillOff();
        }
    }

    public void ActivateSpecialTile()
    {
        int i = 0;
        // for each tile
        foreach(GameObject tilez in tiles)
        {
            // if tile is not occuplied
            if(tilez.GetComponent<TileScript>().occupied == false)
            {
                // add tile to the array
                unOccupiedTiles.Add(tilez);
                i++;
            }
        }

        int specialTileNo = Random.Range(0, i);
        unOccupiedTiles[specialTileNo].GetComponent<TileScript>().SpecialSkill.SetActive(true);
        unOccupiedTiles[specialTileNo].GetComponent<TileScript>().SpecialSkill.GetComponent<SpecialSkillController>().SetSkill();
    }
    #endregion
}
