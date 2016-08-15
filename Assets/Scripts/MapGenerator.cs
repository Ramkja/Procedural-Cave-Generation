﻿using UnityEngine;
using System;
using System.Collections;

public class MapGenerator : MonoBehaviour
{

	//Dimension of the map
	public int width;
	public int height;

    //Random
    public string seed;
    public bool useRandomSeed;

	//Initial fill percent of the map
	[Range(0,100)]
	public int randomFillPercent;

	//2D Array: 0 - Empty tile, 1 - not empty tile
	int[,] map;

	void Start ()
	{
		GenerateMap ();
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
    }

	void GenerateMap ()
	{
		map = new int[width, height];
        //Initial Fill
        RandomFillMap();

        //Smooth iterations
        for (int i = 0; i < 5; ++i)
        {
            SmoothMap();
        }

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(map, 1f);
	}

	void RandomFillMap ()
	{
        if (useRandomSeed)
            seed = Time.time.ToString();

        //Generate the seed
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        //Filling the map
        for(int x=0; x<width; ++x)
            for(int y=0; y<height; ++y)
            {
                if(x == 0 || x == width-1 || y == 0 || y == height-1)
                    //Bordering the map
                    map[x, y] = 1;
                else
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
            }  
	}

    void SmoothMap()
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                //Getting the adjacent tiles not empties
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                //Rules of the map
                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    map[x, y] = 0;

            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; ++neighbourX)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; ++neighbourY)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                        wallCount += map[neighbourX, neighbourY];
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    void OnDrawGizmos()
    {
        /*
        if(map != null)
        {
            //Drawing the map
            for (int x = 0; x < width; ++x)
                for (int y = 0; y < height; ++y)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width/2 + x + 0.5f, 0, -height/2 + y + 0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }           
        }
        */
    }

}
