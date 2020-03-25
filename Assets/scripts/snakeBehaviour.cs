using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snakeBehaviour : MonoBehaviour
{
    CharacterController animalController;
    private Vector3 moveDirection = Vector3.zero;
    private float movementSpeed = 1.0f;
    public float CountdownTime = 3.0f;
    private float timer;
    private GameObject player;
    private bool followingPlayer = false;
    SkinnedMeshRenderer waspRend;
    private Animator anim;
    private bool dead = false;

    private float attackTimerMax = 1.5f;
    private float CurrentAttackTimer = 1.0f;
    private int CurrentHP;
    private int AttackDamage = 5;
    private Quaternion currRot;


    void Start()
    {
        CurrentHP = 100;
        waspRend = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        player = GameObject.Find("Player");
        timer = CountdownTime;
        animalController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        transform.Rotate(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
    }
    // Update is called once per frame
    void Update()
    {
        if (CurrentHP <= 0.0f)
        {
            anim.Play("Death");
            if (dead == false)
            {
                GlobalVariables.kills += 1;
                GlobalVariables.coins += 10;
                dead = true;
            }
            Destroy(gameObject, 1.0f);
        }
        else
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);

            if (dist <= int.Parse(PlayerPrefs.GetString("Render")) * 15)
            {
                if(waspRend.enabled == false)
                {
                    waspRend.enabled = true;
                }
            }
            else
            {
                if (waspRend.enabled == true)
                {
                    waspRend.enabled = false;
                }
            }

            if (dist <= 15)
            {
                followingPlayer = true;
            }
            else if (dist >= 20)
            {
                followingPlayer = false;
                //transform.rotation.Set(0.0f, 0.0f, 0.0f, 0.0f);
            }

            if (followingPlayer == false)
            {
                if (timer >= 0.0f)
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
                    moveDirection = transform.TransformDirection(0.0f, 0.0f, 5.0f);
                    moveDirection = moveDirection * movementSpeed;
                }

                moveDirection.y -= 15f * Time.deltaTime;
                animalController.Move(moveDirection * Time.deltaTime);
            }
            else
            {
                transform.LookAt(player.transform.position);

                if (dist >= 3)
                {
                    moveDirection = transform.TransformDirection(0.0f, 0.0f, 5.0f);
                    moveDirection = moveDirection * movementSpeed;
                    moveDirection.y -= 15f * Time.deltaTime;
                    animalController.Move(moveDirection * Time.deltaTime);
                }
                else
                {
                    CurrentAttackTimer -= Time.deltaTime;
                    if (CurrentAttackTimer <= 0)
                    {
                        CurrentAttackTimer = attackTimerMax;
                        GlobalVariables.currentHP -= AttackDamage;
                        anim.Play("attack");
                    }
                }
            }
        }
    }

    public void ApplyDamage(int damage)
    {
        CurrentHP -= damage;
    }
}
