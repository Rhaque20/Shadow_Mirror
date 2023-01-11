using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptSystem : MonoBehaviour
{
    public enum ArmorType {Neutral, SuperArmor,HyperArmor};
    protected ArmorType armor = ArmorType.Neutral;
    [SerializeField]protected Stats s;
    [SerializeField]protected Animator anim;
    [SerializeField]protected AnimatorOverrideController aoc;
    [SerializeField]protected AnimationClip[] statusAnim = new AnimationClip[4];// Handles the several overdrive animation overrides
    protected Coroutine inStagger = null;
    public enum FlinchType {Neutral, Knockback, Launch};
    public FlinchType flinchMode = FlinchType.Neutral;

    public Stats a_stat
    {
        get{return s;}
        //set{s = value;}
    }

    public ArmorType armormode
    {
        set{armor = value;}
        get{return armor;}
    }


    public virtual IEnumerator Flinch(float staggertime)
    {
        yield return new WaitForSeconds(staggertime);

    }

    public virtual void TriggerKOAnim()
    {

    }

    public virtual void TriggerStagger()
    {
        
    }

    public virtual void TriggerStagger(Vector2 force, bool isLeft)
    {
        
    }

    public virtual void AirLaunch(Vector2 force, float launchpower, bool isLeft)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
