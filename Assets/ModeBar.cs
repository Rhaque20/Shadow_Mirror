using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeBar : MonoBehaviour
{
    [SerializeField]EnemyHUD ehud;
    public enum StressState {Neutral,Overdrive,Break};
    StressState ss = StressState.Neutral;
    public StressState state
    {
        get {return ss;}
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
