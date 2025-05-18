using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnemy : MonoBehaviour
{
    private bool isAlive;
    private bool isIdle;
    private bool jumpAttack;
    private bool isJumpUp;              //Ϊtrueʱ��������Ϊfalseʱ������
    private bool slideAttack;
    private bool isHurt;
    private bool canBeHurt;

    public int bossBlood;               //Ѫ��
    public float attackDistance;        //��������
    public float jumpHeight;             //��Ծ�߶�
    public float jumpUpSpeed = 15.0f;   //������Ծ�ٶ�
    public float jumpDownSpeed = 15.0f; //�����ٶ�
    public float fallDownSpeed = 10.0f; //��׹�ٶ�
    public float slideSpeed = 8.0f;     //�����ٶ�

    public GameObject player;
    public Animator m_Animator;
    public BoxCollider2D m_BoxCollider2D;
    public AudioSource m_AudioSource;
    public GameObject winPanel;         //ɱ��Boss����ʤ���Ի���

    private void Awake()
    {
        isAlive = true;
        isIdle = true;
        jumpAttack = false;
        isJumpUp = true;
        slideAttack = false;
        isHurt = false;
        canBeHurt = true;

        player = GameObject.Find("Player");
        m_Animator = GetComponent<Animator>();
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
        m_AudioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (isAlive)
        {
            
            if (isIdle)
            {
                IsIdle();
            }
            else if (jumpAttack)
            {
                JumpAttack();
            }
            else if (slideAttack)
            {
                SlideAttack();
            }
            else if (isHurt)
            {
                IsHurt();
            }
        }
        else
        {
            IsHurt();
        }
    }





    //������ҵ�λ�ã����õ��˵ĳ�������
    private void EnemyDirection()
    {
        if(player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
    }

    private void IsIdle()
    {
        EnemyDirection();       //���˵ĳ������⣬ֻ��Idle||JumpAttack||slideAttackΪtrueʱ����
        if (Vector3.Distance(player.transform.position, transform.position) <= attackDistance)
        {
            //slideAttack;
            isIdle = false;
            StartCoroutine("IdleToSlideAttack");
        }
        else
        {
            //jumpAttack;
            isIdle = false;
            StartCoroutine("IdleToJumpAttack");
        }
    }

    private void JumpAttack()
    {
        EnemyDirection();       //���˵ĳ�������
        if (isJumpUp)
        {
            //isJumpΪtrueʱ����Ծ������Ϸ�λ�ã����Ҳ����������Ķ���
            Vector3 targetPosition = new Vector3(player.transform.position.x, jumpHeight, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, jumpUpSpeed * Time.deltaTime);
            m_Animator.SetBool("JumpUp", true);
        }
        else
        {
            //isJumpΪfalseʱ��������Ծ�����λ�ã����Ҳ����������Ķ���
            m_Animator.SetBool("JumpUp", false);
            m_Animator.SetBool("JumpDown", true);

            Vector3 targetPosition = new Vector3(transform.position.x, -1.22f, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, jumpDownSpeed * Time.deltaTime);
        }
        if (transform.position.y == jumpHeight)
        {
            isJumpUp = false;       //��������ϵ�Ŀ��߶ȣ��Ͳ���������Ծ
        }
        else if (transform.position.y == -1.22f)
        {
            //��������µ�Ŀ��߶ȣ��ص�Idle״̬�����ر��������Ķ���,boss�ص���ʼ״̬
            jumpAttack = false;
            StartCoroutine("JumpDownToIdle");
        }
    }

    private void SlideAttack()
    {   //���е����λ�ý��й���
        m_Animator.SetBool("Slide", true);
        Vector3 slideTargetPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, slideTargetPosition, slideSpeed * Time.deltaTime);

        //�������Ŀ�껬��λ�ã�boss�ص���ʼ״̬
        if(transform.position == slideTargetPosition)
        {
            m_Animator.SetBool("Slide", false);
            slideAttack = false;
            isIdle = true;      
        }
    }


    private void IsHurt()
    {
        //�ڿ�������ʱӦ��׹
        Vector3 targetPosition = new Vector3(transform.position.x, -1.22f, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallDownSpeed * Time.deltaTime);
    }


    IEnumerator JumpDownToIdle()
    {
        //�ӳ�0.5��ص�Idle״̬�����ر��������Ķ���,boss�ص���ʼ״̬
        yield return new WaitForSeconds(0.5f);
        isIdle = true;
        isJumpUp = true;
        m_Animator.SetBool("JumpUp", false);
        m_Animator.SetBool("JumpDown", false);
    }

    IEnumerator IdleToJumpAttack()
    {
        yield return new WaitForSeconds(1.0f);
        jumpAttack = true;      //�ӳ�1�����������Ծ����״̬
    }

    IEnumerator IdleToSlideAttack()
    {
        yield return new WaitForSeconds(1.0f);
        EnemyDirection();
        slideAttack = true;     //�ӳ�1�����뻬�й���״̬
    }

    IEnumerator SetAnimationHurtToFalse()
    {
        //������תΪidle״̬
        yield return new WaitForSeconds(0.5f);
        m_Animator.SetBool("Hurt", false);
        m_Animator.SetBool("JumpUp", false);
        m_Animator.SetBool("JumpDown", false);
        m_Animator.SetBool("Slide", false);
        isHurt = false;
        isIdle = true;

        yield return new WaitForSeconds(0.3f);
        canBeHurt = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Boss�ܵ���ҵ��˺��󣬹ر�������Ϊ,���������˶���
        if (collision.tag == "PlayerAttack")
        {
            if (canBeHurt)
            {
                m_AudioSource.PlayOneShot(m_AudioSource.clip);  //����������Ч
                int randomInt = UnityEngine.Random.Range(50, 60);
                bossBlood -= randomInt;
                if (bossBlood >= 0)
                {
                    isIdle = false;
                    jumpAttack = false;
                    slideAttack = false;
                    isHurt = true;

                    StopCoroutine("JumpDownToIdle");            
                    StopCoroutine("IdleToJumpAttack");
                    StopCoroutine("IdleToSlideAttack");//���˺�ر�������Ϊ���������˶���
                    m_Animator.SetBool("Hurt", true);

                    StartCoroutine("SetAnimationHurtToFalse");  //������תΪidle״̬
                }
                else
                {
                    isAlive = false;
                    m_BoxCollider2D.enabled = false;
                    StopAllCoroutines();
                    m_Animator.SetBool("Die", true);
                    winPanel.SetActive(true);
                    StartCoroutine("WaitForSecondsLoadScene");
                }
                canBeHurt = false;
            }

        }
    }

    IEnumerator WaitForSecondsLoadScene()
    {
        yield return new WaitForSeconds(1.5f);
        winPanel.SetActive(false);
        string levelName = SceneManager.GetActiveScene().name;
        string temp = levelName.Substring(5);
        int levelNumber = int.Parse(temp);

        int clearedLevel = PlayerPrefs.GetInt("clearedLevel");
        if (levelNumber > clearedLevel)
        {
            PlayerPrefs.SetInt("clearedLevel", levelNumber);
        }

        SceneManager.LoadScene("Select");
    }
}


