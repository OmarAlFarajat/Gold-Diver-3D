using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public GameObject bubbles;

    private float spawnTime = 0f;
    [SerializeField]
    public float lifeTime = 10f;

    void Start()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        // Destroy gold after a given duration
        if (Time.time - spawnTime > lifeTime)
        {
            Destroy(gameObject);
            Instantiate(bubbles, gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    // If it collides with the player, give them the appropriate amount of gold. 
    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            if (gameObject.tag == "GoldSmall")
            {
                other.attachedRigidbody.mass += 1;
                PlayerController.goldCarried += 1;
            }
            else if (gameObject.tag == "GoldMedium")
            {
                other.attachedRigidbody.mass += 2;
                PlayerController.goldCarried += 2;
            }
            else if (gameObject.tag == "GoldLarge")
            {
                other.attachedRigidbody.mass += 10;
                PlayerController.goldCarried += 10;
            }
            Instantiate(bubbles, gameObject.transform.position, gameObject.transform.rotation);

            Destroy(gameObject);
            //Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
        }

    }
}
