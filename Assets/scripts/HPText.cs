using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPText : MonoBehaviour
{
    public Text HPUI;
    public Text KillUI;
    public Text CoinUI;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        HPUI.text = "HP: " + GlobalVariables.currentHP;
        KillUI.text = ""+GlobalVariables.kills;
        CoinUI.text = "" + GlobalVariables.coins;
    }
}
