using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float playerSpeed = 5.0f;     //����ٶ�
    public float jumpForce = 20.0f;      //��Ծ����
    public float bulletDistance;        //�ӵ�����λ�õ�ƫ����

    public Animator m_Animator;        //��Ҷ�������
    public Rigidbody2D m_RigidBody2D;  //��Ҹ���
    public GameObject attackCollider;   //��ҹ�����ײ��
    public GameObject bulletPrefab;     //�ӵ�
    public AudioClip[] m_AudioClip; //��Ч
    public AudioSource m_AudioSource;

    public bool isJumpPressed;
    public bool isJumping;

    private bool isAttack;
    private bool isHurt;
    private bool canBeHurt;             //������ʱ�������

    public int playerBlood;            //���Ѫ��
    public int playerBullet;            //����ӵ�

    public Canvas m_Canvas;                    //����UI
    public GameObject diePanel;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        isJumpPressed = false;
        isJumping = false;
        isAttack = false;
        bulletDistance = 0;
        isHurt = false;
        canBeHurt = true;

        playerBlood = PlayerPrefs.GetInt("PlayerBlood");
        playerBullet = PlayerPrefs.GetInt("Bullet");
    }

    void Start()
    {

    }

    
    void Update()
    {
        PressSpaceJump();
        PressE_KnifeAttack();
        PressMouseLeftShoot();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        PlayerJump();
    }

    private void PressSpaceJump()
    {
        //���¿ո���������û������Ծ������״̬ʱ�ſ���Ծ
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false && isHurt == false)
        {
            isJumpPressed = true;   //���¿ո������Ϊtrue
            isJumping = true; //���¿ո����������Ϊtrue,����ڴ��ڼ䲻����Ծ
        }
    }

    private void PressE_KnifeAttack()
    {
        if (Input.GetKeyDown(KeyCode.E) && isHurt == false)
        {
            m_AudioSource.PlayOneShot(m_AudioClip[3]);
            m_Animator.SetTrigger("Attack");
            isAttack = true;        //����E�����빥��״̬
        }
    }

    private void PressMouseLeftShoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isHurt == false && Time.timeScale == 1.0f)
        {
            if (playerBullet > 0)
            {
                int numBullet = PlayerPrefs.GetInt("Bullet");
                numBullet--;
                PlayerPrefs.SetInt("Bullet", numBullet);
                m_Canvas.BulletUpdate();                //�����ӵ���һ������UI

                m_AudioSource.PlayOneShot(m_AudioClip[2]);  //������Ч
                m_Animator.SetTrigger("Shoot");             //�����������
                isAttack = true;                //�����������������״̬
            }
        }
    }




    private void PlayerMove()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); //�������Ҽ����������ƶ�

        if (isAttack || isHurt)
        {
            moveX = 0;      //��ɫ����ʱ�����ƶ�
        }

        //��������ת��
        if (moveX > 0)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (moveX < 0)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }

        //���Ҽ�����ʵ��Idle��Run��������
        m_Animator.SetFloat("Run", Mathf.Abs(moveX));

        if (!isHurt)
        {
            //��ҵ��ٶ�
            m_RigidBody2D.velocity = new Vector2(moveX * playerSpeed, m_RigidBody2D.velocity.y);
        }
    }

    private void PlayerJump()
    {
        //���¿ո����Ծ
        if (isJumpPressed)
        {
            m_RigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumpPressed = false; //ʩ�����ϵ�������Ϊfalse

            m_Animator.SetBool("Jump", true);   //���¿ո��������Ծ����
        }
    }
    private void SetIsAttackFalse()
    {
        isAttack = false;                       //��������ʱ�ָ�״̬
        m_Animator.ResetTrigger("Attack");      //���ù���״̬
        m_Animator.ResetTrigger("Shoot");
    }

    //����ʱ��ײ����Ч
    public void SetAttackColliderOn()
    {
        attackCollider.SetActive(true);
    }

    //��������ʱ��ײ����ʧ
    public void SetAttackColliderOff()
    {
        attackCollider.SetActive(false);
    }


    //������ҵķ�������ӵ���ƫ����
    public void BulletInstantiate()
    {

        if (transform.localScale.x == 0.5f)
        {
            bulletDistance = 1.0f;
        }
        else if (transform.localScale.x == -0.5f)
        {
            bulletDistance = -1.0f;
        }
        Vector3 temp = new Vector3(transform.position.x + bulletDistance, transform.position.y, transform.position.z);
        Instantiate(bulletPrefab, temp, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && isHurt == false && canBeHurt == true) 
        {
            int randomInt = UnityEngine.Random.Range(7,10 );
            playerBlood -= randomInt;                      //��ҿ�Ѫ

            PlayerPrefs.SetInt("PlayerBlood", playerBlood);
            m_Canvas.LifeUpdate();              //��������ֵUI

            if (playerBlood > 0)
            {
                m_AudioSource.PlayOneShot(m_AudioClip[1]);
                isHurt = true;
                canBeHurt = false;
                m_Animator.SetBool("Hurt", true);

                //������ҵķ�������˵Ľ�ɫ�������ϵ���
                if (transform.localScale.x == 0.5f)
                {
                    m_RigidBody2D.velocity = new Vector2(-2.5f, 10.0f);
                }
                else if (transform.localScale.x == -0.5f)
                {
                    m_RigidBody2D.velocity = new Vector2(2.5f, 10.0f);
                }

            }
            StartCoroutine("SetIsHurtFalse");       //Э��ִ����Hurt�������ص�Idle����
        }else if(playerBlood <= 0)
        {
            m_AudioSource.PlayOneShot(m_AudioClip[4]);
            isHurt = true;                          //��ɫ����ʱ����ֹ��Ұ���������ʱ��Ȼ��Ӧ
            isAttack = true;                        //��ɫ�����ƶ�
            m_RigidBody2D.velocity = new Vector2(0f, 0f);   //��ɫ�ٶ�Ϊ0
            m_Animator.SetBool("Die", true);        //��ɫѪ��Ϊ0��������������

            PlayerPrefs.SetInt("PlayerBlood", 100);
            PlayerPrefs.SetInt("Bullet", 200);
            PlayerPrefs.SetInt("PlayerStone", 0);
            diePanel.SetActive(true);               //����Game Over�Ի���
            StartCoroutine("WaitForSecondsLoadScene");
        }
    }

    IEnumerator SetIsHurtFalse()
    {
        yield return new WaitForSeconds(1.0f);
        isHurt = false;     //1�������״̬���
        m_Animator.SetBool("Hurt", false);      //���˺�1��ص�Idle״̬

        yield return new WaitForSeconds(1.0f);
        canBeHurt = true;               //1���޵�ʱ����ҿ�����
    }

    public void ForIsHurtSetting()
    {
        isAttack = false;                       //��������ʱ�ָ�״̬
        m_Animator.ResetTrigger("Attack");      //���ù���״̬
        m_Animator.ResetTrigger("Shoot");
        attackCollider.SetActive(false);
    }


    //��Ҹ�������˸����ص���һ��ʱ�Ĵ���
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && isHurt == false && canBeHurt == true)
        {
            int randomInt = UnityEngine.Random.Range(7, 10);
            playerBlood -= randomInt;                      //��ҿ�Ѫ

            PlayerPrefs.SetInt("PlayerBlood", playerBlood);
            m_Canvas.LifeUpdate();              //��������ֵUI

            if (playerBlood > 0)
            {
                m_AudioSource.PlayOneShot(m_AudioClip[1]);
                isHurt = true;
                canBeHurt = false;
                m_Animator.SetBool("Hurt", true);

                //������ҵķ�������˵Ľ�ɫ�������ϵ���
                if (transform.localScale.x == 0.5f)
                {
                    m_RigidBody2D.velocity = new Vector2(-2.5f, 10.0f);
                }
                else if (transform.localScale.x == -0.5f)
                {
                    m_RigidBody2D.velocity = new Vector2(2.5f, 10.0f);
                }

            }
            StartCoroutine("SetIsHurtFalse");       //Э��ִ����Hurt�������ص�Idle����
        }
        else if (playerBlood <= 0)
        {
            m_AudioSource.PlayOneShot(m_AudioClip[4]);
            isHurt = true;                          //��ɫ����ʱ����ֹ��Ұ���������ʱ��Ȼ��Ӧ
            isAttack = true;                        //��ɫ�����ƶ�
            m_RigidBody2D.velocity = new Vector2(0f, 0f);   //��ɫ�ٶ�Ϊ0
            m_Animator.SetBool("Die", true);        //��ɫѪ��Ϊ0��������������

            PlayerPrefs.SetInt("PlayerBlood", 100);
            PlayerPrefs.SetInt("Bullet", 200);
            PlayerPrefs.SetInt("PlayerStone", 0);
            diePanel.SetActive(true);
            StartCoroutine("WaitForSecondsLoadScene");

        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name == "BoundBottom")
        {
            m_AudioSource.PlayOneShot(m_AudioClip[4]);
            playerBlood = 0;
            isHurt = true;                          //��ɫ������������
            isAttack = true;                        
            m_RigidBody2D.velocity = new Vector2(0f, 0f);   
            m_Animator.SetBool("Die", true);
        }
    }

    IEnumerator WaitForSecondsLoadScene()
    {
        yield return new WaitForSeconds(1.5f);
        diePanel.SetActive(false);
        string levelName = SceneManager.GetActiveScene().name;
        string temp = levelName.Substring(5);
        int levelNumber = int.Parse(temp);

        int clearedLevel = PlayerPrefs.GetInt("clearedLevel") + 1;
        if (levelNumber > clearedLevel)
        {
            PlayerPrefs.SetInt("clearedLevel", levelNumber);
        }

        SceneManager.LoadScene("Select");            //��ɫ����������ѡ��ؿ�
    }

}
