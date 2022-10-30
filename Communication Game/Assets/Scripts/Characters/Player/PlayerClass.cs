using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerClass : CharacterClass, IDamageable, IHealable, IBoostable
{
    public delegate void DeathDelegate();

    public PlayerState playerState;

    public int Level;

    public DeathDelegate player1deathEvent;

    public DeathDelegate player2deathEvent;
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

    
    public override void Death()
    {
        switch (playerState)
        {
            case PlayerState.Player1:
            if (player1deathEvent != null)
            {
                player1deathEvent();
            }
            break;
            
            case PlayerState.Player2: 
                if (player2deathEvent != null)
                {
                    player2deathEvent();
                }
                break;
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
