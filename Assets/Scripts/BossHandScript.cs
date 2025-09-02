using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandScript : MonoBehaviour
{
    Vector3 position;
    Vector3 direction;
    Vector3 velocity;
    float speed;

    float aliveTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        aliveTime -= Time.deltaTime;
        if (aliveTime < 0)
        {
            Destroy(gameObject);
        }

        Move();
    }

    void Move()
    {
        velocity = direction * speed;
        position += velocity * Time.deltaTime;
        transform.position = position;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void Setting(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
