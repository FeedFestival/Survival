using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Assets.Editor;
using Assets.Scripts.Utils;
using UnityEditor;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    private Main _main;

    private div _body;

    private int index = 0;

    public void Init(Main main)
    {
        _main = main;

        FillScope();
    }

    public int InspectorScreenWidth;
    public int InspectorScreenHeight;
    public string InspectorScreenName;

    public void Build(bool getElements)
    {
        //Debug.Log("Build view " + (getElements ? " and getting the element " : ""));

        var cameraControl = Camera.main.GetComponent<CameraControl>();
        if (cameraControl != null)
            cameraControl.Init(true);

        if (getElements)
            GetBasicElements();

        style_utils.buildDiv(_body);
    }

    public void GetBasicElements()
    {
        _body = new div
        {
            Id = index.ToString(),
            elementName = transform.gameObject.name,
            children = new List<div>(),
            type = divType.body,
            element = transform.GetComponent<RectTransform>()
        };
        style_utils.SetAnchor(AnchorType.TopLeft, _body.element);
        _body.element.localPosition = new Vector3(-(_body.element.sizeDelta.x / 2), (_body.element.sizeDelta.y / 2), 0);

        style_utils.getDivs(transform, _body);
    }

    public void FillScope()
    {
        _main.Game.scope = new Dictionary<string, GameObject>();

        Transform[] allChildren = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            if ((child.gameObject.name.Contains("View") && child.gameObject.name != "Viewport") ||
                child.gameObject.name.Contains("Dropdown") || child.gameObject.name.Contains("Button") ||
                child.gameObject.name.Contains("Text"))
            {
                if (child.gameObject.name.Contains("{"))
                {
                    var s = child.gameObject.name.Split('{', '}');
                    _main.Game.scope.Add(s[1], child.gameObject);
                }
            }
            else if (child.gameObject.name.Contains("{"))
            {
                var s = child.gameObject.name.Split('{', '}');
                _main.Game.scope.Add(s[1], child.gameObject);
            }
        }
    }

    public void SwitchResolution(bool portrait)
    {
        if (portrait)
        {
            EditorUtils.SetSizeToScreen(GameViewSizeGroupType.Android, 480, 800);
            Screen.SetResolution(480, 800, true);
        }
        else
        {
            EditorUtils.SetSizeToScreen(GameViewSizeGroupType.Android, 800, 480);
            Screen.SetResolution(800, 480, true);
        }

        // we need to wait a frame for the canvas to process its new resolution, then build the layout.
        StartCoroutine(RebuildView());
    }

    public IEnumerator RebuildView()
    {
        yield return new WaitForSeconds(0.01f);
        Build(true);
    }
}