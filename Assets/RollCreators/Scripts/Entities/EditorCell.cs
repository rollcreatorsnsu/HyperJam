using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D), typeof(Image))]
public class EditorCell : MonoBehaviour
{
    public Editor editor;
    [HideInInspector] public Element element;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
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

    public void SetElement(Element e)
    {
        element = e;
        GetComponent<SpriteRenderer>().sprite = Util.GetElementSprite(e.type);
    }
}
