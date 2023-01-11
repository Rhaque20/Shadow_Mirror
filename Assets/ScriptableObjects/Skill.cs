using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public new string name;
    public float power;
    public Sprite Icon;
    public float SPcost;
    public int element;
    public List<AnimationClip> attackAnim;
    public int [] skillchain = new int[3];
    public StatusEffect [] chains = new StatusEffect[3];
    public bool special = false;
    public EnumLibrary.Target []t = new EnumLibrary.Target[3];
    public EnumLibrary.Elevation elevation = EnumLibrary.Elevation.grounded;

    public StatusEffect ApplyChain(int i)
    {
        bool hasChain = false;
        for (int j = 0; j < skillchain.Length; j++)
        {
            hasChain = (skillchain[j] == i);
            if (hasChain)
                break;
        }
        if (!hasChain)
            return null;
        
        chains[i].chain = i;
        
        return chains[i];
    }
    
    //public Statuses[] chains = new Statuses[3];
}