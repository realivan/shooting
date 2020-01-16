using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
    private Image bg;
    private Image joystick;
    private Vector2 inVector;

    void Start() {
        joystick = transform.GetComponentInChildren<Image>();
        bg = transform.GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bg.rectTransform, eventData.position, eventData.pressEventCamera, out pos)) {
            pos.x = pos.x / bg.rectTransform.sizeDelta.x;
            pos.y = pos.y / bg.rectTransform.sizeDelta.y;
        }
        inVector = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1);
        inVector = (inVector.magnitude > 1f) ? inVector.normalized : inVector;
        joystick.rectTransform.anchoredPosition = new Vector2(
            inVector.x * (bg.rectTransform.sizeDelta.x * 0.5f),
            inVector.y * (bg.rectTransform.sizeDelta.y * 0.5f));
    }

    public void OnPointerDown(PointerEventData eventData) {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData) {
        inVector = Vector2.zero;
        joystick.rectTransform.anchoredPosition = inVector;
    }

    public float DirX() {
        if (inVector.x != 0) {
            return inVector.x;
        } else
            return Input.GetAxis("Horozontal");
    }

    public float DirY(){
        if (inVector.y != 0) {
            return inVector.y;
        } else
            return Input.GetAxis("Vertical");
    }
}
