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
                if (element.connected != connected) // TODO: fix
                {
                    if (element.type == ElementType.LAMP)
                    {
                        lampSound.Play();
                    }
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
        if (e.type == ElementType.NONE && element.type == ElementType.NONE) return;
        if (element.type != ElementType.RESISTOR && e.type == ElementType.RESISTOR && begin)
        {
            StartCoroutine(Break());
        }
        if (e.type == ElementType.INDUCTION && e.connected)
        {
            e.type = ElementType.INDUCTION_USED;
            Instantiate(timer,  (Vector2)rect.position + rect.rect.size / 2 * rect.localScale, Quaternion.identity);
            inductionSound.Play();
        }
        if (element.type != e.type)
        {
            spriteRenderer.sprite = Util.GetElementSprite(e);
            staticImage.sprite = Util.GetElementStaticSprite(e);
            List<Sprite> electricitySprites = Util.GetElementElectricitySprites(e);
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
            light.sprite = Util.GetElementLightSprite(e);
        }
        if (!e.connected && e.type != ElementType.CONDENSER_ON)
        {
            foreach (SpriteRenderer el in electricity)
            {
                el.color = Color.clear;
            }
            light.color = Color.clear;
        }
        else
        {
            foreach (SpriteRenderer el in electricity)
            {
                el.color = Color.white;
            }
            light.color = Color.white;
        }
        element = e;
        if (element.type == ElementType.RESISTOR)
        {
            light.sprite = Util.GetElementLightSprite(e);
            light.color = new Color(1, 1, 1, Math.Abs(element.resistorLives) * 0.2f);
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
