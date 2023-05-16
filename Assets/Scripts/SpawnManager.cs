using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;

    [SerializeField]
    private GameObject[] powerUps;


    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }


    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3);

        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-8, 8);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(randomX, 8, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3);
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-8, 8);
            float randomTime = Random.Range(3, 7);
            int RandomPowerUp = Random.Range(0,powerUps.Length);
            GameObject powerUp = Instantiate(powerUps[RandomPowerUp], new Vector3(randomX, 8, 0), Quaternion.identity);
            yield return new WaitForSeconds(randomTime);
        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
