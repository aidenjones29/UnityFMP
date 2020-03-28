using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dayNightPicker : MonoBehaviour
{
    // Start is called before the first frame update
    public Material night;
    public Material day;
    public Light[] lights;

    public Color dayColour;
    public Color nightColour;

    void Start()
    {
        int randomTime = Random.Range(0, 100);

        if(randomTime <= 50)
        {
            RenderSettings.skybox = day;
            RenderSettings.fogColor = dayColour;
            for(int i = 0; i < lights.Length; i++)
            {
                lights[i].intensity = 1;
            }
        }
        else
        {
            RenderSettings.skybox = night;
            RenderSettings.fogColor = nightColour;
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].intensity = 0.4f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
