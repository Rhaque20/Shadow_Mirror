using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariables : MonoBehaviour
{
    //Obtainable on self
    public EnemyVariables ev;
    public EnemyStats stats;
    public AdvancedStats a_Stats;
    public Forces force;
    public EnemyMobile movement;
    public EnemyAI enemy_AI;
    public EnemyScan enemy_Scan;
    public Charger charger;
    public EnemyStatusChange enemyStatus;
    public EnemyDamageContact edc;
    public Collider2D collider;
    public Rigidbody2D groundrigid;

    //Obtainable on Model
    public Animator anim;
    public Rigidbody2D bodyrigid;
    public EnemyCore ec;
    public Overdrive overDrive;
    public SpriteList sl;
    // Start is called before the first frame update
    void Start()
    {
        ev = GetComponent<EnemyVariables>();

        stats = GetComponent<EnemyStats>();
        stats.ev = ev;

        a_Stats = GetComponent<AdvancedStats>();
        force = GetComponent<Forces>();

        movement = GetComponent<EnemyMobile>();
        movement.ev = ev;

        enemy_AI = GetComponent<EnemyAI>();
        enemy_AI.ev = ev;

        enemy_Scan = GetComponent<EnemyScan>();
        enemy_Scan.ev = ev;

        charger = GetComponent<Charger>();
        charger.enemyVariable = ev;
        
        enemyStatus = GetComponent<EnemyStatusChange>();
        edc = GetComponent<EnemyDamageContact>();
        collider = GetComponent<Collider2D>();
        groundrigid = GetComponent<Rigidbody2D>();


        // Getting from model
        GameObject g = transform.GetChild(0).GetChild(0).gameObject;

        ec = g.GetComponent<EnemyCore>();
        ec.ev = ev;
        

        anim = g.GetComponent<Animator>();
        bodyrigid = g.GetComponent<Rigidbody2D>();

        overDrive = g.GetComponent<Overdrive>();
        overDrive.ev = ev;

        sl = g.GetComponent<SpriteList>();

        
        ec.LoadUp();
        enemy_AI.LoadUp();
    }
}
