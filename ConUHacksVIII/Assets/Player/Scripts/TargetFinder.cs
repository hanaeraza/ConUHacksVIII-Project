using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Launch a raycast from the center of the player's camera and store the collision point.
public class TargetFinder : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] GameObject visualiser;
    Ray ray;
    Vector3 targetCoordinates;
    public Vector3 TargetCoordinates { get { return targetCoordinates; } }

    // Update is called once per frame
    void Update()
    {
        ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            targetCoordinates = hit.point;
        }
    }
}
