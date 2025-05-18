using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralEnemy : MonoBehaviour
{
    public Vector3 targetPosition;          //敌人移动到的目标位置
    public Vector3 orginPosition;           //敌人原始位置
    public Vector3 tempPosition;           //敌人中转位置

    public float enemySpeed;                //敌人速度
    public bool isFirstIdle;
    public bool isAfterBattleCheck;         //影响敌人方向的条件
    public int enemyBlood;                  //敌人血条
    public bool isAlive;                    //敌人是否存活

    private Animator m_Animator;            //敌人动画控制对象
    private GameObject player;              //玩家对象
    public GameObject attackCollider;       //攻击碰撞体
    public BoxCollider2D m_BoxCollider2D;   //敌人碰撞体
    public SpriteRenderer m_SR;             //    
    public AudioSource m_AudioSource;
 

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
        m_SR = GetComponent<SpriteRenderer>();
        m_AudioSource = GetComponent<AudioSource>();

        orginPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        isFirstIdle = true;
        isAfterBattleCheck = false;
        player = GameObject.Find("Player");
        isAlive = true;
    }

    private void Update()
    {
        if (isAlive)            //敌人存活时才能执行以下代码
        {

            if (Vector3.Distance(player.transform.position, transform.position) < 2.2f)
            {
                PE_LessDistance();
            }
            else
            {
                PE_MoreDistance();
            }

            EnemyMove();
        }
    }
    private void PE_LessDistance()
    {
        if (player.transform.position.x <= transform.position.x)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        else
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return;         //正在执行攻击动画时直接return
        }
        m_Animator.SetTrigger("Attack");
        isAfterBattleCheck = true;
        return;             //攻击时直接return，不执行后面的代码
    }

    private void PE_MoreDistance()
    {
        //isAterBattleCheck为true时执行以下代码，目的是不影响下面的协程执行
        if (isAfterBattleCheck)
        {
            if (tempPosition == targetPosition)
            {
                StartCoroutine(Turn(false));             //协程控制转向
            }
            else if (tempPosition == orginPosition)
            {
                StartCoroutine(Turn(true));             //协程控制转向
            }
        }
        isAfterBattleCheck = false;
    }

    private void EnemyMove()
    {

        if (transform.position.x == targetPosition.x)
        {
            m_Animator.SetTrigger("Idle");          //播放回到Idle的动画
            tempPosition = orginPosition;
            isFirstIdle = false;
            StartCoroutine(Turn(true));             //协程控制转向

        }
        else if (transform.position.x == orginPosition.x)
        {
            if (!isFirstIdle)                           //防止第一次重复播放idle
            {
                m_Animator.SetTrigger("Idle");          //播放回到Idle的动画
            }

            tempPosition = targetPosition;
            StartCoroutine(Turn(false));            //协程控制转向
        }

        //如果执行到walk动画，敌人开始移动
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            transform.position = Vector3.MoveTowards(transform.position, tempPosition, enemySpeed * Time.deltaTime);
        }
    }
    IEnumerator Turn(bool turn)
    {
        yield return new WaitForSeconds(2.0f);
        if (turn)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);  //敌人走到目标位置后转向
        }
        else
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);  //敌人走到目标位置后转向
        }
    }


    public void SetAttackColliderOn()
    {
        attackCollider.SetActive(true);     //敌人攻击的碰撞体生效
    }

    public void SetAttackColliderOff()
    {
        attackCollider.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")        //敌人受到伤害时的处理
        {
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            int randomInt = UnityEngine.Random.Range(30, 35);
            enemyBlood -= randomInt;
            if (enemyBlood > 0)
            {
                m_Animator.SetTrigger("Hurt");
            }
            else if(enemyBlood <= 0)
            {
                isAlive = false;
                m_BoxCollider2D.enabled = false;    //敌人死亡关掉自身的碰撞体
                m_Animator.SetTrigger("Die");
                StartCoroutine("EnemyAfterDie");    //该协程使角色死亡1秒后颜色变淡，最后消失
            }
        }
    }

    IEnumerator EnemyAfterDie()
    {
        yield return new WaitForSeconds(1.0f);
        m_SR.material.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);

        yield return new WaitForSeconds(1.0f);
        m_SR.material.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);

        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
}
