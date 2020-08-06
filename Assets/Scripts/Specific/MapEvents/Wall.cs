using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wall : MonoBehaviour
{
    public GameObject TopPartOfTheWall;

    [SerializeField]
    private WallState _wallState;
    public WallState WallState
    {
        get
        {
            return _wallState;
        }
        set
        {
            _wallState = value;
            if (_wallState == WallState.Hide)
                TopPartOfTheWall.SetActive(false);
            else
                TopPartOfTheWall.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A)) WallState = (WallState == WallState.Hide ? WallState.Show : WallState.Hide);
    }
}
