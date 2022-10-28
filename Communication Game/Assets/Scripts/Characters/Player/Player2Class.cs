using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using General;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Level = General.Level;


public class Player2Class : CharacterClass, IDamageable, IHealable, IBoostable
{
    public delegate void DeathDelegate();


    public int Level;

    private void Awake()
    {
        
        charBase = (CharacterBase) Instantiate(charBase);
        
        gameObject.name = charBase.Name;
        values.myStats.level = Level;
        level = new Level(values.myStats.level, OnLevelUp);

       
        ModifyStat(attributes, 1, 1, 1, 1);
        SetUpCharacter();
    }

    private void LateUpdate()
    {
       
    }

    public DeathDelegate deathEvent;
    public override void Death()
    {
        if (deathEvent != null)
        {
            deathEvent();
        }
        
        Destroy(gameObject);
    }

    public void BoostStats(int amount, StatBoost boost)
    {
        isStatsBoosted = true;
        StartCoroutine(BoostStatsTime(amount, boost));

    }

    IEnumerator BoostStatsTime(int amount, StatBoost boost)
    {
        Attributes boostedAttribute = null;
        switch (boost)
        {
            case StatBoost.Attack:
                attributes.A_Modifier = amount;
                boostedAttribute = attributes;
                isStatsBoosted = true;
                break;
            case StatBoost.Defence:
                attributes.D_Modifier = amount;
                boostedAttribute = attributes;
                isStatsBoosted = true;
                break;
            case StatBoost.SpecialAttack:
                isStatsBoosted = true;
                boostedAttribute = attributes;
                attributes.S_Modifier_A = amount;
                break;
            case StatBoost.SpecialDefence:
                isStatsBoosted = true;
                boostedAttribute = attributes;
                attributes.S_Modifier_D = amount;
                break;
            default:
                isStatsBoosted = false;
                break;
        }

       
        yield return new WaitForSeconds(10);
        if (boostedAttribute != null)
        {
            boostedAttribute.A_Modifier = 1;
            boostedAttribute.D_Modifier = 1;
            boostedAttribute.S_Modifier_A = 1;
            boostedAttribute.S_Modifier_D = 1;
        }

        isStatsBoosted = false;
    }
}
