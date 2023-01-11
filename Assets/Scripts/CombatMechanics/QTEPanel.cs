using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QTEPanel : MonoBehaviour
{
    [SerializeField]private Animator anim;
    [SerializeField]TMP_Text chainreq;
    [SerializeField]Image effect, chain;
    [SerializeField]Image QTEMember;
    [SerializeField]Image Frame;
    [SerializeField]Color32 []elements = new Color32[7];
    public bool active = false, left = true;
    Vector2 originalpos;
    public int chainlevel;
    // Start is called before the first frame update
    void Start()
    {
        //originalpos = transform.position;

    }

    public void ReadyUp(int element)
    {
        active = true;
        Frame.color = elements[element];
        chainreq.color = Color.black;
        
        if (left)
            anim.SetBool("ready",true);
        else
            anim.SetBool("ready2",true);
    }

    public void RecoverFrame()
    {
        Frame.color = elements[0];
        chainreq.color = Color.white;
        anim.SetBool("ready",false);
        anim.SetBool("ready2",false);
        active = false;
        //transform.position = originalpos;
    }

    public void Reinitialize(Skill s, int req)
    {
        bool initialized = false;
        chainreq.text = req.ToString();
        if (s.skillchain[0] != -1)
        {
            effect.sprite = s.chains[0].Icon;
            effect.enabled = true;
        }
        else
            effect.enabled = false;
        
        for (int i = 1; i < 3; i++)
        {
            if (s.skillchain[i] == req)
            {
                chain.sprite = s.chains[i].Icon;
                chain.enabled = true;
                initialized = true;
            }
        }

        if (!initialized)
            chain.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
