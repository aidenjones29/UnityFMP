using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animalMovement : MonoBehaviour
{
    CharacterController animalController;
    private Vector3 moveDirection = Vector3.zero;
    private float movementSpeed = 1.0f;
    public float CountdownTime = 3.0f;
    private float timer;
    private GameObject player;
    private bool followingPlayer = false;
    private Animator anim;
    private int CurrentHP = 100;
    private SkinnedMeshRenderer rend;
    private Quaternion currRot;
    private bool dead = false;

    void Start()
    {
        rend = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        player = GameObject.Find("Player");
        timer = CountdownTime;
        anim = GetComponent<Animator>();
        animalController = GetComponent<CharacterController>();
        transform.Rotate(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
    }
    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (CurrentHP <= 0.0f)
        {
            anim.Play("Death");
            if (dead == false)
            {
                GlobalVariables.kills += 1;
                GlobalVariables.coins += 2;
                dead = true;
            }
            Destroy(gameObject, 1.5f);
        }
        else
        {
            if (dist <= int.Parse(PlayerPrefs.GetString("Render")) * 15)
            {
                if (rend.enabled == false)
                {
                    rend.enabled = true;
                }
            }
            else
            {
                if (rend.enabled == true)
                {
                    rend.enabled = false;
                }
            }

            if (dist <= 5)
            {
                followingPlayer = true;
            }
            else if (dist >= 20)
            {
                followingPlayer = false;
                transform.rotation.Set(0.0f, 0.0f, 0.0f, 0.0f);
            }

            if (followingPlayer == false)
            {
                if(timer >= 0.0f)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    transform.Rotate(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
                    timer = CountdownTime;
                }

                if (animalController.isGrounded)
                {
                    moveDirection = transform.TransformDirection(0.0f, 0.0f, 1.0f);
                    moveDirection = moveDirection * movementSpeed;
                }

                moveDirection.y -= 15f * Time.deltaTime;
                animalController.Move(moveDirection * Time.deltaTime);
            }
            else
            {
                transform.LookAt(player.transform.position);
                currRot = transform.rotation;
                currRot.x = 0; currRot.z = 0;
                transform.rotation = currRot;

                if (dist >= 3)
                {
                    moveDirection = transform.TransformDirection(0.0f, 0.0f, 1.0f);
                    moveDirection.y -= 15f * Time.deltaTime;
                    animalController.Move(moveDirection * Time.deltaTime);
                }
            }
        }
    }

    public void ApplyDamage(int damage)
    {
        CurrentHP -= damage;
    }
}
