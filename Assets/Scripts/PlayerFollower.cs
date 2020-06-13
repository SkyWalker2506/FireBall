//This class is on the main camera to follow player.
//You may optimize it on SetPosition section and
//Write a proper way to update blocks positions on the map to make it an infite gameplay.

using UnityEngine;

public class PlayerFollower : MonoBehaviour {

    Transform player { get { return GameManager.Instance.Player; } }
    float xCamera;
    float yDifference;
    float zDifference;

    private void OnEnable()
    {
        ActionSystem.OnLevelLoaded += SetPosition;
    }

    private void OnDisable()
    {
        ActionSystem.OnLevelLoaded -= SetPosition;
    }

    public void SetPosition()
    {
        xCamera = transform.position.x;
        zDifference = player.position.z - transform.position.z;
        yDifference = player.position.y - transform.position.y;
    }

    private void Update()
    {
        if(player != null)
        {
            transform.position = new Vector3(xCamera, player.position.y - yDifference, player.position.z - zDifference);
        }
    }

 

}
