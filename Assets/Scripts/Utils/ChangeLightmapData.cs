using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLightmapData : MonoBehaviour
{
    public List<Texture2D> lightmappingAtlases;
    public int[] lightmappAtlasesCount;

    private List<LightmapData[]> lightmaps;

    public List<GameObject> GameObjects;

    // Start is called before the first frame update
    void Start()
    {
        //var lightmapsCount = lightmappingAtlases.Count / 2;

        //int atlasIndex = 0;
        //for (var i = 0; i < lightmaps.Count; i++)
        //{
        //    lightmaps[i] = new LightmapData();

        //    lightmaps[i].lightmapDir = lightmappingAtlases[atlasIndex];
        //    atlasIndex++;
        //    lightmaps[i].lightmapColor = lightmappingAtlases[atlasIndex];
        //    atlasIndex++;
        //}

        //lightmaps = new List<LightmapData[]>();

        //for (int i = 0; i < lightmappAtlasesCount.Length; i++)
        //{
        //    lightmaps.Add(new LightmapData[lightmappAtlasesCount[i]]);
        //    for (int atlasIndex = 0; atlasIndex < lightmappAtlasesCount[i]; atlasIndex++)
        //    {
        //        lightmaps[i][atlasIndex] = new LightmapData();
        //        lightmaps[i][atlasIndex].lightmapColor = lightmappingAtlases[atlasIndex];
        //    }
        //}

        //LightmapSettings.lightmaps = lightmaps[0];
    }

    public void ChangeLightmapTo(int index)
    {
        lightmaps = new List<LightmapData[]>();

        for (int i = 0; i < lightmappAtlasesCount.Length; i++)
        {
            lightmaps.Add(new LightmapData[lightmappAtlasesCount[i]]);
            for (int atlasIndex = 0; atlasIndex < lightmappAtlasesCount[i]; atlasIndex++)
            {
                lightmaps[i][atlasIndex] = new LightmapData();
                lightmaps[i][atlasIndex].lightmapColor = lightmappingAtlases[atlasIndex];
            }
        }

        LightmapSettings.lightmaps = lightmaps[index];
    }

    public void ChangeLightmapV2()
    {
        foreach (GameObject item in GameObjects)
        {
            var x = 0;
        }

    }
}
