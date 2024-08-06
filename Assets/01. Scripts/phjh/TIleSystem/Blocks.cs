using System;
using UnityEngine;

//�Ÿ������
//������ ������ ����������
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

    //���⼭ ���� �������ش�
    public abstract void SetBlock();

    public virtual void BlockEvent() { }

    public virtual void BlockEvent(Vector3 pos, int breaks = 1) { }

    protected void MiningEffect(Vector3 pos)
    {
        //Ǯ�Ŵ������� ����Ʈ �����ͼ� �ؾ���
        Managers.instance.PoolMng.PopAndPushEffect("GroundEffect", transform.position, 0.5f);
    }
}

public enum BlockType
{
    None = 0, //�̰� �ʱ�ȭ ���������� ���Ͽ���
    UnBreakableBlock = 1,
    BreakableBlock = 2,
    OreBlock = 3,
    InteractionBlock = 4,   //�� �̺�Ʈ ������ �߰��Ҷ� �� ���� + ���ó� ���������͵� �����غ���
}