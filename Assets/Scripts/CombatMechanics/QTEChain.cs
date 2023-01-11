using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QTEChain : MonoBehaviour
{
    [HideInInspector]public int chain = 0;
    bool start;
    [SerializeField]Image timefill;
    [SerializeField]TMP_Text chainlevel;
    Coroutine currentChain;
    [SerializeField]GameObject ChainDisplay;
    [SerializeField]Color32[] elementcolors = new Color32[6];


    // Start is called before the first frame update
    void Start()
    {
        ChainDisplay.SetActive(false);
    }

    private IEnumerator ChainMode()
    {
        float timer = 10f;
        
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            timefill.fillAmount = (timer)/10f;
            yield return null;
        }
        currentChain = null;
        chain = 0;
        ChainDisplay.SetActive(false);

        //Debug.Log("Calling!");
    }

    void ResetTimer()
    {
        StopCoroutine(currentChain);
        currentChain = null;
        timefill.fillAmount = 1.0f;
    }

    public void Increment(int element)
    {
        ChainDisplay.SetActive(true);
        chain++;
        if (chain > 3)
        {
            chain = 0;
            if (currentChain != null)
            {
                ResetTimer();
            }
            ChainDisplay.SetActive(false);
            return;
        }
        timefill.color = elementcolors[element];
        if (currentChain != null)
        {
            ResetTimer();
        }

        currentChain = StartCoroutine(ChainMode());
    }

    // Update is called once per frame
    void Update()
    {

        chainlevel.text = chain.ToString();

    }
}
