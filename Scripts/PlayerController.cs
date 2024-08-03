using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public string PlayerNumber;
    public string PlayerColour;
    public PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = this.transform.GetComponent<PhotonView>();
        // Sets Player Colour
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerColour = "White";
        }
        else
        {
            PlayerColour = "Black";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
