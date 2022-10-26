using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = SeededRandom.random.Next(n+ 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    public static int GreatestCommonDenominator(int a, int b)
    {
        while (a != 0 && b != 0)
        {
            if (a > b)
            {
                a %= b;
            }

            else
            {
                b %= a;
            }
        }

        return a == 0 ? b : a;
        
    }
}