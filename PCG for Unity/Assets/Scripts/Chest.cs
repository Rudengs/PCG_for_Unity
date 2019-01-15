using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    public Sprite openSprite;
    public Item randomItem;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Open()
    {
        spriteRenderer.sprite = openSprite;

        randomItem.RandomItemInit();
        GameObject toInstantiate = randomItem.gameObject;
        GameObject instance = Instantiate(toInstantiate, new Vector3(transform.position.x, transform.position.y, 0),
                                Quaternion.identity) as GameObject;
        instance.transform.SetParent(transform.parent);

        gameObject.layer = 10;
        spriteRenderer.sortingLayerName = "Items";
    }
}
