using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Noise : MonoBehaviour
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale) {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        // we set scale since just using int x, int y will show same values.
        if (scale <= 0) {
            scale = 0.0001f; // minimum value of scale
        }
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                float sampleX = x / scale;
                float sampleY = y / scale;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }
}
