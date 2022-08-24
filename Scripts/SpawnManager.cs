using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;

    [SerializeField]
    private float _enemySpeed = 2.0f;
    private float _difficultyMultiplier = 1.1f;
    private bool _stopSpawning = false;
 

    public void StartSpawning()
    {
        
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    //Spawn enemy every 5 seconds
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-8f, 8f);
            Vector3 enemySpawn = new Vector3(randomX, 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, enemySpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3.5f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-8f, 8f);
            Vector3 powerupSpawn = new Vector3(randomX, 7, 0);
            int randomPowerup = Random.Range(0, 3); //change to 0, 3 when shield is implemented
            Instantiate(_powerups[randomPowerup], powerupSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 7));
        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void increaseDifficulty()
    {
 
        _enemySpeed *= _difficultyMultiplier;
    }

    public float getSpeed()
    {
        return _enemySpeed;
    }
}
