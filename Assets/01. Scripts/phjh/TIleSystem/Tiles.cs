using System;
using UnityEngine;

    //�Ÿ������
    //���������
    //������ ������ ����������.
public class Tile
{
    public Lazy<GameObject> Objects;
    //�̰� idamageable �����������̽�/Ŭ���� ������� �����ϸ� ������ ����

    public bool Breakable { get; private set; } = false;

    //public int? OreInfo (���� �������� ����ִ� �� �ϳ�)
    
    public TileState tileState { get; private set; } = TileState.None;

    private int breakTime = 0;

    public void Init(bool walkable, int breakTimes = 0, TileState state = TileState.None, int? oreInfo = null)
    {
        if (walkable)
        {
            Objects = new Lazy<GameObject>();
            breakTime = breakTimes;
            tileState = state;
            //oreinfo = ore
        }
        else
        {
            Breakable = false;
            tileState = TileState.Blocked;
        }
    }

    public void DamageTile()
    {
        //Objects.Value.Getdamage();
    }

    public void BlockBreak()
    {
        if (Breakable == false || tileState == TileState.Walkable)
            return;

        breakTime--;

        if(breakTime <= 0)
        {
            //if(ore != null)
            //  SpawnOre()
            tileState = TileState.Walkable;
        }
    }

    public void SpawnOre()
    {
        //���⼭ ������ ������ ���ش�
    }

}

public enum TileState
{
    Blocked = 0,
    Walkable = 1,
    Attacking = 2,
    Immuniate = 3,
    Interaction = 4,
    None = 5,   //�ʱ�ȭ�� �� ��Ÿ��� �Ҷ� ����
}

