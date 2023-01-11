using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour
{
    // This script is used exclusively on enemies
    public EnemyMobile em;
    [SerializeField]private EnemyScan es;
    EnemyVariables ev;

    public float delay;
    bool aiming = false,hasContact = false;
    Coroutine waiting = null;
    public int threshold;
    int windup;
    
    public Vector2 attacksize;

    public LayerMask playerLayers;

    public GameObject groundpoint,attackPoint;
    public EnemyDamageContact dc;

    public bool aim
    {
        get{return aiming;}
        set{aiming = value;}
    }

    public EnemyVariables enemyVariable
    {
        set{ev = value;}
        get{return ev;}
    }

    public Coroutine wait
    {
        get{return waiting;}
    }

    public void AttackRecover(bool recover)
    {
        ev.groundrigid.Sleep();
        ev.bodyrigid.isKinematic = false;
        aiming = false;
        //strike = false;
        waiting = null;
        //em.anim.SetInteger("skill",0);
        dc.hasContact = false;
        /**
        if (ev.stats.state < 1)
            ev.stats.armor = 0f;
        **/
        if (recover)
        {
            dc.ReturnCollision();
            es.ReturnNeutral();
        }

    }
    // Start is called before the first frame update
    private IEnumerator AttackCooldown(float waitTime, int situation)
    {
        
        yield return new WaitForSeconds(waitTime);

        switch(situation)
        {
            case 1:
            //strike = true;
            em.movable = false;
            break;
            case 2:
            Debug.Log("Recover!");
            //em.anim.SetBool("Strike",false);
            //em.movable = true; 
            AttackRecover(true);
            break;
            case 3:
            AttackRecover(false);
            break;
        }

        
    }
    /**
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireCube(attackPoint.transform.position,new Vector3(attacksize.x,attacksize.y,0f));
    }
    **/

    public float AnglefromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        //Debug.Log("Measured degree is "+n);
        if (n < 180 && !em.isRight)
            n -= 180;
        return n;
    }

    public float AngleFromVector(Vector3 dir, float angleRange)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        //Debug.Log("Measured degree is "+n);
        if (n < 180 && !em.isRight)
            n -= 180;
        return n;
    }

    public Vector3 Direction()
    {
        return (em.player.transform.position - groundpoint.transform.position).normalized;
    }

    void Start()
    {
        es = GetComponent<EnemyScan>();
    }

    public void Launch()
    {

        Debug.Log("LAUNCH!");
        //em.anim.SetInteger("skill",0);
        //em.anim.SetBool("Strike",true);
        //chargepoint = em.player.transform;
        //windup = 0;
        aiming = false;
        dc.hasContact = true;
        ev.groundrigid.WakeUp();
        ev.bodyrigid.isKinematic = true;
        ev.groundrigid.AddForce(Direction() * 20f * ev.groundrigid.mass,ForceMode2D.Impulse);
        if (waiting == null)
            waiting = StartCoroutine(AttackCooldown(0.5f,2));
    }

    public void LungeForward()
    {
        ev.groundrigid.WakeUp();
        ev.bodyrigid.isKinematic = true;
        ev.groundrigid.AddForce(Direction() * 10f * ev.groundrigid.mass,ForceMode2D.Impulse);
        if (waiting != null)
            StopCoroutine(waiting);

        waiting = StartCoroutine(AttackCooldown(0.25f,3));
    }


    void FixedUpdate()
    {
        /**
        if (aiming)
        {
            attackPoint.transform.eulerAngles = new Vector3(0f,0f,AnglefromVector(Direction()));
            attackPoint.transform.position = groundpoint.transform.position;
        }
        **/
    }

    // Update is called once per frame
    void Update()
    {
        /**
        if (!strike && waiting == null)
        {
            waiting = StartCoroutine(AttackCooldown(delay,1));
        }
        else if (strike && em.anim.GetInteger("Attack") == 0 && ev.stats.staggertime <= 0f)
        {
            // Uses enemymobile to use animator
            em.anim.SetInteger("Attack",1);
            em.anim.Play("WindUp");
            em.canMove = false;
            if (ev.stats.state < 1)
                ev.stats.armor = 1500f;
        }

        if (dc.hasContact)
        {
            attackPoint.transform.position = groundpoint.transform.position;
            if (Vector2.Distance(groundpoint.transform.position,em.player.transform.position) <= 2f && dc.hits == 0)
            {
                Collider2D[] player = Physics2D.OverlapBoxAll(attackPoint.transform.position,attacksize,0f,playerLayers);

                foreach(Collider2D p in player)
                {
                    if (p.gameObject.CompareTag("Player"))
                    {
                        dc.AttackOverlay(p);
                        break;
                    }
                }
            }
        }
        **/
    }
}
