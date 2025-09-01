using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    InputScript input;

    Vector3 position;
    Vector3 direction;
    Vector3 velocity;
    [SerializeField] float speed;

    Vector3 aim;

    //line
    LineRenderer line;

    //íe
    [SerializeField] GameObject bullet;
    float shotInterval;
    [SerializeField] float kShotInterval;

    // Start is called before the first frame update
    void Start()
    {
        input = FindAnyObjectByType<InputScript>();

        position = transform.position;
        aim = Vector3.forward;

        //line
        line = GetComponent<LineRenderer>();

        //íe
        shotInterval = kShotInterval;
    }

    // Update is called once per frame
    void Update()
    {
        direction = input.lStick;
        if (input.rStick.x != 0 || input.rStick.z != 0) { aim = input.rStick; }

        Shot();
        Move();
        Rotate();
        DrawLineFront();
    }

    void Shot()
    {
        shotInterval -= Time.deltaTime;
        if (shotInterval < 0)
        {
            shotInterval = kShotInterval;

            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            newBullet.GetComponent<BulletScript>().Setting(aim);
        }
    }

    void Move()
    {
        velocity = direction * speed;
        position += velocity * Time.deltaTime;
        transform.position = position;
    }

    void Rotate()
    {
        transform.rotation = Quaternion.LookRotation(aim);
    }

    void DrawLineFront()
    {
        //ëOÇ…ê¸Çï`âÊ
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + aim * 10f);
    }
}
