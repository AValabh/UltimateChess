using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TileScript : MonoBehaviourPunCallbacks
{
    [Header("Tile Information")]
    // tiles position in grid
    public Vector2Int Position;
    // check if tile has a piece
    public bool occupied;
    public bool occupiedBlack;
    public bool occupiedWhite;
    // check to see if hovered on
    public bool tileHoveredOn = false;
    // check to see if tile is selected
    public bool tileIsSelected = false;
    // check to see if tile is a possible move
    public bool tileIsPossibleMove = false;
    // holds the gameobject on tile
    public GameObject pieceOnTile;
    // piece intending to come onto tile
    [HideInInspector] public GameObject pieceMovingToOccupiedTile;

    [Header("Materials")]
    // this objects original material
    public Material originalMaterial;
    // this objects hovered material
    public Material hoveredMaterial;
    // this objects selected material
    public Material selectedMaterial;
    // this objects is a possible move material
    public Material possibleMoveMaterial;

    // Holds Positions of white pieces
    private GameObject[] whitePositions;
    // Holds Positions of black pieces
    private GameObject[] blackPositions;

    [HideInInspector] public GameManagerScript gameManager;
    public PhotonView pv;

    // Special Skills
    public GameObject SpecialSkill;
    public List <Vector2> tilesBurn;
    public List <Vector2> tilesLigtning;

    private void Start()
    {
        //Debug.Log("Start function in TileScript called; " + pv.ViewID);
        originalMaterial = this.gameObject.GetComponent<MeshRenderer>().material;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        // get all pieces active
        if(gameManager.Connectivity == "Offline")
        {
            whitePositions = GameObject.Find("Board").GetComponent<PositionScript>().whitePieces;
            blackPositions = GameObject.Find("Board").GetComponent<PositionScript>().blackPieces;
            Debug.Log("Setting Special Skills Off");
        }
        if(gameManager.Connectivity == "Online")
        {
            pv = this.transform.GetComponent<PhotonView>();
            pv.RPC("GetPieces", RpcTarget.All);
        }
    }

    void Update()
    {
        // check to see if raycast hits tile
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            if(hit.transform.name == this.name && tileIsSelected != true && tileIsPossibleMove != true)
            {
                isHovered(); 
            }
            if(hit.transform.name != this.name)
            {
                ResetHovered();
            }
        }

        if(tileIsSelected)
        {
            // sets material to selected material
            this.GetComponent<MeshRenderer>().material = selectedMaterial;
            gameManager.makingMove = true;
        }
        if (tileIsPossibleMove)
        {
            // sets material to possible move material
            this.GetComponent<MeshRenderer>().material = possibleMoveMaterial;
        }
    }

    // Start of turn calls
    public void UpdateTileInfo()
    {
        Debug.Log("Updating Tile Info");
        // get all pieces active
        whitePositions = GameObject.Find("Board").GetComponent<PositionScript>().whitePieces;
        blackPositions = GameObject.Find("Board").GetComponent<PositionScript>().blackPieces;


        // loop through all active white pieces to see if on this tile
        for (int i = 0; i < whitePositions.Length; i++)
        {
            // check if white pieces share position with this tile
            if (this.Position == whitePositions[i].GetComponent<PieceScript>().Position)
            {
                // set tile to occupied
                occupiedWhite = true;
                occupiedBlack = false;
                // store piece on the tile
                pieceOnTile = whitePositions[i];
                if (occupiedWhite == true)
                {
                    break;
                }
            }
            else { occupiedWhite = false; }
        }
        // loop through all active black pieces to see if on this tile
        for (int i = 0; i < blackPositions.Length; i++)
        {
            // check if black pieces share position with this tile
            if (this.Position == blackPositions[i].GetComponent<PieceScript>().Position)
            {
                // set tile to occupied
                occupiedBlack = true;
                occupiedWhite = false;
                // store piece on the tile
                pieceOnTile = blackPositions[i];
                if (occupiedBlack == true)
                {
                    break;
                }
            }
            else { occupiedBlack = false; }
        }
        // sets occupied if any piece on tile
        if (occupiedWhite || occupiedBlack)
        {
            occupied = true;
        }
        else { occupied = false; }
    }
    // Set Special skill inactive
    public void SetSpecialSkillOff()
    {
        SpecialSkill.SetActive(false);
    }
    // functionality while hovered on
    public void isHovered()
    {
        // sets material to hovered material
        this.GetComponent<MeshRenderer>().material = hoveredMaterial;
        tileHoveredOn = true;
    }
 
    // functionality while tile is selected
    public void isSelected()
    {
        tileIsSelected = true;
        // sets material to selected material
        this.GetComponent<MeshRenderer>().material = selectedMaterial;
        gameManager.makingMove = true;
    }

    // functionality if tile is a possible move
    public void isPossibleMove()
    {
        tileIsPossibleMove = true;
        // sets material to possible move material
        this.GetComponent<MeshRenderer>().material = possibleMoveMaterial;
    }

    // functionality to reset Hovered tile
    public void ResetHovered()
    {
        // sets tile to default material
        this.GetComponent<MeshRenderer>().material = originalMaterial;
        // sets all indicators to false
        tileHoveredOn = false;
    }

    // functionality to reset tile
    public void ResetTile()
    {
        // sets tile to default material
        this.GetComponent<MeshRenderer>().material = originalMaterial;
        // sets all indicators to false
        tileHoveredOn = false;
        tileIsSelected = false;
        tileIsPossibleMove = false;
    }
    //-----------------------------------------------------------------------------------------------------
    // Functionality for skill effect
    public void SkillEffect(string SkillType, string DestroyObjectsTag)
    {
        if(SkillType == "x2")
        {
            //skip turn of DestroyObjects Tag
        }
        if(SkillType == "Fire")
        {
            // Destroy gameobjects of tag in a + shape aside from king
            tilesBurn.Add( new Vector2 (Position.x + 1, Position.y));
            tilesBurn.Add(new Vector2(Position.x , Position.y + 1));
            tilesBurn.Add(new Vector2(Position.x , Position.y - 1));
            tilesBurn.Add(new Vector2(Position.x - 1, Position.y));
            DestroyFromSkill(tilesBurn, DestroyObjectsTag);
        }
        if(SkillType == "Lightning")
        {
            // Destroy gameobjects of tag in x shape aside from king
            tilesLigtning.Add(new Vector2(Position.x - 1, Position.y - 1));
            tilesLigtning.Add(new Vector2(Position.x - 1, Position.y + 1));
            tilesLigtning.Add(new Vector2(Position.x + 1, Position.y + 1));
            tilesLigtning.Add(new Vector2(Position.x + 1, Position.y - 1));
            DestroyFromSkill(tilesLigtning, DestroyObjectsTag);
        }
    }

    // Find pieces from skill, requires positions and tag of colour to destroy
    public void DestroyFromSkill(List<Vector2> positionsOfTiles, string DestroyObjectsOfTag)
    {
        PositionScript positionScript = GameObject.Find("Board").GetComponent<PositionScript>();
        GameObject[] tilesInScene = positionScript.tiles;
        // Check each tile
        foreach(GameObject tile in tilesInScene)
        {
            // Check each positon
            foreach(Vector2 positions in positionsOfTiles)
            {
                // if positions match
                if (tile.GetComponent<TileScript>().Position == positions)
                {
                    // if conditions met
                    if(tile.GetComponent<TileScript>().occupied == true && tile.GetComponent<TileScript>().pieceOnTile.name != "King" && tile.GetComponent<TileScript>().pieceOnTile.tag == DestroyObjectsOfTag)
                    {
                        Destroy(tile.GetComponent<TileScript>().pieceOnTile);
                    }
                }
            }
        }
    }
    //------------------------------------------------------------------------------------------------------
    // Start of turn calls
    [PunRPC]
    public void UpdateTileInfoOnline()
    {
        Debug.Log("Updating Tile Info");
        // get all pieces active
        whitePositions = GameObject.Find("Board").GetComponent<PositionScript>().whitePieces;
        blackPositions = GameObject.Find("Board").GetComponent<PositionScript>().blackPieces;


        // loop through all active white pieces to see if on this tile
        for (int i = 0; i < whitePositions.Length; i++)
        {
            // check if white pieces share position with this tile
            if (this.Position == whitePositions[i].GetComponent<PieceScript>().Position)
            {
                // set tile to occupied
                occupiedWhite = true;
                occupiedBlack = false;
                // store piece on the tile
                pieceOnTile = whitePositions[i];
                if (occupiedWhite == true)
                {
                    break;
                }
            }
            else { occupiedWhite = false; }
        }
        // loop through all active black pieces to see if on this tile
        for (int i = 0; i < blackPositions.Length; i++)
        {
            // check if black pieces share position with this tile
            if (this.Position == blackPositions[i].GetComponent<PieceScript>().Position)
            {
                // set tile to occupied
                occupiedBlack = true;
                occupiedWhite = false;
                // store piece on the tile
                pieceOnTile = blackPositions[i];
                if (occupiedBlack == true)
                {
                    break;
                }
            }
            else { occupiedBlack = false; }
        }
        // sets occupied if any piece on tile
        if (occupiedWhite || occupiedBlack)
        {
            occupied = true;
        }
        else { occupied = false; }
    }

    // functionality to reset tile
    [PunRPC]
    public void ResetTileOnline()
    {
        // sets tile to default material
        this.GetComponent<MeshRenderer>().material = originalMaterial;
        // sets all indicators to false
        tileHoveredOn = false;
        tileIsSelected = false;
        tileIsPossibleMove = false;
    }

    // Get White and Black Pieces
    [PunRPC]
    public void GetPieces()
    {
        whitePositions = GameObject.Find("Board").GetComponent<PositionScript>().whitePieces;
        blackPositions = GameObject.Find("Board").GetComponent<PositionScript>().blackPieces;
    }
    // Set Special skill inactive
    [PunRPC]
    public void SetSpecialSkillOffOnline()
    {
        SpecialSkill.SetActive(false);
    }
    [PunRPC]
    public void SetSpecialSkillONnOnline()
    {
        SpecialSkill.SetActive(true);
    }
    // Functionality for skill effect
    [PunRPC]
    public void SkillEffectOnline(string SkillType, string DestroyObjectsTag)
    {
        if (SkillType == "x2")
        {
            //skip turn of DestroyObjects Tag
        }
        if (SkillType == "Fire")
        {
            // Destroy gameobjects of tag in a + shape aside from king
            tilesBurn.Add(new Vector2(Position.x + 1, Position.y));
            tilesBurn.Add(new Vector2(Position.x, Position.y + 1));
            tilesBurn.Add(new Vector2(Position.x, Position.y - 1));
            tilesBurn.Add(new Vector2(Position.x - 1, Position.y));
            pv.RPC("DestroyFromSkillOnline",RpcTarget.All, tilesBurn, DestroyObjectsTag);
        }
        if (SkillType == "Lightning")
        {
            // Destroy gameobjects of tag in x shape aside from king
            tilesLigtning.Add(new Vector2(Position.x - 1, Position.y - 1));
            tilesLigtning.Add(new Vector2(Position.x - 1, Position.y + 1));
            tilesLigtning.Add(new Vector2(Position.x + 1, Position.y + 1));
            tilesLigtning.Add(new Vector2(Position.x + 1, Position.y - 1));
            pv.RPC("DestroyFromSkillOnline", RpcTarget.All,tilesLigtning, DestroyObjectsTag);
        }
    }

    // Find pieces from skill, requires positions and tag of colour to destroy
    [PunRPC]
    public void DestroyFromSkillOnline(List<Vector2> positionsOfTiles, string DestroyObjectsOfTag)
    {
        PositionScript positionScript = GameObject.Find("Board").GetComponent<PositionScript>();
        GameObject[] tilesInScene = positionScript.tiles;
        // Check each tile
        foreach (GameObject tile in tilesInScene)
        {
            // Check each positon
            foreach (Vector2 positions in positionsOfTiles)
            {
                // if positions match
                if (tile.GetComponent<TileScript>().Position == positions)
                {
                    // if conditions met
                    if (tile.GetComponent<TileScript>().occupied == true && tile.GetComponent<TileScript>().pieceOnTile.name != "King" && tile.GetComponent<TileScript>().pieceOnTile.tag == DestroyObjectsOfTag)
                    {
                        Destroy(tile.GetComponent<TileScript>().pieceOnTile);
                    }
                }
            }
        }
    }
}
