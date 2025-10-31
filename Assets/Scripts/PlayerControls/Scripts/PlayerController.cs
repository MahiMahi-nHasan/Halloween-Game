using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed =10f;
    private CharacterController cc;
    public Animator cam;
    public int maxHealth = 3;
    public Slider staminaBar;

    private Vector3 inputVector;
    private Vector3 movementVector;
    private float grav = -10f;
    private float damping = 5f;
    private int health;

    private int meter = 500;
    private bool isWalking;
    private bool run;
    
    void Awake()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        health = maxHealth;
        staminaBar.maxValue = 499;
        staminaBar.value = meter;
    }
    private void Start()
    {
        UIManager.Instance.UpdateHealth(health);
    }

    void Update()
    {
        if (meter >= 499)
        {
            meter = 499;
            run = true;
        }
        if (meter <= 0)
        {
            meter = 0;
            run = false;
        }
        staminaBar.value = meter;
        GetInput();
        MovePlayer();
        cam.SetBool("isWalking", isWalking);
    }
    void GetInput()
    {
        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D)

            )
        {
            inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            inputVector.Normalize();
            inputVector = transform.TransformDirection(inputVector);
            isWalking = true;
        }
        else
        {
            inputVector = Vector3.Lerp(inputVector, Vector3.zero, damping * Time.deltaTime);
            isWalking= false;
        }
        movementVector = (inputVector * moveSpeed) + (Vector3.up * grav);
    }
    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.Space) && meter != 0 && run)
        {
            cc.Move(1.5f * movementVector * Time.deltaTime);
            meter -= 2;
            Debug.Log("Am Sprinting!");
        }
        else
        {
            cc.Move(movementVector * Time.deltaTime);
            meter += 1;
        }
    }
    public void DamagePlayer()
    {
        health--;
        UIManager.Instance.UpdateHealth(health);
        if (health < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
