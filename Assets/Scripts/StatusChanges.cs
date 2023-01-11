using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatusChanges : MonoBehaviour
{
    public bool debuffed = false, buffed = false, dot = false;
    public bool enemy = false;
    public int debuffct = 0,buffct = 0,val = 0, counter = 0;
    //StatusEffect[] effects;
    protected Dictionary<string,StatusEffect> effects;
    [SerializeField]protected StatusDisplay sd;
    Coroutine[] effecttimers = new Coroutine[10];
    public StatusEffect s1,s2;
    public Stats parameters;

    [SerializeField]protected StatusText st;

    public StatusDisplay a_sd
    {
        get{return sd;}
        set{sd = value;}
    }

    public Dictionary<string,StatusEffect> eff
    {
        get{return effects;}
    }


    protected IEnumerator Duration(StatusEffect effect, GameObject icon, int i)
    {
        //Image time = GetComponentInChildren(typeof(Image)) as Image;
        GameObject timer = icon.transform.GetChild(0).gameObject;
        Image time = timer.GetComponent<Image>();
        float draintime = 1f;
        
        while (effect.duration > 0f)
        {
            yield return new WaitForSeconds(draintime);
            effect.duration -= draintime;
            time.fillAmount = effect.duration/effect.maxduration;
        }
        effecttimers[effect.index] = null;
        effects.Remove(effect.name);
        CounterChange(effect.type,false);
        sd.ClearIcon(i);
    }

    public void CounterChange(StatusEffect.Category type, bool increase)
    {
        if (increase)
        {
            if (type == StatusEffect.Category.buff)
                buffct++;
            if (type == StatusEffect.Category.debuff)
                debuffct++;
        }
        else
        {
            if (type == StatusEffect.Category.buff)
                buffct--;
            if (type == StatusEffect.Category.debuff)
                debuffct--;
        }
    }

    public void ApplyEffect(StatusEffect effect)
    {
        int i;
        //Debug.Log("About to apply "+effect.name);
        if (debuffct + buffct <= 10)
        {
            // If Effect already exists refresh it
            if (effects.ContainsKey(effect.name))
            {
                // Get effect from the holder;
                /**
                StatusEffect temp = effects[effect.name];

                Debug.Log("Contains dupe and temp index is "+ temp.index);
                // End existing coroutine
                StopCoroutine(effecttimers[temp.index]);
                Debug.Log("Derp");
                // Remove any lingering reference to the coroutine
                effecttimers[temp.index] = null;
                
                

                // Decrement buff/debuff count
                CounterChange(effect.type,false);

                // Remove existing Icon
                sd.ClearIcon(temp.index);
                **/

                //Debug.Log("IT exists "+effect.name);
                Remove(effect.name);

            }

            effects.Add(effect.name,effect);// If effect doesn't exist, add it to dictionary

            for (i = 0; i < effecttimers.Length; i++)
            {
                if (effecttimers[i] == null)
                {
                    effect.index = i;
                    Debug.Log("Placing in index: "+effect.index+ " of effect: "+effect.name);
                    GameObject curIcon = sd.AddtoDisplay(effect);
                    TMP_Text level = curIcon.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
                    switch(effect.chain)
                    {
                        case 1:
                            level.text = "I";
                            break;
                        case 2:
                            level.text = "II";
                            break;
                        case 3:
                            level.text = "III";
                            break;
                        default:
                            level.text = "X";
                            break;
                    }
                    effect.duration = effect.maxduration;
                    CounterChange(effect.type,true);
                    effecttimers[i] = StartCoroutine(Duration(effect,curIcon,i));
                    break;
                }
            }
            

        }
        else
        {
            Debug.Log("Total count of buffs + debuffs is "+ (debuffct + buffct));
        }
    }

    public void Remove(string name)
    {
        StatusEffect s;
        if (effects.ContainsKey(name))
        {
            s = effects[name];
            StopCoroutine(effecttimers[s.index]);
            effecttimers[s.index] = null;
            CounterChange(s.type, false);
            sd.ClearIcon(s.index);
            effects.Remove(name);
        }
    }

    public void EffectRemoval(int count, bool searchDebuff)
    {
        StatusEffect s;
        List<string> names = new List<string>();
        if (debuffct + buffct > 0)
        {
            foreach (KeyValuePair<string, StatusEffect> entry in effects) 
            {
                s = entry.Value;


                if (s.type == StatusEffect.Category.debuff && searchDebuff)
                {
                    names.Add(entry.Key);
                    count--;
                }
                else if (s.type == StatusEffect.Category.buff && !searchDebuff)
                {
                    names.Add(entry.Key);
                    count--;
                }

                if (count == 0)
                    break;

            }

            foreach(string key in names)
            {
                Remove(key);
            }

        }
    }

    public void ClearAllType(bool searchDebuff)
    {
        List<string> names = new List<string>();
        StatusEffect s;

        if (debuffct + buffct > 0)
        {
            foreach (KeyValuePair<string, StatusEffect> entry in effects) 
            {
                s = entry.Value;

                if (s.type == StatusEffect.Category.debuff && searchDebuff)
                {
                    names.Add(entry.Key);
                    
                }
                else if (s.type == StatusEffect.Category.buff && !searchDebuff)
                {
                    names.Add(entry.Key);
                }
            }

            foreach(string key in names)
            {
                Remove(key);
            }

           
        }
    }

    public void ApplyCheck(StatusEffect s, float potency)
    {
        
        if (Random.Range(1,100f) <= s.chance)
        {
            if (Random.Range(1,100f) >= (potency - parameters.resistance))
            {
                //Debug.Log("Checking to apply "+s.name);
                ApplyEffect(s);
                //st.DisplayText(s.name);
            }
            else
                Debug.Log("Effect fail!");
        }
    }



    void Start()
    {
        effects = new Dictionary<string,StatusEffect>();
    }

    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            ApplyEffect(s1);
            ApplyEffect(s2);
        }

        if (buffct > 1)
        {
            Debug.Log("There is buff");
        }

    }
}