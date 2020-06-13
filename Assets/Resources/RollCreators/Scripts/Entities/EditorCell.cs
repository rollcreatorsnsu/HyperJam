using UnityEngine;

public class EditorCell : MonoBehaviour
{
    public Editor editor;
    [HideInInspector] public Element element;
    private Collider2D collider;

    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider == collider)
            {
                if (editor.selectedCell == this)
                {
                    element.Turn();
                    transform.rotation = Quaternion.Euler(0, 0, element.rotation * 90);
                }
                else
                {
                    editor.selectedCell = this;
                }
            }
        }
    }

    public void SetElement(Element e)
    {
        element = e;
        GetComponent<SpriteRenderer>().sprite = Util.GetElementSprite(e);
    }
}
