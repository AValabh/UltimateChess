using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyScript : MonoBehaviourPunCallbacks
{
    public TMP_InputField RoomNameInput;
    public TMP_Text PlayerName;
    public TMP_Text RoomName;
    public GameObject LobbyMenu;
    public GameObject RoomMenu;
    public RoomItem RoomItemPrefab;
    List<RoomItem> RoomItemsList =  new List<RoomItem>();
    public Transform ContentBlock;
    public float updateTimeInterval = 1.5f;
    float nextUpdateTime;
    public GameObject PlayButton;
    public GameObject MatchSelect;
    public Toggle NormalMatchToggle;
    public Toggle SpeedMatchToggle;
    public void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("Attempting to join lobby - LobbyScript");
            PhotonNetwork.JoinLobby();
        }
        else { Debug.Log("Photon Network is not ready"); }
        NormalMatchToggle.isOn = false;
        SpeedMatchToggle.isOn = false;
        MatchSelect.SetActive(false);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to lobby - LobbyScript");
        PlayerName.text = PhotonNetwork.NickName;
    }

    public void OnCreateRoom()
    {
        // Check that there is a room name
        if(RoomNameInput.text.Length >= 1)
        {
            // Create a room with 2 players max
            PhotonNetwork.CreateRoom(RoomNameInput.text, new RoomOptions() { MaxPlayers = 2 , CleanupCacheOnLeave = false});
            Debug.Log("Room Created");

        }
    }

    // called when succesfully joined room
    public override void OnJoinedRoom()
    {
        LobbyMenu.SetActive(false);
        RoomMenu.SetActive(true);
        RoomName.text = PhotonNetwork.CurrentRoom.Name;
        RoomInfomation roominfo = GameObject.Find("RoomScreen").GetComponent<RoomInfomation>();
        roominfo.SetRoomInfo();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + updateTimeInterval;
        }
    }

    [PunRPC]
    public void UpdateRoomList(List<RoomInfo> list)
    {
        // destroy each roomPrefab in the list
        foreach (RoomItem item in RoomItemsList)
        {
            Destroy(item.gameObject);
        }
        RoomItemsList.Clear();

        foreach(RoomInfo room in list)
        {
            RoomItem newroom = Instantiate(RoomItemPrefab, ContentBlock);
            newroom.SetRoomName(room.Name);
            newroom.SetOwnerName(room.MaxPlayers.ToString());
            RoomItemsList.Add(newroom);
        }
    }

    // join room Button
    public void JoinRoom(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player has entered room");
    }

    public void OnLeaveRoomButton()
    {
        PhotonNetwork.CurrentRoom.Players.Remove(2);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
        LobbyMenu.SetActive(true);
        RoomMenu.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    private void Update()
    {
        if (NormalMatchToggle.isOn)
        {
            SpeedMatchToggle.isOn = false;
        }
        if (SpeedMatchToggle.isOn)
        {
            NormalMatchToggle.isOn = false;
        }
        if(PhotonNetwork.IsMasterClient )
        {
            MatchSelect.SetActive(true);
            if((NormalMatchToggle.isOn || SpeedMatchToggle.isOn) && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                PlayButton.SetActive(true);
            }
        }
        else
        {
            MatchSelect.SetActive(false);
            PlayButton.SetActive(false);
        }
    }

    public void OnClickPlayButton()
    {
        PhotonNetwork.CurrentRoom.IsVisible = false;
        if(NormalMatchToggle.isOn)
        {
            PhotonNetwork.LoadLevel("NormalMatchOnline");
        }
        if(SpeedMatchToggle.isOn)
        {
            PhotonNetwork.LoadLevel("SpeedMatchOnline");
        }
    }
}
