using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public List<GameObject> Obstacles;

    public void Start()
    {
        StartCoroutine(ObstaclesManager());
    }
    IEnumerator ObstaclesManager()
    {

        while (true)
        {
            var randTime = Random.Range(0, 4);
            yield return new WaitForSeconds(randTime+4);

            for (int i = randTime; i < Obstacles.Count; i++) 
            {
                Obstacles[i].SetActive(true); 
            }

            randTime = Random.Range(0, 4);
            yield return new WaitForSeconds(randTime+4);

            for (int i = randTime; i < Obstacles.Count; i++)
            {
                Obstacles[i].SetActive(false);
            }
        }
    }
}
