using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawingPoints : MonoBehaviour
{
    public Transform spawnPoints;
    Transform[] spawnPositions;
    // Start is called before the first frame update
    void Start()
    {
        spawnPositions = spawnPoints.GetComponentsInChildren<Transform>();
        //Debug.Log(spawnPositions.Length);
    }

    // Update is called once per frame
    void Update()
    {
        RandomSpawn();
    }
    private void RandomSpawn()
    {
        int i = UnityEngine.Random.Range(1, spawnPositions.Length);

        //Debug.Log(spawnPositions[i].transform.position);
        transform.position = spawnPositions[i].transform.position;

    }
}
