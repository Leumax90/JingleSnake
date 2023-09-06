using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class SnakeManager : MonoBehaviour
{
    private SnakeModel cubeModel;
    public SnakeBlock blockReference;
    private SnakeBlock cubeFruit;
    private List<GameObject> Obstacle;
    public ParticleSystem explosionParticle;
    public GameObject gameOver;
    public GameObject winner;
    public GameObject restart;
    private float waitTime = 0.5f;
    private readonly float outFence = 10.2f;

    public void Start()
    {
        // find the model
        cubeModel = FindObjectOfType<SnakeModel>();

        // find the model Obstacles
        Obstacle = GameObject.FindGameObjectWithTag("ObstaclePool").GetComponent<ObstacleManager>().Obstacles;

        // create the 2 initial blocks
        var obj = Instantiate(blockReference, transform.position, Quaternion.identity);
        cubeModel.SnakeBlocks.Add(obj);

        obj = Instantiate(blockReference, new Vector3(-1, 0.5f, 0), Quaternion.identity);
        cubeModel.SnakeBlocks.Add(obj);

        // create the fruit
        cubeFruit = Instantiate(blockReference, new Vector3(Random.Range(-10, 10), 0.5f, Random.Range(-10, 10)), Quaternion.identity);
        StartCoroutine(Move());
    }

    public void Update()
    {
        // input controller for PC
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(0, -90, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            transform.Rotate(0, 90, 0);
        }
    }

    // input controller for Android
    public void Left() { transform.Rotate(0, -90, 0); }

    public void Right() { transform.Rotate(0, 90, 0); }

    IEnumerator Move()
    {
        while (true)
        {
            // wait for seconds, and thus sets the Speed
            yield return new WaitForSeconds(waitTime);
            // move body
            for (int i = cubeModel.SnakeBlocks.Count - 1;
                i >= 1; i--)
                cubeModel.SnakeBlocks[i].transform.position =
                    cubeModel.SnakeBlocks[i - 1].transform.position;
            // move head
            Vector3 newpos = cubeModel.SnakeBlocks[0].transform.position + transform.forward;

            // check collision if distance between fruit and head <0.95
            if (Vector3.Distance(newpos, cubeFruit.transform.position) < 0.95)
            { // turn fruit to snake, and create new one
                cubeModel.SnakeBlocks.Insert(0, cubeFruit);
                cubeFruit = Instantiate(blockReference, new Vector3(Random.Range(-10, 10), 0.5f, Random.Range(-10, 10)), Quaternion.identity);
                UpdateWaitTime();
            }
            else // move
                cubeModel.SnakeBlocks[0].transform.position = newpos;

            Vector3 posHead = cubeModel.SnakeBlocks[0].transform.position;
            // if the snake goes out of range, it restarts
            if (posHead.x >= outFence || posHead.x <= -outFence || posHead.z >= outFence || posHead.z <= -outFence)
            {
                Explosion();
                yield return new WaitForSeconds(2);
                Restart();
            }
            // if the snake bites itself, the game restarts
            for (int i = 4; i < cubeModel.SnakeBlocks.Count; i++)
            {
                if (Vector3.Distance(posHead, cubeModel.SnakeBlocks[i].transform.position) < 0.5)
                {
                    Explosion();
                    yield return new WaitForSeconds(2);
                    Restart();
                }
            }

            // if the snake hits an obstacle, the game restarts
            for (int i = 0; i < Obstacle.Count; i++)
            {
                if (Obstacle[i].activeSelf == true && Vector3.Distance(posHead, Obstacle[i].transform.position) < 0.5)
                {
                    Explosion();
                    yield return new WaitForSeconds(2);
                    Restart();
                }
            }
            //if the snake gets 10 pieces, victory!
            if (cubeModel.SnakeBlocks.Count >= 10)
            {
                youWin();
                yield break;
            }

        }
    }

    // every time you eat fruit, increase the speed
    private void UpdateWaitTime()
    {
        if (waitTime > 0.15)
            waitTime -= 0.1f;
    }

    // gameover
    void Explosion()
    {
        gameOver.SetActive(true);
        explosionParticle.transform.position = cubeModel.SnakeBlocks[1].transform.position;
        cubeModel.SnakeBlocks[0].gameObject.SetActive(false);
        explosionParticle.Play();
    }
    public void Restart() //Restarts the level
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void youWin()
    {
        winner.SetActive(true);
        restart.SetActive(true);
    }
}
