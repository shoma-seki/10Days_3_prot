using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    Vector3 position;
    Vector3 direction;
    Vector3 velocity;

    PlayerScript player;

    //攻撃フェーズ
    enum AttackPhase
    {
        None, Move, Shot, RushPrepare, Rush, Stomp
    }

    AttackPhase attackPhase;
    bool GoNextPhase;

    //None
    float noneTime;
    [Header("Noneフェーズ")]
    [SerializeField] float kNoneTime;

    //Move
    float moveTime;
    [Header("Moveフェーズ")]
    [SerializeField] float kMoveTime;
    [SerializeField] float moveSpeed;

    //Shot
    [Header("Shotフェーズ")]
    [SerializeField] GameObject hand;
    Vector3 shotDirection;
    [SerializeField] float shotSpeed;
    float shotInterval;
    [SerializeField] float kShotInterval;
    int shotCount;

    //RusPrepare
    float rushPrepareTime;
    [Header("RushPrepareフェーズ")]
    [SerializeField] float kRushPrepareTime;

    //Rush
    float rushTime;
    float rushSpeed;
    Vector3 rushDirection;
    bool isRush;
    public bool IsRush { get { return isRush; } }
    [Header("Rushフェーズ")]
    [SerializeField] float kRushTime;
    [SerializeField] float kRushSpeed;

    //Stomp
    enum StompPP
    {
        Jump, Move, Stomp
    }
    StompPP stompPhase;
    bool isStomp;
    float stompMoveTime;
    int stompCount;
    [Header("Stompフェーズ")]
    [SerializeField] int maxStompCount;
    [SerializeField] float kStompMoveTime;
    [SerializeField] float stompMoveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float stompSpeed;

    //HP関連
    bool isDamage;
    Color color;

    //アタックフェーズ操作
    [SerializeField] AttackPhase startAttackPhase;

    //float phaseInterval;
    //[SerializeField] float kPhaseInterval;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerScript>();
        attackPhase = startAttackPhase;

        position = transform.position;

        moveTime = kMoveTime;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("attackPhase = " + attackPhase);

        PhaseChange();
        Phase();
        Damage();
        Rotate();
    }

    void PhaseChange()
    {
        if (GoNextPhase)
        {
            if (attackPhase == AttackPhase.Move)
            {
                int rand = Random.Range(0, 1500);
                if (rand < 500)
                {
                    attackPhase = AttackPhase.Shot;
                    shotInterval = kShotInterval;
                    shotCount = 0;
                }
                else if (rand < 1000)
                {
                    attackPhase = AttackPhase.RushPrepare;
                    rushPrepareTime = kRushPrepareTime;
                }
                else
                {
                    attackPhase = AttackPhase.Stomp;
                    stompCount = 0;
                    stompPhase = StompPP.Jump;
                }

                GoNextPhase = false;
            }
        }

        if (GoNextPhase)
        {
            if (attackPhase == AttackPhase.None || attackPhase == AttackPhase.Shot || attackPhase == AttackPhase.Rush || attackPhase == AttackPhase.Stomp)
            {
                attackPhase = AttackPhase.Move;
                moveTime = kMoveTime;

                GoNextPhase = false;
            }
        }
    }

    void Phase()
    {
        switch (attackPhase)
        {
            case AttackPhase.None:

                NonePhase();

                break;

            case AttackPhase.Move:

                MovePhase();

                break;

            case AttackPhase.Shot:

                ShotPhase();

                break;

            case AttackPhase.RushPrepare:

                RushPreparePhase();

                break;

            case AttackPhase.Rush:

                RushPhase();

                break;

            case AttackPhase.Stomp:

                StompPhase();

                break;
        }
    }

    void RushPreparePhase()
    {
        rushPrepareTime -= Time.deltaTime;
        if (rushPrepareTime < 0)
        {
            attackPhase = AttackPhase.Rush;
            rushDirection = (player.transform.position - transform.position).normalized;
            rushDirection.y = 0;
            rushTime = kRushTime;
            rushSpeed = kRushSpeed;
        }
    }

    void RushPhase()
    {
        rushTime -= Time.deltaTime;
        if (rushTime < 0)
        {
            GoNextPhase = true;
        }

        velocity = rushDirection * rushSpeed;
        position += velocity * Time.deltaTime;
        transform.position = position;
    }

    void NonePhase()
    {
        noneTime -= Time.deltaTime;
        if (noneTime < 0)
        {
            GoNextPhase = true;
        }
    }

    void MovePhase()
    {
        if (moveTime == kMoveTime)
        {
            Vector3 eToPDirection = (player.transform.position - transform.position).normalized;
            int rand = Random.Range(0, 1000);
            float angle;
            if (rand < 500)
            {
                angle = 30;
            }
            else
            {
                angle = -30;
            }
            direction = RotateAroundAxis(eToPDirection.normalized, angle, Vector3.up);
        }

        moveTime -= Time.deltaTime;
        if (moveTime < 0)
        {
            GoNextPhase = true;
        }

        //動かす
        velocity = direction.normalized * moveSpeed;
        position += velocity * Time.deltaTime;
        position.y = 1.51f;
        transform.position = position;
    }

    void ShotPhase()
    {
        shotInterval -= Time.deltaTime;
        if (shotInterval < 0)
        {
            shotCount++;
            shotInterval = kShotInterval;

            //発射
            shotDirection = (player.transform.position - transform.position).normalized;
            shotDirection.y = 0;

            GameObject newHand = Instantiate(hand, transform.position, Quaternion.identity);
            newHand.GetComponent<BossHandScript>().Setting(shotDirection.normalized, shotSpeed);
        }

        if (shotCount > 3)
        {
            GoNextPhase = true;
        }
    }

    void StompPhase()
    {
        //Debug.Log("stompPhase = " + stompPhase);

        switch (stompPhase)
        {
            case StompPP.Jump:

                if (position.y < 8)
                {
                    position.y += jumpSpeed * Time.deltaTime;
                    transform.position = position;
                }
                else
                {
                    stompPhase = StompPP.Move;
                    stompMoveTime = kStompMoveTime;
                }

                break;

            case StompPP.Move:

                //移動
                Vector3 playerSkyPos = player.transform.position;
                playerSkyPos.y = position.y;
                position = Vector3.Lerp(transform.position, playerSkyPos, stompMoveSpeed * Time.deltaTime);
                transform.position = position;

                //時間
                stompMoveTime -= Time.deltaTime;
                if (stompMoveTime < 0)
                {
                    stompPhase = StompPP.Stomp;
                }

                break;

            case StompPP.Stomp:

                position.y -= stompSpeed * Time.deltaTime;
                transform.position = position;

                if (isStomp)
                {
                    isStomp = false;
                    stompCount++;

                    if (stompCount >= maxStompCount)
                    {
                        GoNextPhase = true;
                    }
                    else
                    {
                        stompPhase = StompPP.Jump;
                    }

                }

                break;
        }
    }

    void Damage()
    {
        if (isDamage)
        {
            isDamage = false;
            color = Color.white;
        }

        color = Color.Lerp(color, Color.red, 5f * Time.deltaTime);
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tatami")
        {
            TatamiScript tatami;
            if (other.gameObject.TryGetComponent<TatamiScript>(out tatami))
            {
                tatami.IsColored = false;
            }

            if (stompPhase == StompPP.Stomp)
            {
                isStomp = true;
            }
        }

        if (other.tag == "Bullet")
        {
            isDamage = true;
        }

        if (other.tag == "Player")
        {
            PlayerScript player;
            if (other.TryGetComponent<PlayerScript>(out player))
            {
                player.KnockBackOn((player.transform.position - transform.position).normalized);
                attackPhase = AttackPhase.None;
                noneTime = kNoneTime;
            }
        }
    }

    void Rotate()
    {
        transform.rotation = Quaternion.LookRotation(velocity.normalized);
        if (attackPhase == AttackPhase.Shot)
        {
            transform.rotation = Quaternion.LookRotation(shotDirection.normalized);
        }
    }

    Vector3 RotateAroundAxis(Vector3 v, float angleDeg, Vector3 axis)
    {
        return Quaternion.AngleAxis(angleDeg, axis.normalized) * v;
    }
}
