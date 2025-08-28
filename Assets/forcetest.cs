using UnityEngine;

public class forcetest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody2D ours;
    public Transform targ;    
    public Vector3 rotation;


    basic_character self;

    void Update()
    {
        self = gameObject.GetComponent<basic_character>();
        TryGetComponent(out Rigidbody2D ours);
        var dir = targ.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
 
    }
}
