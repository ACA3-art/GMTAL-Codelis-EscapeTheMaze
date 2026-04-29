using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public BoxCollider2D spawnArea;

    public Vector2 getRandomPosition()
    {
        Vector2 localPos = new Vector2(
            Random.Range(-spawnArea.size.x / 2, spawnArea.size.x / 2),
            Random.Range(-spawnArea.size.y / 2, spawnArea.size.y / 2)
        );

        return spawnArea.transform.TransformPoint(localPos);
    }
}
