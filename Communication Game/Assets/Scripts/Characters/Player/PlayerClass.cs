using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerClass : CharacterClass, IDamageable, IHealable, IBoostable, IManaHealable
{
    public delegate void DeathDelegate();

    public PlayerState playerState;

    public int Level;

    public bool isDead;

    public GameObject map;

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
    
    public void Revive(int amount)
    {
        if (isDead)
        {
            values.myStats.currentHP = Mathf.FloorToInt((values.myStats.MaxHP / 100) * amount);
            gameObject.SetActive(true);
            isDead = false;
        }
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


        isDead = true;
        gameObject.SetActive(false);
    }
    
}
