using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFunctionNode : MonoBehaviour
{
    public EnemyScan es;
    public Telegraph t;
    public EnemyMobile em;
    public Charger charge;
    public EnemyAI ea;
    public EnemyStats stats;

    private int j = 0;

    void AccessReturnNeutral()
    {
        es.ReturnNeutral();
    }

    void AccessAttackScan(int i)
    {
        es.AttackScan(i);
    }

    void NonAttackScan(int i)
    {
        es.NonAttackScan(i);
    }

    void AttackRecover()
    {
        charge.AttackRecover(true);
    }

    void AccessLaunch()
    {
        charge.Launch();
    }

    void Death()
    {
        stats.Death();
    }

    void SetJ(int val)
    {
        Debug.Log("Setting J");
        j = val;
    }

    void LungeForward()
    {
        charge.LungeForward();
    }

    void SummonTelegraphs(int i)
    {
        Debug.Log("Setting telegraph on function node");
        ea.SummonTelegraphs(i,j);
    }

    void ReleaseAttack()
    {
        ea.ReleaseAttack();
    }
}
