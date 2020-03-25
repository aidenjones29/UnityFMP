using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public Slider renderSize;
    public Slider volume;

    public Text renderSizeVal;
    public Text volumeSizeVal;
    private float volumeFloat;
    private int volumeInt;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Render"))
        {
            renderSize.value = int.Parse(PlayerPrefs.GetString("Render"));
        }
      
        if (PlayerPrefs.HasKey("Volume"))
        {
            volume.value = PlayerPrefs.GetFloat("Volume");
        }
    }

    // Update is called once per frame
    void Update()
    {
        renderSizeVal.text = renderSize.value.ToString();
        volumeFloat = volume.value * 100; volumeInt = (int)volumeFloat;
        volumeSizeVal.text = volumeInt.ToString();
    }

    public void AcceptChanges()
    {
        PlayerPrefs.SetString("Render", renderSizeVal.text);
        PlayerPrefs.SetFloat("Volume", volume.value);
        SceneManager.LoadScene("MainMenu");
    }
}
