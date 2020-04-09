using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBehaviour : MonoBehaviour
{
    CharacterController animalController;
    private Vector3 moveDirection = Vector3.zero;
    private float movementSpeed = 1.0f;
    private float timer;
    private GameObject player;
    private bool followingPlayer = false;
    SkinnedMeshRenderer dragRend;
    private Animator anim;
    private bool dead = false;

    private float attackTimerMax = 1.5f;
    private float CurrentAttackTimer = 1.0f;
    private int CurrentHP;
    private int AttackDamage = 10;
    private Quaternion currRot;


    void Start()
    {
        CurrentHP = 150;
        dragRend = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        player = GameObject.Find("Player");
        animalController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
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

            if (dist <= 60)
            {
                dragRend.enabled = true;
            }
            else
            {
                dragRend.enabled = false;
            }

            if (dist <= 15)
            {
                followingPlayer = true;
            }
            else if (dist >= 20)
            {
                followingPlayer = false;
            }

            if (followingPlayer == false)
            {

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
                        anim.Play("Attack");
                    }
                }
            }
        }
    }

    public void ApplyDamage(int damage)
    {
        CurrentHP -= damage;
        anim.Play("Hit");
    }
}
