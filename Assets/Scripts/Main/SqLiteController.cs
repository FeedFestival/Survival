using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Main;

public class SqLiteController : MonoBehaviour
{
    private SqLiteService _sqLiteService;

    public void Initialize(Main main, SqLiteService dataService)
    {
        //_main = main;
        _sqLiteService = dataService;
    }

    public void CleanUpUsers()
    {
        var dataService = new SqLiteService("Database.db");

        dataService.CleanUpUsers();
    }

    public void RecreateDataBase()
    {
        var dataService = new SqLiteService("Database.db");

        dataService.CreateDB();
        FillDB(dataService);
    }

    public void FillDB(SqLiteService service)
    {

        // app
        var app = new app
        {
            name = "KeepScores Complete Edition",
        };
        app.id = service.AddApp(app);
        
        // game types.
        var game = new game
        {
            app_id = app.id,
            Type = new type
            {
                name = "Standard",
                prog_id = "STANDARD"
            },
            unity = true
        };
        service.CreateGame(game);

        game = new game
        {
            app_id = app.id,
            Type = new type
            {
                name = "Claim",
                prog_id = "CLAIM"
            },
            unity = true
        };
        service.CreateGame(game);

        game = new game
        {
            app_id = app.id,
            Type = new type
            {
                name = "Munchkin",
                prog_id = "MUNCHKIN"
            },
            unity = true
        };
        service.CreateGame(game);

        game = new game
        {
            app_id = app.id,
            Type = new type
            {
                name = "Solo/Uno",
                prog_id = "SOLO_UNO"
            },
            unity = true
        };
        service.CreateGame(game);

        game = new game
        {
            app_id = app.id,
            Type = new type
            {
                name = "One Night Ultimate Warewolf",
                prog_id = "ONENIGHT_WAREWOLF"
            },
            unity = true
        };
        service.CreateGame(game);

        game = new game
        {
            app_id = app.id,
            Type = new type
            {
                name = "Checkers",
                prog_id = "CHECKERS"
            },
            unity = true
        };
        service.CreateGame(game);

        game = new game
        {
            app_id = app.id,
            Type = new type
            {
                name = "Backgammon",
                prog_id = "BACKGAMMON"
            },
            unity = true
        };
        service.CreateGame(game);
    }
}
