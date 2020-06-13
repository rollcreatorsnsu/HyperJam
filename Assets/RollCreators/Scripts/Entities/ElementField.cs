using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(BoxCollider2D), typeof(Image))]
public class ElementField : MonoBehaviour
{
    public Game game;
    [HideInInspector] public Element element;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            element.Turn();
            transform.rotation = Quaternion.Euler(0, 0, element.rotation * 90);
            game.UpdateField();
        }
    }

    public void SetElement(Element e)
    {
        element = e;
//        GetComponent<SpriteRenderer>().sprite = Util.GetElementSprite(element.type); TODO
        transform.rotation = Quaternion.Euler(0, 0, element.rotation * 90);
        if (element.type == ElementType.RESISTOR)
        {
            StartCoroutine(Break());
        }
    }

    private IEnumerator Break()
    {
        while (true)
        {
            yield return new WaitForSeconds(5); // TODO: timing
            element.type = ElementType.BROKEN_RESISTOR;
            game.UpdateField();
        }
    }
    
    
}
