using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    InputReader input;

    [SerializeField]
    GameObject obj;

    void Start()
    {
        input.AttackEvent += DoAttack;
    }

    public void DoAttack()
    {
        Instantiate(obj,transform.position,Quaternion.identity);
    }

}
