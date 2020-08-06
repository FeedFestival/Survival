using System.Collections.Specialized;
using Assets.Scripts.Main;
using UnityEngine;

public class Main : MonoBehaviour
{
    [HideInInspector]
    public CanvasController CanvasController;

    [HideInInspector]
    public CameraControl CameraControl;

    [HideInInspector]
    public SqLiteService SqLiteService;

    [HideInInspector]
    public Game Game;

    [HideInInspector]
    public user LoggedUser;

    public void Init(Game game)
    {
        Game = game;
        
        Transform[] allChildren = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            switch (child.name)
            {
                case "<body":
                    CanvasController = child.GetComponent<CanvasController>();
                    break;

                default:
                    break;
            }
        }
        CanvasController.Init(this);

        SqLiteService = new SqLiteService("Database.db");

        CameraControl = GetComponent<CameraControl>();
    }
}