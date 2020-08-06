using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Assets.Scripts.Utils;
using System.IO;

public class SceneLightSaver : MonoBehaviour
{
    public ChangeLightmapData ChangeLightmapData;
    public string Path;
    public GameObject Ceiling;
    public List<string> SceneNames;
    public List<GameObject> ObjectsToShow;

    //

    int index = 1;
    bool isLastScene;
    string SceneBakeOptionsPath = "Assets/StreamingAssets/SceneBakeOptions.txt";
    string sceneBakeOptions;

    public void GenerateLighting()
    {
        Ceiling.SetActive(true);

        Lightmapping.BakeAsync();
        Lightmapping.completed = () =>
        {
            Ceiling.SetActive(false);
        };
    }

    public void GenerateVariationsLighting(bool continuing = false)
    {
        Ceiling.SetActive(true);
        isLastScene = false;
        if (continuing == false)
        {
            index = 1;
        }

        if (index == SceneNames.Count)
        {
            index = 0;
            isLastScene = true;
        }

        string scenePath = Path + "/" + SceneNames[index] + ".unity";

        bool saveOK = EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);
        if (saveOK == false) return;

        if (ObjectsToShow[index])
            ObjectsToShow[index].SetActive(true);

        Lightmapping.Bake();

        if (ObjectsToShow[index])
            ObjectsToShow[index].SetActive(false);

        Ceiling.SetActive(false);

        saveOK = EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);
        if (saveOK == false) return;

        if (isLastScene)
        {
            index = 1;
            //ChangeLightmapData.lightmappingAtlases = new List<Texture2D>();
            //ChangeLightmapData.lightmappAtlasesCount = new int[SceneNames.Count];

            //for (var i = 0; i < SceneNames.Count; i++)
            //{
            //    string filesPath = Path + "/" + SceneNames[i] + "/";
            //    // searches the current directory
            //    int fCount = Directory.GetFiles(filesPath, "*", SearchOption.TopDirectoryOnly).Length;
            //    // remove the metas and LightingData, we are only interested in images
            //    fCount = (fCount / 2) - 1;

            //    for (int f = 0; f < fCount; f++)
            //    {
            //        string name = "Lightmap-" + f + "_comp_light.exr";
            //        ChangeLightmapData.lightmappingAtlases.Add(utils.LoadLightmapFromDisk(filesPath + name));
            //    }

            //    ChangeLightmapData.lightmappAtlasesCount[i] = fCount;
            //}
            
        }
        else
        {
            index++;
            GenerateVariationsLighting(true);
        }
    }

}
