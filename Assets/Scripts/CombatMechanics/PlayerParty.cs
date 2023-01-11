using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour,ISaveSystem
{
    // Start is called before the first frame update

    [SerializeField] private GameObject[] party = new GameObject[3];
    [SerializeField]private int a = 0, i = 0,n = 0;
    //public static PlayerParty instance;
    [SerializeField] private NormalAttack na;
    SkillPallete s;
    [SerializeField] QTEChain qc;
    [SerializeField] QTEPanel Q1,Q2;
    //[SerializeField]StatusChanges [] sc;
    [SerializeField]GameObject TeamUI;
    [SerializeField]GameObject playeractives;
    [SerializeField]PlayerHUD phud;
    //Vector2[] panel = new Vector2[3];
    Stats[] pstats = new Stats[3];

    [SerializeField]GameManager gm;
    [SerializeField]GameObject[] parents = new GameObject[3];

    public int active
    {
        get{return a;}
    }

    public GameManager GM
    {
        get{return gm;}
    }

    public GameObject[] a_party
    {
        get{return party;}
    }

    public void ApplyEffecttoParty(StatusEffect s, float potency)
    {
        StatusChanges sc;
        if (s.type == StatusEffect.Category.buff)
        {
            /**
            foreach(GameObject g in party)
            {
                sc = g.GetComponent<StatusChanges>();
                Debug.Log("Applying "+s.name+" to "+g.name);
                sc.ApplyEffect(s);
            }
            **/

            for (int i = 0 ; i < n; i++)
            {
                sc = playeractives.transform.GetChild(i).gameObject.GetComponent<StatusChanges>();
                //Debug.Log("Applying "+s.name+" to "+party[i].name);
                sc.ApplyEffect(s);
            }
        }
    }

    void ActivateStarts(GameObject g)
    {
        g.GetComponent<Stats>().Start();
        g.transform.GetChild(0).gameObject.GetComponent<NAAnims>().Start();
    }

    public void LoadData(GameData data)
    {
        for (int i = 0; i < party.Length; i++)
        {
            if (data.levels[i] < 1)
                continue;
            
            party[i].GetComponent<Stats>().level = data.levels[i];
        }
    }
    public void SaveData(ref GameData data)
    {
        for (int i = 0; i < party.Length; i++)
        {
            data.levels[i] = party[i].GetComponent<Stats>().level;
        }

        Debug.Log("First character level is "+data.levels[0]);
    }

    void Start()
    {
        GameObject g;
        StatusChanges sc;
        // for (i = 0 ; i < 3; i++)
        // {
        //     panel[i] = TeamUI.transform.GetChild(i).position;
        // }
        for (i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.CompareTag("Player"))
            {
                n++;
                party[i] = transform.GetChild(i).gameObject;// As the characters are children to this, get the child.
                //party[i].GetComponent<Beatemupmove>().PP = this;
                party[i].GetComponent<PlayerDamageContact>().qte = qc;
                sc = playeractives.transform.GetChild(i).gameObject.GetComponent<StatusChanges>();// Getting their Status Effects
                //Debug.Log("Getting "+TeamUI.transform.GetChild(i).gameObject.name);
                sc.a_sd = phud.a_ps[i].Statuses.GetComponent<StatusDisplay>();// Getting Status Display of their UI
                pstats[i] = party[i].GetComponent<Stats>();// Get their stats
                phud.a_ps[i].Loadup(pstats[i],party[i].transform.GetChild(0).gameObject.name);//Load up player status with their stats
                
                sc.parameters = pstats[i];
                party[i].GetComponent<StatGrowth>().Initialize();
                pstats[i].sc = sc;

                party[i].transform.GetChild(0).GetComponent<CharacterCore>().a_pp = GetComponent<PlayerParty>();

                if (i != a)
                {
                    ActivateStarts(party[i]);
                    party[i].SetActive(false);
                    if (n == 2)
                    {
                        SkillPallete skp = party[i].GetComponent<SkillPallete>();
                        Q1.Reinitialize(skp.groundskills[(int)skp.skillQTE.y],skp.chainreq);
                    }
                }
            }
        }

        if (n < 2)
        {
            TeamUI.transform.GetChild(1).gameObject.SetActive(false);
            //TeamUI.transform.GetChild(2).gameObject.SetActive(false);
        }

        if (n < 3)
        {
            TeamUI.transform.GetChild(2).gameObject.SetActive(false);
        }

        for (int i = 0; i < 3; i++)
        {
            parents[i] = TeamUI.transform.GetChild(i).gameObject;
        }
        //sc = new StatusChanges[n];
        /**
        for (i = 0; i < 3; i++)
        {
            if (i == n)
            {
                TeamUI.transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                g = TeamUI.transform.GetChild(i).gameObject.transform.GetChild(3).gameObject;
                sc[i] = new StatusChanges();
                sc[i].a_sd = g.GetComponent<StatusDisplay>();
            }
        }
        **/
        i = 0;
    }

    void OffieldSPRegen()
    {
        Stats s;

        /**
        foreach(GameObject g in party)
        {
            if (g != party[a])
            {
                s = g.GetComponent<Stats>();
                s.SPChange( 3;

                if (s.a_curSP > s.a_maxSP)
                    s.a_curSP = s.a_maxSP;
            }
        }
        **/
        for (int i = 0; i < n; i++)
        {
            if (i != a && pstats[i].a_curSP < pstats[i].a_maxSP)
            {
                pstats[i].SPChange(3 * Time.deltaTime);
            }
            else if (pstats[i].a_curSP < pstats[i].a_maxSP)
                pstats[i].SPChange(1 * Time.deltaTime);
        }
    }

    void SwitchMember(int switchto)
    {
        //Debug.Log("Switching");
        Beatemupmove bm, bm2;
        SkillPallete skp = party[switchto].GetComponent<SkillPallete>();
        Skill s = null;
        Stats pstat = null;
        
        party[switchto].transform.position = party[a].transform.position;
        //party[switchto].transform.localScale = party[a].transform.localScale;
        //Debug.Log("Switch to is "+switchto);
        bm = party[switchto].GetComponent<Beatemupmove>();
        bm2 = party[a].GetComponent<Beatemupmove>();
        bm.a_flip = bm2.a_flip;
        
        //pstat = party[switchto].GetComponent<Stats>();


        if (skp.skillQTE.x == 0)
            s = skp.groundskills[(int)skp.skillQTE.y];
        else
            s = skp.airskills[(int)skp.skillQTE.y];

        
        if (qc.chain == skp.chainreq && pstats[switchto].a_curSP >= s.SPcost)
        {
            //Debug.Log("Calling a QTE");
            party[switchto].GetComponent<Collider2D>().isTrigger = true;
            party[switchto].SetActive(true);
            pstats[switchto].SPChange(-s.SPcost);
            bm.a_canmove = false;
            bm.cd = false;
            bm.ad = 0;
            skp.ActivateQTE();
            //qc.Increment();
            i = a;
        }
        else if(!skp.QTEsummon)
        {
            party[switchto].GetComponent<Collider2D>().isTrigger = false;
            bm.a_canmove = true;
            party[switchto].SetActive(true);
            party[a].SetActive(false);
            bm.restoreDash();
            // Transform.SetParent()
            
            // if (switchto != 0)
            //     TeamUI.transform.GetChild(a).position = panel[switchto];
            // else
            // {
            //     if (n == 2)
            //         TeamUI.transform.GetChild(a).position = panel[1];
            //     else
            //     {
            //         TeamUI.transform.GetChild(a-1).position = panel[1];
            //         TeamUI.transform.GetChild(a).position = panel[2];
            //     }
            // }
            
            // TeamUI.transform.GetChild(a).localScale = new Vector2(0.8f,0.8f);
            // TeamUI.transform.GetChild(switchto).position = panel[0];
            // TeamUI.transform.GetChild(switchto).localScale = new Vector2(1f,1f);

            


            if (switchto != 0 || n == 2)
            {
                Debug.Log("Switching to "+parents[switchto].name);
                parents[a].transform.GetChild(0).SetParent(parents[switchto].transform,false);
                parents[switchto].transform.GetChild(0).SetParent(parents[a].transform,false);
            }
            else
            {
                parents[a-1].transform.GetChild(0).SetParent(parents[1].transform,false);
                parents[a].transform.GetChild(0).SetParent(parents[2].transform,false);
            }
            a = switchto;


            na.Reinitialize();
            phud.Reinitialize(a);
        }
        //Debug.Log("a_curSP is now "+pstat.a_curSP + "for "+party[switchto].name);
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("i is "+i);
        if (Input.GetKeyDown("q"))
        {
            i = (a - 1);
            //Debug.Log("i is "+i);
            if (i < 0)
            {
                i = n - 1;
            }
            //Debug.Log("i is now" +i);
        }
        if (Input.GetKeyDown("e"))
        {
            i = (a + 1) % n;
        }

        if (party[i] != null && i != a)
        {
            SwitchMember(i);
        }

        // This preps up the panel animation;
        if (qc.chain == Q1.chainlevel && !Q1.active)
        {
            
            int j = (a + 1) % n;
            if (j < 0)
                j = n - 1;
            s = party[j].GetComponent<SkillPallete>();// Issue here somehow
            Q1.ReadyUp(s.groundskills[(int)s.skillQTE.y].element);
        }
        else if (qc.chain != Q1.chainlevel)
        {
            //Debug.Log("Recovering");
            Q1.RecoverFrame();
        }

        OffieldSPRegen();
    }
}
