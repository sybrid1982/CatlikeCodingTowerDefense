using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public Enemy Enemy { get; private set; }

    public Vector3 Position => transform.position;

    void Awake ()
    {
        Enemy = transform.root.GetComponent<Enemy>();
        Debug.Assert(Enemy != null, "Target Point without Enemy root!", this);
        Debug.Assert(GetComponent<SphereCollider>() != null,
            "Target Point without Sphere Collider", this);
        Debug.Assert(gameObject.layer == 9, "Target Point on wrong layer!", this);
    }
}
