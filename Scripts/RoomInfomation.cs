using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomInfomation : MonoBehaviourPunCallbacks
{
    public TMP_Text P1Name;
    public TMP_Text P2Name;
    public TMP_Text PlayerCount;
    // called on OnJoinedRoom
    public void SetRoomInfo()
    { 
        // Display Player Count
        PlayerCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        // If Player is only Player in room set player 1 nickname to players nickname
        if(PlayerCount.text == "1")
        {
            Debug.Log("Player 1 has entered room - SetRoomInfo");
            P1Name.text = PhotonNetwork.CurrentRoom.GetPlayer(1).NickName;
            P2Name.text = "Connecting...";
        }
        // if player is second player in room set player 2 nickname
        if(PlayerCount.text == "2")
        {
            Debug.Log("Player 2 has entered room - SetRoomInfo");
            // If player one is master client join room as second player
            if (PhotonNetwork.CurrentRoom.GetPlayer(1).IsMasterClient)
            {
                Debug.Log("Joining room as second player - SetRoomInfo");
                P1Name.text = PhotonNetwork.CurrentRoom.GetPlayer(1).NickName;
                P2Name.text = PhotonNetwork.PlayerList[1].NickName;
            }
        }
    }

    // When a new player enters room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Display Player Count
        PlayerCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();

            // If player two has left room and new player joins they take player ones spot
            if (P2Name.text == "Connecting...")
            {
                Debug.Log("Replacing Player 2 - OnPlayerEnteredRoom");
                P2Name.text = newPlayer.NickName;
                P1Name.text = PhotonNetwork.CurrentRoom.GetPlayer(1).NickName;
            }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        // Set Player 2 name to Connecting
        P2Name.text = "Connecting...";
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonNetwork.LeaveRoom();
    }
}
