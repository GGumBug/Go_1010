using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockArrangeSystem : MonoBehaviour
{
    private Vector2Int          blockCount;
    private Vector2             blockHalf;
    private BackGroundBlock[]   backGroundBlocks;
    private StageController     stageController;

    // 다른 스크립트의 변수가 그대로 필요할때 SetUp함수를 만들어서 한번에 깔끔하게 전달.
    public void Setup(Vector2Int blockCount, Vector2 blockHalf, BackGroundBlock[] backGroundBlocks, StageController stageController)
    {
        this.blockCount         = blockCount;
        this.blockHalf          = blockHalf;
        this.backGroundBlocks   = backGroundBlocks;
        this.stageController    = stageController;
    }     

    public bool TryArrangementBlock(DragBlock block)
    {
        for (int i = 0; i < block.childBolck.Length; i++)
        {
            //현재 블록의 위치에 자식 블록 localposition을 더해준 값
            Vector3 position = block.transform.position + block.childBolck[i];

            //블록이 맵 내부에 위치하는지
            if (!IsBlockInsideMap(position))        return false;
            //현재 위치에 다른 블록이 있는지
            if (!IsOtherBlockInThisBlock(position)) return false;
        }

        for (int i = 0; i < block.childBolck.Length; i++)
        {
            Vector3 position = block.transform.position + block.childBolck[i];

            backGroundBlocks[PositionToIndex(position)].FillBlock(block.color);
        }

        stageController.AfterBlockArrangment(block);

        return true;
    }

    private bool IsBlockInsideMap(Vector2 position)
    {
        if (position.x < -blockCount.x * 0.5f + blockHalf.x || position.x > blockCount.x * 0.5f - blockHalf.x || 
        position.y < -blockCount.y * 0.5f + blockHalf.y || position.y > blockCount.y * 0.5f - blockHalf.y)
        {
            return false;
        }

        return true;
    }

    private int PositionToIndex(Vector2 position)
    {
        float x = blockCount.x * 0.5f - blockHalf.x + position.x;
        float y = blockCount.y * 0.5f - blockHalf.y - position.y;

        return (int)(y * blockCount.x + x);
    }

    private bool IsOtherBlockInThisBlock(Vector2 position)
    {
        int index = PositionToIndex(position);

        if (backGroundBlocks[index].BlockState == BlockState.Fill)
        {
            return false;
        }

        return true;
    }
}
