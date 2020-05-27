using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Text GoldCarried, GoldCollected, OxygenTanks, Lives, Level;

    private Rigidbody rigidbody;
    private ConstantForce constantForce;
    private BoxCollider boxCollider;
    private Camera camera;
    public GameObject bubbles;
    public GameObject shiny;
    public static Vector3 playerPosition; 

    private float moveForce = 450f;
    public bool moving = false;
    float moveDelay = 0.5f;
    public static int goldCarried = 0;
    public static int goldCollected = 0;
    [SerializeField]
    public static int oxygenTanks = 3;
    [SerializeField]
    public static int lives = 2;
    [SerializeField]
    public static int level = 1;
    private float startTime = 0;
    private float timeInterval = 30;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        constantForce = GetComponent<ConstantForce>();
        boxCollider = GetComponent<BoxCollider>();
        camera = GetComponentInChildren<Camera>();
        playerPosition = transform.position;
        rigidbody.freezeRotation = true;

        GoldCarried.text = goldCarried.ToString();
        GoldCollected.text = goldCollected.ToString();
        Level.text = level.ToString();
        Lives.text = lives.ToString();
        OxygenTanks.text = oxygenTanks.ToString();
    }

    void Update()
    {
        // Mouse look
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Update stack (see method comments below)
        controllerManager();
        resetMoveTimer();
        resolveVerticalForces();
        worldWrap();
        playerPosition = transform.position;
        levelTiming();

        // Update text information
        GoldCarried.text = goldCarried.ToString();
        GoldCollected.text = goldCollected.ToString();
        Level.text = level.ToString();
        Lives.text = lives.ToString();
        OxygenTanks.text = oxygenTanks.ToString();
    }
   
    // Resolves gravity and buoyant forces with constant force component (archimedes principle)
    void resolveVerticalForces()
    {
        float displacedArea;
        if (transform.position.y < 35f)
        {
            displacedArea = boxCollider.size.x * boxCollider.size.z * (35f - transform.position.y);
            rigidbody.drag = 0.75f;
            rigidbody.angularDrag = 0.75f;
        }
        else
        {
            displacedArea = 0f;
            rigidbody.drag = 0.0f;
            rigidbody.angularDrag = 0.0f;
        }
        Vector3 gravityForce = new Vector3(0, rigidbody.mass * Physics.gravity.y, 0);
        Vector3 buoyantForce = new Vector3(0, 5f * displacedArea * -Physics.gravity.y, 0);
        constantForce.force = gravityForce + buoyantForce;
    }

    // Method for inputs. Some position logic is used to ensure the player can't float out passed the water surface
    void controllerManager()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            Instantiate(shiny, gameObject.transform.position + camera.transform.forward, gameObject.transform.rotation).GetComponent<Rigidbody>().AddForce(camera.transform.forward*20.0f, ForceMode.Impulse);
        }


        if (Input.GetAxisRaw("Horizontal") > 0f && !moving)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            rigidbody.AddForce(transform.right * moveForce, ForceMode.Impulse);
            moving = true;
            if(transform.position.y < 30f)
                Instantiate(bubbles, gameObject.transform.position, gameObject.transform.rotation);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f && !moving)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            rigidbody.AddForce(-transform.right * moveForce, ForceMode.Impulse);
            moving = true;
            if (transform.position.y < 30f)
                Instantiate(bubbles, gameObject.transform.position, gameObject.transform.rotation);
        }
        else if (Input.GetAxisRaw("Vertical") > 0f && !moving)
        {
            rigidbody.AddForce(camera.transform.forward * moveForce, ForceMode.Impulse);
            moving = true;
            if (transform.position.y < 30f)
                Instantiate(bubbles, gameObject.transform.position, gameObject.transform.rotation);
        }
        else if (Input.GetAxis("Vertical") < 0f && !moving)
        {
            rigidbody.AddForce(-camera.transform.forward * moveForce, ForceMode.Impulse);
            moving = true;
            if (transform.position.y < 30f)
                Instantiate(bubbles, gameObject.transform.position, gameObject.transform.rotation);
        }
        else if (Input.GetAxis("Jump") > 0f && !moving && transform.position.y < 35f)
        {
            rigidbody.AddForce(transform.up * moveForce*1.5f, ForceMode.Impulse);
            moving = true;
            if (transform.position.y < 30f)
                Instantiate(bubbles, gameObject.transform.position, gameObject.transform.rotation);
        }

    }

    // Reset movement timer, this makes sure the movement isn't continuous
    void resetMoveTimer()
    {
        if (moveDelay <= 0)
        {
            moveDelay = 1f;
            moving = false;
        }
        else if (moving)
        {
            moveDelay -= Time.deltaTime;
        }
    }

    // Wrap the player to the other side of the cube when it gets to a certain location 
    void worldWrap()
    {
        if (transform.position.x > 24f)
            transform.position = new Vector3(-24f, transform.position.y, transform.position.z);
        else if (transform.position.x < -24f)
            transform.position = new Vector3(24f, transform.position.y, transform.position.z);
        else if (transform.position.z > 24f)
            transform.position = new Vector3(transform.position.x, transform.position.y, -24f);
        else if (transform.position.z < -24f)
            transform.position = new Vector3(transform.position.x, transform.position.y, 24f);
    }

    // Controls how frequently the difficulty level increases
    void levelTiming() {
        if (Time.time - startTime > timeInterval)
        {
            startTime = Time.time;
            level++;
        }
    }

    // Use with collisions to inflict damage on the player and also handles respawn and death logic 
    public void hurt()
    {
        oxygenTanks--;
        OxygenTanks.text = oxygenTanks.ToString();

        if (oxygenTanks == 0)
        {
            if (lives == 0)
            {
                SceneManager.LoadScene("Death");
            }
            else
            {
                lives--;
                oxygenTanks = 3;
                rigidbody.mass -= goldCarried;
                goldCarried = 0;
                transform.position = new Vector3(0.0f, 36.78f, -1.15f);
            }
        }



    }

    // Transfer gold to submarine and ensure that player's mass is adjusted. 
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Submarine")
        {
            goldCollected += goldCarried;
            rigidbody.mass -= goldCarried;
            goldCarried = 0;
        }
    }

}
