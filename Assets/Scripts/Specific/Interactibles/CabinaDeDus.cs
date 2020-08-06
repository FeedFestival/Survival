using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinaDeDus : MonoBehaviour
{
    public Animation CabinaDeDusController;
    public Material GlassBlindsAnimationMaterial;
    public List<GameObject> GlassObjectsToChange;

    public DoorState CurrentDoorstate;
    public GlassBlinds GlassBlinds;

    public GameObject EmptyGameObject;
    public Color32 BlindsColor;
    public Color32 OpenBlindsColor;

    private List<Material> GlassMaterialsToChange;

    public string NameOfMaterial;

    public int columns;
    public int rows;
    public float framesPerSecond;

    private float tileSize;
    private int rowIndex;
    private Direction glassBlindsAnimationDirection;

    // Use this for initialization
    void Start()
    {
        CabinaDeDusController.Play(DoorState.Still.ToString());

        tileSize = (1 / (float)rows);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            DoorInteraction(DoorState.Open);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            DoorInteraction(DoorState.Close);
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            ChangeGlassBlinds();
        }
    }

    public void DoorInteraction(DoorState doorstate)
    {
        if (CurrentDoorstate == doorstate) return;
        CurrentDoorstate = doorstate;

        CabinaDeDusController.Play(doorstate.ToString());
    }

    public void ChangeGlassBlinds()
    {
        GlassBlinds = (GlassBlinds == GlassBlinds.Closed ? GlassBlinds.Opened : GlassBlinds.Closed);

        float startOffset = 0f;
        glassBlindsAnimationDirection = Direction.Forward;

        if (GlassBlinds == GlassBlinds.Closed)
        {
            startOffset = tileSize * rows;
            glassBlindsAnimationDirection = Direction.Backwards;
        }

        if (GlassMaterialsToChange == null)
        {
            GlassMaterialsToChange = new List<Material>();

            foreach (var gameObject in GlassObjectsToChange)
            {
                if (gameObject.GetComponent<SkinnedMeshRenderer>() != null)
                {
                    int i = 0;
                    foreach (var item in gameObject.GetComponent<SkinnedMeshRenderer>().materials)
                    {
                        if (item.name == NameOfMaterial + " (Instance)")
                        {
                            Material[] mats = gameObject.GetComponent<SkinnedMeshRenderer>().materials;
                            mats[i] = GlassBlindsAnimationMaterial;
                            mats[i].mainTextureScale = new Vector2(1 / columns, tileSize);
                            mats[i].mainTextureOffset = new Vector2(0, startOffset);
                            gameObject.GetComponent<SkinnedMeshRenderer>().materials = mats;

                            GlassMaterialsToChange.Add(gameObject.GetComponent<SkinnedMeshRenderer>().materials[i]);
                            break;
                        }
                    }
                }
                else
                {
                    gameObject.GetComponent<Renderer>().material = GlassBlindsAnimationMaterial;
                    gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1 / columns, tileSize);
                    gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, startOffset);
                    GlassMaterialsToChange.Add(gameObject.GetComponent<Renderer>().material);
                }
            }
        }

        if (glassBlindsAnimationDirection == Direction.Backwards)
        {
            foreach (Material mat in GlassMaterialsToChange)
            {
                mat.color = OpenBlindsColor;
            }

            LeanTween.color(EmptyGameObject, BlindsColor, 1f)
                .setOnUpdateColor((Color color) =>
                {
                    foreach (Material mat in GlassMaterialsToChange)
                    {
                        mat.color = color;
                    }
                });
        }
        else
        {
            foreach (Material mat in GlassMaterialsToChange)
            {
                mat.color = BlindsColor;    
            }

            LeanTween.color(EmptyGameObject, OpenBlindsColor, 1f)
                .setOnUpdateColor((Color color) =>
                {
                    foreach (Material mat in GlassMaterialsToChange)
                    {
                        mat.color = color;
                    }
                });
        }


        rowIndex = 0;
        StartCoroutine(updateTiling());
    }

    private IEnumerator updateTiling()
    {
        while (true)
        {
            if (rowIndex == (rows - 1))
            {
                break;
            }

            var firstMat = GlassMaterialsToChange[0];

            var x = firstMat.mainTextureOffset.x;
            var y = firstMat.mainTextureOffset.y;
            y = (glassBlindsAnimationDirection == Direction.Backwards) ? (y - tileSize) : (y + tileSize);

            for (var i = 0; i < GlassMaterialsToChange.Count; i++)
            {
                GlassMaterialsToChange[i].mainTextureOffset = new Vector2(x, y);
            }

            rowIndex++;

            yield return new WaitForSeconds(1f / framesPerSecond);
        }
    }
}
