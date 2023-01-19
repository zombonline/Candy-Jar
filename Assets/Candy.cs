using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Sprite[] sprites;

    public void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];   
    }

}
