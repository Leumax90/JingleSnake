using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Transform ThisTransform;
    public void Start()
    {
        ThisTransform = transform;
        StartCoroutine(Obstacles());
    }
    IEnumerator Obstacles()
    {
        while (true)
        {
            var randPosx = Random.Range(-10, 10);
            var randPosz = Random.Range(-10, 10);

            ThisTransform.position = new Vector3(randPosx, 0.1f, randPosz);
            yield break;
        }
    }
}
