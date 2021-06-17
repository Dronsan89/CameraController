using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject[] _objects;

    float minPointX, maxPointX, minPointY, maxPointY, minPointZ, maxPointZ;
    Camera camera;
    Renderer rnd = null;

    Vector3 positionCentreAllObject;
    Vector3 minPoint;
    Vector3 maxPoint;
    Vector3 targetPosition;
    Vector3 directionForTarget;

    float screenWidth;
    float screenHeight;
    float screenOffset = 100f;

    float currentValueFieldOfVies;

    float speedFieldOfView = 20;
    float speedChangeFieldOfView = 500;
    float speedRotation = 20;

    void Start()
    {
        camera = GetComponent<Camera>();
        screenHeight = Camera.main.pixelHeight;
        screenWidth = Camera.main.pixelWidth;
    }

    void Update()
    {
        ResetValue();

        FindPoisitionCentreAllObject();

        ScaleFieldOfView();

        RotationForTarget();
    }

    private void ResetValue()
    {
        minPointY = int.MaxValue; maxPointY = 0; minPointX = int.MaxValue; maxPointX = 0; minPointZ = int.MaxValue; maxPointZ = 0;
        positionCentreAllObject = Vector3.zero;
    }

    private void FindPoisitionCentreAllObject()
    {
        for (int i = 0; i < _objects.Length; i++)
        {
            rnd = _objects[i].GetComponent<Renderer>();

            minPoint = camera.WorldToScreenPoint(rnd.bounds.min);
            maxPoint = camera.WorldToScreenPoint(rnd.bounds.max);

            minPointX = Mathf.Min(minPoint.x, minPointX);
            minPointY = Mathf.Min(minPoint.y, minPointY);
            minPointZ = Mathf.Min(minPoint.z, minPointZ);
            maxPointX = Mathf.Max(maxPoint.x, maxPointX);
            maxPointY = Mathf.Max(maxPoint.y, maxPointY);
            maxPointZ = Mathf.Max(maxPoint.z, maxPointZ);
        }
        positionCentreAllObject = new Vector3((minPointX + maxPointX) / 2, (minPointY + maxPointY) / 2, (minPointZ + maxPointZ) / 2);
    }

    private void ScaleFieldOfView()
    {
        currentValueFieldOfVies = camera.fieldOfView;
        if ((Mathf.Abs(maxPointX - minPointX)) > (screenWidth - screenOffset) || (Mathf.Abs(maxPointY - minPointY)) > (screenHeight - screenOffset))
            currentValueFieldOfVies += Time.deltaTime * speedFieldOfView;
        else
            currentValueFieldOfVies -= Time.deltaTime * speedFieldOfView;
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, currentValueFieldOfVies, Time.deltaTime * speedChangeFieldOfView);
    }

    private void RotationForTarget()
    {
        targetPosition = camera.ScreenToWorldPoint(positionCentreAllObject);
        directionForTarget = targetPosition - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionForTarget), speedRotation * Time.deltaTime);
    }
}
