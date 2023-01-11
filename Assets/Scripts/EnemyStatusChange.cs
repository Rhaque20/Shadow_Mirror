using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusChange : StatusChanges
{
    // Start is called before the first frame update
    Overdrive od;

    void Start()
    {
        effects = new Dictionary<string,StatusEffect>();
        sd = this.transform.Find("EnemyUI/Statuses").gameObject.GetComponent<StatusDisplay>();
        parameters = GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
