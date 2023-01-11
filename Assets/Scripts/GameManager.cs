using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;
    float defaultTimeScale = 1.0f;
    public TMP_Text fps;
    PlayerHUD phud;
    Coroutine tracker;
    [SerializeField]GameObject explosion;
    // Update is called once per frame

    public float dts
    {
        get{return defaultTimeScale;}
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
        phud = GameObject.Find("Player_HUD").GetComponent<PlayerHUD>();
    }

    private IEnumerator ManageCooldown(float waitTime, int vardelay)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        switch(vardelay)
        {
            case 1:
            //Time.timeScale = 1f;
            defaultTimeScale = 1f;
            explosion.SetActive(false);
            break;
        }
        tracker = null;
    }

    public void SetTimeScale(float scale)
    {
        defaultTimeScale = scale;
        Time.timeScale = scale;
    }

    public void StartTimeFreeze(float slowTime)
    {
        if (tracker == null)
        {
            if (explosion != null)
            {
                explosion.SetActive(true);
                Vector2 pos = phud.pp.a_party[phud.pp.active].transform.position;
                explosion.transform.position = new Vector2(pos.x,pos.y + 0.5f);
                explosion.GetComponent<ParticleSystem>().Play();
            }
            defaultTimeScale = 0.3f;
            Time.timeScale = 0.3f;
            
            tracker = StartCoroutine(ManageCooldown(slowTime,1));
        }
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            //Application.Quit();
        }

        if (Time.timeScale != defaultTimeScale && !phud.selecting)
        {
            Time.timeScale = defaultTimeScale;
        }
        
    }
}
