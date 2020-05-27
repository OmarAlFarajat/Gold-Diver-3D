// Simple PIP window camera that follows the player and shows the entire scene
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIPCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(PlayerController.playerPosition);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(PlayerController.playerPosition);
    }
}
