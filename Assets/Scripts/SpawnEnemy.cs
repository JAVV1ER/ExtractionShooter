using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = Unity.Mathematics.Random;

public class SpawnEnemy :MonoBehaviour
{
    public GameObject enemy;
    private Collider2D _collider2D;
    
    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        var bounds = _collider2D.bounds;
        Random random = new Random();
        random.InitState();
        var posX = random.NextFloat(bounds.min.x, bounds.max.x);
        var posY = random.NextFloat(bounds.min.y, bounds.max.y);
        Instantiate(enemy, new Vector3(posX,posY), Quaternion.identity);
    }
    
}