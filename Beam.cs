using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public float beamLife = 1f;
    private Vector2 beamSpawnpoint;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        beamSpawnpoint = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > beamLife) Destroy(this.gameObject);
        timer += Time.deltaTime;
    }
}
