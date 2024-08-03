using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareMaterial : MonoBehaviour
{
    public GameObject GameObjectToShareMaterial;
    Material RenderToCopy;
    // Start is called before the first frame update
    void Start()
    {
        RenderToCopy = GameObjectToShareMaterial.GetComponent<MeshRenderer>().material;
        this.gameObject.GetComponent<MeshRenderer>().material = RenderToCopy;
    }
}
