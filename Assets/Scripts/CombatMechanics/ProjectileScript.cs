using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    SpriteRenderer sr;
    public Stats stats;
    public float modifier;
    public bool ready = true , destroyable = false;
    public int element;
    public LayerMask targetLayers;// Which object layers will be affected
    // AoE = Creates a field around the point that can help or harm
    // Ballistic = Standard projectile, will destroy on impact
    // Mine = Remain idle and destroy on impact
    public enum projtype {ballistic,mine,Aoe};
    public projtype type;

    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();// Get current rigidbody
        sr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();// Get sprite of projectile
        gameObject.SetActive(false);// Then set the object false
    }
    // This is for lingering projectiles that do a damaging aoe
    private IEnumerator Linger(float lifetime)
    {
        
        while (lifetime > 0f)
        {
            yield return new WaitForSeconds(0.5f);
            DamageScan(0.5f);
            lifetime -= 0.5f;
        }
        
        gameObject.SetActive(false);
        ready = true;
    }

    // This is for projectile movement, it will travel for a set period of time
    // And then do something when projectile lifetime ends
    private IEnumerator Travel(float traveltime, float lifetime)
    {
        yield return new WaitForSeconds(traveltime);
        rb.Sleep();

        if(type == projtype.ballistic)
        {
            gameObject.SetActive(false);
            ready = true;
        }
        else
            StartCoroutine(Linger(lifetime));
    }

    public void Fire(Vector2 direction)
    {
        ready = false;
        rb.velocity = direction;
        StartCoroutine(Travel(0.1f,5f));
    }

    public void Fire(Vector2 direction, float flighttime)
    {
        ready = false;
        rb.velocity = direction;
        StartCoroutine(Travel(flighttime,5f));
    }

    // This handles what to do when projectile collides with player
    void OnTriggerEnter2D(Collider2D col)
    {
        float bonus;
        bool crit;
        // This is for player projectiles
        if (targetLayers == LayerMask.GetMask("enemy"))
        {
            if (col.gameObject.gameObject.CompareTag("enemy"))
            {
                Debug.Log("Kunai Hit!");

                // Gathers additional stats for damage calculation
                crit = stats.DidCrit();
                bonus = stats.DamageBonus(modifier,crit,element);

                // Performs damage calculation
                col.gameObject.GetComponent<Stats>().damagecalc(stats.TotalATK,bonus,crit,element);

                // If object doesn't have any form of lingering, deactivate it
                if (type != projtype.Aoe)
                    gameObject.SetActive(false);
            }
        }

        // This is for enemy projectiles
        if (targetLayers == LayerMask.GetMask("Player"))
        {
            if (col.gameObject.gameObject.CompareTag("Player"))
            {
                float height = col.gameObject.GetComponent<Beatemupmove>().model.transform.localPosition.y;

                if (height < 1f)// If target is too high to simulate player being up in the air.
                {
                    crit = stats.DidCrit();
                    bonus = stats.DamageBonus(modifier,crit,element);
                    col.gameObject.GetComponent<Stats>().damagecalc(stats.TotalATK,bonus,crit,element);
                    if (type != projtype.Aoe)
                        gameObject.SetActive(false);
                }
            }
        }
    }
    // This is for scanning outside the standard collider trigger for aoe scans
    void DamageScan(float multiplier)
    {
        Collider2D[] hitEnemies;
        hitEnemies = Physics2D.OverlapBoxAll(transform.position,sr.size * sr.transform.localScale,0f,targetLayers);
        Stats s;
        Forces f;
        float bonus, damage;
        bool crit;
        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("enemy") || enemy.gameObject.CompareTag("Player"))
            {
                
                crit = stats.DidCrit();
                bonus = stats.DamageBonus(modifier * multiplier,crit,element);
                s = enemy.gameObject.GetComponent<Stats>();
                f = enemy.gameObject.GetComponent<Forces>();
                damage = s.damagecalc(stats.TotalATK,bonus,crit,element);
                f.od.StressFill(damage);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
