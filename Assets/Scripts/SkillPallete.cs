using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPallete : MonoBehaviour
{
    public Skill[] groundskills = new Skill[4];
    public Skill[] airskills = new Skill[4];
    int skillchain = 0;
    public Vector2 skillQTE;// x = 0/1 for ground or air and y = 0 - 3 for index of skill;
    public int chainreq = 0;
    [SerializeField]NAAnims naa;
    public int skillsel = 0;
    public bool QTEsummon = false;

    public void ActivateQTE()
    {
        skillsel = (int)skillQTE.y + 1;
        QTEsummon = true;
        if (skillQTE.x == 0f)
            naa.PlayGenericSkill(groundskills[(int)skillQTE.y].attackAnim,true);
        else
            naa.PlayGenericSkill(airskills[(int)skillQTE.y].attackAnim,true);
    }

    public void QTEvanish()
    {
        if (QTEsummon)
        {
            QTEsummon = false;
            gameObject.SetActive(false);
        }
    }
}
