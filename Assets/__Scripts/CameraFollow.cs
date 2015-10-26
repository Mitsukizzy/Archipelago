using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public float smooth = 1.5f;         // The relative speed at which the camera will catch up.

    private Transform target;           // Reference to the target's transform.
    private Transform player;           // Reference to the player's transform.
    private Vector3 relCameraPos;       // The relative position of the camera from the player.
    private float relCameraPosMag;      // The distance of the camera from the player.
    private Vector3 newPos;             // The position the camera is trying to reach.

    private float rightBound;
    private float leftBound;
    private float topBound;
    private float bottomBound;
    private SpriteRenderer spriteBounds;

    void Awake()
    {
        // Setting up the reference.
        player = GameObject.FindGameObjectWithTag ( "Char" ).transform;
        target = player;
        transform.position = new Vector3 ( player.position.x, player.position.y+3, transform.position.z );

        // Setting the relative position as the initial relative position of the camera in the scene.
        relCameraPos = transform.position - target.position;
        relCameraPosMag = relCameraPos.magnitude - 0.5f;

        float vertExtent = GetComponent<Camera>().orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;
        spriteBounds = GameObject.Find ( "MapBackground" ).GetComponent<SpriteRenderer> ();
        leftBound = ( float )( horzExtent - spriteBounds.bounds.size.x / 2.0f );
        rightBound = ( float )( spriteBounds.bounds.size.x / 2.0f - horzExtent );
        bottomBound = ( float )( vertExtent - spriteBounds.bounds.size.y / 2.0f );
        topBound = ( float )( spriteBounds.bounds.size.y / 2.0f - vertExtent );
    }

    void Update()
    {
        // The standard position of the camera is the relative position of the camera from the player.
        Vector3 standardPos = target.position + relCameraPos;

        // The abovePos is directly above the player at the same distance as the standard position.
        Vector3 abovePos = target.position + Vector3.up * relCameraPosMag;

        // An array of 5 points to check if the camera can see the player.
        Vector3[] checkPoints = new Vector3[5];

        // The first is the standard position of the camera.
        checkPoints[0] = standardPos;

        // The next three are 25%, 50% and 75% of the distance between the standard position and abovePos.
        checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25f);
        checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
        checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75f);

        // The last is the abovePos.
        checkPoints[4] = abovePos;

        // Run through the check points...
        for (int i = 0; i < checkPoints.Length; i++)
        {
            // ... if the camera can see the player...
            if (ViewingPosCheck(checkPoints[i]))
                // ... break from the loop.
                break;
        }

        // Lerp the camera's position between it's current position and it's new position.
        Vector3 newPosition = Vector3.Lerp ( transform.position, newPos, smooth * Time.deltaTime );
        // Keep camera within bounds of the map background
        newPosition.x = Mathf.Clamp ( newPosition.x, leftBound, rightBound );
        newPosition.y = Mathf.Clamp ( newPosition.y, bottomBound, topBound );
        transform.position = newPosition;

        // Make sure the camera is looking at the player.
        SmoothLookAt();
    }


    bool ViewingPosCheck(Vector3 checkPos)
    {
        RaycastHit hit;

        // If a raycast from the check position to the player hits something...
        if (Physics.Raycast(checkPos, target.position - checkPos, out hit, relCameraPosMag))
            // ... if it is not the player...
            if (hit.transform != target)
                // This position isn't appropriate.
                return false;

        // If we haven't hit anything or we've hit the player, this is an appropriate position.
        newPos = checkPos;
        return true;
    }


    void SmoothLookAt()
    {
        // Create a vector from the camera towards the player.
        Vector3 relPlayerPosition = target.position - transform.position;

        // Create a rotation based on the relative position of the player being the forward vector.
        Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);

        // Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
        //transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.deltaTime);
    }

    public void ChangeTarget ( Transform newTarget )
    {
        target = newTarget;
    }

    public void TargetPlayer ()
    {
        target = player;
    }
}
