using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteList : MonoBehaviour
{
    [SerializeField]List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    // Start is called before the first frame update
    void Start()
    {
        int n = transform.childCount;
        SpriteRenderer sr;

        for(int i = 0; i < n; i++)
        {
            sr = transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
            if (sr == null)
                break;
            sprites.Add(sr);
        }
    }

    // public void DeathFade()
    // {
    //     foreach(SpriteRenderer s in sprites)
    //     {

    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}
