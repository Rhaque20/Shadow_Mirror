using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatGrowth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private float[] HP = new float[2], ATK = new float[2], DEF = new float[2];
    public PlayerStats s;
    public void Initialize()
    {
        int levelmod = s.level - 1;
        s.equipment = GetComponent<CharacterEquips>();
        s.health = Mathf.Round(HP[0] + ((HP[1]/99f) * levelmod));
        s.health += s.equipment.flatHP + (s.health * s.equipment.totalStats[0]);
        s.maxHP = s.health;
        //s.health = Mathf.Round(s.health);
        s.attack = ATK[0] + (ATK[1]/99f) * levelmod;
        s.defense = DEF[0] + (DEF[1]/99f) * levelmod;
        s.a_curSP = 300f;
        s.a_maxSP = 300f;
        s.critrate = 0.05f;
        s.critdmg = 0.5f;
        
    }
}
