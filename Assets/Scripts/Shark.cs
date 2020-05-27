using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    private PlayerController player;
    public GameObject bubbles;
    private Collider other;
    private Rigidbody rigidbody;
    private Vector3 initialLookAt;

    private float speed = 0.0f;
    private float spawnTime = 0;
    private float despawnTime = 0;
    private int lastLevel = 0;
    [SerializeField]
    public bool shiny = false;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        rigidbody = GetComponent<Rigidbody>();

        spawnTime = Time.time;
        despawnTime = Random.Range(30, 40);

        // Adds additional speed based on the difficulty level
        float levelSpeed = 0;
        if(PlayerController.level > lastLevel)
        for (int i = 0; i < PlayerController.level; i++)
            levelSpeed += 0.25f;
        lastLevel = PlayerController.level;
        speed = Random.Range(2f + levelSpeed, 3f + levelSpeed);

        // Used to get the shark back to moving straight after the shiny object disappears
        initialLookAt = transform.position; 
        initialLookAt.z = 25f;

        rigidbody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {

        // "other" is assigned a reference to the shiny object, if it doesn't exist (null), then set bool to false
        if (!other)
            shiny = false; 

        // If there is no shiny object nearby, then move normally
        if (!shiny)
        {
            rigidbody.velocity = new Vector3(0, 0, transform.localScale.z * speed);
            transform.LookAt(new Vector3(transform.position.x, transform.position.y, 25f));

        }
        // If there is a shiny object, then move towards it while looking in its direction
        else
        {
            rigidbody.velocity = (other.gameObject.transform.position - transform.position).normalized * speed;
            transform.LookAt(other.gameObject.transform.position);
        }

        // Despawn after a certain time
        if (Time.time - spawnTime > despawnTime)
        {
            Instantiate(bubbles, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
        worldWrap();
    }
    
    // Wrap the shark to the other side of the cube when it gets to a certain location 
    void worldWrap()
    {
        if (transform.position.z > 22f)
            transform.position = new Vector3(transform.position.x, transform.position.y, -22.5f);
    }

    // When a shiny object is nearby, the shark will collide with its trigger and assign a reference which is used in update() above. 
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Shiny")
        {
            this.other = other; 
            shiny = true;
        }

    }

    // Hurt the player when the shark collides with the player
        void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.hurt();
        }

    }


}
