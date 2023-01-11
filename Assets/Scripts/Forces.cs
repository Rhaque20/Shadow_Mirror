using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forces : MonoBehaviour
{
    public EnemyStats stats;
    public Overdrive od;
    public float axisY;
    public bool yeet = false, airslap = false,onplatform = false;

    public float floorcheck = 0f;
    public SpriteRenderer shadow;
    public GameObject puppet;
    public Rigidbody2D rigid; // Body
    public Rigidbody2D groundrigid;// Shadow
    public BoxCollider2D body;
    Collision2D bux;
    Coroutine kb = null, airtime = null;
    // Start is called before the first frame update
    void Start()
    {
        groundrigid.freezeRotation = true;
        stats = GetComponent<EnemyStats>();
    }

    private IEnumerator Physics(float waitTime, int vardelay)
    {
        yield return new WaitForSeconds(waitTime);

        switch(vardelay)
        {
            // Airborne
            case 1:
                groundrigid.Sleep();
                kb = null;
                break;
            case 2:
                rigid.isKinematic = false;
                rigid.WakeUp();
                airtime = null;
                break;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("obstacle"))
        {
            //Debug.Log("I hit a block by trigger");
        }
    }
    
    /**
    public void ShadowDrop()
    {
        //axisY = groundpoint.transform.position.y;
        shadow.transform.position = new Vector2(transform.position.x,transform.position.y);
        elevation = 0f;
        if (!onplatform)
        {
            Physics2D.IgnoreCollision(body,bux.collider,false);
            //Physics2D.IgnoreLayerCollision(6,8,false);
            bux = null;
        }
    }
    **/

    public void Fall()
    {
        rigid.WakeUp();
        floorcheck = 0.1f;
        rigid.gravityScale = 1.5f;
        //puppet.sortingOrder = 0;
        //shadow.sortingOrder = 0;
        //ShadowDrop();
        
        //randomtimer = 0.25f;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //delay = 0.1f;
        
        
        // if (onplatform && col.gameObject.CompareTag("obstacle"))
        // {

        //     yeet = true;
        //     onplatform = false;
        //     elevation -= bonus;
        //     bonus = 0f;
        //     Fall();
            
            
        //     //sp.ShadowDrop();
           
            
        // }
        
    }

/**
    void OnCollisionExit2D (Collision2D col)
    {
        if (onplatform && col.gameObject.CompareTag("obstacle"))
        {
            yeet = true;
            Debug.Log("Fallin through!");
            onplatform = false;
            //Fall();
        }
    }
**/

    void OnCollisionEnter2D (Collision2D col)
    {
        // HeightMap h;
        // if (col.gameObject.CompareTag("obstacle"))
        // {
        //     if (!yeet && !onplatform)
        //     {
        //         Jump(750f);
        //         h = col.gameObject.GetComponent<HeightMap>();
                
        //             onplatform = true;
        // //Debug.Log("Getting on Top");
        //             bonus = h.height;
        //             elevation += bonus;
        //             axisY = transform.position.y + h.height;
                    
        //             shadow.transform.position = new Vector2(transform.position.x,axisY);
        //             shadow.sortingOrder = 1;
        //             //puppet.sortingOrder = 1;

        //             if (bux != null)
        //             {
        //                 Physics2D.IgnoreCollision(body,bux.collider,false);
        //                 bux = col;
        //                 Physics2D.IgnoreCollision(body,bux.collider,true);
        //             }
        //             else
        //             {
        //                 bux = col;
        //                 Physics2D.IgnoreCollision(body,bux.collider,true);
        //             }
        //             //Debug.Log("bux is "+bux.name);
        //         //floorcheck = 0.2f;
        //     }
            
        //}
        
        //Attack();
    }

    public void Stagger(bool left,int attack,float knock)
    {
        
        
        if (od.armormode != Overdrive.ArmorType.Neutral)
        {
            return;
        }
        
        if (attack == 1)
        {
            groundrigid.WakeUp();

            if (yeet)// For aircombos
            {
                rigid.Sleep();
                rigid.isKinematic = true;
            }

            if (left)
            {
                groundrigid.AddForce(new Vector2(-1f * knock * groundrigid.mass,0f),ForceMode2D.Impulse);
            }
            else
            {
                groundrigid.AddForce(new Vector2(knock * groundrigid.mass,0f),ForceMode2D.Impulse);
            }

            //stats.staggertime = 0.5f;
            //airtime = 0.5f;

            if(kb == null)
            {
                kb = StartCoroutine(Physics(0.25f,1));
            }
            if (airtime != null)
            {
                StopCoroutine(airtime);
            }
            airtime = StartCoroutine(Physics(0.5f,2));
            
        }
        else if (!yeet)
        {
            Jump(knock);
        }
        
    }

    void Jump(float knock)
    {
        rigid.isKinematic = false;
        if (rigid.IsSleeping())
            rigid.WakeUp();
        //axisY = shadow.transform.position.y;
        rigid.gravityScale = 1.5f;
        rigid.AddForce(new Vector2(0f,knock* rigid.mass));
        yeet = true;
        floorcheck = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (floorcheck != 0)
        {
            floorcheck -= Time.deltaTime;
            if (floorcheck <= 0)
                floorcheck = 0;
        }
        
        shadow.transform.position = transform.position;
        if (yeet)
        {
            if (floorcheck == 0)
            {
                if (puppet.transform.position.y <= shadow.transform.position.y)
                {
                    rigid.gravityScale = 0f;
                    rigid.Sleep();
                    yeet = false;
                }
            }
        }
        else
        {
            shadow.transform.position = transform.position;
            puppet.transform.position = transform.position;
        }
    }
}
