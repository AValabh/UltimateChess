using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void NormalMatchButton()
    {
        SceneManager.LoadScene("NormalMatch");
    }
    public void SpeedMatchButton()
    {
        SceneManager.LoadScene("SpeedMatch");
    }
    public void OnlineMatchButton()
    {
        SceneManager.LoadScene("MultiplayerMenu");
    }
}
