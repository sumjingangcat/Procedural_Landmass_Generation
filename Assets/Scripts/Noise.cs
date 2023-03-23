using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Noise : MonoBehaviour
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int seed, int octaves, float persistance, float lacunarity, Vector2 offset) {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }
        
        // we set scale since just using int x, int y will show same values.
        if (scale <= 0) {
            scale = 0.0001f; // minimum value of scale
        }

        float maxNoiseHeight = float.MinValue; // definitely small than realMaxNoiseHeight
        float minNoiseHeight = float.MaxValue; // definitely larger than realMinNoiseHeight
        
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = x / scale * frequency + octaveOffsets[i].x;
                    float sampleY = y / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // what is PerlinNoise?
                    noiseHeight += perlinValue * amplitude; // how amplitude decrease the diff?

                    amplitude *= persistance;
                    frequency *= lacunarity; // what if just add lacunarity to frequency?
                }

                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        
        // Comparing normalized VS !normalized
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                // Mathf.InverseLerp (min, max, target)
                // target normalized regarding min as 0, max as 1.
                // if target out of range of min or max, it results 0 or 1 instead of outside value.
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
        
        // return the normalized noiseMap. To do so, we use maxValue and MinValue.
        return noiseMap;
    }
}
