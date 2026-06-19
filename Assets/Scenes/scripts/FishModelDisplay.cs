using UnityEngine;
using UnityEngine.UI;

public class FishModelDisplay : MonoBehaviour
{
    [Header("Model 3D tiap ikan")]
    public GameObject[] fishModels;

    [Header("Rotasi")]
    public float rotateSpeed = 40f;

    [Header("Referensi")]
    public Camera displayCamera;
    public RawImage displayRawImage;

    private GameObject currentModel;
    private int currentIndex = -1;
    private float currentYAngle = 0f;
    private Quaternion[] initialRotations;
    private RenderTexture rt;

    public static FishModelDisplay Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        initialRotations = new Quaternion[fishModels.Length];
        for (int i = 0; i < fishModels.Length; i++)
        {
            if (fishModels[i] != null)
            {
                initialRotations[i] = fishModels[i].transform.localRotation;
                fishModels[i].SetActive(false);
            }
        }

        // Use RenderTexture already assigned to the camera in Inspector
        rt = displayCamera.targetTexture;

        if (displayCamera != null)
        {
            displayCamera.clearFlags = CameraClearFlags.SolidColor;
            displayCamera.backgroundColor = new Color(0, 0, 0, 0);
            displayCamera.nearClipPlane = 0.01f;
            displayCamera.farClipPlane = 100f;
            displayCamera.fieldOfView = 60f;
        }

        if (displayRawImage != null)
            displayRawImage.texture = rt;
    }

    void Update()
    {
        if (currentModel == null || currentIndex < 0) return;

        currentYAngle += rotateSpeed * Time.deltaTime;

        Vector3 initialEuler = initialRotations[currentIndex].eulerAngles;
        currentModel.transform.localEulerAngles = new Vector3(
            initialEuler.x,
            currentYAngle,
            initialEuler.z
        );
    }

    public void ShowModel(int fishIndex)
    {
        Debug.Log($"ShowModel index: {fishIndex}");

        if (fishModels == null || fishIndex < 0
            || fishIndex >= fishModels.Length
            || fishModels[fishIndex] == null)
        {
            Debug.LogError($"Model index {fishIndex} tidak valid!");
            return;
        }

        foreach (var m in fishModels)
            if (m != null) m.SetActive(false);

        currentModel = fishModels[fishIndex];
        currentIndex = fishIndex;
        currentYAngle = 0f;

        currentModel.SetActive(true);
        SetLayerRecursive(currentModel, LayerMask.NameToLayer("FishDisplay"));
        currentModel.transform.localRotation = initialRotations[fishIndex];

        Debug.Log($"Model aktif: {currentModel.name}");

        if (displayCamera != null)
            displayCamera.transform.LookAt(currentModel.transform.position);
    }

    public void HideModel()
    {
        foreach (var m in fishModels)
            if (m != null) m.SetActive(false);
        currentModel = null;
        currentIndex = -1;
        Debug.Log("Semua model disembunyikan");
    }

    void OnDestroy()
    {
        if (rt != null) rt.Release();
    }

    void SetLayerRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursive(child.gameObject, layer);
    }
}