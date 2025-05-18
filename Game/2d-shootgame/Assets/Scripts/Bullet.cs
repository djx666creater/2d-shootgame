using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject player;               //ȡ�������Ϸ����
    private Rigidbody2D m_Rigidbody2D;      //�ӵ�����

    public float bulletSpeed;               //����ٶ�

    private void Awake()
    {
        player = GameObject.Find("Player");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        //������ҵķ�������ӵ��ķ���
        if (player.transform.localScale.x == 0.5f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            m_Rigidbody2D.AddForce(Vector2.right * bulletSpeed, ForceMode2D.Impulse);
        }else if(player.transform.localScale.x == -0.5f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            m_Rigidbody2D.AddForce(Vector2.left * bulletSpeed, ForceMode2D.Impulse);
        }

       Destroy(this.gameObject, 4.0f);         //5��������ӵ�����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "GeneralEnemy" || collision.tag == "Ground" ||collision.tag == "Enemy")
        {
            Destroy(this.gameObject);       //���ӵ���������ʱ�����Լ�
        }

    }
}
