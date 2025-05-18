using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralEnemy : MonoBehaviour
{
    public Vector3 targetPosition;          //�����ƶ�����Ŀ��λ��
    public Vector3 orginPosition;           //����ԭʼλ��
    public Vector3 tempPosition;           //������תλ��

    public float enemySpeed;                //�����ٶ�
    public bool isFirstIdle;
    public bool isAfterBattleCheck;         //Ӱ����˷��������
    public int enemyBlood;                  //����Ѫ��
    public bool isAlive;                    //�����Ƿ���

    private Animator m_Animator;            //���˶������ƶ���
    private GameObject player;              //��Ҷ���
    public GameObject attackCollider;       //������ײ��
    public BoxCollider2D m_BoxCollider2D;   //������ײ��
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
        if (isAlive)            //���˴��ʱ����ִ�����´���
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
            return;         //����ִ�й�������ʱֱ��return
        }
        m_Animator.SetTrigger("Attack");
        isAfterBattleCheck = true;
        return;             //����ʱֱ��return����ִ�к���Ĵ���
    }

    private void PE_MoreDistance()
    {
        //isAterBattleCheckΪtrueʱִ�����´��룬Ŀ���ǲ�Ӱ�������Э��ִ��
        if (isAfterBattleCheck)
        {
            if (tempPosition == targetPosition)
            {
                StartCoroutine(Turn(false));             //Э�̿���ת��
            }
            else if (tempPosition == orginPosition)
            {
                StartCoroutine(Turn(true));             //Э�̿���ת��
            }
        }
        isAfterBattleCheck = false;
    }

    private void EnemyMove()
    {

        if (transform.position.x == targetPosition.x)
        {
            m_Animator.SetTrigger("Idle");          //���Żص�Idle�Ķ���
            tempPosition = orginPosition;
            isFirstIdle = false;
            StartCoroutine(Turn(true));             //Э�̿���ת��

        }
        else if (transform.position.x == orginPosition.x)
        {
            if (!isFirstIdle)                           //��ֹ��һ���ظ�����idle
            {
                m_Animator.SetTrigger("Idle");          //���Żص�Idle�Ķ���
            }

            tempPosition = targetPosition;
            StartCoroutine(Turn(false));            //Э�̿���ת��
        }

        //���ִ�е�walk���������˿�ʼ�ƶ�
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
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);  //�����ߵ�Ŀ��λ�ú�ת��
        }
        else
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);  //�����ߵ�Ŀ��λ�ú�ת��
        }
    }


    public void SetAttackColliderOn()
    {
        attackCollider.SetActive(true);     //���˹�������ײ����Ч
    }

    public void SetAttackColliderOff()
    {
        attackCollider.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")        //�����ܵ��˺�ʱ�Ĵ���
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
                m_BoxCollider2D.enabled = false;    //���������ص��������ײ��
                m_Animator.SetTrigger("Die");
                StartCoroutine("EnemyAfterDie");    //��Э��ʹ��ɫ����1�����ɫ�䵭�������ʧ
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
