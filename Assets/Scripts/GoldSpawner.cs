using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldSpawner : MonoBehaviour
{
    public GameObject goldSmall, goldMedium, goldLarge;
    public GameObject bubbles;

    [SerializeField]
    public float spawnRate = 5f;

    float nextSpawn = 0f;

    int spawnSelect;

    void Update()
    {
        if (Time.time > nextSpawn)
        {
            // Determine a position with some randomness, but ensure that they spawn at the bottom
            Vector3 newPosition = transform.position;
            newPosition.x = Random.Range(-22, 22);
            newPosition.y = 1;
            newPosition.z = Random.Range(-22, 22);
            transform.position = newPosition;

            // Choose a random gold type to spawn
            spawnSelect = Random.Range(0, 3);
            switch (spawnSelect)
            {
                case 0:
                    Instantiate(goldSmall, transform.position, Quaternion.identity).transform.Rotate(90, 0, 0);
                    Instantiate(bubbles, gameObject.transform.position, gameObject.transform.rotation).transform.Rotate(-90,0,0);      
                    break;
                case 1:
                    Instantiate(goldMedium, transform.position, Quaternion.identity).transform.Rotate(90, 0, 0);
                    Instantiate(bubbles, gameObject.transform.position, gameObject.transform.rotation).transform.Rotate(-90, 0, 0);
                    break;
                case 2:
                    Instantiate(goldLarge, transform.position, Quaternion.identity).transform.Rotate(180, 0, 0);
                    Instantiate(bubbles, gameObject.transform.position, gameObject.transform.rotation).transform.Rotate(-90, 0, 0);
                    break;
            }
            // Timer for next spawn
            nextSpawn = Time.time + spawnRate;
        }




    }
}
