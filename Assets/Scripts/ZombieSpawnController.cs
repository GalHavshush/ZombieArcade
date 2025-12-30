using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;


public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiePerWave = 5;
    public int currentZombiePerWave;

    public float spawnDelay = 0.5f; // delay between each zombie spawn in wave

    public int currentWave = 0;
    public float waveCooldownDuration = 10.0f; // Time between waves

    public bool inCooldown;
    public float coolDownCounter = 0; // For UI

    public List<Enemy> currentZombiesAlive;

    public GameObject zombiePrefab;

    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI cooldownCounterUI;

    public TextMeshProUGUI currentWaveUI;



    private void Start()
    {
        currentZombiePerWave = initialZombiePerWave;
        GlobalRefrences.Instance.waveNumber = currentWave;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();

        currentWave++;
        GlobalRefrences.Instance.waveNumber = currentWave;
        currentWaveUI.text = "Wave: " + currentWave.ToString();

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for(int i=0; i < currentZombiePerWave; i++)
        {
            // Generate random offset
            Vector3 spawnOffset = new Vector3(UnityEngine.Random.Range(-1f,1f),0f,UnityEngine.Random.Range(-1f,1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            // Instantiate zombies
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            // Get enemy script
            Enemy enemyScript = zombie.GetComponent<Enemy>();

            // Track zombie
            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        // Get all dead zombies
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach(Enemy zombie in currentZombiesAlive)
        {
            if(zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }

        // Remove all dead zombies
        foreach(Enemy zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }
        zombiesToRemove.Clear();

        // Start cooldown if all zombies are dead
        if(currentZombiesAlive.Count == 0 && !inCooldown)
        {
            //start cooldown for next wave
            StartCoroutine(waveCooldown());
        }

        // cooldown timer
        if(inCooldown)
        {
            coolDownCounter -= Time.deltaTime;
        }
        else
        {
            coolDownCounter = waveCooldownDuration; // reset counter
        }

        cooldownCounterUI.text = coolDownCounter.ToString("F0");


    }
    private IEnumerator waveCooldown()
    {
        inCooldown = true;

        waveOverUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(waveCooldownDuration);

        inCooldown = false;
        waveOverUI.gameObject.SetActive(false);

        currentZombiePerWave *= 2;
        StartNextWave();
    }
}
