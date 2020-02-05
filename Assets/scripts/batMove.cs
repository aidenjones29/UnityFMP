using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batMove : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;
    private float movementSpeed = 1.0f;
    public float CountdownTime = 10.0f;
    private float timer;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        timer = CountdownTime;
        //transform.Rotate(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist <= 80)
        {
            Renderer[] rs = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (Renderer r in rs) r.enabled = true;
        }
        else
        {
            Renderer[] rs = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (Renderer r in rs) r.enabled = false;
        }

        if (timer >= 0.0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            transform.Rotate(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
            timer = CountdownTime;
        }

        moveDirection = transform.TransformDirection(0.0f, 0.0f, 0.5f);
        moveDirection = moveDirection * movementSpeed;
        gameObject.transform.Translate(moveDirection, Space.Self);
    }
}
