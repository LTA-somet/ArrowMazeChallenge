using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCtrl : ProjectBehaviourScript
{
    public Vector2 direction;
    int layerMaskToHit;
    public Vector2 currentDirection;
    public Vector2 nextDirection;
    public LayerMask layerPlayerMask;

    private float speed = 5f;
    public Transform sprite;

    private void Start()
    {
        sprite = transform.Find("Sprite").transform;
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        int playerLayer = LayerMask.NameToLayer("Player");
        layerMaskToHit = ~(1 << playerLayer);
        SetSprite(direction);
    }

    public void Moverment()
    {
        this.GetComponent<Rigidbody2D>().AddForce(direction * speed, ForceMode2D.Impulse);
    }
    public void Stop()
    {
        
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        SetSprite(direction);
        transform.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerPlayerMask);
            if (hit.collider != null && hit.collider.gameObject.GetComponent<PlayerCtrl>())
            {
                Moverment();
                currentDirection = hit.collider.GetComponent<PlayerCtrl>().direction;
            }
            else
            {
                Debug.Log("no found move obj " + hit.point);
            }
        }
        Vector2 raycastStartPosition = (Vector2)this.transform.position + (currentDirection * 0.5f);
        RaycastHit2D check = Physics2D.Raycast(raycastStartPosition, currentDirection, 2f, layerMaskToHit);
        
        if (check.collider == null || check.collider.GetComponent<Obstacle>() != null)
        {
            Stop();
        }
        Debug.DrawRay(this.transform.position, currentDirection * 1f, Color.red);
      
    }
    void SetSprite(Vector2 dir)
    {
        if (sprite != null)
        {
            if (dir == Vector2.up)
            {
                sprite.rotation = Quaternion.Euler(0, 0, -90);
            }
            else if (dir == Vector2.right)
            {
                sprite.rotation = Quaternion.Euler(0, 0, 180    );
            }
            else if (dir == Vector2.down)
            {
                sprite.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (dir == Vector2.left)
            {
                sprite.rotation = Quaternion.Euler(0, 0, 0);
            }
            
        }
    }
}