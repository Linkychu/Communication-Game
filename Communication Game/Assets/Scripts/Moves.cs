using System;
using UnityEngine;
using UnityEngine.VFX;
using Characters;


public enum MoveType
{
    Physical,
    Special
};
[CreateAssetMenu(fileName = "Move", menuName = "Data/Moves") ]
public class Moves : ScriptableObject
{
    public int baseDamage;
    public MoveType type;
    public Affinities affinity;
    public int manaCost;
    public Vector3 rotationOffset;
    public float range;
    public float lifetime;
    public bool shouldMove;
    public bool isDynamicRanged;
    public GameObject model;
    private CharacterClass user;
    [HideInInspector]
    public AttackEffect effect;
    
    
    
    public void Spawn(Vector3 position, Transform transform, Vector3 rotation)
    {
        Moves move = Instantiate(this);
        var clone = Instantiate(model, position, Quaternion.Euler(rotation + rotationOffset), transform);
        effect = clone.GetComponent<AttackEffect>();
        effect.Spawn(move,shouldMove);
        
    }

    public void setUser(CharacterClass @class)
    {
        effect.SetUser(@class);
    }

        
        
        
        
        
}