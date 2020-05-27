using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : MonoBehaviour
{
    Rigidbody rigidbody;
    public GameObject bubbles;
    private PlayerController player;
    public GameObject projectile; 

    public float speed = 0f;

    // 
    private float spawnTime = 0;
    private float despawnTime = 0;
    private int lastLevel = 0;

    private float shootTime = 0; 
    private float projectileCooldown = 3; 

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>();

        spawnTime = Time.time;
        despawnTime = Random.Range(8, 12);
        shootTime = Time.time;

        // Adds additional speed based on the difficulty level
        float levelSpeed = 0;
        if (PlayerController.level > lastLevel)
            for (int i = 0; i < PlayerController.level; i++)
                levelSpeed += 0.25f;
        lastLevel = PlayerController.level;
        speed = Random.Range(1f + levelSpeed, 2f + levelSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        // Move towards player
        if (player != null)
        {
            transform.LookAt(player.transform.position);
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            // If shooting cooldown is done, shoot a projectile at the player
            if (Time.time - shootTime > projectileCooldown)
            {
                shootTime = Time.time;
                Instantiate(projectile, gameObject.transform.position + transform.forward.normalized, gameObject.transform.rotation).GetComponent<Rigidbody>().AddForce(transform.forward.normalized * 20.0f, ForceMode.Impulse); ;
            }

        }

        // Despawn after a certain time
        if (Time.time - spawnTime > despawnTime)
        {
            Instantiate(bubbles, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }

    // Hurt player on collision
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.hurt();

        }

    }

}
