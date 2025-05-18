using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject player;               //取得玩家游戏对象
    private Rigidbody2D m_Rigidbody2D;      //子弹刚体

    public float bulletSpeed;               //玩家速度

    private void Awake()
    {
        player = GameObject.Find("Player");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        //根据玩家的方向决定子弹的方向
        if (player.transform.localScale.x == 0.5f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            m_Rigidbody2D.AddForce(Vector2.right * bulletSpeed, ForceMode2D.Impulse);
        }else if(player.transform.localScale.x == -0.5f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            m_Rigidbody2D.AddForce(Vector2.left * bulletSpeed, ForceMode2D.Impulse);
        }

       Destroy(this.gameObject, 4.0f);         //5秒后销毁子弹对象
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "GeneralEnemy" || collision.tag == "Ground" ||collision.tag == "Enemy")
        {
            Destroy(this.gameObject);       //当子弹碰到物体时销毁自己
        }

    }
}
