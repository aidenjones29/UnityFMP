using UnityEngine;
using System.Collections;

public static class GlobalVariables
{
    public static string[] position = new string[2];
    public static int currentHP;
}

public class CharMove : MonoBehaviour
{
    CharacterController PlayerController;
    public GameObject player;
    public GameObject water;
    public float movementSpeed = 10.0f;
    public GameObject[] cube;

    private Vector3 moveDirection = Vector3.zero;
    private float x;
    private float y;
    private Vector3 rotateValue;
    private int skin = 0;
    public SimpleHealthBar healthBar;
    private int maxHp = 100;
    private int currHp;

    void Start()
    {
        GlobalVariables.currentHP = 100;
        PlayerController = GetComponent<CharacterController>();
        Cursor.visible = false;
        healthBar.UpdateBar(currHp, maxHp);
    }

    void Update()
    {
        currHp = GlobalVariables.currentHP;
        healthBar.UpdateBar(currHp, maxHp);

        if (PlayerController.isGrounded)
        {
            moveDirection = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")));
            moveDirection = moveDirection * movementSpeed;

            if (Input.GetKey("space"))
            {
                moveDirection.y += 10.0f;
            }
        }

        //Gravity
        moveDirection.y -= 15f * Time.deltaTime;
        PlayerController.Move(moveDirection * Time.deltaTime);

        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");
        rotateValue = new Vector3(x, y * -1, 0);
        transform.eulerAngles = transform.eulerAngles - rotateValue;

        if (player.transform.position.y <= water.transform.position.y + 1.5f)
        {
            PlayerController.center = new Vector3(0.0f, 1.5f, 0.0f);
        }
        else
        {
            PlayerController.center = new Vector3(0.0f, 0.0f, 0.0f);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                GlobalVariables.position = hit.collider.gameObject.name.Split(' ');
            }
        }



        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 15))
            {
                Vector3 MyNormal = hit.normal;
                MyNormal = hit.transform.TransformDirection(MyNormal);
                Vector3 pos = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
                Quaternion rot = hit.transform.rotation;

                if (MyNormal == hit.transform.up)
                {
                    pos.y += 1.5f;
                    Instantiate(cube[skin], pos, rot);
                }
                else if (MyNormal == -hit.transform.up)
                {
                    pos.y -= 1.5f;
                    Instantiate(cube[skin], pos, rot);
                }
                else if (MyNormal == hit.transform.right)
                {
                    pos.x += 1.5f;
                    Instantiate(cube[skin], pos, rot);
                }
                else if (MyNormal == -hit.transform.right)
                {
                    pos.x -= 1.5f;
                    Instantiate(cube[skin], pos, rot);
                }
                else if (MyNormal == hit.transform.forward)
                {
                    pos.z += 1.5f;
                    Instantiate(cube[skin], pos, rot);
                }
                else if (MyNormal == -hit.transform.forward)
                {
                    pos.z -= 1.5f;
                    Instantiate(cube[skin], pos, rot);
                }
            } 

        }
        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.tag == "Destructable")
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            skin = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            skin = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            skin = 2;
        }
    }
}