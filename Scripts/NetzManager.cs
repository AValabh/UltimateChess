using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetzManager : MonoBehaviourPunCallbacks
{
    public static NetzManager instance;

    private void Awake()
    {
        // Create an NetManager instance
        instance = this;
        // Do not destroy while swapping scenes
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        // Connect user to Server
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateOrJoinRoom()
    {
        Debug.Log("Called Create or Join Room");


        if(PhotonNetwork.CountOfRooms > 0)
        {
            Debug.Log("Joining a random room");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("Creating a room");
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2;

            //creates room
            PhotonNetwork.CreateRoom(null, options);
  
        }
    }

    // contols changing of scene
    public void ChangeScene(string SceneName)
    {
        PhotonNetwork.LoadLevel(SceneName);
    }
}
