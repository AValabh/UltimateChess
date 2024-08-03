using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CloseGameOrReturn : MonoBehaviour
{
    public void OnClose()
    {
        Application.Quit();
    }

    public void OnReturnToMainMenu()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        SceneManager.LoadScene("HomeScene");
    }
}
