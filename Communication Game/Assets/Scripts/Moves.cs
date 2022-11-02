using System;
using UnityEngine;
using UnityEngine.VFX;
using Characters;


public enum MoveType
{
    Physical,
    Special,
    Healing
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
    public int HealHealthAmount;
    public int HealManaAmount;
    private CharacterClass user;
    [HideInInspector]
    public AttackEffect effect;

    [SerializeField]private bool shouldStayInPlace;
    
    
    public void Spawn(Vector3 position, Transform transform, Vector3 rotation, bool isPlayer)
    {
        Moves move = Instantiate(this);
        GameObject clone = null;
        
       
        
        if(isPlayer)
        {
            
            if (shouldStayInPlace)
            {
                clone = Instantiate(model, transform.position , transform.rotation, transform);
            }

            else
            {
                clone = Instantiate(model, transform.position + (transform.forward * position.z) + transform.forward, transform.rotation, transform);

            }
        }

        else
        {
            clone = Instantiate(model, position, Quaternion.identity, transform);
        }
        
        effect = clone.GetComponent<AttackEffect>();
      
        
        effect.Spawn(move,shouldMove);
        
    }

    public void setUser(CharacterClass @class)
    {
        effect.SetUser(@class);
    }

        
        
        
        
        
}