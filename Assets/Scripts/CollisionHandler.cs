using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float timeBeforeFinish = 3f;
    [SerializeField] float timeBeforeReloadLevel = 3f;

    [SerializeField] ParticleSystem successSystem;
    [SerializeField] ParticleSystem crashSystem;

    [SerializeField] AudioClip successSound;
    [SerializeField] AudioClip crashSound;

    private AudioSource audioSource;

    private bool end = false;
    private bool isCollidable = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKey();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (end || !isCollidable)
            return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("It's Friendly");
                break;
            case "Finish":
                Finished();
                break;
            default:
                Crashed();
                break;
        }
    }


    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Crashed()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.PlayOneShot(crashSound);

        DisableMoving();
        crashSystem.Play();
        Invoke("ReloadLevel", timeBeforeReloadLevel);
    }

    private void Finished()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.PlayOneShot(successSound);

        DisableMoving();
        successSystem.Play();
        Invoke("LoadNextLevel", timeBeforeFinish);
    }

    private void DisableMoving()
    {
        end = true;
        GetComponent<Movement>().enabled = false;
    }

    private void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(0);
    }

    private void RespondToDebugKey()
    {
        //if (Keyboard.current.lKey.wasPressedThisFrame)
        //{
        //    LoadNextLevel();
        //}
        //else if (Keyboard.current.cKey.wasPressedThisFrame)
        //{
        //    isCollidable = !isCollidable;
        //    successSystem.startLifetime = 0.3f;
        //    successSystem.Play();
        //}
    }
}
