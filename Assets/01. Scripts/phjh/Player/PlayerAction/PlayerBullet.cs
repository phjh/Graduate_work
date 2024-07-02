using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Blocks>(out Blocks block))
        {
            block.BlockEvent();
            Destroy(this.gameObject);
        }
    }
}
