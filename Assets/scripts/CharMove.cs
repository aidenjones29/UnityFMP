using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public static class GlobalVariables
{
    public static string[] position = new string[2];
    public static int currentHP;
    public static int kills;
    public static int coins;
}

public class CharMove : MonoBehaviour
{
    CharacterController PlayerController;
    public GameObject player;
    public GameObject water;
    public GameObject holdingBlock;
    public GameObject armsBlock;
    public GameObject armsSword;
    public SimpleHealthBar healthBar;
    public GameObject[] PlayerHand;
    public Material[] holdingSkins;
    public GameObject[] BossTeleport;
    public GameObject swordUpgrade;
    public GameObject swordOrigional;
    private Renderer blockrend;

    public float movementSpeed = 10.0f;
    private float x;
    private float y;
    private float swingTime = 0.15f;
    private int skin = 0;
    private int maxHp = 100;
    private int currHp;
    public  int swordDamage = 25;
    private bool holdingSword = true;
    private bool SwordPurchased = false;
    private bool swinging = false;
    private Quaternion startRotSword;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 rotateValue;
    private Vector3 playerOldPos;

    void Start()
    {
        blockrend = holdingBlock.GetComponent<Renderer>();
        GlobalVariables.currentHP = 10;
        GlobalVariables.kills = 0;
        GlobalVariables.coins = 0;
        PlayerController = GetComponent<CharacterController>();
        Cursor.visible = false;
        healthBar.UpdateBar(currHp, maxHp);
        startRotSword = armsSword.transform.localRotation;
    }

    void Update()
    {
        currHp = GlobalVariables.currentHP;
        healthBar.UpdateBar(currHp, maxHp);

        if(GlobalVariables.currentHP <= 0)
        {
            SceneManager.LoadScene("MainMenu");
        }

        if (PlayerController.isGrounded)
        {
            moveDirection = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")));
            moveDirection = moveDirection * movementSpeed;

            if (Input.GetKey("space"))
            {
                moveDirection.y += 10.0f;
            }
        }

        if (Input.GetKey("escape"))
        {
            SceneManager.LoadScene("MainMenu");
        }

        //Gravity
        moveDirection.y -= 15f * Time.deltaTime;
        PlayerController.Move(moveDirection * Time.deltaTime);

        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");
        rotateValue = new Vector3(x, y * -1, 0);
        transform.eulerAngles = transform.eulerAngles - rotateValue;

        if (player.transform.position.y <= water.transform.position.y + 1.5f && player.transform.position.y >= -50.0f)
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

                if (hit.collider.gameObject.tag == "WorldBlock")
                {
                    playerOldPos = hit.collider.gameObject.transform.position;
                    playerOldPos.y += 5.0f;
                }
            }
        }



        if (Input.GetMouseButtonDown(0))
        {
            if(holdingSword == false)
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
                        Instantiate(PlayerHand[skin], pos, rot);
                    }
                    else if (MyNormal == -hit.transform.up)
                    {
                        pos.y -= 1.5f;
                        Instantiate(PlayerHand[skin], pos, rot);
                    }
                    else if (MyNormal == hit.transform.right)
                    {
                        pos.x += 1.5f;
                        Instantiate(PlayerHand[skin], pos, rot);
                    }
                    else if (MyNormal == -hit.transform.right)
                    {
                        pos.x -= 1.5f;
                        Instantiate(PlayerHand[skin], pos, rot);
                    }
                    else if (MyNormal == hit.transform.forward)
                    {
                        pos.z += 1.5f;
                        Instantiate(PlayerHand[skin], pos, rot);
                    }
                    else if (MyNormal == -hit.transform.forward)
                    {
                        pos.z -= 1.5f;
                        Instantiate(PlayerHand[skin], pos, rot);
                    }
                } 

            }
            else
            {
                swinging = true;
            }
        }

        if(swinging == true)
        {
            if(swingTime >= 0.0f)
            {
                swingTime -= Time.fixedDeltaTime;
                Vector3 rot = new Vector3( 5.0f, 0.0f, 0.0f );
                armsSword.transform.Rotate(rot, Space.Self);
            }
            else
            {
                swingTime = 0.15f;
                swinging = false;
                armsSword.transform.localRotation = startRotSword;
                armsSword.transform.localPosition = new Vector3(0.8f, 0.9f, 1.0f);

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 10.0f))
                {
                    if (hit.collider.gameObject.tag == "Mob")
                    {
                        hit.collider.gameObject.SendMessage("ApplyDamage", swordDamage);
                    }
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

        if(Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit InteractHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out InteractHit, 10.0f))
            {
                if (InteractHit.collider.gameObject.tag == "Chest")
                {
                    InteractHit.collider.gameObject.SendMessage("Open");
                }
                else if(InteractHit.collider.gameObject.name == "Boss0")
                {
                    gameObject.transform.position = BossTeleport[0].transform.position;
                }
                else if (InteractHit.collider.gameObject.name == "Boss1")
                {
                    gameObject.transform.position = BossTeleport[1].transform.position;
                }
                else if (InteractHit.collider.gameObject.name == "Boss2")
                {
                    gameObject.transform.position = BossTeleport[2].transform.position;
                }
                else if (InteractHit.collider.gameObject.name == "Boss3")
                {
                    gameObject.transform.position = BossTeleport[3].transform.position;
                }
                else if (InteractHit.collider.gameObject.tag == "Door")
                {
                    gameObject.transform.position = playerOldPos;
                }
                else if(InteractHit.collider.gameObject.tag == "Villager")
                {
                    if(SwordPurchased == false && GlobalVariables.coins >= 150)
                    {
                        swordOrigional.SetActive(false); swordUpgrade.SetActive(true);
                        swordDamage = 50; GlobalVariables.coins -= 150;
                        SwordPurchased = true;
                    }
                }
                else if (InteractHit.collider.gameObject.tag == "VillagerHP")
                {
                    if (GlobalVariables.currentHP < 100 && GlobalVariables.coins >= 50)
                    {
                        GlobalVariables.currentHP = 100; GlobalVariables.coins -= 50;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            holdingSword = true;
            armsBlock.transform.localPosition = new Vector3(0.0f, 1.5f, -3.0f);
            armsSword.transform.localPosition = new Vector3(0.8f, 0.9f, 1.0f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            skin = 0; holdingSword = false;
            blockrend.material = holdingSkins[skin];
            armsBlock.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
            armsSword.transform.localPosition = new Vector3(0.8f, 0.9f, -2.0f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            skin = 1; holdingSword = false;
            blockrend.material = holdingSkins[skin];
            armsBlock.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
            armsSword.transform.localPosition = new Vector3(0.8f, 0.9f, -2.0f);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            skin = 2; holdingSword = false;
            blockrend.material = holdingSkins[skin];
            armsBlock.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
            armsSword.transform.localPosition = new Vector3(0.8f, 0.9f, -2.0f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            skin = 3; holdingSword = false;
            blockrend.material = holdingSkins[skin];
            armsBlock.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
            armsSword.transform.localPosition = new Vector3(0.8f, 0.9f, -2.0f);
        }
    }
}