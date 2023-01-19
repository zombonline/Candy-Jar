using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBubbleSpawner : MonoBehaviour
{
    [SerializeField] GameObject bubblePrefab;
    float timer;
    [SerializeField] float minSpawnTime, maxSpawnTime;
    [SerializeField] Transform left, right;
    [SerializeField] float minVelocity, maxVelocity;

    void Update()
    {
        timer -= Time.deltaTime;    
        if(timer < 0)
        {
            timer = Random.Range(minSpawnTime, maxSpawnTime);

            var newBubble = Instantiate(bubblePrefab, new Vector2(Random.Range(left.position.x, right.position.x), transform.position.y), Quaternion.identity);
            newBubble.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(minVelocity, maxVelocity));
            newBubble.GetComponent<Rigidbody2D>().AddTorque(19f);
            var size = Random.Range(0.5f, 1.5f);
            newBubble.transform.localScale = new Vector2(size, size);
            Destroy(newBubble, 7f);
        }


    }

}
