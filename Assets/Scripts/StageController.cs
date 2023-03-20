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
    [SerializeField]
    private UIController            uIController;

    public int                      CurrentScore { private set; get; }
    public int                      HighScore { private set; get; }

    private BackGroundBlock[]       backGroundBlocks;
    private int                     currentDragBlockCount;

    private readonly Vector2Int     blockCount = new Vector2Int(10, 10);
    private readonly Vector2        blockHalf = new Vector2(0.5f, 0.5f);
    private readonly int            maxDragBlockCount = 3;

    private List<BackGroundBlock>   filledBlockList;

    private void Awake() {

        CurrentScore = 0;
        HighScore = PlayerPrefs.GetInt("HighScore");

        filledBlockList = new List<BackGroundBlock>();

        backGroundBlockSpawner.SpawnBlocks(blockCount, blockHalf);

        backGroundBlocks = new BackGroundBlock[blockCount.x * blockCount.y];
        backGroundBlocks = foreGroundBlockSpawner.SpawnBlocks(blockCount, blockHalf);

        blockArrangeSystem.Setup(blockCount, blockHalf, backGroundBlocks, this);

        StartCoroutine(SpawnDragBlocks());
    }

    private IEnumerator SpawnDragBlocks()
    {
        currentDragBlockCount = maxDragBlockCount;

        dragBlockSpawner.SpawnBlocks();

        // WaitUntil은 불값이 참이 될때까지 기다렸다가 실행
        yield return new WaitUntil(() => IsCompleteSpawnBlocks());
    }

    private bool IsCompleteSpawnBlocks()
    {
        int count = 0;
        for (int i = 0; i < dragBlockSpawner.BlockSpawnPoints.Length; i++)
        {
            if (dragBlockSpawner.BlockSpawnPoints[i].childCount != 0 &&
                dragBlockSpawner.BlockSpawnPoints[i].GetChild(0).localPosition == Vector3.zero)
            {
                count++;
            }
        }

        // 이 코드가 참인지 불인지 판단해서 리턴하는것도 가능
        return count == dragBlockSpawner.BlockSpawnPoints.Length;
    }

    public void AfterBlockArrangment(DragBlock block)
    {
        StartCoroutine("OnAfterBlockArrangment", block);
    }

    private IEnumerator OnAfterBlockArrangment(DragBlock block)
    {
        Destroy(block.gameObject);

        int filledLineCount = CheckFilledLine();

        // Pow (x, y) x의 y 승
        int lineScore = filledLineCount == 0 ? 0 : (int)Mathf.Pow(2, filledLineCount - 1) * 10;

        CurrentScore += block.childBolcks.Length + lineScore;

        yield return StartCoroutine(DestroyFilledBlocks(block));

        currentDragBlockCount--;

        if (currentDragBlockCount == 0)
        {
            yield return StartCoroutine(SpawnDragBlocks());
        }

        //Destroy는 프레임의 마지막에 실행되기 때문에 작업이 끝난 후 실행을 체크하기 위해 사용
        yield return new WaitForEndOfFrame();

        if (IsGameOver())
        {
            //Debug.Log("GameOver");

            if (CurrentScore > HighScore)
            {
                PlayerPrefs.SetInt("HighScore", CurrentScore);
            }

            uIController.GameOver();
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

    private bool IsGameOver()
    {
        int dragBlockCount = 0;

        for (int i = 0; i < dragBlockSpawner.BlockSpawnPoints.Length; i++)
        {
            if (dragBlockSpawner.BlockSpawnPoints[i].childCount != 0)
            {
                dragBlockCount++;
                if (blockArrangeSystem.IsPossibleArrangment(
                    dragBlockSpawner.BlockSpawnPoints[i].GetComponentInChildren<DragBlock>()))
                {
                    return false;
                }
            }
        }

        return dragBlockCount != 0;
    }
}
