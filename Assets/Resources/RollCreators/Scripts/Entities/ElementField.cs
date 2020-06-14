using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementField : MonoBehaviour
{
    public Game game;
    [HideInInspector] public Element element;
    [SerializeField] private SpriteRenderer staticImage;
    [SerializeField] private List<SpriteRenderer> electricity;
    [SerializeField] private SpriteRenderer light;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Collider2D collider;

    void Awake()
    {
        collider = GetComponent<Collider2D>();
        UpdateElementSprite(element);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == collider)
            {
                if (element.type == ElementType.COLD_RESISTOR || element.type == ElementType.BROKEN_RESISTOR)
                {
                    element.type = ElementType.RESISTOR;
                    StartCoroutine(Break());
                    GameProgress.resources--;
                }
                else
                {
                    element.Turn();
                }
                game.UpdateField();
            }
        }
    }

    public void UpdateElementSprite(Element e)
    {
        element = e;
        spriteRenderer.sprite = Util.GetElementSprite(element);
        staticImage.sprite = Util.GetElementStaticSprite(element);
        List<Sprite> electricitySprites = Util.GetElementElectricitySprites(element);
        if (electricitySprites != null)
        {
            for (int i = 0; i < electricitySprites.Count; i++)
            {
                electricity[i].sprite = electricitySprites[i];
            }

            for (int i = electricitySprites.Count; i < electricity.Count; i++)
            {
                electricity[i].sprite = null;
            }
        }
        else
        {
            for (int i = 0; i < electricity.Count; i++)
            {
                electricity[i].sprite = null;
            }
        }
        light.sprite = Util.GetElementLightSprite(element);
        transform.rotation = Quaternion.Euler(0, 0, element.rotation * 90);
        staticImage.gameObject.transform.rotation = Quaternion.Euler(0, 0, -transform.rotation.z);
        if (element.type == ElementType.RESISTOR)
        {
            StartCoroutine(Break());
        }
    }

    private IEnumerator Break()
    {
        bool connected;
        do
        {
            connected = element.connected;
            yield return new WaitForSeconds(5); // TODO: timing
        } while (element.connected != connected);

        if (element.type == ElementType.RESISTOR)
        {
            if (connected)
            {
                element.type = ElementType.BROKEN_RESISTOR;
            }
            else
            {
                element.type = ElementType.COLD_RESISTOR;
            }
            game.UpdateField();
        }
    }

}
