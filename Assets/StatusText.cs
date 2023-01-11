using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]List<GameObject> texts = new List<GameObject>();
    List<bool> inUse = new List<bool>();


    private IEnumerator Duration(int i)
    {
        yield return new WaitForSeconds(0.5f);
        RemoveText(i);
    }
    void Start()
    {
        foreach (Transform child in transform)
        {
            texts.Add(child.gameObject);
            inUse.Add(false);
            child.gameObject.SetActive(false);
        }
        
    }

    public void DisplayText(string text)
    {
        for (int i = 0; i < inUse.Count; i++)
        {
            if(!inUse[i])
            {
                inUse[i] = true;
                texts[i].GetComponent<TMP_Text>().text = text;
                texts[i].SetActive(true);
                StartCoroutine(Duration(i));
                return;
            }
        }
    }

    void RemoveText(int i)
    {
        inUse[i] = false;
        texts[i].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
