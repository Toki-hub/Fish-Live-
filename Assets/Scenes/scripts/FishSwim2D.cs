using UnityEngine;

public class FishSwim2D : MonoBehaviour
{
    [Header("Kecepatan")]
    public float speed        = 90f;
    public float minStraight  = 1.5f;
    public float maxStraight  = 3.5f;
    public float maxTurnAngle = 35f;
    public float padding      = 60f;

    [HideInInspector] public float startDelay = 0f;

    private Vector2 direction;
    private RectTransform rt;
    private RectTransform panelRt;
    private float turnTimer;
    private bool isMoving = false;

    void OnEnable()
    {
        rt      = GetComponent<RectTransform>();
        panelRt = transform.parent.GetComponent<RectTransform>();

        direction = new Vector2(Random.Range(-1f, 1f), 0f).normalized;
        ScheduleTurn();

        if (startDelay > 0f)
            Invoke(nameof(StartMoving), startDelay);
        else
            isMoving = true;
    }

    void StartMoving() => isMoving = true;

    void Update()
    {
        if (!isMoving || panelRt == null) return;

        rt.anchoredPosition += direction * speed * Time.deltaTime;

        Rect b      = panelRt.rect;
        Vector2 pos = rt.anchoredPosition;
        bool bounced = false;

        if (pos.x < b.xMin + padding) { direction.x =  Mathf.Abs(direction.x); bounced = true; }
        if (pos.x > b.xMax - padding) { direction.x = -Mathf.Abs(direction.x); bounced = true; }
        if (pos.y < b.yMin + padding) { direction.y =  Mathf.Abs(direction.y); bounced = true; }
        if (pos.y > b.yMax - padding) { direction.y = -Mathf.Abs(direction.y); bounced = true; }
        if (bounced) direction.Normalize();

        turnTimer -= Time.deltaTime;
        if (turnTimer <= 0f)
        {
            direction = Rotate2D(direction, Random.Range(-maxTurnAngle, maxTurnAngle));
            ScheduleTurn();
        }

        // Flip sprite arah kiri/kanan
        Vector3 scale = transform.localScale;
        scale.x = direction.x >= 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;

        // Tilt naik/turun sedikit
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

    void ScheduleTurn() =>
        turnTimer = Random.Range(minStraight, maxStraight);

    public void PauseSwimming(float duration)
    {
        isMoving = false;
        Invoke(nameof(ResumeSwimming), duration);
    }
    void ResumeSwimming() => isMoving = true;
}