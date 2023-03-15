using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundBlockSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject  blockPrefab;
    //블록이 그려지는 순서
    [SerializeField]
    private int         orderInLayer;

    //격자형태로 생성되는 블록의 개수, 블록 하나의 절반 크기
    private Vector2Int  blockCount = new Vector2Int(10, 10);
    private Vector2     blockHalf = new Vector2(0.5f, 0.5f);

    private void Awake() {
        for (int i = 0; i < blockCount.y; ++ i)
        {
            for (int e = 0; e < blockCount.x; ++ e)
            {
                float   px      = -blockCount.x * 0.5f + blockHalf.x + e;
                float   py      = blockCount.y * 0.5f - blockHalf.y - i;
                Vector3 position = new Vector3(px,py,0);

                GameObject clone = Instantiate(blockPrefab, position, Quaternion.identity, transform);

                clone.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
            }
        }
    }
}
