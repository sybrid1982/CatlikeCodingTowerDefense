using UnityEngine;
using static GameTile;

public class GameTileContent : MonoBehaviour
{
    [SerializeField]
    GameTileContentType type = default;

    GameTileContentFactory originFactory;

    public GameTileContentFactory OriginFactory
    {
        get => originFactory;
        set {
            Debug.Assert(originFactory == null, "Redefined Origin Factory");
            originFactory = value;
        }
    }

    public void Recycle()
    {
        originFactory.Reclaim(this);
    }

    public GameTileContentType Type => type;
}
