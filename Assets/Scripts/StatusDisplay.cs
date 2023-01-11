using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusDisplay : MonoBehaviour
{
    public GameObject statusicon;
    public GameObject[] iconrepository = new GameObject[10];
    
    // Start is called before the first frame update

    public GameObject AddtoDisplay(StatusEffect effect)
    {
        //GameObject curIcon = Instantiate(statusicon,transform);
        GameObject curIcon = null;
        for (int j = 0; j < 10; j++)
        {
            if (!iconrepository[j].active)
            {
                curIcon = iconrepository[j];
                curIcon.SetActive(true);
                curIcon.GetComponent<Image>().sprite = effect.Icon;
                break;
            }
        }
        //curIcon.GetComponent<Image>().sprite = effect.Icon;
        //iconrepository[i] = curIcon;
        return curIcon;
    }

    public void ClearIcon(int i)
    {
        //Destroy(iconrepository[i]);
        //iconrepository[i] = null;
        Debug.Log("Called clear!");
        iconrepository[i].transform.GetChild(0).GetComponent<Image>().fillAmount = 1f;
        iconrepository[i].SetActive(false);
    }

    /**
    public GameObject ReturnIcon(StatusEffect effect,int i)
    {
        
        Destroy(iconrepository[i]);
        iconrepository[i] = null;
        iconrepository[i] = Instantiate<GameObject>(statusicon);
        iconrepository[i].GetComponent<Image>().sprite = effect.Icon;
        return iconrepository[i];
    }
    **/
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            iconrepository[i] = transform.GetChild(i).gameObject;
            iconrepository[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
