using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player _player;
    private InputReader _inputReader;

    [SerializeField]
    private GameObject tempBullet;

    //[SerializeField]
    //GameObject WeaponPivot;

    private Vector3 rot;
    private Vector2 mousedir;

    
    private Camera mainCamera;

    public void Init(Player player, InputReader inputReader, GameObject bullet)
    {
        _player = player;
        _inputReader = inputReader;
        this.tempBullet = bullet;
    }

    void Start()
    {
        _inputReader.AttackEvent += DoAttack;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        SetAim();
        SetWeaponDir();
    }

    public void DoAttack()
    {
        if (_player.canMove)
        {


        Vector3 dir = new Vector3(mousedir.x, 0, mousedir.y);

        GameObject bullet = Instantiate(tempBullet, transform.position, Quaternion.identity);
        //bullet.transform.forward = rot;
        Quaternion rotation = Quaternion.LookRotation(rot);
        
        bullet.transform.rotation = Quaternion.Slerp(bullet.transform.rotation, rotation, 1);

        }
    }

    private void SetWeaponDir()
    {
        float rotz = mousedir.y / 6f; //0~15µµ ¿¬»ê 

    }

    #region Methods

    private void SetAim()
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
        var ray = mainCamera.ScreenPointToRay(_inputReader.AimPosition);

        if (Physics.Raycast(ray, out var hitInfo))
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
