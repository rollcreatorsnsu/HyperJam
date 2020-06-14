using System;
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
    [SerializeField] private AudioSource rotationSound;
    [SerializeField] private AudioSource lampSound;
    [SerializeField] private AudioSource inductionSound;
    [SerializeField] private GameObject warningWarmEffect;
    [SerializeField] private GameObject warningColdEffect;
    [SerializeField] private GameObject repairEffect;
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
        if (game.end) return;
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == collider && element.type != ElementType.NONE)
            {
                bool isInduction = element.type == ElementType.INDUCTION;
                bool connected = element.connected;
                if ((element.type == ElementType.COLD_RESISTOR || element.type == ElementType.BROKEN_RESISTOR) && GameProgress.resources > 0)
                {
                    element.type = ElementType.RESISTOR;
                    element.resistorLives = 0;
                    GameObject effect = Instantiate(repairEffect, transform.position, Quaternion.identity);
                    Destroy(effect, 1);
                    GameProgress.resources--;
                }
                else
                {
                    element.Turn();
                    rotationSound.Play();
                }
                game.UpdateField(true);
                if (isInduction && element.type == ElementType.INDUCTION_USED)
                {
                    Instantiate(timer,  (Vector2)rect.position + rect.rect.size / 2 * rect.localScale, Quaternion.identity);
                    inductionSound.Play();
                }
                if (element.type == ElementType.LAMP && element.connected != connected)
                {
                    lampSound.Play();
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
        if (element.type == ElementType.RESISTOR)
        {
            light.color = new Color(1, 1, 1, Math.Abs(element.resistorLives) * 0.2f);
        }
        else
        {
            light.color = Color.white;
        }
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
                    GameObject effect = Instantiate(warningColdEffect, transform.position, Quaternion.identity);
                    Destroy(effect, 1);
                    game.UpdateField(true);
                } 
                else if (element.resistorLives >= 5)
                {
                    element.type = ElementType.BROKEN_RESISTOR;
                    GameObject effect = Instantiate(warningWarmEffect, transform.position, Quaternion.identity);
                    Destroy(effect, 1);
                    game.UpdateField(true);
                }
            }
            game.UpdateField(false);
        }
    }

}
