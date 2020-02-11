﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum eBlocks { Water, Grass, Sand, Dirt}

public class PerlinNoise : MonoBehaviour
{
    // Width and height of the texture in pixels.
    public int pixRes;
    public GameObject worldGroundInstanciate;
    public GameObject character;
    public Texture2D sandTexture;
    public GameObject treeInstanciate;
    public GameObject cactusInstanciate;
    public GameObject[][] instanciated;
    public GameObject[][] treeInstanciated;
    public GameObject grassModel;
    public GameObject deadGrassModel;
    public Image outputImage;
    public Material sandMaterial;
    public Material dirtMaterial;
    public GameObject ZebraObj;
    public GameObject CowObj;
    public GameObject BatInst;
    public GameObject waspInst;
    public GameObject villageHouse1;
    public GameObject firepit;

    private bool done = false;
    private bool playerSet = false;
    private float xOrg;
    private float yOrg;
    private int scale;
    private int blockScale;
    private int height;
    private int randTree;
    private int randMob;
    private Quaternion rotation = Quaternion.Euler(0, 0, 0);
    private Vector3 playerPos;
    private eBlocks[][] blockType;

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;
    private const int renderSize = 100;
    private const int halfRenderSize = renderSize / 2;
    private int[] lastPosition = new int [2];

    private int chunkSize = 10;
    private int[] lastChunk = new int[2];
    private int[] currentChunk = new int[2];
    private int renderChunkSize = 10;

    void Start()
    {
        xOrg = Random.Range(0, 100);
        yOrg = Random.Range(0, 100);
        scale = Random.Range(5, 15);
        blockScale = Random.Range(25, 30);

        instanciated = new GameObject[pixRes][];
        treeInstanciated = new GameObject[pixRes][];
        blockType = new eBlocks[pixRes][];
        rend = GetComponent<Renderer>();

        noiseTex = new Texture2D(pixRes, pixRes);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void CalcNoise()
    {
        float y = 0.0f;

        int xVillage = Random.Range(50, noiseTex.width);
        int yVillage = Random.Range(50, noiseTex.height);

        while (y < noiseTex.height)
        {
            float x = 0.0f;

            instanciated[(int)y] = new GameObject[pixRes];
            treeInstanciated[(int)y] = new GameObject[pixRes];
            blockType[(int)y] = new eBlocks[pixRes];

            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                height = (int)(sample * blockScale) + 3;
                blockType[(int)y][(int)x] = eBlocks.Water;

                if (height > 7 + pixRes / 100)
                {
                    Vector3 position = new Vector3(x * 1.5f, height - (pixRes / 100), y * 1.5f);
                    instanciated[(int)y][(int)x] = Instantiate(worldGroundInstanciate, position, rotation);
                    instanciated[(int)y][(int)x].name = (int)x + " " + (int)y;
                    instanciated[(int)y][(int)x].GetComponent<Renderer>().enabled = false;
                    blockType[(int)y][(int)x] = eBlocks.Grass;

                    if (height < 9 + pixRes / 100)
                    {
                        Renderer m_Renderer = instanciated[(int)y][(int)x].GetComponent<Renderer>();
                        m_Renderer.material = sandMaterial;
                        blockType[(int)y][(int)x] = eBlocks.Sand;
                    }
                    
                    if ((height > 8 + pixRes / 100) && (height < 10 + pixRes / 100))
                    {
                        int randomTexture = Random.Range(0, 100);
                        if (randomTexture > 60)
                        {
                            Renderer m_Renderer = instanciated[(int)y][(int)x].GetComponent<Renderer>();
                            m_Renderer.material = sandMaterial;
                            blockType[(int)y][(int)x] = eBlocks.Sand;
                        }
                    }

                    if ((x == xVillage && y == yVillage) && playerSet == false)
                    {
                        position.y += 1;
                        Instantiate(villageHouse1, position, rotation);

                        character.transform.position = new Vector3(x * 1.5f, height + 10, (y * 1.5f) - 5);

                        playerSet = true;
                    }
                    else if (x == xVillage - 5 && y == yVillage + 5)
                    {
                        position.y += 1;
                        rotation = Quaternion.Euler(0, 90, 0);
                        Instantiate(villageHouse1, position, rotation);
                        rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (x == xVillage + 10 && y == yVillage + 5)
                    {
                        position.y += 1;
                        rotation = Quaternion.Euler(0, 270, 0);
                        Instantiate(villageHouse1, position, rotation);
                        rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (x == xVillage && y == yVillage + 8)
                    {
                        position.y += 1;
                        rotation = Quaternion.Euler(0, 270, 0);
                        Instantiate(firepit, position, rotation);
                        rotation = Quaternion.Euler(0, 0, 0);
                    }
                }
                x++;
            }
            y++;
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();

        outputImage.material.mainTexture = noiseTex;
    }

    void Update()
    {
        if (done == false)
        {
            CalcNoise();
            makeBiomes();
            placeTrees();
            done = true;
        }

        renderSquare();
    }

    void makeBiomes()
    {
        int halfRes = pixRes / 2;
        int borderlineY = halfRes + Random.Range(-(halfRes/2), halfRes/2 );
        int borderlineX = halfRes + Random.Range(-(halfRes / 2), halfRes / 2);

        for (int y2 = 0; y2 < pixRes; y2++)
        {
            for (int x2 = 0; x2 < pixRes; x2++)
            {
                if (y2 > (borderlineY + Random.Range(-5, 5)) && x2 > (borderlineX + Random.Range(-5, 5)))
                {
                    if (instanciated[y2][x2])
                    {
                        Renderer m_Renderer = instanciated[(int)y2][(int)x2].GetComponent<Renderer>();
                        m_Renderer.material = sandMaterial;
                        blockType[(int)y2][(int)x2] = eBlocks.Sand;
                    }

                }
                else if (y2 < (borderlineY + Random.Range(-5, 5)) && x2 > (borderlineX + Random.Range(-5, 5)))
                {
                    if (instanciated[y2][x2])
                    {
                        Renderer m_Renderer = instanciated[(int)y2][(int)x2].GetComponent<Renderer>();
                        m_Renderer.material = dirtMaterial;
                        blockType[(int)y2][(int)x2] = eBlocks.Dirt;
                    }
                }
            }
        }
    }

    void placeTrees()
    {
        for (int y2 = 0; y2 < pixRes; y2++)
        {
            for (int x2 = 0; x2 < pixRes; x2++)
            {
                if(instanciated[(int)y2][(int)x2])
                {
                    Vector3 position = instanciated[(int)y2][(int)x2].transform.position;
                    randTree = Random.Range(0, 1000);
                    randMob = Random.Range(0, 1000);
                    position.y += 3;
                    rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    if (randTree > 997)
                    {
                        if (blockType[(int)y2][(int)x2] == eBlocks.Sand)
                        {
                            treeInstanciated[(int)y2][(int)x2] = Instantiate(cactusInstanciate, position, rotation);
                            treeInstanciated[y2][x2].SetActive(false);
                        }
                        else if (blockType[(int)y2][(int)x2] == eBlocks.Grass)
                        {
                            position.y -= 2;
                            treeInstanciated[(int)y2][(int)x2] = Instantiate(treeInstanciate, position, rotation);
                            treeInstanciated[y2][x2].SetActive(false);
                        }
                    }
                    else if(randTree < 100)
                    {
                        if (blockType[(int)y2][(int)x2] == eBlocks.Grass)
                        {
                            position.y -= 2;
                            treeInstanciated[(int)y2][(int)x2] = Instantiate(grassModel, position, rotation);
                            treeInstanciated[y2][x2].SetActive(false);
                        }
                        else if (blockType[(int)y2][(int)x2] == eBlocks.Dirt)
                        {
                            position.y -= 2;
                            treeInstanciated[(int)y2][(int)x2] = Instantiate(deadGrassModel, position, rotation);
                            treeInstanciated[y2][x2].SetActive(false);
                        }
                    }

                    if (randTree == 996 && randMob <= 500)
                    {
                        position.y -= 2.0f;
                        Instantiate(ZebraObj, position, rotation);
                    }
                    else if (randTree == 995 && randMob <= 500)
                    {
                        position.y -= 2.0f;
                        Instantiate(CowObj, position, rotation);
                    }
                    else if(randTree == 994 && randMob <= 500)
                    {
                        position.y += 20.0f;
                        Instantiate(BatInst, position, rotation);
                    }
                    else if (randTree == 993 && randMob <= 250)
                    {
                        Instantiate(waspInst, position, rotation);
                        position.x += 4;
                        Instantiate(waspInst, position, rotation);
                        position.x -= 8;
                        Instantiate(waspInst, position, rotation);
                    }
                }
            }
        }
    }

    void renderSquare()
    {
        int temp;

        if (int.TryParse(GlobalVariables.position[0], out temp))
        {
            currentChunk[0] = int.Parse(GlobalVariables.position[0]) / chunkSize;
            currentChunk[1] = int.Parse(GlobalVariables.position[1]) / chunkSize;
        }

        if (lastChunk[0] != currentChunk[0] || lastChunk[1] != currentChunk[1])
        {
            for (int yRend = (currentChunk[0] - (renderChunkSize / 2)); yRend < (currentChunk[0] + (renderChunkSize / 2)); yRend++)
            {
                for (int xRend = (currentChunk[1] - (renderChunkSize / 2)); xRend < (currentChunk[1] + (renderChunkSize / 2)); xRend++)
                {
                    if(xRend >= 0 && xRend < (pixRes / 10) && yRend >= 0 && yRend <= (pixRes / 10))
                    {
                        renderChunk(yRend, xRend);
                    }
                }
            }
            lastChunk[0] = currentChunk[0]; lastChunk[1] = currentChunk[1];
        }
        //}
        //int temp;
        //
        //if (int.TryParse(GlobalVariables.position[0], out temp))
        //{
        //    int playerMinX = int.Parse(GlobalVariables.position[0]) - halfRenderSize;
        //    int playerMinZ = int.Parse(GlobalVariables.position[1]) - halfRenderSize;
        //
        //    for (int y = 0; y < (renderSize + 20); y++)
        //    {
        //        for (int x = 0; x < (renderSize + 20); x++)
        //        {
        //            int x1 = (playerMinX - 10) + x;
        //            int y1 = (playerMinZ - 10) + y;
        //
        //            if (x1 >= 0 && x1 < pixRes && y1 >= 0 && y1 < pixRes)
        //            {
        //                if (instanciated[y1][x1] && instanciated[y1][x1].GetComponent<Renderer>().enabled == true)
        //                {
        //                    instanciated[y1][x1].GetComponent<Renderer>().enabled = false;
        //                }
        //                if (treeInstanciated[y1][x1] && treeInstanciated[y1][x1].activeInHierarchy == true)
        //                {
        //                    treeInstanciated[y1][x1].SetActive(false);
        //                }
        //            }
        //        }
        //    }
        //
        //    for (int y = 0; y < renderSize; y++)
        //    {
        //        for (int x = 0; x < renderSize; x++)
        //        {
        //            int x1 = playerMinX + x;
        //            int y1 = playerMinZ + y;
        //    
        //            if (x1 >= 0 && x1 < pixRes && y1 >= 0 && y1 < pixRes)
        //            {
        //                if (instanciated[y1][x1] && instanciated[y1][x1].GetComponent<Renderer>().enabled == false)
        //                {
        //                    instanciated[y1][x1].GetComponent<Renderer>().enabled = true;
        //                }
        //                if (treeInstanciated[y1][x1] && treeInstanciated[y1][x1].activeInHierarchy == false)
        //                {
        //                    treeInstanciated[y1][x1].SetActive(true);
        //                }
        //            }
        //        }
        //    }
        //}
    }

    void renderChunk(int x, int y)
    {
        for (int yRend = 0; yRend < chunkSize; yRend++)
        {
            for (int xRend = 0; xRend < chunkSize; xRend++)
            {
               if (instanciated[y * 10 + yRend][x * 10 + xRend] && instanciated[y * 10 + yRend][x * 10 + xRend].GetComponent<Renderer>().enabled == false)
               {
                   instanciated[y * 10 + yRend][x * 10 + xRend].GetComponent<Renderer>().enabled = true;
               }
               if (treeInstanciated[y * 10 + yRend][x * 10 + xRend] && treeInstanciated[y * 10 + yRend][x * 10 + xRend].activeInHierarchy == false)
               {
                   treeInstanciated[y * 10 + yRend][x * 10 + xRend].SetActive(true);
               }
            }
        }
    }

    void unrenderChunk(int x, int y)
    {
        for (int yRend = 0; yRend < chunkSize; yRend++)
        {
            for (int xRend = 0; xRend < chunkSize; xRend++)
            {
                if (instanciated[y * 10 + yRend][x * 10 + xRend] && instanciated[y * 10 + yRend][x * 10 + xRend].GetComponent<Renderer>().enabled == true)
                {
                    instanciated[y * 10 + yRend][x * 10 + xRend].GetComponent<Renderer>().enabled = false;
                }
                if (treeInstanciated[y * 10 + yRend][x * 10 + xRend] && treeInstanciated[y * 10 + yRend][x * 10 + xRend].activeInHierarchy == true)
                {
                    treeInstanciated[y * 10 + yRend][x * 10 + xRend].SetActive(false);
                }
            }
        }
    }

}