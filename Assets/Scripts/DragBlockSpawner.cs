using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBlockSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[]         blockSpawnPoints;
    [SerializeField]
    private GameObject[]        blockPrefabs;

    private void Awake() {
        StartCoroutine("OnSpawnBlocks");
    }

    private IEnumerator OnSpawnBlocks()
    {
        for (int i = 0; i < blockSpawnPoints.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);

            int index = Random.Range(0, blockPrefabs.Length);
            Instantiate(blockPrefabs[index], blockSpawnPoints[i].position, Quaternion.identity, blockSpawnPoints[i]);
        }
    }
}
