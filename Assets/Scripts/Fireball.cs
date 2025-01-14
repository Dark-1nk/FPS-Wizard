using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float range = 20f;
    public float verticalRange = 20f;
    public float cooldown = 10f;
    public float damage = 5f;

    public AudioClips sfx;
    public LayerMask raycastLayerMask;

    private float nextTimeToFire;
    private BoxCollider fireballTrigger;
    public PlayerMove caster;

    public EnemyManager enemyManager;

    void Start()
    {
        fireballTrigger = GetComponent<BoxCollider>();
        fireballTrigger.size = new Vector3(5, verticalRange, range);
        fireballTrigger.center = new Vector3(0, 0, range * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!caster.hasOrange)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1) && Time.time > nextTimeToFire)
        {
            sfx.PlayOneShot("Fireball");
            Fire();
        }
    }

    void Fire()
    {
        foreach (var enemy in enemyManager.enemiesInTrigger)
        {
            var dir = enemy.transform.position - transform.position;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, range * 1.5f, raycastLayerMask))
            {
                if (hit.transform == enemy.transform)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }

        nextTimeToFire = Time.time + cooldown;
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
