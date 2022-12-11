using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesFiller : MonoBehaviour
{
    [SerializeField]
    private GameObject[] trees = new GameObject[3];
    [SerializeField]
    private BoxCollider2D forestPlace;
    
    public float distance = 1f;
    public int startOrder = 1;

    void Start()
    {
        float boundsSizeY = forestPlace.bounds.size.y;
        float startY = forestPlace.transform.position.y  + boundsSizeY / 2;
        float endY = forestPlace.transform.position.y  - boundsSizeY / 2;

        float boundsSizeX = forestPlace.bounds.size.x;
        float startX = forestPlace.transform.position.x - boundsSizeX / 2;
        float endX = forestPlace.transform.position.x + boundsSizeX / 2;

        int order = startOrder;
        for (float y = startY; y >= endY; y -= distance + Random.Range(0, distance/5))
        {
            for (float x = startX ; x <= endX ; x += distance + Random.Range(0, distance / 5))
            {
                GameObject tree = Instantiate(trees[Mathf.FloorToInt(Random.Range(0, 3))]);
                tree.transform.position = new Vector2(x, y);
                tree.GetComponent<SpriteRenderer>().sortingOrder = order;
            }
            order++;
        }
    }
}
