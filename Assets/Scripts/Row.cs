using UnityEngine;

public class Row : MonoBehaviour
{
    public Tile[] _tiles { get; private set; }

    private void Awake() => _tiles = GetComponentsInChildren<Tile>();
}