using UnityEngine;
using UnityEngine.PlayerLoop;


//In this section, you have to edit OnPointerDown and OnPointerUp sections to make the game behave in a proper way using hJoint
//Hint: You may want to Destroy and recreate the hinge Joint on the object. For a beautiful gameplay experience, joint would created after a little while (0.2 seconds f.e.) to create mechanical challege for the player
//And also create fixed update to make score calculated real time properly.
//Update FindRelativePosForHingeJoint to calculate the position for you rope to connect dynamically
//You may add up new functions into this class to make it look more understandable and cosmetically great.

public class PlayerController : MonoBehaviour {


    [SerializeField]
    private HingeJoint hJoint;
    [SerializeField]
    private LineRenderer lRenderer;
    [SerializeField]
    private Rigidbody playerRigidbody;
    private float lastUpdatedScore;

    private bool gameOver = false;

    [SerializeField]
    float moveForce = 5f;
    bool isTouchingTheScreen;
    void Start ()
    {
        FindRelativePosForHingeJoint(new Vector3(0,10,0));
	}

    private void Update()
    {
        if (GameManager.Instance.CurrentScore - lastUpdatedScore > .05f)
        {
            lastUpdatedScore = GameManager.Instance.CurrentScore;
            ActionSystem.OnScoreChanged?.Invoke();
        }
    }
    private void FixedUpdate()
    {
        SetScore();
        if (isTouchingTheScreen)
        {
            MovePlayerForward();
        }
    }

    public void FindRelativePosForHingeJoint(Vector3 blockPosition)
    {
        transform.rotation = Quaternion.identity;
        hJoint.anchor = (blockPosition - transform.position);
        lRenderer.SetPosition(1, hJoint.anchor);
        lRenderer.enabled = true;
    }

    void PointerDown()
    {
        if (!GameManager.Instance.IsGameOn)
            ActionSystem.OnGameStarted?.Invoke();
        isTouchingTheScreen = true;
        if (hJoint == null)
        {
            hJoint = gameObject.AddComponent<HingeJoint>();
            var target = BlockCreator.GetSingleton().GetRelativeBlock(transform.position);
            if (target != null)
                FindRelativePosForHingeJoint(target.position);
            else
                print("NoTarget");
        }
 
    }
    void PointerUp()
    {
        isTouchingTheScreen = false;
        lRenderer.enabled = false;
        Destroy(hJoint);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Block") && !gameOver)
        {
            PointerUp(); 
            gameOver = true;
            GlobalValues.HighestScore = GameManager.Instance.CurrentScore;
            ActionSystem.OnGameEnded?.Invoke();
        }
    }



 
    public void SetScore()
    {
        if (GameManager.Instance.IsGameOn)
        {
             GameManager.Instance.CurrentScore += playerRigidbody.velocity.z * Time.fixedDeltaTime * 0.1f;
        }
    }

    void MovePlayerForward()
    {
        playerRigidbody.AddForce(transform.forward * moveForce);
    }

}
