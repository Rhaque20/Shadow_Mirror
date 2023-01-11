using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdvancedStats : MonoBehaviour
{
    public Stats stats;
    public float ACC,EVA;
    [SerializeField]private float dmgred;
    // 0 = Physical 1 = Fire 2 = Wind 3 = Earth 4 = Water 5 = Light 6 = Dark
    [SerializeField]private float []eleres = new float[7];
    [SerializeField]private float []eledmg = new float[7];
    // 0 = HP 1 = ATK 2 = DEF 3 = Potency 4 = Resistance 5 = C. Rate 6 = C. DMG 7 = SP Gain
    private float []percentboosts = new float[8];
    // 0 = HP 1 = ATK 2 = DEF
    private int []flatboosts = new int[3];
    // dmgvarmor stat id = 9
    private float defpierce = 0f, dmgvArmor = 0f;
    // For enemy, how much mode damage they take. For player, how much mode damage they deal
    // Stat id = 8
    [SerializeField]private float ODMultiplier = 1f,NormalMultiplier = 1f;

    public float[] pboost
    {
        get{return percentboosts;}
    }

    public float[] eres
    {
        get{return eleres;}
    }

    public float ODx
    {
        get{return ODMultiplier;}
        set{ODMultiplier = value;}
    }

    public float Normalx
    {
        get{return NormalMultiplier;}
        set{NormalMultiplier = value;}
    }

    public float Reduction
    {
        get{return (1 - dmgred);}
        set{dmgred = value;}
    }

    // Accuracy check, will be used for debuff application and if crit can land
    public bool AccuracyCheck(float acc) => Random.Range(1,100) > (EVA - acc);

    public float EleRES(int i) => 1 - eleres[i];// Negative Res acts as positive bonus
    public float EleDMG(int i) => 1 + eledmg[i];// Negative Bonus acts as Positive Resistance
    public float PercentMod(int i) => percentboosts[i];
    
    void Start()
    {
        stats = GetComponent<Stats>();
    }

}