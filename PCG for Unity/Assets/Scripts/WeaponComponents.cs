using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponents : MonoBehaviour {
    public Sprite[] modules;

    private Weapon parent;
    private SpriteRenderer spriteRender;

    private void Start()
    {
        parent = GetComponentInParent<Weapon>();
        spriteRender = GetComponent<SpriteRenderer>();
        spriteRender.sprite = modules[Random.Range(0, modules.Length)];
    }

    private void Update()
    {
        transform.eulerAngles = parent.transform.eulerAngles;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return spriteRender;
    }
}
