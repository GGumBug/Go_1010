using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField]
    private BackGroundBlockSpawner  backGroundBlockSpawner;
    [SerializeField]
    private BackGroundBlockSpawner  foreGroundBlockSpawner;
    [SerializeField]
    private DragBlockSpawner        dragBlockSpawner;
    [SerializeField]
    private BlockArrangeSystem      blockArrangeSystem;

    private BackGroundBlock[]       backGroundBlocks;
    private int                     currentDragBlockCount;

    private readonly Vector2Int     blockCount = new Vector2Int(10, 10);
    private readonly Vector2        blockHalf = new Vector2(0.5f, 0.5f);
    private readonly int            maxDragBlockCount = 3;

    private List<BackGroundBlock>   filledBlockList;

    private void Awake() {
        filledBlockList = new List<BackGroundBlock>();

        backGroundBlockSpawner.SpawnBlocks(blockCount, blockHalf);

        backGroundBlocks = new BackGroundBlock[blockCount.x * blockCount.y];
        backGroundBlocks = foreGroundBlockSpawner.SpawnBlocks(blockCount, blockHalf);

        blockArrangeSystem.Setup(blockCount, blockHalf, backGroundBlocks, this);

        SpawnDragBlocks();
    }

    private void SpawnDragBlocks()
    {
        currentDragBlockCount = maxDragBlockCount;
        dragBlockSpawner.SpawnBlocks();
    }

    public void AfterBlockArrangment(DragBlock block)
    {
        StartCoroutine("OnAfterBlockArrangment", block);
    }

    private IEnumerator OnAfterBlockArrangment(DragBlock block)
    {
        Destroy(block.gameObject);

        int filledLineCount = CheckFilledLine();

        yield return StartCoroutine(DestroyFilledBlocks(block));

        currentDragBlockCount--;

        if (currentDragBlockCount == 0)
        {
            SpawnDragBlocks();
        }
    }

    private int CheckFilledLine()
    {
        int filledLineCount = 0;

        filledBlockList.Clear();
        //가로줄 검사
        for (int y = 0; y < blockCount.y; y++)
        {
            int fillBlockCount = 0;
            for (int x = 0; x < blockCount.x; x++)
            {
                if (backGroundBlocks[y*blockCount.x+x].BlockState == BlockState.Fill) fillBlockCount ++;
            }

            if (fillBlockCount == blockCount.x)
            {
                for (int x = 0; x < blockCount.x; x++)
                {
                    filledBlockList.Add(backGroundBlocks[y*blockCount.x+x]);
                }
                filledLineCount++;
            }
        }
        //세로줄 검사
        for (int x = 0; x < blockCount.x; x++)
        {
            int fillBlockCount = 0;
            for (int y = 0; y < blockCount.y; y++)
            {
                if (backGroundBlocks[y*blockCount.x+x].BlockState == BlockState.Fill) fillBlockCount ++;
            }

            if (fillBlockCount == blockCount.y)
            {
                for (int y = 0; y < blockCount.y; y++)
                {
                    filledBlockList.Add(backGroundBlocks[y*blockCount.x+x]);
                }
                filledLineCount ++;
            }
        }

        return filledLineCount;
    }

    private IEnumerator DestroyFilledBlocks(DragBlock block)
    {
        // 마지막에 배치한 블록과 거리가 가까운 순서로 정렬
        filledBlockList.Sort((a,b)=>
        (a.transform.position-block.transform.position).sqrMagnitude.CompareTo((b.transform.position-block.transform.position).sqrMagnitude));

        for (int i = 0; i < filledBlockList.Count; i++)
        {
            filledBlockList[i].EmptyBlock();

            yield return new WaitForSeconds(0.01f);
        }

        filledBlockList.Clear();
    }
}
