using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSpriteController : MonoBehaviour
{
    public GameObject[] arms;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Activate()
    {
        //Debug.Log("Going to swap with "+forearm.name);
        arms[0].SetActive(false);
        if (arms[1] == null)
            Debug.Log("Forearm is null!");
        arms[1].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
