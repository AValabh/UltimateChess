using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomItem : MonoBehaviour
{
    public TMP_Text RoomName;
    public TMP_Text RoomOwner;
    LobbyScript manager;

    private void Start()
    {
        manager = FindObjectOfType<LobbyScript>();
    }
    public void SetRoomName(string _RoomName)
    {
        RoomName.text = _RoomName;
    }
    public void SetOwnerName(string _RoomOwner)
    {
        RoomOwner.text = _RoomOwner;
    }
    public void JoinRoomButton()
    {
        manager.JoinRoom(RoomName.text);
    }
}
