using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class BossScene1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.audioManager.Play(0, "bgmBoss",true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
