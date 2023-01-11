using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEmissionManager : MonoBehaviour
{
    [SerializeField]List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    [SerializeField]List<Material> materials = new List<Material>();
    [SerializeField]Color emit1 = new Color(255,129,0,0);
    Color tempColor = new Color(0,0,0,0);
    bool GradualEmission = false;

    public bool GradEmit
    {
        get{return GradualEmission;}
        set{GradualEmission = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InstantEmission()
    {
        
    }

    public void ActivateEmission()
    {
        float r = tempColor.r + (emit1.r*Time.deltaTime*2);
        float g = tempColor.g + (emit1.g*Time.deltaTime*2);
        float b = tempColor.b + (emit1.b*Time.deltaTime*2);
        //Debug.Log("R is "+r);
        if (r >= emit1.r)
        {
            r = emit1.r;
        }
        if (g >= emit1.g)
            g = emit1.g;
        if (b >= emit1.b)
            b = emit1.b;
        
        if (r >= emit1.r && g >= emit1.g && b >= emit1.b)
            GradualEmission = false;
        
        tempColor = new Color(r,g,b,0);
        foreach (Material m in materials)
        {
            m.SetColor("_GlowColor",tempColor);
        }
    }

    public void DeactiveEmission()
    {
        foreach (Material m in materials)
        {
            m.SetColor("_GlowColor",new Color32(0,0,0,0));
        }
        tempColor = new Color(0,0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GradualEmission)
        {
            ActivateEmission();
        }
    }
}
