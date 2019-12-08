using UnityEngine;

public class BaseGrabbable : MonoBehaviour
{
    [SerializeField] protected bool _allowOffhandGrab = true;
    [SerializeField] protected bool _snapPosition = false;
    [SerializeField] protected bool _snapOrientation = false;
    [SerializeField] protected Transform _snapOffset;
    [SerializeField] protected Collider[] _grabPoints = null;

    protected bool _grabbedKinematic = false;
    protected Collider _grabbedCollider = null;
    protected Grabber _grabbedBy = null;

    public bool allowOffhandGrab => _allowOffhandGrab;

    public bool isGrabbed => _grabbedBy != null;

    public bool snapPosition => _snapPosition;

    public bool snapOrientation => _snapOrientation;

    public Transform snapOffset => _snapOffset;

    public Grabber grabbedBy => _grabbedBy;

    public Rigidbody grabbedRigidbody => _grabbedCollider.attachedRigidbody;

    public Collider[] grabPoints => _grabPoints;

    public virtual void GrabBegin(Grabber hand, Collider grabPoint) {
        _grabbedBy = hand;
        _grabbedCollider = grabPoint;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public virtual void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity) {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = _grabbedKinematic;
        rb.velocity = linearVelocity;
        rb.angularVelocity = angularVelocity;
        _grabbedBy = null;
        _grabbedCollider = null;
    }

    protected void Awake() {
        if (_grabPoints.Length == 0) {
            // Get the collider from the grabbable
            Collider collider = this.GetComponent<Collider>();
            // Create a default grab point
            _grabPoints = new Collider[1] {collider};
        }
    }

    protected virtual void Start() {
        _grabbedKinematic = GetComponent<Rigidbody>().isKinematic;
    }

    void OnDestroy() {
        if (_grabbedBy != null) {
            // Notify the hand to release destroyed grabbables
            _grabbedBy.ForceRelease(this);
        }
    }
}