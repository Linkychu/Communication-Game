using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Class
{
    Enemy,
    Ally
}


public enum PlayerClassBase
{
    None,
    Warrior,
    Mage,
    Tank,
    Healer
};
[CreateAssetMenu(fileName = "CharacterBase", menuName = "Data/Character")]
public class CharacterBase : ScriptableObject
{
    public string Name;
    public int BaseHP;
    public int BaseAttack;
    public int BaseDefense;
    public int BaseSpecialAttack;
    public int BaseSpecialDefence;
    public int BaseSpeed;
    public int BaseManaAmount;
    private int level;
    public Affinities type;
    public Class myStatus;
    public PlayerClassBase BaseClass;
    public TextAsset DeathMessage;
    
}