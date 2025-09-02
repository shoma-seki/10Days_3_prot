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

    //�e
    [SerializeField] GameObject bullet;
    float shotInterval;
    [Header("�V���b�g�^�C�v���Ƃ̃C���^�[�o��")]
    [SerializeField] float kShotIntervalNormal;
    [SerializeField] float kShotIntervalStrong;
    [SerializeField] float kShotIntervalSuper;
    [SerializeField] float kShotIntervalUltimate;

    //shotType
    float typeChangeInterval;

    [Header("�V���b�g�^�C�v���ς��܂ł̃C���^�[�o��")]
    [SerializeField] float kTypeChangeInterval;

    enum ShotType
    {
        Normal, Strong, Super, Ultimate
    }

    ShotType shotType;

    //ray
    RaycastHit hit;

    //�`�F�C����؂�
    string nowTatami;
    string preTatami;

    //�m�b�N�o�b�N
    bool isKnockBack;
    Vector3 knockBackDirection;
    float knockBackPower;
    [Header("�m�b�N�o�b�N")]
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

        //�e
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

            //�`�F�C����؂�
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
        //�^�C�v�ύX
        typeChangeInterval -= Time.deltaTime;
        if (typeChangeInterval < 0)
        {
            if (shotType < ShotType.Ultimate)
            {
                shotType++;
                typeChangeInterval = kTypeChangeInterval;
            }
        }

        //ShotType���Ƃ̋���
        switch (shotType)
        {
            case ShotType.Normal:

                //����
                shotInterval -= Time.deltaTime;
                if (shotInterval < 0)
                {
                    shotInterval = kShotIntervalNormal;

                    GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    newBullet.GetComponent<BulletScript>().Setting(aim);
                }

                break;

            case ShotType.Strong:

                //����
                shotInterval -= Time.deltaTime;
                if (shotInterval < 0)
                {
                    shotInterval = kShotIntervalStrong;

                    GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    newBullet.GetComponent<BulletScript>().Setting(aim);
                }

                break;

            case ShotType.Super:

                //����
                shotInterval -= Time.deltaTime;
                if (shotInterval < 0)
                {
                    shotInterval = kShotIntervalSuper;

                    GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    newBullet.GetComponent<BulletScript>().Setting(aim);
                }

                break;

            case ShotType.Ultimate:

                //����
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
        //�O�ɐ���`��
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
