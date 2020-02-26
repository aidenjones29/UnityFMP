using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestBehaviour : MonoBehaviour
{
    Vector3 chestPosition;
    Quaternion chestRotation;
    public GameObject chestModel;
    GameObject player;
    bool opened = false;

    // Start is called before the first frame update
    void Start()
    {
        chestPosition = transform.position;
        chestRotation = transform.rotation;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Open()
    {
        if(opened == false)
        {
            GlobalVariables.coins += 50;
            Instantiate(chestModel, chestPosition, chestRotation);
            Destroy(gameObject);
        }
    }
}
