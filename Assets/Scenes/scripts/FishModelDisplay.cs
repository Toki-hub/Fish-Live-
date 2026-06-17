using UnityEngine;
using UnityEngine.UI;

public class FishModelDisplay : MonoBehaviour
{
    [Header("Model 3D tiap ikan")]
    public GameObject[] fishModels;

    [Header("Rotasi")]
    public float rotateSpeed = 40f;

    [Header("Referensi")]
    public Camera   displayCamera;
    public RawImage displayRawImage;

    private GameObject   currentModel;
    private int          currentIndex = -1;
    private float        currentYAngle = 0f;
    private Quaternion[] initialRotations;  // ← simpan rotasi awal tiap model
    private RenderTexture rt;

    public static FishModelDisplay Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        // Simpan rotasi awal semua model sebelum diubah
        initialRotations = new Quaternion[fishModels.Length];
        for (int i = 0; i < fishModels.Length; i++)
        {
            if (fishModels[i] != null)
            {
                initialRotations[i] = fishModels[i].transform.localRotation;
                fishModels[i].SetActive(false);  // semua off di awal
            }
        }

        // Buat RenderTexture
        rt = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32);
        rt.Create();

        Debug.Log($"RT created: {rt.IsCreated()}, width: {rt.width}");

        if (displayCamera != null)
        {
            displayCamera.targetTexture = rt;
            displayCamera.nearClipPlane = 0.01f;
            displayCamera.farClipPlane  = 100f;
            displayCamera.fieldOfView   = 60f;
            Debug.Log($"Camera target texture: {displayCamera.targetTexture != null}");
        }

        if (displayRawImage != null)
        {
            displayRawImage.texture = rt;
            Debug.Log($"RawImage texture set: {displayRawImage.texture != null}");
        }
    }

    void Update()
    {
        if (currentModel == null || currentIndex < 0) return;

            currentYAngle += rotateSpeed * Time.deltaTime;

            // Ambil euler awal (X=-90, Z=0 dari initialRotations)
            Vector3 initialEuler = initialRotations[currentIndex].eulerAngles;

            // Set langsung: X tetap dari awal, Y yang berputar, Z tetap
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

        // Matikan SEMUA model dulu
        foreach (var m in fishModels)
            if (m != null) m.SetActive(false);

        // Aktifkan yang dipilih
        currentModel  = fishModels[fishIndex];
        currentIndex  = fishIndex;
        currentYAngle = 0f;  // reset putaran

        currentModel.SetActive(true);

        // Set layer
        SetLayerRecursive(currentModel, LayerMask.NameToLayer("FishDisplay"));

        // Kembalikan ke rotasi awal (X=-90 tetap, Y=0)
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