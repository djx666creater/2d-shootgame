using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : MonoBehaviour
{
    public AudioClip[] m_AudioClip;
    public AudioSource m_AudioSource;
    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
        string levelName = SceneManager.GetActiveScene().name;
        if (levelName == "MainMenu")
        {
            m_AudioSource.clip = m_AudioClip[0];
            m_AudioSource.Play();
        }
        if(levelName == "Select")
        {
            m_AudioSource.clip = m_AudioClip[1];
            m_AudioSource.Play();
        }
        if(levelName == "Level1" || levelName == "Level2" || levelName == "Level3")
        {
            m_AudioSource.clip = m_AudioClip[2];
            m_AudioSource.Play();
        }
    }
}
