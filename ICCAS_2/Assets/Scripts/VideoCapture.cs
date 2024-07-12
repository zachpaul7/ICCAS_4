using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoCapture : MonoBehaviour
{
    public GameObject InputTexture;
    public RawImage VideoScreen;
    public GameObject VideoBackground;
    public float VideoBackgroundScale;
    public LayerMask _layer;
    public bool UseWebCam = true;
    public int WebCamIndex = 0;
    public VideoPlayer VideoPlayer;

    private WebCamTexture webCamTexture;
    private RenderTexture videoTexture;

    private int videoScreenWidth = 2560;
    private int bgWidth, bgHeight;

    public RenderTexture MainTexture { get; private set; }

    /// <summary>
    /// Initialize Camera
    /// </summary>
    /// <param name="bgWidth"></param>
    /// <param name="bgHeight"></param>
    public void Init(int bgWidth, int bgHeight)
    {
        this.bgWidth = bgWidth;
        this.bgHeight = bgHeight;
        if (UseWebCam) CameraPlayStart();
        else VideoPlayStart();
    }

    /// <summary>
    /// Play Web Camera
    /// </summary>
    private void CameraPlayStart()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length <= WebCamIndex)
        {
            WebCamIndex = 0;
        }

        webCamTexture = new WebCamTexture(devices[WebCamIndex].name);

        var sd = VideoScreen.GetComponent<RectTransform>();
        VideoScreen.texture = webCamTexture;

        webCamTexture.Play();

        // RawImage의 크기를 고정된 값으로 설정
        sd.sizeDelta = new Vector2(1024, 1024); // 여기서 bgHeight를 사용하여 고정된 높이를 설정합니다.
        var aspect = (float)webCamTexture.width / webCamTexture.height;
        VideoBackground.transform.localScale = new Vector3(aspect, 1, 1) * VideoBackgroundScale;
        VideoBackground.GetComponent<Renderer>().material.mainTexture = webCamTexture;

        InitMainTexture();
    }

    /// <summary>
    /// Play video
    /// </summary>
    private void VideoPlayStart()
    {
        videoTexture = new RenderTexture((int)VideoPlayer.clip.width, (int)VideoPlayer.clip.height, 24);

        VideoPlayer.renderMode = VideoRenderMode.RenderTexture;
        VideoPlayer.targetTexture = videoTexture;

        var sd = VideoScreen.GetComponent<RectTransform>();
        // RawImage의 크기를 고정된 값으로 설정
        sd.sizeDelta = new Vector2(1024, 1024); // 여기서 bgHeight를 사용하여 고정된 높이를 설정합니다.
        VideoScreen.texture = videoTexture;

        VideoPlayer.Play();

        var aspect = (float)videoTexture.width / videoTexture.height;

        VideoBackground.transform.localScale = new Vector3(aspect, 1, 1) * VideoBackgroundScale;
        VideoBackground.GetComponent<Renderer>().material.mainTexture = videoTexture;

        InitMainTexture();
    }

    /// <summary>
    /// Initialize Main Texture
    /// </summary>
    private void InitMainTexture()
    {
        this.gameObject.transform.localPosition = new Vector3(5f, 0f, 0f);



        GameObject go = new GameObject("MainTextureCamera", typeof(Camera));

        go.transform.parent = VideoBackground.transform;
        go.transform.localScale = new Vector3(-1.0f, -1.0f, 1.0f);
        go.transform.localPosition = new Vector3(0.0f, 0.0f, -2.0f);
        go.transform.localEulerAngles = Vector3.zero;
        go.layer = _layer;

        var camera = go.GetComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 0.5f;
        camera.depth = -5;
        camera.depthTextureMode = 0;
        camera.clearFlags = CameraClearFlags.Color;
        camera.backgroundColor = Color.black;
        camera.cullingMask = _layer;
        camera.useOcclusionCulling = false;
        camera.nearClipPlane = 1.0f;
        camera.farClipPlane = 5.0f;
        camera.allowMSAA = false;
        camera.allowHDR = false;

        MainTexture = new RenderTexture(bgWidth, bgHeight, 0, RenderTextureFormat.RGB565, RenderTextureReadWrite.sRGB)
        {
            useMipMap = false,
            autoGenerateMips = false,
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Point,
        };

        camera.targetTexture = MainTexture;
        if (InputTexture.activeSelf) InputTexture.GetComponent<Renderer>().material.mainTexture = MainTexture;
    }
}