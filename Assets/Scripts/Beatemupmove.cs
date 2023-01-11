using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beatemupmove : MonoBehaviour
{
    // Movement speed
    [SerializeField] private float hspeed = 5f;
    [SerializeField] private float vspeed = 3f;
    // party_rb = party object rigidbody, rb rigidbody of model
    public enum DodgeType {Dodge,Block};
    DodgeType d = DodgeType.Dodge;
    private Rigidbody2D rb,party_rb;
    // Animator of current model on control
    private Animator anim;
    [SerializeField]AnimatorOverrideController aoc;
    // model = current model, shadow = shadow in party object, QTEspot will be where QTE will be conducted
    public GameObject model,shadow,QTEspot;
    // flip = direction of overall sprite, rtl = when object is ready to stop at fall point, candash = dictates when player can dodge
    private bool rtl = true, candash = true, immobile = false;
    [SerializeField]bool iframe = false;
    // Used to restrict player airdash frequency
    private int airdash = 1;
    Coroutine dodge, suspended;

    [SerializeField]Stats stats;
    AdvancedStats As;

    public float jumpHeight = 500f, dashpower = 250f;
    //
    private bool onground = true;
    [SerializeField]private bool canmove = true;
    [HideInInspector] public float axisY;
    //public static Beatemupmove instance;
    float hMove,vMove;
    // Start is called before the first frame update

    private bool flip = false;

    PlayerParty pp;

    [SerializeField]List<AnimationClip> extraAnims = new List<AnimationClip>();

    public bool a_flip
    {
        get {return flip; }
        set {flip = value; }
    }

    public bool cd
    {
        get{return candash;}
        set{candash = value;}
    }

    public PlayerParty PP
    {
        set{PP = value;}
    }

    public Coroutine suspend
    {
        get{return suspended;}
    }

    public bool invinc
    {
        get {return iframe; }
    }

    public bool a_immobile
    {
        set{immobile = value;}
        get{return immobile;}
    }

    void Awake()
    {
        //rb.Sleep();
        //instance = this;
    }

    public Vector3 curr_position()
    {
        return transform.position;
    }

    // Getters and setters
    public bool a_onground
    {
        get {return onground; }
        set {onground = value; }
    }

    public bool a_canmove
    {
        get {return canmove; }
        set 
        {
            Debug.Log("Setting move to "+value);
            canmove = value; 
        }
    }

    public Rigidbody2D a_rb
    {
        get {return rb; }
    }

    public Rigidbody2D a_party_rb
    {
        get {return party_rb; }
    }

    public AdvancedStats a_As
    {
        get {return As;}
    }

    public Stats parameters
    {
        get {return stats;}
    }

    public int ad
    {
        get{return airdash;}
        set{airdash = value;}
    }

    public void restoreDash()
    {
        //airdash = 1;
        if (dodge != null)
        {
            StopCoroutine(dodge);
            dodge = StartCoroutine(ManageCooldown(0f,1));
        }
    }

    void Start()
    {
        // Get necessary components
        party_rb = GetComponent<Rigidbody2D>();
        anim = model.GetComponent<Animator>();
        rb = model.GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
        As = GetComponent<AdvancedStats>();
        pp = GameObject.Find("Party").GetComponent<PlayerParty>();
        if (pp == null)
            Debug.Log("Couldn't find player party");
        party_rb.freezeRotation = true;
        rb.isKinematic = true;
    }
    // This will trigger on any attack received during iframes
    public void PerfectTrigger()
    {
        //Debug.Log("Invoking time slow");
        pp.GM.StartTimeFreeze(0.5f);
        
    }

    private IEnumerator ManageCooldown(float waitTime, int vardelay)
    {
        yield return new WaitForSeconds(waitTime);

        switch(vardelay)
        {
            
            // Iframe
            case 1:
                /**
                sr.color = new Color32(255,255,255,255);**/
                
                anim.SetTrigger("end");
                
                // Allows player to move
                canmove = true;
                // Resets fall point to where current party object is.
                axisY = transform.position.y;
                // Party rigidbody sleeps
                party_rb.Sleep();
                party_rb.drag = 1000f;
                rb.isKinematic = false;
                // If player isn't in the air, make the rigidbody to sleep
                if (onground)
                    rb.Sleep();
                else
                {
                    // Otherwise stop all velocities (to avoid any offshoots) for a frame and then resume gravity
                    rb.velocity = new Vector2(0,0);
                    rb.gravityScale = 1.5f;
                }
                // Start the dash cooldown. Until it hits 0, player can't consecutive dash
                //StartCoroutine(ManageCooldown(0.1f,5));
                iframe = false;
                StartCoroutine(ManageCooldown(1f,3));
                break;

            // Allows for player to land on groundpoint
            case 2:
                rtl = true;
                break;
            // This will restore any dashes
            case 3:
                airdash = 1;
                break;
            // This will cut the knockback short.
            case 4:
                party_rb.Sleep();
                party_rb.drag = 1000f;
                rb.isKinematic = false;
                break;
            case 5:
                iframe = false;
                StartCoroutine(ManageCooldown(1f,3));
                break;
            case 6:
                GravityFlip(false);
                suspended = null;
                break;
                
                
        }
    }
    // The jump function that allows for illusion of depth in a 2.5D landscape
    public void Jump(float bonus)
    {
        // Can utilize position of party object dictates where player will land
        axisY = transform.position.y;
        rb.isKinematic = false;
        onground = false;
        anim.SetBool("onground",onground);
        rtl = false;
        rb.gravityScale = 1.5f;
        rb.WakeUp();
        rb.AddForce(new Vector2(0, jumpHeight + bonus));
        StartCoroutine(ManageCooldown(0.7f,2));
    }

    public void AerialLaunch(Vector2 force, bool isAirborne)
    {


        rb.isKinematic = true;
        party_rb.drag = 0f;
        party_rb.WakeUp();
        party_rb.AddForce(force * party_rb.mass);
        StartCoroutine(ManageCooldown(0.3f,4));
    }

    public void SuspendFalling(Vector2 position, float falltime)
    {
        onground = false;
        GravityFlip(true);
        model.transform.localPosition = position;
        if (suspended != null)
        {
            StopCoroutine(suspended);
        }
        suspended = StartCoroutine(ManageCooldown(falltime,6));

    }

    // Receiving knockback when attacked by enemy
    public void Knockback(Vector2 direction)
    {
        party_rb.WakeUp();
        canmove = false;        
        party_rb.AddForce(direction * party_rb.mass);
        rb.isKinematic = true;
        party_rb.drag = 0f;
        StartCoroutine(ManageCooldown(0.2f,4));

    }

    // Gather playerinput
    void Update()
    {   
        // Gather Movement Input
        hMove = Input.GetAxisRaw("Horizontal");
        vMove = Input.GetAxisRaw("Vertical");

        // If player was in the air when they dash, upon reaching the ground restore their airdash
        if (onground && !candash && airdash > 0)
        {
            candash = true;
        }

    }
    // Due to how FixedUpdate calls less frequent than Update, this is used to manage any physics involved
    private void FixedUpdate()
    {
        if (model.transform.localPosition.x != 0f)
            model.transform.localPosition = new Vector2(0f,model.transform.localPosition.y);
        // Flips entire gameobject based on direction of input
        if (flip)
            transform.localScale = new Vector2(-1f,1f);
        else
            transform.localScale = new Vector2(1f,1f);

        if (!onground)// This is used for if the player is falling
        {
            if (rb.velocity.y < 0f)
                anim.SetTrigger("fall");
        }
        // Moves gameobject
        Dodge(hMove,vMove);
        Move(hMove,vMove);

    }

    public void Dodge(float x, float y)// Using player input, decide the direction of the dash.
    {
        // If player is able to move and dash cooldown ends
        if (Input.GetKey("c") && candash)
        {
            // Prevents player movement
            canmove = false;
            airdash = 0;
            iframe = true;
            // Wakes up both party rigidbody and model rigidbody as both rigidbody positions are asynchronous.
            party_rb.WakeUp();
            rb.WakeUp();
            party_rb.drag = 0f;
            // Performs a backpedal if dashed from idle
            if (x == 0f && y == 0f)
            {
                party_rb.velocity = new Vector2(-dashpower * transform.localScale.x,0);

                aoc["BackDash"] = extraAnims[0];
                
            }
            // Otherwise dash based on player direction in a circular manner
            else
            {
                party_rb.velocity = new Vector2(dashpower * x,dashpower/2f * y);
                //anim.Play("Dodge");
                //rb.velocity = party_rb.velocity;
                aoc["BackDash"] = extraAnims[1];
            }
            //anim.runtimeAnimatorController = aoc;
            anim.Play("Backpedal");

            //rb.velocity = party_rb.velocity;
            rb.isKinematic = true;// This allows the player model to move alongside the base.

            // Suspends player gravity to prevent falling while dashing and removes an airdash
            if (!onground)
            {
                rb.gravityScale = 0f;
            }
            // Prevents continous dashing and starts iframe duration
            candash = false;
            dodge = StartCoroutine(ManageCooldown(0.2f,1));
            //anim.Play("Dodge");
        }
    }

    // Update is called once per frame
    public void Move(float hmove,float vmove)
    {

        if (canmove)
        {
            // While in the air player will move slower when they move up and down
            if (!onground)
            {
                vmove *= 0.5f;
                axisY = transform.position.y;// Changes landing position when player is moving
            }
            
            
            Vector3 movement = new Vector3(hmove * hspeed, vmove * vspeed, 0.0f);// Move player position based on input
            transform.position += (movement * Time.deltaTime);


            if (onground)
            {
                model.transform.localPosition = Vector3.zero;
            }

            // If there is some form of movement input
            if (hMove != 0 || vMove != 0)
            {
                anim.SetBool("walk", true);
                if (hMove < 0)
                {
                    // Advanced stats will handle flipping as each individual will have their own directions to face
                    flip = true;// Face Left
                }
                else if (hMove > 0)
                {
                    flip = false;
                }
            }
            // Otherwise remain idle
            else
                anim.SetBool("walk", false);

            // If player is on the ground and inputs space
            if (onground && Input.GetKey("space"))
            {
                //Debug.Log("Jump!");
                anim.Play("Jump");
                Jump(0.0f);
            }

        }
        else
            anim.SetBool("walk", false);
        // Stops player falling.
        if (model.transform.position.y <= axisY && rtl && !onground)
        {
            OnLanding();
        }
    }
    // Used for aerial combat to prevent player from falling while attacking
    public void GravityFlip(bool isSuspend)
    {
        if (isSuspend)
        {
            rb.gravityScale = 0f;
            rb.Sleep();
            //model.transform.position = new Vector2(model.transform.position.x,model.transform.position.y - axisY);
        }
        else
        {
            rb.gravityScale = 1.5f;
            anim.SetBool("onground",onground);
            //anim.Play("Fall");
        }
    }
    // When the player "lands" aka their y position reaches their last recorded position
    void OnLanding()
    {
        // Sets player onground
        //Debug.Log("Land!");
        onground = true;
        anim.SetBool("onground",onground);
        // Removes gravity and continously set up axisY
        rb.gravityScale = 0f;
        rb.Sleep();
        axisY = transform.position.y;
    }
}
