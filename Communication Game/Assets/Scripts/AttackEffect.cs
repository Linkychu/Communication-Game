using System;
using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine;
using Characters;


public class AttackEffect : MonoBehaviour
{
    private Rigidbody rb;

    private SphereCollider _collider;

    private bool shouldMove;

    private CharacterClass _class;

    private Moves moveRef;

    private bool spawned = false;
    // Start is called before the first frame update
    void Start()
    {
        
       
    }

    public void Spawn(Moves move, bool ShouldMove)
    {
        _collider = GetComponent<SphereCollider>();

        if (!move.isDynamicRanged)
        {
            _collider.radius = move.range;
        }

        Destroy(this.gameObject, move.lifetime);
        
        moveRef = move;

        shouldMove = ShouldMove;

        spawned = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(!spawned)
            return;
        if(shouldMove)
            return;
        transform.Translate(0,0,0);
    }
    
    public void SetUser(CharacterClass @class)  
    {
        _class = @class;
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterClass enemyClass = other.GetComponent<CharacterClass>();
        if(enemyClass == null)
            return;
        if (_class.charBase.myStatus == enemyClass.charBase.myStatus)
            return;
        IDamageable damageable = other.GetComponent<IDamageable>();
        damageable.Damage(DamageManager.instance.DamageCalculator(moveRef.baseDamage, _class.values.myStats.Attack, enemyClass.values.myStats.MaxHP,
            enemyClass.values.myStats.Defense, _class.values.myStats.SpecialAttack, enemyClass.values.myStats.SpecialDefence, _class.attributes.A_Modifier, _class.attributes.D_Modifier, _class.attributes.S_Modifier_A, moveRef.type,
            moveRef.affinity, enemyClass.charBase.type, _class.values.myStats.level, enemyClass.values.myStats.level, enemyClass.attributes.D_Modifier, enemyClass.attributes.S_Modifier_A));
       
    }
}