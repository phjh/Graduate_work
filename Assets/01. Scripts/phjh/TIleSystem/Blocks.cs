using System;
using UnityEngine;

//어떤타일인지
//누구의 공격이 진행중인지
[Serializable]
public abstract class Blocks : PoolableMono
{
    public Lazy<GameObject> block;

    public BlockType blockType { get; protected set; } = BlockType.None;

    public void Init(Vector3 pos, GameObject block = null, string name = null)
    {
        if (block == null)
        {
            Logger.LogError($"{this.gameObject.name} block is null! InstanceID : {this.gameObject.GetInstanceID()}");
            return;
        }

        block.transform.position = pos;
        this.block = new(block);
        SetBlock();
    }

    public void DeleteBlock()
    {
        blockType = BlockType.None;
        MapManager.Instance.DeleteBlock(transform.position, gameObject.name);
    }

    //여기서 블럭을 세팅해준다
    public abstract void SetBlock();

    public virtual void BlockEvent() { }

    public virtual void BlockEvent(Vector3 pos, int breaks = 1) { }

    protected void MiningEffect(Vector3 pos)
    {
        //풀매니저에서 이펙트 가져와서 해야함
        Managers.instance.PoolMng.PopAndPushEffect("GroundEffect", transform.position, 0.5f);
    }
}

public enum BlockType
{
    None = 0, //이건 초기화 같은데에서 쓰일예정
    UnBreakableBlock = 1,
    BreakableBlock = 2,
    OreBlock = 3,
    InteractionBlock = 4,   //뭐 이벤트 같은거 추가할때 쓸 예정 + 가시나 함정같은것도 생각해보자
}