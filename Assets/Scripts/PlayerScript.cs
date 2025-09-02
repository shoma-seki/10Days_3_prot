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

    //弾
    [SerializeField] GameObject bullet;
    float shotInterval;
    [Header("ショットタイプごとのインターバル")]
    [SerializeField] float kShotIntervalNormal;
    [SerializeField] float kShotIntervalStrong;
    [SerializeField] float kShotIntervalSuper;
    [SerializeField] float kShotIntervalUltimate;

    //shotType
    float typeChangeInterval;

    [Header("ショットタイプが変わるまでのインターバル")]
    [SerializeField] float kTypeChangeInterval;

    enum ShotType
    {
        Normal, Strong, Super, Ultimate
    }

    ShotType shotType;

    //ray
    RaycastHit hit;

    //チェインを切る
    string nowTatami;
    string preTatami;

    //ノックバック
    bool isKnockBack;
    Vector3 knockBackDirection;
    float knockBackPower;
    [Header("ノックバック")]
    [SerializeField] float kKnockBackPower;
    float knockBackTime;
    [SerializeField] float kKnockBackTime;

    // Start is called before the first frame update
    void Start()
    {
        input = FindAnyObjectByType<InputScript>();

        position = transform.position;
        aim = Vector3.forward;

        //line
        line = GetComponent<LineRenderer>();

        //弾
        shotInterval = kShotIntervalNormal;
        typeChangeInterval = kTypeChangeInterval;
        shotType = ShotType.Normal;
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
        RayThrow();
        KnockBack();

        //Debug.Log("ShotType = " + shotType);
    }

    void RayThrow()
    {
        int layerMask = LayerMask.GetMask("Tatami");
        if (Physics.Raycast(position, Vector3.down, out hit, 10f, layerMask))
        {
            TatamiScript tatami;
            if (hit.transform.gameObject.TryGetComponent<TatamiScript>(out tatami))
            {
                tatami.IsColored = true;
            }

            //チェインを切る
            nowTatami = tatami.name;

            if (preTatami != nowTatami)
            {
                if (shotType > ShotType.Normal)
                {
                    shotType--;
                }
                typeChangeInterval = kTypeChangeInterval;
            }

            preTatami = nowTatami;
        }
    }

    void Shot()
    {
        //タイプ変更
        typeChangeInterval -= Time.deltaTime;
        if (typeChangeInterval < 0)
        {
            if (shotType < ShotType.Ultimate)
            {
                shotType++;
                typeChangeInterval = kTypeChangeInterval;
            }
        }

        //ShotTypeごとの挙動
        switch (shotType)
        {
            case ShotType.Normal:

                //発射
                shotInterval -= Time.deltaTime;
                if (shotInterval < 0)
                {
                    shotInterval = kShotIntervalNormal;

                    GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    newBullet.GetComponent<BulletScript>().Setting(aim);
                }

                break;

            case ShotType.Strong:

                //発射
                shotInterval -= Time.deltaTime;
                if (shotInterval < 0)
                {
                    shotInterval = kShotIntervalStrong;

                    GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    newBullet.GetComponent<BulletScript>().Setting(aim);
                }

                break;

            case ShotType.Super:

                //発射
                shotInterval -= Time.deltaTime;
                if (shotInterval < 0)
                {
                    shotInterval = kShotIntervalSuper;

                    GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    newBullet.GetComponent<BulletScript>().Setting(aim);
                }

                break;

            case ShotType.Ultimate:

                //発射
                shotInterval -= Time.deltaTime;
                if (shotInterval < 0)
                {
                    shotInterval = kShotIntervalUltimate;

                    GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    newBullet.GetComponent<BulletScript>().Setting(aim);
                }

                break;
        }
    }

    void Move()
    {
        if (!isKnockBack)
        {
            velocity = direction * speed;
            position += velocity * Time.deltaTime;
            transform.position = position;
        }
    }

    void Rotate()
    {
        transform.rotation = Quaternion.LookRotation(aim);
    }

    void DrawLineFront()
    {
        //前に線を描画
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + aim * 10f);
    }

    void KnockBack()
    {
        if (isKnockBack)
        {
            knockBackTime -= Time.deltaTime;
            if (knockBackTime < 0)
            {
                isKnockBack = false;
            }

            velocity = knockBackDirection * knockBackPower;
            position += velocity * Time.deltaTime;
            transform.position = position;

            knockBackPower -= 15f * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand")
        {
            isKnockBack = true;
            knockBackPower = kKnockBackPower;
            knockBackTime = kKnockBackTime;
            knockBackDirection = (transform.position - other.transform.position).normalized;
            knockBackDirection.y = 0;
        }
    }
}
