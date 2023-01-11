using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telegraph : MonoBehaviour
{
    [SerializeField] Color32 weak,strong;
    [SerializeField] SpriteRenderer display,meterdisplay;
    [SerializeField] GameObject meter;
    Coroutine queueup;
    public enum fillType{linear,radial};
    [SerializeField]private bool ready = false,lockon = false;
    [SerializeField]Telegraph.fillType fill = Telegraph.fillType.linear;
    [SerializeField]Transform center;
    [SerializeField]EnemyMobile em;
    [SerializeField]Collider2D range;
    

    public bool aim
    {
        get{return lockon;}
        set{lockon = value;}
    }
    // Start is called before the first frame update
    public EnemyMobile mobile
    {
        set{em = value;}
        get{return em;}
    }
    public bool r
    {
        get {return ready; }
        set {ready = value; }
    }

    public Coroutine q
    {
        get {return queueup;}
        set {queueup = value;}
    }

    public SpriteRenderer d
    {
        get{return display;}
    }
    public Collider2D zone
    {
        get{return range;}
    }
    // Display for both the telegraph and the bar filling up are disabled at the start
    void Start()
    {
        display.enabled = false;
        meterdisplay = meter.GetComponent<SpriteRenderer>();
        meterdisplay.enabled = false;
    }

    // Starts the telegraph of current marker, used for non-tracking
    public void TelegraphSetup(bool strong, float time, bool showTelegraph)
    {
        //Sets the activeness of the telegraph to on
        if (queueup != null)
            return;

        TelegraphToggle(showTelegraph);
        if (strong)// Strong means bypasses i-frames
        {
            display.color = this.strong; // Sets it to blue, might let it be changable in settings.
        }
        else// Otherwise it can be dodged with i-frames
        {
            display.color = weak;
        }

        queueup = StartCoroutine(Timer(time));
        
    }
    // Used for tracking.
    public void TelegraphSetup(bool strong, float time, bool isLockOn, bool showTelegraph)
    {
        //Debug.Log("Called telegraph setup");

        lockon = isLockOn;

        TelegraphToggle(showTelegraph);
        if (strong)
        {
            display.color = this.strong;
        }
        else
        {
            display.color = weak;
        }
        if (queueup == null)
            queueup = StartCoroutine(Timer(time));
    }

    // Sets the telegraphs on and off.
    public void TelegraphToggle(bool on)
    {
        meterdisplay.enabled = on;
        display.enabled = on;
    }

    private IEnumerator Timer(float timelimit)
    {
        float curtime = timelimit;

        while (curtime > 0f)
        {
            curtime -= Time.deltaTime;
            if (fill == Telegraph.fillType.linear)
                meter.transform.localScale = new Vector2((1 - curtime/timelimit),meter.transform.localScale.y);
            else
                meter.transform.localScale = new Vector2((1 - curtime/timelimit),(1 - curtime/timelimit));
            yield return null;
        }

        ready = true;
        if (!lockon)
            TelegraphToggle(false);
    }

    public void StopTelegraph()
    {
        if (queueup != null)
        {
            StopCoroutine(queueup);
            queueup = null;   
        }
        ready = false;
        TelegraphToggle(false);
        lockon = false;
    }

}
