using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestBehaviour : MonoBehaviour
{
    Vector3 chestPosition;
    Quaternion chestRotation;
    public GameObject chestModel;
    public MeshRenderer top;
    public MeshRenderer bottom;
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
        float dist = Vector3.Distance(transform.position, player.transform.position);

        if(dist <= int.Parse(PlayerPrefs.GetString("Render")) * 10)
        {
            if(top.enabled == false)
            {
                top.enabled = true;
                bottom.enabled = true;
            }
        }
        else
        {
            if (top.enabled == true)
            {
                top.enabled = false;  
                bottom.enabled = false;
            }
        }
    }

    void Open()
    {
        if(opened == false)
        {
            GlobalVariables.coins += int.Parse(gameObject.name);
            Instantiate(chestModel, chestPosition, chestRotation);
            Destroy(gameObject);
        }
    }
}
