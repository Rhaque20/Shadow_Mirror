using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMobile : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyVariables ev;
    public float speed, turnspeed,spacing = 3f;
    bool launched = false;
    public GameObject player = null,body = null,shadow = null;
    public Animator anim;
    public SpriteRenderer sr;
    public Vector2 offset;
    public Forces f;
    private Vector2 movement;
    [SerializeField]private bool canMove = true, needTurn = false, faceRight = false,canTurn = true;
    Coroutine turntimer;
    [SerializeField]PlayerParty pp;

    public enum Mode {Toward, Strafe, Retreat};

    Mode curStatus;

    public bool movable
    {
        get {return canMove; }
        set {
            Debug.Log("Setting canMove to "+value);
            canMove = value; 
            if (value)
                canTurn = true;
            }
    }

    public bool isRight
    {
        get {return faceRight;}
        set {faceRight = value;}
    }

    public bool turnable
    {
        get{return canTurn;}
        set{canTurn = value;}
    }

    void Start()
    {
        // Player location dictated by shadow
        //player = GameObject.Find("Party");
        
        //pp = player.GetComponent<PlayerParty>();

        player = pp.a_party[pp.active];
        sr = shadow.GetComponent<SpriteRenderer>();
        // Using the main enemy body sprite
        //sr = body.GetComponent<SpriteRenderer>();
    }

    public float Direction()
    {

        if (transform.position.x != player.transform.position.x)
        {
            return Mathf.Sign(player.transform.position.x - transform.position.x);
        }
        else
            return 0;

    }

    private IEnumerator Timer(float waitTime, int situation)
    {
        yield return new WaitForSeconds(waitTime);

        switch(situation)
        {
            case 1:
                needTurn = false;
                //sr.flipX = !sr.flipX;
                Facing(Direction());

                turntimer = null;
                break;
        }
    }

    void Facing(float xdir)
    {

        if (xdir > 0f)
        {
            //sr.flipX = false;

            body.transform.localScale = new Vector2(-1f,1);
            //Debug.Log("Body is "+body.transform.localScale);
            faceRight = true;
        }
        if (xdir < 0f)
        {
            //sr.flipX = true;
            body.transform.localScale = new Vector2(1,1);
            
            faceRight = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log("Collided with "+col.gameObject.name);
    }

    void Movement(Vector2 direction)
    {
        // Moves the groundpoint which will in turn move both the shadow and body
        
        
        // Moves the rigidbody via a direction. The player position indirectly influences this
        f.groundrigid.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
        //body.transform.position = new Vector2(transform.position.x + offset.x,transform.position.y + offset.y);
        //shadow.transform.position = transform.position;
        //Debug.Log("Moving");
        //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        /**
        if (!f.yeet)
        {
            if (f.stats.staggertime <= 0f && canMove && !needTurn)
            {
                anim.SetBool("Stagger",false);

                if ((Direction() > 0f && faceRight) || (Direction() < 0f && !faceRight))
                    needTurn = true;
                else
                {
                    if (turntimer != null)
                    {
                        StopCoroutine(turntimer);
                        turntimer = null;
                        needTurn = false;
                    }
                    Movement(movement);
                }
            }
            else if (f.stats.staggertime >= 0f || !canMove || needTurn)
            {
                // When staggered, still move the body and shadow based on position of ground point for knockback
                if (turntimer != null)
                    Debug.Log("turntimer not null");
                body.transform.position = new Vector2(transform.position.x + offset.x,transform.position.y + offset.y);
                shadow.transform.position = transform.position;
                if (f.stats.staggertime > 0f)
                {
                    anim.Play("Stagger");
                    if (turntimer != null)
                    {
                        StopCoroutine(turntimer);
                        turntimer = null;
                    }
                }
                /**
                if (needTurn && turntimer == null && f.stats.staggertime <= 0f)
                    turntimer = StartCoroutine(Timer(2f,1));
                **/
        if (ev.stats.health <= 0f)
        {
            canMove = false;
            canTurn = false;
        }
        if (canTurn)
            Facing(movement.x);
        if (canMove)
            Movement(movement);

        
    }

    // Update is called once per frame
    void Update()
    {
        player = pp.a_party[pp.active];
        Vector2 direction = player.transform.position - transform.position;
        movement = direction;
        if (canMove)
        {
            if (Vector2.Distance(transform.position,player.transform.position) > (spacing + sr.bounds.size.x/2f))
            {
                direction = player.transform.position - transform.position;
                direction.Normalize();
                movement = direction;
                anim.SetBool("walk",true);
            }
            else
            {
                movement = Vector2.zero;
                anim.SetBool("walk",false);
            }
        }
        else
            anim.SetBool("walk",false);
    }
}
