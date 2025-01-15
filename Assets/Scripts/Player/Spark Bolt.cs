using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkBolt : MonoBehaviour
{
    public float range = 20f;
    public float verticalRange = 20f;
    public float fireRate = 1f;
    public float bigDamage = 2;
    public float smallDamage = 1;

    public WandAnimator wand;
    public AudioClips sfx;
    public LayerMask raycastLayerMask;

    private float nextTimeToFire;
    private BoxCollider gunTrigger;

    public EnemyManager enemyManager;

    void Start()
    {
        gunTrigger = GetComponent<BoxCollider>();
        gunTrigger.size = new Vector3(1, verticalRange, range);
        gunTrigger.center = new Vector3 (0,0,range * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > nextTimeToFire)
        {
            sfx.PlayOneShot("Spark");
            wand.Bolt();
            Fire();
        }
    }

    void Fire()
    {
        foreach (var enemy in enemyManager.enemiesInTrigger)
        {
            var dir = enemy.transform.position - transform.position;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, range *1.5f, raycastLayerMask))
            {
                if(hit.transform == enemy.transform)
                {
                    float dist = Vector3.Distance(enemy.transform.position, transform.position);

                    if (dist > range * 0.5f)
                    {
                        enemy.TakeDamage(smallDamage);
                    }
                    else
                    {
                        enemy.TakeDamage(bigDamage);
                    }
                }
            }
        }

        nextTimeToFire = Time.time + fireRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.transform.GetComponent<Enemy>();

        if (enemy)
        {
            enemyManager.AddEnemy(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.transform.GetComponent<Enemy>();

        if (enemy)
        {
            enemyManager.RemoveEnemy(enemy);
        }
    }
}
