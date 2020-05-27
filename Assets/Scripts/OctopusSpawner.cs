using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusSpawner : MonoBehaviour
{


    public GameObject octopus;
    public float spawnRate = 0f;
    float nextSpawn = 0f;
    public GameObject bubbles;

    // Update is called once per frame
    void Update()
    {
        // Spawn rate changes based on difficulty level
        spawnRate = 20f / (PlayerController.level * 0.75f);    

        if (Time.time > nextSpawn)
        {
            // Calculate a new random position in the scene in front of the bounds in the -z direction
            Vector3 newPosition = transform.position;
            float buffer = 1f;
            newPosition.x = Random.Range(-20, 20);
            newPosition.y = Random.Range(2, 25);
            newPosition.z = -20f;
            transform.position = newPosition;

            // Creates an octopus and produce some bubbles when it spawns
            Instantiate(octopus, transform.position, Quaternion.identity);
            Instantiate(bubbles, gameObject.transform.position, gameObject.transform.rotation);

            // Set the time for the next spawn
            nextSpawn = Time.time + spawnRate;
        }
    }
}
