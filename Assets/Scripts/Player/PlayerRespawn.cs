using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager UIManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        UIManager = FindObjectOfType<UIManager>();
    }

    private void CheckRespawn()
    {
        // Check nếu checkpoint có hiệu lực
        if (currentCheckpoint == null)
        {
            // Hiển thị Over game
            UIManager.GameOver();
            return;
        }
        transform.position = currentCheckpoint.position;
        playerHealth.Respawn();

        // Di chuyển camera về checkpoint room
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    // Activate checkpoints
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
