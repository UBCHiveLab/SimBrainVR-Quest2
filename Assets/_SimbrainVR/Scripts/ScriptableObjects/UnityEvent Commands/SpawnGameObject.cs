using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Spawn GameObject_", menuName = "ScriptableObjects/UnityEvent Commands/Spawn GameObject")]
public class SpawnGameObject : ScriptableObject
{
    [SerializeField] private List<RandomPrefabItem> randomPrefabsToSpawn = new List<RandomPrefabItem>();

    public void Spawn()
    {
        GameObject objectToSpawn = GetRandomPrefabFromList();

        if (objectToSpawn != null)
            Instantiate(objectToSpawn);
    }

    public void SpawnWithParent(Transform newParent)
    {
        GameObject objectToSpawn = GetRandomPrefabFromList();

        if (objectToSpawn != null)
            Instantiate(objectToSpawn, newParent);

    }
    public void SpawnWithParent(Transform newParent, Vector3 spawnPosition)
    {
        GameObject objectToSpawn = GetRandomPrefabFromList();

        if (objectToSpawn != null)
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity, newParent);

    }

    public void SpawnAtThisPosition(Transform hintTransform)
    {
        GameObject objectToSpawn = GetRandomPrefabFromList();

        if (objectToSpawn != null)
            Instantiate(objectToSpawn, hintTransform.position, Quaternion.identity);
    }
    public GameObject SpawnAtThisPosition(Vector3 positionToSpawn)
    {
        GameObject objectToSpawn = GetRandomPrefabFromList();

        if (objectToSpawn != null)
            return Instantiate(objectToSpawn, positionToSpawn, Quaternion.identity);

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
}

[System.Serializable]
public class RandomPrefabItem
{
    public float weight = 1f;
    public GameObject thisObject = default;

}
