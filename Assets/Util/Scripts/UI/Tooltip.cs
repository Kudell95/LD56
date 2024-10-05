using TMPro;
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI HeaderField;
    public TextMeshProUGUI ContentField;
    public Image ImageField;
    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public RectTransform rectTransform;

    public float OffsetX;
    public float OffsetY;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }


    private void Update()
    {
        Vector2 pos = Input.mousePosition;

        float pivotX = pos.x / Screen.width;
        float pivotY = pos.y / Screen.height;
        float xOffsetAmt = 0;
        float yOffsetAmt = 0;

        rectTransform.pivot = new Vector2(pivotX, pivotY);


        if (pos.y > Screen.height / 2 && OffsetX > 0)
        {
            xOffsetAmt = -OffsetX;
            yOffsetAmt = -OffsetY;
        }
        else if (pos.y < Screen.height / 2 && OffsetX < 0)
        {
            xOffsetAmt = OffsetX;
            yOffsetAmt = OffsetY;
        }


        pos = new Vector2(pos.x + xOffsetAmt, pos.y + yOffsetAmt);

        transform.position = pos;
    }



    public void SetText(string content, string header = "", Sprite image = null)
    { 

        if (image == null)
            ImageField.gameObject.SetActive(false);
        else
        {
            ImageField.gameObject.SetActive(true);
            ImageField.sprite = image;
        }


        if (string.IsNullOrEmpty(header))
            HeaderField.gameObject.SetActive(false);
        else
        {
            HeaderField.gameObject.SetActive(true);
            HeaderField.text = header;
        }

        ContentField.text = content;



        int headerLength = HeaderField.text.Length;
        int contentLength = ContentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
    }
}
