using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class SpecialSkillController : MonoBehaviourPunCallbacks
{
    public int skillNo;
    public Image Fire;
    public Image Lightning;
    public TMP_Text x2;
    public string SkillType;
    public GameManagerScript gameManager;
    public PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        // Sets Images false;
        Fire.enabled = false;
        Lightning.enabled = false;
        x2.enabled = false;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }
    public void SetSkill()
    {
        // Get random between 1 and 3
        skillNo = Random.Range(1, 3);

        // Set Skill dependant on number
        if(skillNo  == 1)
        {
            SkillType = "Fire";
            Fire.enabled = true;
            Lightning.enabled = false;
            x2.enabled = false;
        }
        if (skillNo == 2)
        {
            SkillType = "Lightning";
            Fire.enabled = false;
            Lightning.enabled = true;
            x2.enabled = false;
        }
        if (skillNo == 3)
        {
            SkillType = "x2";
            Fire.enabled = false;
            Lightning.enabled = false;
            x2.enabled = true;
        }
    }

    [PunRPC]
    public void SetSkillOnline(int skillNo)
    {
        // Set Skill dependant on number
        if (skillNo == 1)
        {
            SkillType = "Fire";
            Fire.enabled = true;
            Lightning.enabled = false;
            x2.enabled = false;
        }
        if (skillNo == 2)
        {
            SkillType = "Lightning";
            Fire.enabled = false;
            Lightning.enabled = true;
            x2.enabled = false;
        }
        if (skillNo == 3)
        {
            SkillType = "x2";
            Fire.enabled = false;
            Lightning.enabled = false;
            x2.enabled = true;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        Debug.LogWarning("Calling Special Skill");
        if(collision.gameObject.tag == "White")
        {
            // Activate Skill Effect of this tile, destroy gameobjects of tiles around of that tag
            this.gameObject.GetComponentInParent<TileScript>().SkillEffect(SkillType, "Black");
        }
        if (collision.gameObject.tag == "Black")
        {
            // Activate Skill Effect of this tile, destroy gameobjects of tiles around of that tag
            this.gameObject.GetComponentInParent<TileScript>().SkillEffect(SkillType, "White");
        }
        // Set tile inactive
        this.gameObject.SetActive(false);
    }
}
