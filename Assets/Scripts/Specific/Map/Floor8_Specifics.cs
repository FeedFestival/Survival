using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor8_Specifics : MonoBehaviour
{
    private bool switched;

    public ChangeLightmapData ChangeLightmapData;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //switched = !switched;
            //if (switched)
            //    ChangeLightmapData.ChangeLightmapTo(1);
            //else
            //    ChangeLightmapData.ChangeLightmapTo(0);

            ChangeLightmapData.ChangeLightmapV2();
        }

    }
}
