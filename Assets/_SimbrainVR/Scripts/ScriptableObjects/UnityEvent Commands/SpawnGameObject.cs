using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Spawn GameObject_", menuName = "ScriptableObjects/UnityEvent Commands/Spawn GameObject")]
public class SpawnGameObject : ScriptableObject
{
    [SerializeField] private List<RandomPrefabItem> randomPrefabsToSpawn = new List<RandomPrefabItem>();

    [SerializeField] private bool randomizePositionAfterSpawn = false;
    [SerializeField] private Vector3 randomAreaAroundSpawnLocation = Vector3.one;

    public void Spawn()
    {
        GameObject objectToSpawn = GetRandomPrefabFromList();

        if (objectToSpawn != null)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn);

            if (randomizePositionAfterSpawn)
                RandomizePositionAfterSpawn(spawnedObject);
        }
    }

    public void SpawnWithParent(Transform newParent)
    {
        GameObject objectToSpawn = GetRandomPrefabFromList();

        if (objectToSpawn != null)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, newParent);

            if (randomizePositionAfterSpawn)
                RandomizePositionAfterSpawn(spawnedObject);
        }

    }
    public void SpawnWithParent(Transform newParent, Vector3 spawnPosition)
    {
        GameObject objectToSpawn = GetRandomPrefabFromList();

        if (objectToSpawn != null)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity, newParent);

            if (randomizePositionAfterSpawn)
                RandomizePositionAfterSpawn(spawnedObject);
        }

    }

    public void SpawnAtThisPosition(Transform hintTransform)
    {
        GameObject objectToSpawn = GetRandomPrefabFromList();

        if (objectToSpawn != null)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, hintTransform.position, Quaternion.identity);

            if (randomizePositionAfterSpawn)
                RandomizePositionAfterSpawn(spawnedObject);
        }
    }
    public GameObject SpawnAtThisPosition(Vector3 positionToSpawn)
    {
        GameObject objectToSpawn = GetRandomPrefabFromList();

        if (objectToSpawn != null)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, positionToSpawn, Quaternion.identity);

            if (randomizePositionAfterSpawn)
                RandomizePositionAfterSpawn(spawnedObject);

            return spawnedObject;
        }

        return null;
    }

    private GameObject GetRandomPrefabFromList()
    {
        float totalWeights = 0f;

        foreach (RandomPrefabItem weightedPrefabItem in randomPrefabsToSpawn)
        {
            totalWeights += weightedPrefabItem.weight;

        }

        if (totalWeights == 0f)
            return null;

        float weightAux = 0f;
        float random = Random.Range(0f, totalWeights);

        foreach (RandomPrefabItem weightedPrefabItem in randomPrefabsToSpawn)
        {
            weightAux += weightedPrefabItem.weight;

            if (weightAux > random)
            {
                return weightedPrefabItem.thisObject;
            }

        }

        return null;
    }

    private void RandomizePositionAfterSpawn(GameObject spawnedObject)
    {
        Vector3 randomOffset = UnityEngine.Random.insideUnitSphere;

        Vector3 newPosition = spawnedObject.transform.position + new Vector3(randomOffset.x * randomAreaAroundSpawnLocation.x, randomOffset.y * randomAreaAroundSpawnLocation.y, randomOffset.z * randomAreaAroundSpawnLocation.z);

        spawnedObject.transform.position = newPosition;
    }
}

[System.Serializable]
public class RandomPrefabItem
{
    public float weight = 1f;
    public GameObject thisObject = default;

}
