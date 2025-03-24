using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject audioManagerPrefab;
    public AudioManager audioManager;

    void Start()
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 確保 GameManager 不會被銷毀

            if (audioManagerPrefab != null) 
            {
                audioManager = Instantiate(audioManagerPrefab).GetComponent<AudioManager>();
                DontDestroyOnLoad(audioManager.gameObject); // 確保 AudioManager 不會被銷毀
            }
            else 
            {
                Debug.LogError("audioManagerPrefab 尚未在 Inspector 設定！");
            }
        }
        else 
        {
            Destroy(gameObject); // 避免產生多個 GameManager
        }
    }
}
