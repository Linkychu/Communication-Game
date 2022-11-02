using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public BossData data;

    private Animator _animator;

    private int animLayer = 0;

    private bool canStart;

    private Moves currentEffect;

    private BossClass myClass;

    public float AttackRange = 3f;

    internal NavMeshAgent agent;

    private Transform playerTarget;

    private AttackEffect LeftArm;
    public GameObject Canvas;
    private AttackEffect RightArm;
    
    public enum BossStates
    {
        Idle,
        Chasing,
        Attacking
    }
    
    public BossStates state;

    public Moves PunchEffect;
    // Start is called before the first frame update
    void Start()
    {
        data = (BossData)Instantiate(data);
        FindObjectOfType<BossTriggerScript>().OnPlayerEnterTrigger += OnPlayerEnterTrigger;
        _animator = GetComponent<Animator>();
        _animator.speed = 0;
        myClass = GetComponent<BossClass>();
        agent = GetComponent<NavMeshAgent>();
        LeftArm = GameObject.FindWithTag("ArmL").GetComponent<AttackEffect>();
        RightArm = GameObject.FindWithTag("ArmR").GetComponent<AttackEffect>();
        LeftArm.gameObject.SetActive(false);
        RightArm.gameObject.SetActive(false);
        myClass.OnBossDeath += OnBossDeath;
        
    }

    private void OnBossDeath()
    {
       Destroy(gameObject);
    }

    private void OnPlayerEnterTrigger(object sender, EventArgs e)
    {
        StartFight();
    }

    void StartFight()
    {
        _animator.speed = 1;
        canStart = true;
        Canvas.SetActive(true);
    }


    public void SetTarget(Transform target)
    {
        playerTarget = target;
    }

    public void ResetTarget()
    {
        playerTarget = null;
    }

    public void AttackTarget(int index)
    {
        if (playerTarget == null) return;
        LeftArm.SetUser(myClass);
        RightArm.SetUser(myClass);
        var effect = Instantiate(PunchEffect, playerTarget.position, Quaternion.identity);
        LeftArm.Spawn(effect, false);
        RightArm.Spawn(effect, false);
        Destroy(effect, 0.1f);

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchState(BossStates enterState)
    {
        state = enterState;
    }
}
