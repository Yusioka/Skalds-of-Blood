using UnityEngine;

public class Hook : MonoBehaviour
{
    bool active;
    float speed = 20f;
    GameObject target;
    LineRenderer lineRenderer;
    Vector3 startPosition;
    Vector3 endDirection;
    Vector3 endPosition;
    bool end;

    private void Awake()
    {
        lineRenderer = GetComponentInParent<LineRenderer>();
    }

    public void Initialize(float range, Vector3 startPosition, Vector3 endPoint)
    {
        this.startPosition = startPosition;

        endDirection = (endPoint - startPosition).normalized;
        endPosition = startPosition + (endDirection * range);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

        transform.LookAt(endPosition);
        active = true;
    }

    private void Update()
    {
        if (active) 
        { 
            if(Vector3.Distance(transform.position, endPosition) <= 1f) 
            { 
                if(end)
                {
                    GetComponentInParent<DestroyHook>().DestroyObject();
                }

                endPosition = startPosition;
                endDirection = (startPosition - transform.position).normalized;
                end = true;
            }
            else
            {
                transform.position += endDirection * speed * Time.deltaTime;

                if (end && target != null)
                {
                    target.transform.position = transform.position;
                }
                if (!end)
                {
                    transform.Rotate(new Vector3(0, 0, 15f));
                }
            }
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && !end)
        {
            target = collision.gameObject;
            endPosition = startPosition;
            endDirection = (startPosition - transform.position).normalized; 
            end = true;
        }
    }
}