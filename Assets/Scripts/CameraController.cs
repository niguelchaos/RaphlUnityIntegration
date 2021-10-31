using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    private float baseXOffset = 1.5f;
    private float walkXOffset = 3f;
    private float sprintXOffset = 6f;
    float followVelocityX = 0;
    [SerializeField] private float followSmoothTimeX = 0.4f;


    private void LateUpdate()
    {
        // transform.position = new Vector3(player.transform.position.x, player.transform.position.y + yOffset, transform.position.z);
        FollowPlayer();
    }

    private void FollowPlayer()
    {

        if (player.GetComponent<PlayerController>().GetInputDirX() != 0) {
            if (player.GetComponent<PlayerController>().IsSprinting())
            {
                xOffset = sprintXOffset;
            }
            else {
                xOffset = walkXOffset;
            }
        }
        else if (player.GetComponent<PlayerController>().GetInputDirX() == 0) {
            xOffset = baseXOffset;
        }
        
        float currentXAdjustment;
        Vector2 target;

        if (player.GetComponent<PlayerController>().IsFacingRight()) 
        {   currentXAdjustment = xOffset;   } 
        else 
        {   currentXAdjustment = -xOffset;  }

        float targetX =  player.transform.position.x + currentXAdjustment;
        float targetY =  player.transform.position.y + yOffset;
        target = new Vector2(targetX, targetY);

        float newPosX = Mathf.SmoothDamp(transform.position.x, target.x, ref followVelocityX, followSmoothTimeX);

        transform.position = new Vector3(newPosX, targetY, transform.position.z);
    }
}
