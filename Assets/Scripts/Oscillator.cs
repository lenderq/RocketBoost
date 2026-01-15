using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;

    [SerializeField] float speed;

    [SerializeField] bool rollBack;

    Vector3 startPosition;
    Vector3 endPosition;

    private float movementFactor;

    private bool isRolled = true;

    private void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + movementVector;
    }

    private void Update()
    {
        movementFactor = Mathf.PingPong(speed * Time.time, 1);
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);
        RollbackLogic();
    }

    private void RollbackLogic()
    {
        if (rollBack && !isRolled && (movementFactor > 0.99 || movementFactor < 0.01))
        {
            transform.Rotate(0, -180, 0);
            isRolled = true;
        }

        if (movementFactor > 0.4 && movementFactor < 0.6)
            isRolled = false;
    }
}
