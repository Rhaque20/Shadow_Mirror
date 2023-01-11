using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GearCard : MonoBehaviour
{
    // Start is called before the first frame update
    //public enum imagetype{background,upgradeBackDrop,raritybar,icon,bottomdivider};
    //public enum textfields{upgradeLevel,title,piece,mainstatName,mainStatVal};
    Image background, upgradeBackDrop, rarityBar, icon, bottomdivider;
    TMP_Text upgradeLevel, title, piece,mainstatName, mainStatVal;
    //Image[] gearImages = new Image[8];
    //TMP_Text[] textFields = new TMP_Text[]
    //GameObject[] substats = new GameObject[4];
    GameObject substat;
    [SerializeField]GameObject seteffect;
    [SerializeField]EquipmentSprites es;
    [SerializeField]Color[] backdropColors = new Color[6];

    void Start()
    {
        background = transform.GetChild(0).gameObject.GetComponent<Image>();
        upgradeBackDrop = transform.GetChild(1).gameObject.GetComponent<Image>();
        upgradeLevel = upgradeBackDrop.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        rarityBar = transform.GetChild(2).gameObject.GetComponent<Image>();
        title = transform.GetChild(3).gameObject.GetComponent<TMP_Text>();
        icon = transform.GetChild(4).gameObject.GetComponent<Image>();
        piece = transform.GetChild(5).gameObject.GetComponent<TMP_Text>();
        mainstatName = transform.GetChild(6).gameObject.GetComponent<TMP_Text>();
        mainStatVal = transform.GetChild(7).gameObject.GetComponent<TMP_Text>();
        substat = transform.GetChild(9).gameObject;
        seteffect = substat.transform.GetChild(5).GetChild(0).GetChild(0).GetChild(0).gameObject;

        
    }

    public void WriteSetEffect(int set)
    {
        SetBonus sb = es.bonusDesc[set];
        TMP_Text tmp = seteffect.GetComponent<TMP_Text>();

        tmp.text = "<b>2-Piece Set</b><br>"+sb.twoPiece+"<br><br>";
        tmp.text += "<b>4-Piece Set</b><br>"+sb.fourPiece;
    }

    public void UpdateCard(Armor a)
    {
        background.color = backdropColors[a.rarity - 1];
        if (a.enhancelevel == 0)
            upgradeBackDrop.gameObject.SetActive(false);
        else
        {
            upgradeBackDrop.gameObject.SetActive(true);
            upgradeLevel.text ="+"+a.enhancelevel.ToString();
        }

        rarityBar.sprite = es.rarityBar[a.rarity - 1];
        title.text = a.name;
        icon.sprite = es.GetArmorIcon(a.set.ToString(),a.slot.ToString());
        piece.text = a.grade.ToString()+" | "+a.slot.ToString();
        mainstatName.text = ArmorAttributes.StatName(a.stat[0]);

        if (a.statval[0] >= 1f)
            mainStatVal.text = a.statval[0].ToString();
        else
            mainStatVal.text = (a.statval[0] * 100f).ToString()+"%";
        
        int difference = 4 - a.statval.Length + 1;
        GameObject g;
        TMP_Text subName, subValue, subRare;
        for(int i = 1; i < 5; i++)
        {
            g = substat.transform.GetChild(i-1).gameObject;
            if (i >= a.stat.Length)
            {
                g.SetActive(false);
            }
            else
            {
                g.SetActive(true);
                subName = g.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
                subValue = g.transform.GetChild(2).gameObject.GetComponent<TMP_Text>();
                subRare = g.transform.GetChild(3).gameObject.GetComponent<TMP_Text>();

                subName.text = ArmorAttributes.StatName(a.stat[i]);
                subValue.text = (a.statval[i] * 100f).ToString()+"%";
                subRare.text = a.statrarity[i].ToString();
                //sub
            }
        }
        WriteSetEffect((int)a.set);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
