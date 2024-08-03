using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    GameManagerScript GameManager;
    string turn;

    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        turn = GameManager.turn;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collision Detected");
        if(turn == "White" && this.gameObject.tag == "Black")
        {
            Destroy(this.gameObject);
        }
        if(turn == "Black" && this.gameObject.tag == "White")
        {
            Destroy(this.gameObject);
        }
    }
}
