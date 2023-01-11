using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemySkill", menuName = "EnemySkill")]
public class EnemySkill : ScriptableObject
{
    public new string name;
    public float power;
    public Sprite Icon;
    public LayerMask targetZone;
    public float spCost = 0f, spGain = 0f;
    public float ATKSPD = 1.0f;
    public List<AnimationClip> attackAnim;
    public StatusEffect [] chains = null;
    public int[] chainLevel = null;
    public bool showTelegraph = true;
    public EnumLibrary.Element affinity = EnumLibrary.Element.Physical;
    
    public EnemyScan.AimType aim;
    public int numMarkers = 1;
    public Vector2[] markerScales;
    public bool[] strong = new bool[1];
    [SerializeField]bool useAnimLength = true;

    public int element
    {
        get{return ReturnElementNum();}
    }

    int ReturnElementNum()
    {
        switch(affinity)
        {
            case EnumLibrary.Element.Fire:
                return 1;
            case EnumLibrary.Element.Wind:
                return 2;
            case EnumLibrary.Element.Earth:
                return 3;
            case EnumLibrary.Element.Water:
                return 4;
            case EnumLibrary.Element.Light:
                return 5;
            case EnumLibrary.Element.Dark:
                return 6;
        }

        return 0;
    }
    
    public float getATKSPD()
    {
        if (attackAnim.Count > 0 && useAnimLength)
        {
            return attackAnim[0].length;
        }
        
        return ATKSPD;
    }
    
    //public Statuses[] chains = new Statuses[3];
}