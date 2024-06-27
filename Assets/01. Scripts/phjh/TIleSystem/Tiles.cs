using System;
using UnityEngine;
using UnityEngine.Tilemaps;

//�Ÿ������
//���������
//������ ������ ����������

[Serializable]
public abstract class Tiles
{

    public TileType tileState { get; private set; } = TileType.None;

    public void Init(int breakTimes = 0, TileType state = TileType.None, int? oreInfo = null)
    {
        if(state == TileType.None)
        {

        }

        SetBlock(null);
        {
            tileState = state;
            //oreinfo = ore
        }
        {
            tileState = TileType.BreakableBlock;
        }
    }

    public void SetBlock(GameObject obj)
    {
        if (obj == null)
            return;

        //���� prefab�������ִ°�
    }

    public virtual void DoBlockEvent() { } 

}

public enum TileType
{
    UnBreakableBlock = 0,
    BreakableBlock = 1,
    OreBlock = 2,
    InteractionBlock = 3,   //�� �̺�Ʈ �����ŵ� �����������
    None = 4, //�̰� �ʱ�ȭ ���������� ���Ͽ���
}