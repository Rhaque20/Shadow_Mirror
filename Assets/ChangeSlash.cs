using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ChangeSlash : MonoBehaviour
{
    [SerializeField]private VisualEffect vfx;
    [SerializeField]private Color baseSlash;
    [SerializeField]private Color subSlash;
    // Main Element
    [SerializeField]private Color elemainslash1;
    [SerializeField]private Color elesubslash1;
    // Sub Element
    [SerializeField]private Color elemainslash2;
    [SerializeField]private Color elesubslash2;
    // Start is called before the first frame update
    void Start()
    {
        vfx = transform.GetChild(0).gameObject.GetComponent<VisualEffect>();
    }

    public void PhysicalSlash()
    {
        Vector4 mainSlash = new Vector4(baseSlash.r,baseSlash.g,baseSlash.b,1.0f);
        Vector4 sub = new Vector4(subSlash.r,subSlash.g,subSlash.b,1.0f);
        vfx.SetVector4(Shader.PropertyToID("SlashColor_Main"),mainSlash);
        vfx.SetVector4(Shader.PropertyToID("SlashColor_Main"),sub);
    }

    public void PrimaryEleSlash()
    {
        Vector4 mainSlash = new Vector4(elemainslash1.r,elemainslash1.g,elemainslash1.b,1.0f);
        Vector4 sub = new Vector4(elesubslash1.r,elesubslash1.g,elesubslash1.b,1.0f);
        vfx.SetVector4(Shader.PropertyToID("SlashColor_Main"),mainSlash);
        vfx.SetVector4(Shader.PropertyToID("SlashColor_Main"),sub);
        //vfx.S
    }

    public void SecondaryEleSlash()
    {
        Debug.Log("Calling sub slash");
        Vector4 mainSlash = new Vector4(elemainslash2.r,elemainslash2.g,elemainslash2.b,1.0f);
        Vector4 sub = new Vector4(elesubslash2.r,elesubslash2.g,elesubslash2.b,1.0f);
        vfx.SetVector4(Shader.PropertyToID("SlashColor_Main"),mainSlash);
        vfx.SetVector4(Shader.PropertyToID("SlashColor_Main"),sub);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
