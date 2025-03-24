using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAudio : MonoBehaviour
{
     public AudioSource walkSound; // 角色移動音效
    private bool isWalking = false;

    public void SetWalking(bool walking)
    {
        if (walking && !isWalking)
        {
            isWalking = true;
            walkSound.Play();
        }
        else if (!walking && isWalking)
        {
            isWalking = false;
            walkSound.Stop();
        }
    }
}
