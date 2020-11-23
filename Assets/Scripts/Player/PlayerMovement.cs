using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    Vector3 movement;
    public CharacterController controller;
    public GameObject playerGameobject;
    public Transform playerTrasform;


    private void FixedUpdate()
    {
        if (playerTrasform == null) GetPlayer(playerTrasform);
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        movement = new Vector3(h, 0.0f, v);

        controller.Move(movement * speed * Time.deltaTime);
    }

    public void GetPlayer(Transform _player)
    {
        _player = playerGameobject.transform;
    }
}
