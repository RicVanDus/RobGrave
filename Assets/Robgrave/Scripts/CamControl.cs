using UnityEngine;

public class CamControl : MonoBehaviour
{
    
    public Transform target;
    public float camSpeed = 10.0f;

    [SerializeField] private Transform CameraBounds;

    private Vector2 CamBoundsX;
    private Vector2 CamBoundsY;

    private void Start()
    {
        CamBoundsX.x = (CameraBounds.transform.localScale.x / 2) + CameraBounds.transform.position.x;
        CamBoundsX.y = (CameraBounds.transform.localScale.x / 2 * -1) + CameraBounds.transform.position.x;

        CamBoundsY.x = (CameraBounds.transform.localScale.z / 2) + CameraBounds.transform.position.z;
        CamBoundsY.y = (CameraBounds.transform.localScale.z / 2 * -1) + CameraBounds.transform.position.z;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, target.position, Time.deltaTime * camSpeed);

        newPosition.x = Mathf.Clamp(newPosition.x, CamBoundsX.y, CamBoundsX.x);
        newPosition.z = Mathf.Clamp(newPosition.z, CamBoundsY.y, CamBoundsY.x);
        
        transform.position = newPosition;
    }
}
