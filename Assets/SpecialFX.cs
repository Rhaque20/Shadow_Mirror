using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFX : MonoBehaviour
{
    // Start is called before the first frame update
    public Color32 phycolormain,phycolorsub,elecolormain,elecolorsub;
    [SerializeField]GameObject SlashFX;
    Animator anim;
    void Start()
    {
        anim = SlashFX.GetComponent<Animator>();
    }

    void CallSlash(int elemental)
    {
        SpriteRenderer main, sub;
        main = SlashFX.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        sub = SlashFX.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();

        if (elemental == 0)
        {
            main.color = new Color(phycolormain.r,phycolormain.g,phycolormain.b,main.color.a);
            sub.color = new Color(phycolorsub.r,phycolorsub.g,phycolorsub.b,sub.color.a);
        }
        else
        {
            Debug.Log("Elemental");
            //main.color = new Color(elecolormain.r,elecolormain.g,elecolormain.b,main.color.a);
            //sub.color = new Color(elecolorsub.r,elecolorsub.g,elecolorsub.b,sub.color.a);
            main.color = elecolormain;
            sub.color = elecolorsub;
        }
        //SlashFX.SetActive(true);
        anim.Play("Slash");
        //anim.SetTrigger("FullSlash");
    }

    // Update is called once per frame
    /**
    void Update()
    {
        
    }
    **/
}
