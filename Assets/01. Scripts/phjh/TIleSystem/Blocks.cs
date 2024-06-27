using System;
using UnityEngine;

//�Ÿ������
//������ ������ ����������
[Serializable]
public abstract class Blocks
{
    public Lazy<GameObject> block;
    public BlockType blockType { get; private set; } = BlockType.None;

    public void Init(BlockType type)
    {
        if (type == BlockType.None)
        {
            type = BlockType.None;
            return;
        }

        blockType = type;

        SetBlock();
    }

    public virtual void SetBlock() 
    {
        //���⼭ ���� �������ش�
        block = new();
    }

    public virtual void DoBlockEvent() { } 

}

public enum BlockType
{
    None = 0, //�̰� �ʱ�ȭ ���������� ���Ͽ���
    UnBreakableBlock = 1,
    BreakableBlock = 2,
    OreBlock = 3,
    InteractionBlock = 4,   //�� �̺�Ʈ �����ŵ� ����������� + ���ó� ���������͵� �����غ���
}