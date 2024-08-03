using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MultiplayerMenu : MonoBehaviourPunCallbacks
{
    public TMP_InputField PlayerName;
    public TMP_Text ButtonText;
    private void Awake()
    {

    }
    public void OnConnectButton(){
        if(PlayerName.text.Length >= 1)
        {
            // sets playername on the server
            PhotonNetwork.NickName = PlayerName.text;
            // rename button while waiting for server connection
            ButtonText.text = "Connecting..";
            PhotonNetwork.AutomaticallySyncScene = true;
            // connect user to master client
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // On server connection load lobby
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master server - Multiplayer Menu");
        //PhotonNetwork.OpJoinLobby();
        SceneManager.LoadScene("Lobby");
    }
}
