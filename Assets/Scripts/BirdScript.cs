using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdScript : MonoBehaviour {

    public static BirdScript instance;

    [SerializeField]
    private Rigidbody2D myRigidBody;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private float forwardspeed = 3;

    [SerializeField]
    private float bounceSpeed = 4;

    private bool didFlap;

    public bool isAlive;

    private Button flapButton;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip flapClip, pointClip, diedClip;

    public int score;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        isAlive = true;
        score = 0;
        flapButton = GameObject.FindGameObjectWithTag("FlapButton").GetComponent<Button>();
        flapButton.onClick.AddListener(() => FlapTheBird());
        setCamerasX();
    }

    // Use this for initialization
    void Start () {
		
	}

    void setCamerasX() {
        CameraScript.offsetX = (Camera.main.transform.position.x - transform.position.x) - 1f;
    }

    public float GetPositionX()
    {
        return transform.position.x;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isAlive)
        {
            Vector3 temp = transform.position;
            temp.x += forwardspeed * Time.deltaTime;
            transform.position = temp;

            if (didFlap)
            {
                didFlap = false;
                myRigidBody.velocity = new Vector2(0, bounceSpeed);
                audioSource.PlayOneShot(flapClip);
                anim.SetTrigger("flap");
            }

            if (myRigidBody.velocity.y >= 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                float angle = 0;
                angle = Mathf.Lerp(0, -90, -myRigidBody.velocity.y / 7);
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
	}

    public void FlapTheBird()
    {
        didFlap = true;
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Pipe" || target.gameObject.tag == "Ground" )
        {
            if (isAlive)
            {
                Debug.Log("Collide");
                isAlive = false;
                audioSource.PlayOneShot(diedClip);
                anim.SetTrigger("bird died");
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
       if (target.tag == "PipeHolder")
        {   
            if (isAlive)
            {
                score++;
                audioSource.PlayOneShot(pointClip);
            }
        }
    }
}
