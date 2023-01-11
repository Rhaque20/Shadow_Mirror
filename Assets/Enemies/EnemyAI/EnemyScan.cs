using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScan : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyVariables ev;
    public GameObject[] attackMarkers;
    public GameObject markerSet;
    Telegraph[] t;
    SpriteRenderer sr;
    public enum Mode {Searching,Charging,Strike,Recover};
    public enum AimType{rigid,freeAim,tracker};
    public EnemyStats s;

    List<Collider2D> hitPlayers = new List<Collider2D>();

    public LayerMask attackLayers;
    ContactFilter2D c = new ContactFilter2D();

    [SerializeField]private Mode curStatus = Mode.Searching;
    private AimType aimtype = AimType.rigid;
    PlayerParty pp;

    bool enablescanner = true;

    [SerializeField]EnemyMobile em;
    [SerializeField]GameObject groundpoint;
    public Overdrive od;
    [SerializeField]EnemyCore ec;

    public Mode mode
    {
        get {return curStatus; }
        set {curStatus = value; }
    }

    public AimType aim
    {
        get {return aimtype;}
        set 
        {
            aimtype = value;
            if (aimtype == EnemyScan.AimType.rigid)
            {
                attackMarkers[0].transform.eulerAngles = Vector2.zero;
            }
        }
    }

    public PlayerParty party
    {
        get{return pp;}
        set{pp = value;}
    }

    public Telegraph[] a_t
    {
        get {return t; }
    }

    public List<Collider2D> hp
    {
        get {return hitPlayers; }
        //set {hitPlayers = value;}
    }

    public bool IsInLayerMask(string layerName)
    {
        return  attackLayers == (attackLayers | (1 << LayerMask.NameToLayer(layerName)));
    }

    public void AddLayer(string layerName)
    {
        attackLayers |= (1 << LayerMask.NameToLayer(layerName));
        //c.SetLayerMask(attackLayers);
    }

    public void RemoveLayer(string layerName)
    {
        attackLayers &= ~(1 << LayerMask.NameToLayer(layerName));
        //c.SetLayerMask(attackLayers);
    }

    public void SetLayers(LayerMask lm)
    {
        Debug.Log("Calling SetLayer");
        attackLayers = lm;
        
    }


    

    public void ReturnNeutral()
    {
        Debug.Log("Returning to Neutral");
        if (aimtype != EnemyScan.AimType.tracker)
        {
            for (int i = 0; i < attackMarkers.Length; i++)
            {
                t[i].r = false; // Sets ready on telegraph to false
                t[i].TelegraphToggle(false);
                t[i].q = null;
                t[i].transform.localPosition = Vector2.zero;
            }
        }
        od.ResumeMotion(true);
    }

    public void PositionScan(Vector2 position)
    {
        markerSet.transform.position = position;
        enablescanner = true;
    }

    public void HideScan()
    {
        enablescanner = false;
    }

    //public void IntializeEnemyScan()

    void Start()
    {
        //Debug.Log("Called enemy scan start!");
        s = GetComponent<EnemyStats>();
        attackMarkers = new GameObject[markerSet.transform.childCount];
        t = new Telegraph[markerSet.transform.childCount];
        //sr = new SpriteRenderer[attackMarkers.Length];
        for (int i = 0; i < markerSet.transform.childCount; i++)
        {
            attackMarkers[i] = markerSet.transform.GetChild(i).gameObject;
            t[i] = attackMarkers[i].GetComponent<Telegraph>();
            t[i].mobile = em;
        }
        c.SetLayerMask(attackLayers);
        //Debug.Log("Enemy Variable EC is "+ev.ec == null);
        //ec = ev.ec;
    }

    public void InitalizeSize(Vector2 scale, int i)
    {
        //sr = attackMarkers[i].GetComponent<SpriteRenderer>();
        //attacksizes[i] = new Vector2(sr.size.x*scale.x, sr.size.y*scale.y);
        t[i].transform.localScale = scale;
    }

    public void InitializeScan(bool scanPlayer, bool scanEnemy)
    {
        if (!scanPlayer && IsInLayerMask("Player"))
            RemoveLayer("Player");
            
    }

    public void InitalizeTracker(Vector2 scale, int i)
    {
        //sr = attackMarkers[i].GetComponent<SpriteRenderer>();
        //attacksizes[i] = new Vector2(sr.size.x*scale.x, sr.size.y*scale.y);
        t[i].transform.localScale = scale;
        t[i].aim = true;
    }

    public float AnglefromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        //Debug.Log("Measured degree is "+n);
        if (n < 180)
        {
            //Debug.Log("Changing Angle");
            n -= 180;
        }
        return n;
    }

    public float AngleFromVector(Vector3 dir, float angleRange)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        //Debug.Log("Measured degree is "+n);
        if (n < 180 && !em.isRight)
        {
            //
            n -= 180;
        }
        return n;
    }

    public Vector3 Direction()
    {
        return (em.player.transform.position - groundpoint.transform.position).normalized;
    }

    void FixedUpdate()
    {
        if (aimtype == EnemyScan.AimType.freeAim)
        {
            Telegraph tele;
            for (int i = 0; i < markerSet.transform.childCount; i++)
            {
                tele = markerSet.transform.GetChild(i).gameObject.GetComponent<Telegraph>();
                if(tele.q != null)
                {
                    markerSet.transform.GetChild(i).localEulerAngles = new Vector3(0f,0f,AnglefromVector(Direction()));
                    PositionScan(em.transform.position);
                }
            }
        }
        else if (aimtype == EnemyScan.AimType.tracker)
        {
            for (int i = 0; i < markerSet.transform.childCount; i++)
            {
                if(t[i].aim)
                {
                    if (t[i].r)
                    {
                        Debug.Log("Mark!");
                        AttackScan(i);
                        t[i].aim = false;
                        t[i].q = null;
                        t[i].transform.localPosition = Vector2.zero;
                        t[i].TelegraphToggle(false);
                        t[i].r = false;
                    }
                    else
                    {
                        Debug.Log("Tracking player "+pp.a_party[pp.active].name);
                        t[i].transform.position = pp.a_party[pp.active].transform.position;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < markerSet.transform.childCount; i++)
            {
                if (t[i].q != null)
                {
                    t[i].transform.localRotation = Quaternion.identity;
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        /**
        if (col.gameObject.CompareTag("Player") && curStatus == Mode.Searching && enablescanner)
        {
            // Sets current status from search to charge
            curStatus = Mode.Charging;
            // Enables the main attack marker and changes it size
            sr.enabled = true;
            attacksize = new Vector2(sr.size.x * attackMarker.transform.localScale.x,sr.size.y * attackMarker.transform.localScale.y);
            // Disables boxcollider trigger to allow it to work again once it's reenabled
            trigger.enabled = false;
            
        }
        **/
        
    }

    public void AttackScan(int i)
    {
        //sr = attackMarkers[i].GetComponent<SpriteRenderer>();
        //hitPlayers = Physics2D.OverlapBoxAll(scanPlace(i),attacksizes[i],0f,attackLayers);
        //Debug.Log("Testing to see if Player is in layer "+IsInLayerMask("Player"));
        c.SetLayerMask(attackLayers);
        if (em.turnable && aimtype == EnemyScan.AimType.rigid)
        {

            float xSign = Mathf.Sign(em.body.transform.localScale.x);
            float dir = Mathf.Sign(t[i].transform.localScale.x);
            //Mathf.Sign(em.body.transform.localScale.x);

            if ((xSign > 0f && dir < 0f) || (xSign < 0f && dir > 0f))
            {
                t[i].transform.localPosition = new Vector2(t[i].transform.localPosition.x * -1, t[i].transform.localPosition.y);
                t[i].transform.localScale = new Vector2(t[i].transform.localScale.x * -1, t[i].transform.localScale.y);
            }
            else
            {
                t[i].transform.localPosition = new Vector2(t[i].transform.localPosition.x, t[i].transform.localPosition.y);
                t[i].transform.localScale = new Vector2(t[i].transform.localScale.x, t[i].transform.localScale.y);
            }
        }
        t[i].zone.OverlapCollider(c,hitPlayers);
        ev.ec.AttackProcess(hitPlayers);
        hitPlayers.Clear();
    }

    public void NonAttackScan(int i)
    {
        c.SetLayerMask(attackLayers);
        t[i].zone.OverlapCollider(c,hitPlayers);
        ev.ec.NonAttackProcess(hitPlayers);
        hitPlayers.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        /**
        if (aimtype == AimType.rigid && (Vector2)t.transform.localPosition != zonepos)
            t.transform.localPosition = zonepos;
        **/
    }
}
