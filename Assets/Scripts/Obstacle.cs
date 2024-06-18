using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Obstacle : ProjectBehaviourScript
{
    public Vector2 direction;
    public bool isClick;
    List<GameObject> neighBors;
    Vector2 currentPos;
    Dictionary<Transform, Vector2> defaultDir;
    Dictionary<Transform, Sprite> defaultSprite;
    private void Start()
    {
        defaultDir = new Dictionary<Transform, Vector2>();
        defaultSprite = new Dictionary<Transform, Sprite>();
        isClick = false;
        neighBors = new List<GameObject>();
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        currentPos = transform.position;
    }

    public void Moverment()
    {
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        this.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
        
    }
    public void Stop()
    {
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isClick)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.GetComponent<Obstacle>())
            {
                isClick = !isClick;
                neighBors = findSurroundingGameObjects2D(currentPos);
                
                foreach (var item in neighBors)
                {
                    StepScript stepScript = item.gameObject.GetComponent<StepScript>();

                   
                    if(stepScript != null)
                    {
                        if(stepScript.direction != Vector2.zero)
                        {
                            defaultDir.Add(item.gameObject.transform, stepScript.direction);
                            defaultSprite.Add(item.gameObject.transform, stepScript.sprite);
                            item.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = stepScript.sprites[1];
                            item.transform.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = 1;
                            stepScript.direction = new Vector2(item.transform.position.x - this.transform.position.x, item.transform.position.y - transform.position.y);
                        }
                        item.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = stepScript.sprites[1];
                        item.transform.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = -1;
                        stepScript.direction = new Vector2(item.transform.position.x - this.transform.position.x, item.transform.position.y - transform.position.y);
                    }
                  
                }
            }
        }

        if (isClick && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.GetComponent<StepScript>() == true)
            {
                this.direction = hit.collider.gameObject.GetComponent<StepScript>().direction;
                transform.position = (Vector2)(transform.position) + direction;
                currentPos = transform.position;
                foreach (var item in neighBors)
                {
                    StepScript stepScript = item.gameObject.GetComponent<StepScript>();

                    stepScript.direction = Vector2.zero;
                    item.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = stepScript.sprites[0];
                    item.transform.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = -1;

                    if (defaultDir.ContainsKey(item.gameObject.transform)){
                        item.gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite[item.gameObject.transform];
                        item.gameObject.GetComponent<StepScript>().direction = defaultDir[item.gameObject.transform];
                        item.transform.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = -1;
                    }
                    if (defaultSprite.ContainsKey(item.gameObject.transform))
                    {
                        item.gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite[item.gameObject.transform];
                        
                    }

                }
                defaultDir.Clear();
                defaultSprite.Clear();
                neighBors.Clear();
                isClick = !isClick;
            }
        }

    }
    List<GameObject> findSurroundingGameObjects2D(Vector2 point)
    {
        
        List<GameObject> surroundingGameObjects = new List<GameObject>();

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 || j == 0)
                {
                    Vector2 position = new Vector2(point.x + i, point.y + j);
                    Collider2D collider = Physics2D.OverlapPoint(position);
                    if (collider != null)
                    {
                        StepScript step = collider.GetComponent<StepScript>();
                        if (step != null)
                            surroundingGameObjects.Add(collider.gameObject);
                    }
                    
                    else continue;
                }
                
                
            }
        }
        foreach(var item in surroundingGameObjects)
        {
            Debug.Log("Round " + item.transform.position);
        }
        return surroundingGameObjects;
    }
}
