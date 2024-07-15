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

    public abstract void BlockEvent();

    protected void MiningEffect()
    {
        //Ǯ�Ŵ������� ����Ʈ �����ͼ� �ؾ���
        //Managers.instance.PoolMng.Pop("miningeffect");
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