using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionSignal = .7f;
    public float explosionLife = .8f;
    private Vector2 explosionSpawnpoint;
    private Animator animator = null;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        explosionSpawnpoint = new Vector2(transform.position.x, transform.position.y);
        StartCoroutine(Explodes());
    }

    // Update is called once per frame
    void Update()
    {
        
        // if (timer > explosionLife) Destroy(this.gameObject);
        // timer += Time.deltaTime;
    }
    private IEnumerator Explodes()
    {
        yield return new WaitForSeconds(explosionSignal); 
        animator.SetBool("explode", true);
        yield return new WaitForSeconds(explosionLife); 
        Destroy(this.gameObject);
    }
}
