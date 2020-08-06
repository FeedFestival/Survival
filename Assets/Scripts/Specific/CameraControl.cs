using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public bool debug;

    public void Init(bool editorView = false)
    {
        //Debug.Log(GetMainGameViewSize().ToString());

        //Debug.Log("w: " + Screen.width + ", h: " + Screen.height);
        //Debug.Log("w: " + Screen.currentResolution.width + ", h: " + Screen.currentResolution.height);

        float width, height;
        if (editorView)
        {
            var size = GetMainGameViewSize();
            width = size.x;
            height = size.y;
        }
        else
        {
            width = (float)Screen.width;
            height = (float)Screen.height;
        }

        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        float windowaspect = width / height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

    public static Vector2 GetMainGameViewSize()
    {
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
        return (Vector2)Res;
    }

    public Transform LookAt;

    private bool smooth = true;
    private float smoothSpeed = 0.125f;
    private Vector3 offset = new Vector3(-4.77f, 20, 4.47f);

    private void LateUpdate()
    {
        Vector3 desiredPosition = LookAt.transform.position + offset;

        if (smooth)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }
        else
        {
            transform.position = desiredPosition;
        }
    }

    //private CameraEdge CameraEdge;
    //private CameraEdgeSpeed CameraEdgeSpeed;

    //[HideInInspector]
    //public CameraCursor CameraCursor;
    //[HideInInspector]
    //public CameraView CameraView;

    //[HideInInspector]
    //public HUD HUD;

    //[HideInInspector]
    //KeyboardInput KeyboardInput;

    //[HideInInspector]
    //public float YDistanceFromPlayer;

    //[HideInInspector]
    //public Transform thisTransform;

    //private Vector3 DesiredPosition;

    //private float minCameraPanSpeed = 4.42f;
    //private float CameraPanSpeed;

    //private Vector3 vectorUp = new Vector3(0, 0.73f, 0.46f);
    //private Vector3 cameraDirection;

    //private bool _centerCamera;
    //public bool CenterCamera
    //{
    //    set
    //    {
    //        _centerCamera = value;

    //        if (_centerCamera)
    //        {
    //            CameraPanSpeed = 2.3f;
    //        }
    //    }
    //    get
    //    {
    //        return _centerCamera;
    //    }
    //}
    //public bool PanCamera;

    //Transform ObjectToFollow;

    //public void Initialize()
    //{
    //    GlobalData.CameraControl = this;

    //    //  Scripts initialization
    //    CameraCursor = GetComponent<CameraCursor>();
    //    CameraCursor.Initialize();

    //    CameraView = GetComponent<CameraView>();
    //    //CameraView.Initialize(this);

    //    KeyboardInput = GetComponent<KeyboardInput>();
    //    KeyboardInput.Initialize(this);

    //    //  Props
    //    YDistanceFromPlayer = 18f;

    //    thisTransform = this.transform;

    //    CameraPanSpeed = minCameraPanSpeed;

    //    HUD = GetComponent<HUD>();
    //    HUD.Initialize(this);

    //    CenterCamera = true;

    //    ObjectToFollow = GlobalData.Player.transform; // HARD_CODED
    //}

    //void Update()
    //{
    //    //if (Input.GetKey(KeyCode.Space))
    //    //{
    //    //    if (!CenterCamera)
    //    //        CenterCamera = true;
    //    //}
    //    if (CenterCamera)
    //    {
    //        CenterCameraOn();
    //    }
    //    //if (PanCamera && !this.HUD.I_INVENTORY_button.pressed)
    //    //{
    //    //    ThisUnitTransform.Translate(cameraDirection * CameraPanSpeed * Time.deltaTime);
    //    //    if (CheckYDistance())
    //    //    {
    //    //        DesiredPosition = new Vector3(ThisUnitTransform.position.x, YDistanceFromPlayer, ThisUnitTransform.position.z);
    //    //        ThisUnitTransform.position = Vector3.Lerp(ThisUnitTransform.position, DesiredPosition, Time.deltaTime * 2f);
    //    //    }
    //    //}
    //}

    //private void CenterCameraOn()
    //{
    //    if (GlobalData.Player != null)
    //    {
    //        if (HUD.I_INVENTORY_button.pressed == false)
    //        {
    //            if (ObjectToFollow == null)
    //            {
    //                ObjectToFollow = GlobalData.Player.transform;
    //            }
    //            DesiredPosition = new Vector3(ObjectToFollow.position.x - 8f, YDistanceFromPlayer, ObjectToFollow.position.z + 8f);
    //        }
    //        else
    //        {
    //            //Debug.Log(DesiredPosition);
    //            DesiredPosition = new Vector3(ObjectToFollow.position.x - 4.30f, 11f, ObjectToFollow.position.z + 4.30f);
    //        }
    //        thisTransform.position = Vector3.Lerp(thisTransform.position, DesiredPosition, Time.deltaTime * CameraPanSpeed);
    //    }
    //}

    //public void CameraPan(CameraEdge cameraEdge, CameraEdgeSpeed cameraEdgeSpeed)
    //{
    //    if (GlobalData.Player.UnitPrimaryState != UnitPrimaryState.Walk)
    //    {
    //        CameraEdge = cameraEdge;
    //        CameraEdgeSpeed = cameraEdgeSpeed;
    //        MoveCameraDirection();
    //        CenterCamera = false;
    //        PanCamera = true;
    //    }
    //}

    //public void MoveCameraDirection()
    //{
    //    if (CameraEdgeSpeed == CameraEdgeSpeed.Slow)
    //        CameraPanSpeed = minCameraPanSpeed / 4;
    //    else
    //        CameraPanSpeed = minCameraPanSpeed;

    //    switch (CameraEdge)
    //    {
    //        case CameraEdge.T:

    //            cameraDirection = vectorUp;
    //            break;

    //        case CameraEdge.TR:

    //            cameraDirection = (vectorUp + Vector3.right);
    //            break;

    //        case CameraEdge.R:

    //            cameraDirection = Vector3.right;
    //            break;

    //        case CameraEdge.DR:

    //            cameraDirection = ((-vectorUp) + Vector3.right);
    //            break;

    //        case CameraEdge.D:

    //            cameraDirection = -vectorUp;
    //            break;

    //        case CameraEdge.DL:

    //            cameraDirection = ((-vectorUp) + (-Vector3.right));
    //            break;

    //        case CameraEdge.L:

    //            cameraDirection = -Vector3.right;
    //            break;

    //        case CameraEdge.TL:

    //            cameraDirection = (vectorUp + (-Vector3.right));
    //            break;

    //        default:
    //            break;
    //    }
    //}

    //private bool CheckYDistance()
    //{
    //    if (GlobalData.Player != null)
    //    {
    //        if (ObjectToFollow == null)
    //        {
    //            ObjectToFollow = GlobalData.Player.transform; // HARD_CODED
    //        }
    //        var distance = Mathf.Round((ObjectToFollow.position.y + YDistanceFromPlayer) * 1000f) / 1000f;
    //        var cameraCurrentPosition = Mathf.Round((thisTransform.position.y) * 1000f) / 1000f;
    //        if (distance != cameraCurrentPosition)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}
