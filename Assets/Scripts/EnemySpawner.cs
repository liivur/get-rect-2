using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject character;
    public float baseSpawnCooldown = 5;
    public float levelLength = 20;
    public float levelMultiplier = 0.8f;
    public float spawnRange = 100;

    private int level = 1;

    IEnumerator LevelCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        level += 1;
        StartCoroutine(LevelCooldown(levelLength));
    }

    IEnumerator SpawnCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        StartCoroutine(SpawnCooldown(baseSpawnCooldown * Mathf.Pow(levelMultiplier, level)));
        Spawn();
    }

    public void Spawn()
    {
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(prefab, character.transform.position + new Vector3(Random.Range(-spawnRange, spawnRange), transform.position.y, 0), transform.rotation);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCooldown(baseSpawnCooldown));
        StartCoroutine(LevelCooldown(levelLength));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
