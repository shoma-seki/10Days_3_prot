using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Vector3 position;
    Vector3 direction;
    Vector3 velocity;
    public float speed;

    float aliveTime;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = direction * speed;
        position += velocity * Time.deltaTime;
        transform.position = position;

        aliveTime += Time.deltaTime;
        if (aliveTime > 3f)
        {
            Destroy(gameObject);
        }
    }

    public void Setting(Vector3 direction)
    {
        this.direction = direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boss")
        {
            Destroy(gameObject);
        }
    }
}
