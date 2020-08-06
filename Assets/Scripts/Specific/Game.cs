using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Main;
using Assets.Scripts.Utils;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    #region Game

    public Main Main;

    [HideInInspector]
    public Dictionary<string, GameObject> scope;

    void Awake()
    {
        Main.Init(this);

        // this usualy stays in a callback function
        Init();
    }

    public void SetSession(string sessionId)
    {

    }

    public void HandshakeSessionCallback(bool success, string fUniqueId = null)
    {

    }

    public void FetchUserCallback(user user)
    {
    }

    #endregion

    public void Init()
    {
        Main.CameraControl.Init();

        //Scoreboard = GetComponent<Scoreboard>();

        //// set the view to portrait.
        //Main.CanvasController.SwitchResolution(true);

        //utils.InitColors();
        //InitViews();
        //InitButtonLogic();

        //// get the main user.
        //Main.LoggedUser = Main.SqLiteService.GetUser(1);
        //if (Main.LoggedUser == null)
        //{
        //    //  If there is no user, we go to the create user view.
        //    scope["RegistrationView"].SetActive(true);
        //    RegistrationView.Init(this);
        //}
        //else
        //{
        //    StartApp();
        //}
    }

    public void StartApp()
    {
        //scope["RegistrationView"].SetActive(false);

        //NavbarView.Init(this);
        //scope["NavbarView"].SetActive(true);
        //NavbarView.SelectButton(NavbarButton.HomeButton);

        //scope["StartView"].SetActive(true);

        //scope["NewGameButton"].GetComponent<Button>().onClick.AddListener(
        //    () =>
        //    {
        //        scope["StartView"].SetActive(false);

        //        NewGameView.Init(this);
        //        scope["NewGameView"].SetActive(true);
        //    }
        //    );

        //scope["ContinueGameButton"].GetComponent<Button>().onClick.AddListener(
        //    () =>
        //    {

        //    }
        //    );

        //StartCoroutine(Main.CanvasController.RebuildView());
    }

    public void InitButtonLogic()
    {
        //scope["StartGameButton"].GetComponent<Button>().onClick.AddListener(
        //    () =>
        //    {
        //        scope["StartView"].SetActive(false);

        //        scope["ScoreView"].SetActive(true);
        //        Main.CanvasController.SwitchResolution(false);

        //        Scoreboard.Init(this);
        //    }
        //    );
    }

    public void InitViews()
    {
        //NavbarView = scope["NavbarView"].GetComponent<NavbarView>();
        //if (NavbarView == null)
        //    scope["NavbarView"].AddComponent<NavbarView>();
        //NavbarView = scope["NavbarView"].GetComponent<NavbarView>();
        //scope["NavbarView"].SetActive(false);

        //scope["NewGameView"].SetActive(false);
        //scope["ScoreView"].SetActive(false);

        //RegistrationView = scope["RegistrationView"].GetComponent<RegistrationView>();
        //if (RegistrationView == null)
        //    scope["RegistrationView"].AddComponent<RegistrationView>();
        //RegistrationView = scope["RegistrationView"].GetComponent<RegistrationView>();

        //scope["StartView"].SetActive(false);

        //NewGameView = scope["NewGameView"].GetComponent<NewGameView>();
        //if (NewGameView == null)
        //    scope["NewGameView"].AddComponent<NewGameView>();
        //NewGameView = scope["NewGameView"].GetComponent<NewGameView>();
        //scope["NewGameView"].SetActive(false);
    }
}