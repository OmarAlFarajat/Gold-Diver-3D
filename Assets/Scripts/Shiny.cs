using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shiny : MonoBehaviour
{
    private Rigidbody rigidbody;
    private BoxCollider boxCollider;
    private ConstantForce constantForce;

    private float spawnTime = 0f;
    public float lifeTime = 5f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        constantForce = GetComponent<ConstantForce>();
        rigidbody.freezeRotation = true;
        spawnTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        resolveVerticalForces();

        // Destroy after a certain time
        if (Time.time - spawnTime > lifeTime)
        {
            Destroy(gameObject);
        }
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

}
