using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnTimeline : ScriptableObject
{
    public SpawnOrder[] spawnOrders;
    int currentIndexPos;
    public SpawnOrder GetSpawnOrder(float time)
    {
        if(spawnOrders[currentIndexPos+1].time > time)
        {
            currentIndexPos++;
            return spawnOrders[currentIndexPos];
        } else
        {
            return new SpawnOrder(0, -1);
        }
    }
}
