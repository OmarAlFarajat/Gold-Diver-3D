using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float spawnTime = 0f;
    [SerializeField]
    public float lifeTime = 5f;
    private PlayerController player;
    void Start()
    {
        spawnTime = Time.time;
        player = FindObjectOfType<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        // Destroy after a certain amount of time
        if (Time.time - spawnTime > lifeTime)    
            Destroy(gameObject);        
    }

    // Hurt the player when the projectile collides
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.hurt();
            Destroy(gameObject);
        }
    }
}
