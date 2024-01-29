using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float swordLife = 8f;
    private Vector2 swordSpawnpoint;
    private float timer = 0f;
    private float speed = 8f;
    // Start is called before the first frame update
    void Start()
    {
        swordSpawnpoint = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > swordLife) Destroy(this.gameObject);
        timer += Time.deltaTime;
        transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
    }
}
