using System;
using UnityEngine;

//어떤타일인지
//누구의 공격이 진행중인지
[Serializable]
public abstract class Blocks : MonoBehaviour
{
    public Lazy<GameObject> block;

    private TileSystem tileSystem;

    public BlockType blockType { get; protected set; } = BlockType.None;

    public void Init(Vector3 pos, GameObject block = null, Transform BlockParent = null, TileSystem tilesys = null)
    {
        if (block == null)
        {
            Logger.LogError($"{this.gameObject.name} block is null! InstanceID : {this.gameObject.GetInstanceID()}");
            return;
        }

        if(BlockParent == null)
        {
            BlockParent = this.transform;
        }

        if(tilesys == null)
        {
            Logger.LogWarning("tile sys is null at" + this.gameObject.name);
        }

        //이건 나중에 풀매니저로 대체 할 예정
        block.transform.position = pos;
        block.transform.SetParent(BlockParent);
        this.block = new(block);
        tileSystem = tilesys;

        SetBlock();
    }

    public void DeleteBlock()
    {
        blockType = BlockType.None;
        //풀매니저로 변경

        if(tileSystem != null)
            tileSystem.tileblocks.Remove(this);

        Destroy(block.Value.gameObject);
    }

    //여기서 블럭을 세팅해준다
    public abstract void SetBlock();

    public abstract void BlockEvent();
}

public enum BlockType
{
    None = 0, //이건 초기화 같은데에서 쓰일예정
    UnBreakableBlock = 1,
    BreakableBlock = 2,
    OreBlock = 3,
    InteractionBlock = 4,   //뭐 이벤트 같은거도 재미있을지도 + 가시나 함정같은것도 생각해보자
}