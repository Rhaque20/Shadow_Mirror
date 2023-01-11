using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status Effect", menuName = "Status Effect")]
public class StatusEffect : ScriptableObject
{
    // sbuff/sdebuff/sinstant means strong meaning undispellable for sinstant, bypasses resistance
    public enum Category {buff,debuff};
    [SerializeField]public enum classifier {normal,modifier,gradual,CC};

    public new string name;

    public int chain = 0;// Determines which index of effect array to use.

    public float maxduration = 30f,chance = 100;
    public float[] effect = new float[4];// 0 = Always actives, 1 - 3 = Skill Chain Power
    [HideInInspector]public float duration;

    [HideInInspector]public int index;

    public Sprite Icon;

    public bool instant = false, dispellable = true, special = false;

    public Category type;
    public classifier efftype;

    Stats afflictedstats;
    
    public void Activate(Stats s)
    {
        afflictedstats = s;
    }

    public void Refresh()
    {
        duration = maxduration;
    }

    public bool isBuff()
    {
        return (!special && type == Category.buff);
    }

}