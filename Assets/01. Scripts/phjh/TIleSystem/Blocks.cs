using System;
using UnityEngine;

//�Ÿ������
//������ ������ ����������
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

        //�̰� ���߿� Ǯ�Ŵ����� ��ü �� ����
        block.transform.position = pos;
        block.transform.SetParent(BlockParent);
        this.block = new(block);
        tileSystem = tilesys;

        SetBlock();
    }

    public void DeleteBlock()
    {
        blockType = BlockType.None;
        //Ǯ�Ŵ����� ����

        if(tileSystem != null)
            tileSystem.tileblocks.Remove(this);

        Destroy(block.Value.gameObject);
    }

    //���⼭ ���� �������ش�
    public abstract void SetBlock();

    public abstract void BlockEvent();
}

public enum BlockType
{
    None = 0, //�̰� �ʱ�ȭ ���������� ���Ͽ���
    UnBreakableBlock = 1,
    BreakableBlock = 2,
    OreBlock = 3,
    InteractionBlock = 4,   //�� �̺�Ʈ �����ŵ� ����������� + ���ó� ���������͵� �����غ���
}