using UnityEngine;

public class HealthCollect : MonoBehaviour
{
    [SerializeField] private float healthValue;
    [SerializeField] private AudioClip sound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.instance.PlaySound(sound);
            collision.GetComponent<Health>().AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}
