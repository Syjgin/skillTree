using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LinesRenderer : MonoBehaviour
    {
        [SerializeField] private Sprite _sprite;
        
        public void DrawLine(Vector2 startPoint, Vector2 endPoint)
        {
            var line = new GameObject
            {
                name = $"line between {startPoint} and {endPoint}"
            };
            var image = line.AddComponent<Image>();
            image.sprite = _sprite;
            image.color = Color.black;
            var rect = line.GetComponent<RectTransform>();
            rect.SetParent(transform);
            rect.localPosition = (startPoint + endPoint) / 2f;
            var diff = startPoint - endPoint;
            rect.sizeDelta = new Vector2(diff.magnitude, 2f);
            rect.rotation = Quaternion.Euler(0,0,180 * Mathf.Atan(diff.y / diff.x) / Mathf.PI);
        }
    }
}