using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepScript : ProjectBehaviourScript
{
    public Sprite[] sprites;

    public Sprite sprite;

    public Vector2 direction;
    private void Start()
    {
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCtrl player = collision.gameObject.GetComponent<PlayerCtrl>();
        if (player != null && this.direction != Vector2.zero)
        {
            player.direction = this.direction;
        }
        
    }
    
}
