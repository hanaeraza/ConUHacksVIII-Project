using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPuller : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity != Vector3.zero) {
            Debug.Log("Can't move");
            //playerController.CanMove = false;
        }
        else {
            Debug.Log("Can move");
            //playerController.CanMove = true;
        }
    }
}
