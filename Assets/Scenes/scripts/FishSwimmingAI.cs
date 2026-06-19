using UnityEngine;

public class FishSwimmingAI : MonoBehaviour
{
    [Header("Kecepatan")]
    public float swimSpeed   = 0.07f;
    public float maxTurnRate = 60f;     // derajat per detik

    [Header("Area")]
    public float maxRadius  = 0.12f;
    public float swimHeight = 0.04f;   // tinggi tetap di atas marker

    [Header("Belok acak")]
    public float minStraightTime = 1.5f;  // detik jalan lurus minimum
    public float maxStraightTime = 3.5f;  // detik jalan lurus maksimum
    public float maxTurnAngle    = 40f;   // besar belok maksimal (derajat)

    [Header("Model")]
    [Tooltip("0 / 90 / 180 / 270 — sesuaikan jika ikan mundur")]
    public float modelYOffset = 0f;

    private float currentAngle;   // heading sekarang (derajat)
    private float targetAngle;    // heading yang dituju
    private float turnTimer;
    private Transform markerCenter;

    void Start()
    {
        markerCenter = transform.parent;

        // Mulai dari arah random
        currentAngle = Random.Range(0f, 360f);
        targetAngle  = currentAngle;

        // Set posisi awal tepat di atas marker
        if (markerCenter != null)
        {
            Vector3 startPos = markerCenter.position;
            startPos.y       = markerCenter.position.y + swimHeight;
            transform.position = startPos;
        }

        ScheduleTurn();
    }

    void Update()
    {
        if (markerCenter == null) return;

        // ── Cek boundary ──────────────────────────────────────────
        Vector3 offset = transform.position - markerCenter.position;
        offset.y = 0f;

        if (offset.magnitude > maxRadius * 0.85f)
        {
            // Hadap ke tengah marker
            Vector3 toCenter = -offset.normalized;
            targetAngle = Mathf.Atan2(toCenter.x, toCenter.z) * Mathf.Rad2Deg;
        }
        else
        {
            // Timer belok normal
            turnTimer -= Time.deltaTime;
            if (turnTimer <= 0f)
            {
                // Belok kiri atau kanan sedikit
                targetAngle += Random.Range(-maxTurnAngle, maxTurnAngle);
                ScheduleTurn();
            }
        }

        // ── Putar heading perlahan ────────────────────────────────
        currentAngle = Mathf.MoveTowardsAngle(
            currentAngle, targetAngle, maxTurnRate * Time.deltaTime
        );

        // ── Hitung arah maju dari angle ───────────────────────────
        float rad  = currentAngle * Mathf.Deg2Rad;
        Vector3 fwd = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad));

        // ── Gerak maju, Y dikunci ─────────────────────────────────
        Vector3 pos = transform.position + fwd * swimSpeed * Time.deltaTime;
        pos.y = markerCenter.position.y + swimHeight;  // ← Y tidak pernah berubah
        transform.position = pos;

        // ── Rotasi visual (Y saja) ────────────────────────────────
        transform.rotation = Quaternion.Euler(
            0f, currentAngle + modelYOffset, 0f
        );
    }

    void ScheduleTurn()
    {
        turnTimer = Random.Range(minStraightTime, maxStraightTime);
    }

    public void PauseSwimming(float duration)
    {
        enabled = false;
        Invoke(nameof(ResumeSwimming), duration);
    }

    void ResumeSwimming() => enabled = true;
}