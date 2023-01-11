using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidDriver : MonoBehaviour
{
    /**
    public Stats s;
    public GameObject attackPoint,mainBody,voidBody,groundpoint,shadow;
    SpriteRenderer sr;
    Animator anim;
    NormalAttack na;
    public CecileCore cc;
    public bool onEntry = false,ready = false,canMove = true,release = false;
    float hMove, vMove;
    public float airheight;
    public Vector2 movepow, movespd;
    public Shadowpuppetry sp;
    public Skill vhop;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void CanRelease()
    {
        ready = true;
    }

    void SkillDamage()
    {
        sp.sr.flipX = sr.flipX;
        Debug.Log("Cecile Sprite Flip: "+sp.sr.flipX);
        cc.SkillAttack(true,1,vhop);
    }

    private IEnumerator Duration(float duration)
    {
        yield return new WaitForSeconds(duration);
        release = true;
    }

    void ReturnSize()
    {
        s.As.EVA -= 100f;
        transform.localScale = new Vector3(1f,1f,0f);
        canMove = true;
        mainBody.SetActive(true);
        voidBody.SetActive(false);
        //sp.sr.flipX = sr.flipX;
        sp.enabled = true;
        cc.na.delay = false;

        sp.onground = false;
        mainBody.transform.position = new Vector3(groundpoint.transform.position.x,groundpoint.transform.position.y + airheight,0f);
        na.Airtime();
        //sp.rb.WakeUp();
        sp.rb.gravityScale = 1.5f;
        Physics2D.IgnoreLayerCollision(6,7,false);
    }
    
    public void StartPos(Transform startpos,NormalAttack na)
    {
        transform.position = startpos.position;
        this.na = na;
    }

    private void FixedUpdate()
    {
        if (canMove)
            Move(movepow.x,movepow.y);
    }

    void Move(float hmove,float vmove)
    {
        Vector3 movement = new Vector3(hmove * movespd.x, vmove * movespd.y, 0.0f);
        transform.position += (movement * Time.deltaTime);
        groundpoint.transform.position = transform.position;
        shadow.transform.position = transform.position;
        mainBody.transform.position = transform.position;

        if (hmove < 0)
        {
            sr.flipX = true;

        }
        else if (hmove > 0)
        {
            sr.flipX = false;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            movepow.x = Input.GetAxisRaw("Horizontal");
            movepow.y = Input.GetAxisRaw("Vertical");
        }
        if (onEntry)
        {
            onEntry = false;
            anim.Play("Void_WindUp");
            anim.SetBool("hold",true);
            Physics2D.IgnoreLayerCollision(6,7,true);
            StartCoroutine(Duration(2f));
            s.As.EVA += 100f;
        }

        if ((Input.GetKeyDown("x") && anim.GetBool("hold") && ready) || release)
        {
            ready = false;
            release = false;
            canMove = false;
            transform.localScale = new Vector3(2f,2f,0f);
            anim.SetBool("hold",false);
            //sp.shadowb.mass = 100000f;
        }
    }
    **/
}
