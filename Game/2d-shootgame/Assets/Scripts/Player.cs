using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float playerSpeed = 5.0f;     //玩家速度
    public float jumpForce = 20.0f;      //跳跃力量
    public float bulletDistance;        //子弹生成位置的偏移量

    public Animator m_Animator;        //玩家动画控制
    public Rigidbody2D m_RigidBody2D;  //玩家刚体
    public GameObject attackCollider;   //玩家攻击碰撞体
    public GameObject bulletPrefab;     //子弹
    public AudioClip[] m_AudioClip; //音效
    public AudioSource m_AudioSource;

    public bool isJumpPressed;
    public bool isJumping;

    private bool isAttack;
    private bool isHurt;
    private bool canBeHurt;             //有闪避时间的条件

    public int playerBlood;            //玩家血条
    public int playerBullet;            //玩家子弹

    public Canvas m_Canvas;                    //更新UI
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
        //按下空格键后并且玩家没有在跳跃和受伤状态时才可跳跃
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false && isHurt == false)
        {
            isJumpPressed = true;   //按下空格键后设为true
            isJumping = true; //按下空格键后立即设为true,玩家在此期间不能跳跃
        }
    }

    private void PressE_KnifeAttack()
    {
        if (Input.GetKeyDown(KeyCode.E) && isHurt == false)
        {
            m_AudioSource.PlayOneShot(m_AudioClip[3]);
            m_Animator.SetTrigger("Attack");
            isAttack = true;        //按下E键进入攻击状态
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
                m_Canvas.BulletUpdate();                //发射子弹减一并更新UI

                m_AudioSource.PlayOneShot(m_AudioClip[2]);  //播放音效
                m_Animator.SetTrigger("Shoot");             //播放射击动画
                isAttack = true;                //按下鼠标左键进入射击状态
            }
        }
    }




    private void PlayerMove()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); //键盘左右键控制人物移动

        if (isAttack || isHurt)
        {
            moveX = 0;      //角色攻击时不能移动
        }

        //控制人物转向
        if (moveX > 0)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (moveX < 0)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }

        //左右键控制实现Idle到Run动画过渡
        m_Animator.SetFloat("Run", Mathf.Abs(moveX));

        if (!isHurt)
        {
            //玩家的速度
            m_RigidBody2D.velocity = new Vector2(moveX * playerSpeed, m_RigidBody2D.velocity.y);
        }
    }

    private void PlayerJump()
    {
        //按下空格键跳跃
        if (isJumpPressed)
        {
            m_RigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumpPressed = false; //施加向上的力后设为false

            m_Animator.SetBool("Jump", true);   //按下空格键播放跳跃动画
        }
    }
    private void SetIsAttackFalse()
    {
        isAttack = false;                       //攻击结束时恢复状态
        m_Animator.ResetTrigger("Attack");      //重置攻击状态
        m_Animator.ResetTrigger("Shoot");
    }

    //攻击时碰撞体生效
    public void SetAttackColliderOn()
    {
        attackCollider.SetActive(true);
    }

    //攻击结束时碰撞体消失
    public void SetAttackColliderOff()
    {
        attackCollider.SetActive(false);
    }


    //根据玩家的方向调整子弹的偏移量
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
            playerBlood -= randomInt;                      //玩家扣血

            PlayerPrefs.SetInt("PlayerBlood", playerBlood);
            m_Canvas.LifeUpdate();              //更新生命值UI

            if (playerBlood > 0)
            {
                m_AudioSource.PlayOneShot(m_AudioClip[1]);
                isHurt = true;
                canBeHurt = false;
                m_Animator.SetBool("Hurt", true);

                //根据玩家的方向给受伤的角色后退向上的力
                if (transform.localScale.x == 0.5f)
                {
                    m_RigidBody2D.velocity = new Vector2(-2.5f, 10.0f);
                }
                else if (transform.localScale.x == -0.5f)
                {
                    m_RigidBody2D.velocity = new Vector2(2.5f, 10.0f);
                }

            }
            StartCoroutine("SetIsHurtFalse");       //协程执行由Hurt动画返回到Idle动画
        }else if(playerBlood <= 0)
        {
            m_AudioSource.PlayOneShot(m_AudioClip[4]);
            isHurt = true;                          //角色死亡时，防止玩家按下其他键时让然响应
            isAttack = true;                        //角色不能移动
            m_RigidBody2D.velocity = new Vector2(0f, 0f);   //角色速度为0
            m_Animator.SetBool("Die", true);        //角色血量为0，播放死亡动画

            PlayerPrefs.SetInt("PlayerBlood", 100);
            PlayerPrefs.SetInt("Bullet", 200);
            PlayerPrefs.SetInt("PlayerStone", 0);
            diePanel.SetActive(true);               //弹出Game Over对话框
            StartCoroutine("WaitForSecondsLoadScene");
        }
    }

    IEnumerator SetIsHurtFalse()
    {
        yield return new WaitForSeconds(1.0f);
        isHurt = false;     //1秒后受伤状态解除
        m_Animator.SetBool("Hurt", false);      //受伤后1秒回到Idle状态

        yield return new WaitForSeconds(1.0f);
        canBeHurt = true;               //1秒无敌时间玩家可闪避
    }

    public void ForIsHurtSetting()
    {
        isAttack = false;                       //攻击结束时恢复状态
        m_Animator.ResetTrigger("Attack");      //重置攻击状态
        m_Animator.ResetTrigger("Shoot");
        attackCollider.SetActive(false);
    }


    //玩家刚体与敌人刚体重叠在一起时的处理
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && isHurt == false && canBeHurt == true)
        {
            int randomInt = UnityEngine.Random.Range(7, 10);
            playerBlood -= randomInt;                      //玩家扣血

            PlayerPrefs.SetInt("PlayerBlood", playerBlood);
            m_Canvas.LifeUpdate();              //更新生命值UI

            if (playerBlood > 0)
            {
                m_AudioSource.PlayOneShot(m_AudioClip[1]);
                isHurt = true;
                canBeHurt = false;
                m_Animator.SetBool("Hurt", true);

                //根据玩家的方向给受伤的角色后退向上的力
                if (transform.localScale.x == 0.5f)
                {
                    m_RigidBody2D.velocity = new Vector2(-2.5f, 10.0f);
                }
                else if (transform.localScale.x == -0.5f)
                {
                    m_RigidBody2D.velocity = new Vector2(2.5f, 10.0f);
                }

            }
            StartCoroutine("SetIsHurtFalse");       //协程执行由Hurt动画返回到Idle动画
        }
        else if (playerBlood <= 0)
        {
            m_AudioSource.PlayOneShot(m_AudioClip[4]);
            isHurt = true;                          //角色死亡时，防止玩家按下其他键时让然响应
            isAttack = true;                        //角色不能移动
            m_RigidBody2D.velocity = new Vector2(0f, 0f);   //角色速度为0
            m_Animator.SetBool("Die", true);        //角色血量为0，播放死亡动画

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
            isHurt = true;                          //角色掉下悬崖死亡
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

        SceneManager.LoadScene("Select");            //角色死亡，进入选择关卡
    }

}
