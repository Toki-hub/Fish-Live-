using UnityEngine;
using UnityEngine.UI;

public class FishSwim2D : MonoBehaviour
{
    [Header("Kecepatan")]
    public float speed        = 90f;
    public float minStraight  = 1.5f;
    public float maxStraight  = 3.5f;
    public float maxTurnAngle = 35f;
    public float edgeMargin   = 20f;   // jarak ekstra dari tepi panel

    [HideInInspector] public float startDelay = 0f;

    private Vector2 direction;
    private RectTransform rt;
    private RectTransform panelRt;
    private float turnTimer;
    private bool isMoving = false;
    private Vector2 halfSize;

    void OnEnable()
    {
        rt      = GetComponent<RectTransform>();
        panelRt = transform.parent.GetComponent<RectTransform>();

        // Wajib — koordinat anchoredPosition harus match dengan panelRt.rect
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot     = new Vector2(0.5f, 0.5f);

        halfSize = rt.sizeDelta * 0.5f;

        Rect b = GetClampedBounds();
        rt.anchoredPosition = new Vector2(
            Random.Range(b.xMin, b.xMax),
            Random.Range(b.yMin, b.yMax)
        );

        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-0.3f, 0.3f)).normalized;
        ScheduleTurn();

        if (startDelay > 0f) Invoke(nameof(StartMoving), startDelay);
        else isMoving = true;
    }

    void StartMoving() => isMoving = true;

    // Batas gerak dikurangi setengah ukuran sprite — badan ikan
    // tidak akan pernah poke keluar dari rect AquariumPanel
    Rect GetClampedBounds()
    {
        Rect r = panelRt.rect;
        return new Rect(
            r.xMin + halfSize.x + edgeMargin,
            r.yMin + halfSize.y + edgeMargin,
            r.width  - 2 * (halfSize.x + edgeMargin),
            r.height - 2 * (halfSize.y + edgeMargin)
        );
    }

    void Update()
    {
        if (!isMoving || panelRt == null) return;

        Vector2 nextPos = rt.anchoredPosition + direction * speed * Time.deltaTime;
        Rect bounds = GetClampedBounds();
        bool bounced = false;

        if (nextPos.x < bounds.xMin) { nextPos.x = bounds.xMin; direction.x =  Mathf.Abs(direction.x); bounced = true; }
        if (nextPos.x > bounds.xMax) { nextPos.x = bounds.xMax; direction.x = -Mathf.Abs(direction.x); bounced = true; }
        if (nextPos.y < bounds.yMin) { nextPos.y = bounds.yMin; direction.y =  Mathf.Abs(direction.y); bounced = true; }
        if (nextPos.y > bounds.yMax) { nextPos.y = bounds.yMax; direction.y = -Mathf.Abs(direction.y); bounced = true; }

        if (bounced) direction.Normalize();

        // HARD CLAMP — posisi dipaksa pas di batas, tidak bisa overshoot
        rt.anchoredPosition = nextPos;

        turnTimer -= Time.deltaTime;
        if (turnTimer <= 0f)
        {
            direction = Rotate2D(direction, Random.Range(-maxTurnAngle, maxTurnAngle));
            ScheduleTurn();
        }

        Vector3 scale = transform.localScale;
        scale.x = direction.x >= 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;

        rt.localRotation = Quaternion.Euler(0f, 0f,
            Mathf.Clamp(direction.y * 25f, -20f, 20f));
    }

    Vector2 Rotate2D(Vector2 v, float deg)
    {
        float rad = deg * Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad),
            v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad)
        ).normalized;
    }

    void ScheduleTurn() => turnTimer = Random.Range(minStraight, maxStraight);

    public void PauseSwimming(float duration)
    {
        isMoving = false;
        Invoke(nameof(ResumeSwimming), duration);
    }
    void ResumeSwimming() => isMoving = true;
}