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
    [SerializeField] private GameObject timer;
    private Collider2D collider;
    private RectTransform rect;
    private bool begin = false;

    void Awake()
    {
        collider = GetComponent<Collider2D>();
        rect = GetComponent<RectTransform>();
        UpdateElementSprite(element);
        begin = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == collider)
            {
                bool isInduction = element.type == ElementType.INDUCTION;
                if (element.type == ElementType.COLD_RESISTOR || element.type == ElementType.BROKEN_RESISTOR)
                {
                    element.type = ElementType.RESISTOR;
                    element.resistorLives = 0;
                    GameProgress.resources--;
                }
                else
                {
                    element.Turn();
                }
                game.UpdateField();
                if (isInduction && element.type == ElementType.INDUCTION_USED)
                {
                    Instantiate(timer,  (Vector2)rect.position + rect.rect.size / 2 * rect.localScale, Quaternion.identity);
                }
            }

            if (!begin)
            {
                StartCoroutine(Break());
                begin = true;
            }
        }
    }

    public void UpdateElementSprite(Element e)
    {
        if (element.type != ElementType.RESISTOR && e.type == ElementType.RESISTOR && begin)
        {
            StartCoroutine(Break());
        }
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
    }

    private IEnumerator Break()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (element.type == ElementType.RESISTOR)
            {
                if (element.connected)
                {
                    element.resistorLives++;
                }
                else
                {
                    element.resistorLives--;
                }

                if (element.resistorLives <= -5)
                {
                    element.type = ElementType.COLD_RESISTOR;
                    game.UpdateField();
                } 
                else if (element.resistorLives >= 5)
                {
                    element.type = ElementType.BROKEN_RESISTOR;
                    game.UpdateField();
                }
            }
        }
    }

}
