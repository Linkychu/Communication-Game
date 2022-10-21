using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;
public class SeededRandom : MonoBehaviour
{
    public static SeededRandom instance {get; set;}
    public int seed;
    public bool generateRandom;
    public static Random random;
    
    void Awake()
    {
        instance = this;

        GenerateSeed();
    }
    
    public static int Range(int min, int max)
    {
        int x = random.Next(min, max);
        return x;
    }

    public static int Value()
    {
        int x = random.Next();

        return x;
    }
    
    void GenerateSeed()
    {
        if (generateRandom)
        {
            seed = (int) System.DateTime.Now.Ticks;
        }


        random = new Random(seed);  
        UnityRandom.InitState(seed);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
