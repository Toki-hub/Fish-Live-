using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class TrashItem : MonoBehaviour, IPointerClickHandler
{
    private RectTransform rt;
    private RectTransform panelRt;
    private float fallSpeed;
    private float bottomPadding;
    private bool landed = false;

    public void Init(RectTransform panel, float speed, float padding)
    {
        panelRt = panel;
        fallSpeed = speed;
        bottomPadding = padding;
        rt = GetComponent<RectTransform>();

        Image img = GetComponent<Image>();
        if (img != null) img.raycastTarget = true;
    }

    void Update()
    {
        if (landed || panelRt == null) return;

        Vector2 pos = rt.anchoredPosition;
        pos.y -= fallSpeed * Time.deltaTime;

        float floorY = panelRt.rect.yMin + bottomPadding;
        if (pos.y <= floorY)
        {
            pos.y = floorY;
            landed = true;
        }
        rt.anchoredPosition = pos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
}