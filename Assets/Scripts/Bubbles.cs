using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{
    float duration = 3f;
    void Update()
    {
        // Destroy bubble emitter after a certain duration
        duration -= Time.deltaTime;
        if (duration <= 0)
            Destroy(gameObject);
    }
}
