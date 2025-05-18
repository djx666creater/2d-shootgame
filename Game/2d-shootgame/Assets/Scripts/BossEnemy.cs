using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnemy : MonoBehaviour
{
    private bool isAlive;
    private bool isIdle;
    private bool jumpAttack;
    private bool isJumpUp;              //为true时往上跳，为false时往下跳
    private bool slideAttack;
    private bool isHurt;
    private bool canBeHurt;

    public int bossBlood;               //血条
    public float attackDistance;        //攻击距离
    public float jumpHeight;             //跳跃高度
    public float jumpUpSpeed = 15.0f;   //往上跳跃速度
    public float jumpDownSpeed = 15.0f; //往下速度
    public float fallDownSpeed = 10.0f; //下坠速度
    public float slideSpeed = 8.0f;     //滑行速度

    public GameObject player;
    public Animator m_Animator;
    public BoxCollider2D m_BoxCollider2D;
    public AudioSource m_AudioSource;
    public GameObject winPanel;         //杀死Boss弹出胜利对话框

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





    //根据玩家的位置，设置敌人的朝向问题
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
        EnemyDirection();       //敌人的朝向问题，只在Idle||JumpAttack||slideAttack为true时调用
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
        EnemyDirection();       //敌人的朝向问题
        if (isJumpUp)
        {
            //isJump为true时，跳跃到玩家上方位置，并且播放往上跳的动画
            Vector3 targetPosition = new Vector3(player.transform.position.x, jumpHeight, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, jumpUpSpeed * Time.deltaTime);
            m_Animator.SetBool("JumpUp", true);
        }
        else
        {
            //isJump为false时，往下跳跃到玩家位置，并且播放往下跳的动画
            m_Animator.SetBool("JumpUp", false);
            m_Animator.SetBool("JumpDown", true);

            Vector3 targetPosition = new Vector3(transform.position.x, -1.22f, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, jumpDownSpeed * Time.deltaTime);
        }
        if (transform.position.y == jumpHeight)
        {
            isJumpUp = false;       //如果在向上的目标高度，就不能往上跳跃
        }
        else if (transform.position.y == -1.22f)
        {
            //如果在向下的目标高度，回到Idle状态，并关闭往下跳的动画,boss回到初始状态
            jumpAttack = false;
            StartCoroutine("JumpDownToIdle");
        }
    }

    private void SlideAttack()
    {   //滑行到玩家位置进行攻击
        m_Animator.SetBool("Slide", true);
        Vector3 slideTargetPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, slideTargetPosition, slideSpeed * Time.deltaTime);

        //如果已在目标滑行位置，boss回到初始状态
        if(transform.position == slideTargetPosition)
        {
            m_Animator.SetBool("Slide", false);
            slideAttack = false;
            isIdle = true;      
        }
    }


    private void IsHurt()
    {
        //在空中受伤时应下坠
        Vector3 targetPosition = new Vector3(transform.position.x, -1.22f, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallDownSpeed * Time.deltaTime);
    }


    IEnumerator JumpDownToIdle()
    {
        //延迟0.5秒回到Idle状态，并关闭往下跳的动画,boss回到初始状态
        yield return new WaitForSeconds(0.5f);
        isIdle = true;
        isJumpUp = true;
        m_Animator.SetBool("JumpUp", false);
        m_Animator.SetBool("JumpDown", false);
    }

    IEnumerator IdleToJumpAttack()
    {
        yield return new WaitForSeconds(1.0f);
        jumpAttack = true;      //延迟1秒后进入可以跳跃攻击状态
    }

    IEnumerator IdleToSlideAttack()
    {
        yield return new WaitForSeconds(1.0f);
        EnemyDirection();
        slideAttack = true;     //延迟1秒后进入滑行攻击状态
    }

    IEnumerator SetAnimationHurtToFalse()
    {
        //由受伤转为idle状态
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
        //Boss受到玩家的伤害后，关闭所有行为,并开启受伤动画
        if (collision.tag == "PlayerAttack")
        {
            if (canBeHurt)
            {
                m_AudioSource.PlayOneShot(m_AudioSource.clip);  //播放受伤音效
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
                    StopCoroutine("IdleToSlideAttack");//受伤后关闭所有行为并开启受伤动画
                    m_Animator.SetBool("Hurt", true);

                    StartCoroutine("SetAnimationHurtToFalse");  //由受伤转为idle状态
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


