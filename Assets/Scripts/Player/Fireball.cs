using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fireball : MonoBehaviour
{
    public float range = 20f;
    public float verticalRange = 20f;
    public float cooldown = 10f;
    public float damage = 5f;
    public Image fireSpellVisual;
    public WandAnimator wand;

    public Color readyColor = Color.white; // Color when ready
    public Color cooldownColor = new(1f, 1f, 1f, 0.5f); // Color when on cooldown

    public AudioClips sfx;
    public LayerMask raycastLayerMask;

    private BoxCollider fireballTrigger;
    public PlayerMove caster;

    public EnemyManager enemyManager;
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        fireballTrigger = GetComponent<BoxCollider>();
        fireballTrigger.size = new Vector3(5, verticalRange, range);
        fireballTrigger.center = new Vector3(0, 0, range * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (caster.hasOrange)
        {
            fireSpellVisual.gameObject.SetActive(true);
        }

        if (!caster.hasOrange)
        {
            return;
        }

        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                EndCooldown();
            }
        }

        if (Input.GetMouseButtonDown(1) && !isOnCooldown)
        {
            sfx.PlayOneShot("Fireball");
            wand.Fire();
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

        StartCooldown();
    }

    void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = cooldown;

        // Update UI opacity to indicate cooldown
        if (fireSpellVisual != null)
        {
            fireSpellVisual.color = cooldownColor;
        }
    }

    void EndCooldown()
    {
        isOnCooldown = false;

        // Reset UI opacity to indicate readiness
        if (fireSpellVisual != null)
        {
            fireSpellVisual.color = readyColor;
        }
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
