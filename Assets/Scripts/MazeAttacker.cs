using UnityEngine;

public class MazeAttacker : MonoBehaviour
{
    public float speed = 2f;
    private bool hasBall = false;
    public Animator Anim;
    public float x;
    public float z;
    public FixedJoystick joystick;
    public bool pc;
    private void Start()
    {
        Anim = gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        if (pc)
        {
            x= Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }
        else
        {
            x = joystick.Horizontal;
            z = joystick.Vertical;
        }
        Anim.SetFloat("x", x);
        Anim.SetFloat("z", z);

        Vector3 movement = new Vector3(x, 0.0f, z);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MazeBall"))
        {
            hasBall = true;
            Destroy(other.gameObject);
            Debug.Log("Ball picked up!");
        }

        if (other.CompareTag("EnemyGate") && hasBall)
        {
            MazeGameManager.Instance.PlayerWins();
        }
    }

    public void PickUpBall()
    {
        hasBall = true;
        Debug.Log("Ball picked up by attacker!");
    }
} 