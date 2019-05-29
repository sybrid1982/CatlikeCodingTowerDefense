using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    // An Enemy Type will need at least a speed, a model, and a material
    // Health is an obvious addition if we're going to have damage being done
    [SerializeField]
    public float speed = 1f;
    [SerializeField]
    public int health = 50;
    [SerializeField]
    public Mesh mesh;
    [SerializeField]
    public Material material;
    [SerializeField]
    public float scale = 1f;
    // We can then pass this scriptable object to newly generated enemies from the factory to set the initial values;
}
