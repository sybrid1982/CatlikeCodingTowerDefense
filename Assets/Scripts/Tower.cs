﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum TowerType
{
    Laser, Mortar
}
public abstract class Tower : GameTileContent
{
    [SerializeField, Range(1.5f, 10.5f)]
    protected float targetingRange = 1.5f;
    const int enemyLayerMask = 1 << 9;

    static Collider[] targetsBuffer = new Collider[100];

    public abstract TowerType TowerType { get; }

    protected bool TrackTarget(ref TargetPoint target)
    {
        if (target == null)
        {
            return false;
        }
        Vector3 a = transform.localPosition;
        Vector3 b = target.Position;
        float x = a.x - b.x;
        float z = a.z - b.z;
        float r = targetingRange + 0.125f;

        if (x * x + z * z > r * r)
        {
            target = null;
            return false;
        }
        return true;
    }

    protected bool AcquireTarget(out TargetPoint target)
    {
        Vector3 a = transform.localPosition;
        Vector3 b = a;
        b.y += 3f;
        int hits = Physics.OverlapCapsuleNonAlloc(
            a, b, targetingRange, targetsBuffer, enemyLayerMask
        );
        if(hits > 0)
        {
            target = targetsBuffer[Random.Range(0, hits)].GetComponent<TargetPoint>();
            Debug.Assert(target != null, "Targeted non-Enemy!", targetsBuffer[0]);
            return true;
        }
        target = null;
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, targetingRange);
    }
}
