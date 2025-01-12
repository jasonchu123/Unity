using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    enum SpawnerType { Straight, Spin }

    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife = 1f;
    public float speed = 1f;

    [Header("Bullet Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float firingRate = 5f;

    private GameObject spawnedBullet;
    private float timer = 0f;
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;
        if(spawnerType == SpawnerType.Spin)transform.eulerAngles = new Vector3(0f,0f,transform.eulerAngles.z+1f);
        if(timer >= firingRate)
        {
            Fire();
            timer = 0;
        }
    }
    private void Fire()
    {
        if(bullet)
        {
            spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            spawnedBullet.GetComponent<Bullet>().speed = speed;
            spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
            spawnedBullet.transform.rotation = transform.rotation;
        }
    }  
}