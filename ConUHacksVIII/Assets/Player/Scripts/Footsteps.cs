using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] AudioClip[] walkClips;
    [SerializeField] AudioClip[] sprintClips;
    [SerializeField] AudioClip[] crouchClips;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip landClip;
    [SerializeField]  AudioSource audioSource;
    [SerializeField]  float footstepRate = 0.4f;
    private float currentStepRate;
    private float lastFootstepTime;
    private AudioClip[] currentClips;
    public bool IsCrouching = false;
    public bool IsSprinting = false;
    public bool IsMoving;
    void Update ()
    {
        if (IsCrouching) {
            currentStepRate = footstepRate * 1.8f;
            currentClips = crouchClips;
        } else if (IsSprinting) {
            currentStepRate = footstepRate * 0.7f;
            currentClips = sprintClips;
        }
        else {
            currentStepRate = footstepRate;
            currentClips = walkClips;
        }

        if(IsMoving)
        {
            if(Time.time - lastFootstepTime > currentStepRate)
            {
                lastFootstepTime = Time.time;
                audioSource.PlayOneShot(currentClips[Random.Range(0, currentClips.Length)]);
            }
        }
    }

    public void PlayJump() {
        audioSource.PlayOneShot(jumpClip);
    }

    public void PlayLand() {
        audioSource.PlayOneShot(landClip);
    }
}
