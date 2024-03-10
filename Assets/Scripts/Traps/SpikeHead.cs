using UnityEngine;

public class SpikeHead : EnemiesDamage
{
    [Header("SpikeHead Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float delay;
    [SerializeField] private LayerMask playerLayer;

    [Header("SFX")]
    [SerializeField] private AudioClip sound;

    private Vector3[] direction = new Vector3[4];
    private Vector3 destination;
    private float checkTimer;
    private bool atk;


    private void OnEnable()
    {
        Stop();
    }

    private void Update()
    {
        // Tấn công khi player di chuyển
        if (atk)
            transform.Translate(destination * Time.deltaTime * speed);
        else
            checkTimer += Time.deltaTime;
            if (checkTimer > delay)
                CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        CalcDirections();

        // Check spikeHead nhìn player ở 4 hướng
        for (int i = 0; i < direction.Length; i++)
        {
            Debug.DrawRay(transform.position, direction[i], Color.cyan);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction[i], range, playerLayer);

            if (hit.collider != null && !atk)
            {
                atk = true;
                destination = direction[i];
                checkTimer = 0;
            }
        }     
    }

    private void CalcDirections()
    {
        direction[0] = transform.right * range; // bên phải
        direction[1] = -transform.right * range; // bên trái
        direction[2] = transform.up * range; // bên trên
        direction[3] = -transform.up * range; // bên dưới
    }

    private void Stop()
    {
        // trả về vị trí ban đầu
        destination = transform.position;
        atk = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(sound);
        base.OnTriggerEnter2D(collision);
        Stop();// Dừng spikeHead khi chạm vào vật thể (something)
    }
}
