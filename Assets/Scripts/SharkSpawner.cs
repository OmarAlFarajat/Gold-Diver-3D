using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkSpawner : MonoBehaviour
{

    public GameObject shark;
    public GameObject bubbles;

    public float spawnRate = 0f;
    float nextSpawn = 0f;

    void Update()
    {
        // Spawn rate changes based on difficulty level
        spawnRate = 5f / (PlayerController.level * 0.75f);

        // Ensures that spawning only occurs at a given time interval
        if (Time.time > nextSpawn)
        {
            // Calculate a new random position in the scene in front of the bounds in the -z direction
            Vector3 newPosition = transform.position;
            newPosition.x = Random.Range(-20, 20);
            newPosition.y = Random.Range(2, 25);
            newPosition.z = -20f;
            transform.position = newPosition;

            // Creates a shark of a random size and produce some bubbles when it spawns
            float scale = Random.Range(0.4f, 0.8f);
            (Instantiate(shark, transform.position, Quaternion.identity)).transform.localScale = new Vector3(scale, scale, scale);
            Instantiate(bubbles, gameObject.transform.position, gameObject.transform.rotation);

            // Set the time for the next spawn
            nextSpawn = Time.time + spawnRate;
        }
    }
}
