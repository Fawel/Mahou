using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class CustomEvent:UnityEvent<string>
        {

        }

public class PlayerMovement : MonoBehaviour {
    
    public CustomEvent _event;
    private Rigidbody rg;
    public Text scoretext;
    public Text wintext;
    private int score;
    public float speed;
    public int elegance_score=0;
    public int health = 100;
    Danmaku danmaku;
    void Start()
    {
        _event = new CustomEvent();
        score = 0;
     //   work_with_UI();
        rg = GetComponent<Rigidbody>();
        _event.AddListener(eventmethod);
    }

	void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical"); float horizontal = Input.GetAxis("Horizontal");

//        Vector3 movement = new Vector3(horizontal, 0, vertical);
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            rg.AddForce(new Vector3(0, 500f, 0));
//        }
//        rg.AddForce(movement*speed*Time.deltaTime);
  //      work_with_UI();
    }
    void Update()
    {
        if (wintext.IsActive())
            _event.Invoke("Mah event Worked");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pickup"))
        {
            Destroy(other.gameObject);
            score++;
        }
        //if (other.gameObject.CompareTag("Danmaku"))
        //{
        //    Destroy(other.gameObject);
        //}
    }

//    void work_with_UI()
//    {
//        scoretext.text = "Score: " + score;
//        if (score == 4)
//            wintext.gameObject.SetActive(true);
//        
//
//    }

    void eventmethod(string test)
    {
        print(test);
    }
}
