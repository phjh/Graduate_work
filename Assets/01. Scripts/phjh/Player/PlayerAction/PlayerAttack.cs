using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    InputReader input;

    [SerializeField]
    GameObject obj;

    [SerializeField]
    GameObject WeaponPivot;

    Vector2 mousedir;

    Vector3 rot;

    void Start()
    {
        input.AttackEvent += DoAttack;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //GetMouseDir();
        Aim();
        SetWeaponDir();
    }

    void GetMouseDir()
    {
        mousedir = input.AimPosition - new Vector2(1920,1080)/2;
    }

    public void DoAttack()
    {
        Vector3 dir = new Vector3(mousedir.x, 0, mousedir.y);

        GameObject bullet = Instantiate(obj,transform.position, Quaternion.identity);
        //bullet.transform.forward = rot;
        bullet.transform.rotation.SetLookRotation(rot);
        //bullet.transform.LookAt(dir);
    }

    private void SetWeaponDir()
    {
        float rotz = mousedir.y / 6f; //0~15µµ ¿¬»ê 

    }

    private float SetAngle()
    {
        float angle = 0f;
        angle += mousedir.y / 6f;
        if(Mathf.Abs(mousedir.y/180) > 0)
        {
            //angle -= 
        }
        return angle;
    }



        #region Datamembers

        #region Editor Settings

        [SerializeField] private LayerMask groundMask;

        #endregion
        #region Private Fields

        private Camera mainCamera;

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks


        #endregion

        private void Aim()
        {
            var (success, position) = GetMousePosition();
            if (success)
            {
                // Calculate the direction
                var direction = position - transform.position;

                // You might want to delete this line.
                // Ignore the height difference.
                direction.y = 0;

                // Make the transform look in the direction.
                //transform.forward = direction;
                rot = direction;
            
        }
        }

        private (bool success, Vector3 position) GetMousePosition()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
            {
                // The Raycast hit something, return with the position.
                return (success: true, position: hitInfo.point);
            }
            else
            {
                // The Raycast did not hit anything.
                return (success: false, position: Vector3.zero);
            }
        }

        #endregion
}
