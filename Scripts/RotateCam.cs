using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RotateCam : MonoBehaviourPunCallbacks
{
    public bool RotateCamera = false;
    public float smooth = 1f;
    [HideInInspector] public string turn;
    [HideInInspector] public GameManagerScript gameManager;
    public PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start function in RotateCam called;");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        if(gameManager.Connectivity == "Online")
        {
            pv = this.transform.GetComponent<PhotonView>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetAngles;
        if (RotateCamera == true)
        {
            if(turn == "Black")
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 180, 0), smooth * Time.deltaTime);
            }
            if(turn == "White")
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), smooth * Time.deltaTime);
            }

        }
    }

    [PunRPC]
    void TurnColour(string colour)
    {
        turn = colour;
    }
}
