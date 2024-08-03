using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIconFollowCam : MonoBehaviour
{
    public GameObject CamPivot;

    void Start()
    {
        CamPivot = GameObject.Find("CameraPivot");
    }
    // Update is called once per frame
    void Update()
    {
        // make skill image and 
        this.transform.rotation = CamPivot.transform.rotation;
    }
}
