using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabber : MonoBehaviour
{
    private float grabBegin = 0.55f;
    private float grabEnd = 0.35f;


    [SerializeField] protected Transform gripTransform = null;
    [SerializeField] protected Collider[] grabVolumes = null;

    /* Should be OVRInput.Controller.LTouch or OVRInput.Controller.RTouch. */
    [SerializeField] protected OVRInput.Controller m_controller;

    /** We put the OVRCamera transform here! */
    [SerializeField] protected Transform _parentTransform;


    protected bool _grabVolumeEnabed = true;
    protected Vector3 _lastPos;
    protected Quaternion _lastRot;
    protected Quaternion _anchorOffsetRotation;
    protected Vector3 _anchorOffsetPosition;
    protected float m_prevFlex;
    protected BaseGrabbable _grabbedObj = null;
    protected Vector3 _grabbedObjectPosOff;
    protected Quaternion _grabbedObjectRotOff;
    protected Dictionary<BaseGrabbable, int> _grabCandidates = new Dictionary<BaseGrabbable, int>();
    protected bool operatingWithoutOVRCameraRig = true;

    public BaseGrabbable grabbedObject => _grabbedObj;

    public void ForceRelease(BaseGrabbable grabbable) {
        bool canRelease = (
            (_grabbedObj != null) &&
            (_grabbedObj == grabbable)
        );
        if (canRelease) {
            GrabEnd();
        }
    }

    protected virtual void Awake() {
        _anchorOffsetPosition = transform.localPosition;
        _anchorOffsetRotation = transform.localRotation;

        // If we are being used with an OVRCameraRig, let it drive input updates, which may come from Update or FixedUpdate.

        OVRCameraRig rig = null;
        if (transform.parent != null && transform.parent.parent != null)
            rig = transform.parent.parent.GetComponent<OVRCameraRig>();

        if (rig != null) {
            rig.UpdatedAnchors += (r) => { OnUpdatedAnchors(); };
            operatingWithoutOVRCameraRig = false;
        }
    }

    protected virtual void Start() {
        _lastPos = transform.position;
        _lastRot = transform.rotation;
        if (_parentTransform == null) {
            if (gameObject.transform.parent != null) {
                _parentTransform = gameObject.transform.parent.transform;
            }
            else {
                _parentTransform = new GameObject().transform;
                _parentTransform.position = Vector3.zero;
                _parentTransform.rotation = Quaternion.identity;
            }
        }
    }

    void FixedUpdate() {
        if (operatingWithoutOVRCameraRig) {
            OnUpdatedAnchors();
        }
    }

    void OnUpdatedAnchors() {
        Vector3 handPos = OVRInput.GetLocalControllerPosition(m_controller);
        Quaternion handRot = OVRInput.GetLocalControllerRotation(m_controller);
        Vector3 destPos = _parentTransform.TransformPoint(_anchorOffsetPosition + handPos);
        Quaternion destRot = _parentTransform.rotation * handRot * _anchorOffsetRotation;
        GetComponent<Rigidbody>().MovePosition(destPos);
        GetComponent<Rigidbody>().MoveRotation(destRot);

        MoveGrabbedObject(destPos, destRot);

        _lastPos = transform.position;
        _lastRot = transform.rotation;

        float prevFlex = m_prevFlex;
        // Update values from inputs
        m_prevFlex = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller);

        CheckForGrabOrRelease(prevFlex);
    }

    void OnDestroy() {
        if (_grabbedObj != null) {
            GrabEnd();
        }
    }

    void OnTriggerEnter(Collider otherCollider) {
        // Get the grab trigger
        BaseGrabbable grabbable = otherCollider.GetComponent<BaseGrabbable>() ??
                                  otherCollider.GetComponentInParent<BaseGrabbable>();
        if (grabbable == null) return;

        // Add the grabbable
        int refCount = 0;
        _grabCandidates.TryGetValue(grabbable, out refCount);
        _grabCandidates[grabbable] = refCount + 1;
    }

    void OnTriggerExit(Collider otherCollider) {
        BaseGrabbable grabbable = otherCollider.GetComponent<BaseGrabbable>() ??
                                  otherCollider.GetComponentInParent<BaseGrabbable>();
        if (grabbable == null) return;

        // Remove the grabbable
        int refCount = 0;
        bool found = _grabCandidates.TryGetValue(grabbable, out refCount);
        if (!found) {
            return;
        }

        if (refCount > 1) {
            _grabCandidates[grabbable] = refCount - 1;
        }
        else {
            _grabCandidates.Remove(grabbable);
        }
    }

    protected void CheckForGrabOrRelease(float prevFlex) {
        if ((m_prevFlex >= grabBegin) && (prevFlex < grabBegin)) {
            GrabBegin();
        }
        else if ((m_prevFlex <= grabEnd) && (prevFlex > grabEnd)) {
            GrabEnd();
        }
    }

    protected virtual void GrabBegin() {
        float closestMagSq = float.MaxValue;
        BaseGrabbable closestGrabbable = null;
        Collider closestGrabbableCollider = null;

        // Iterate grab candidates and find the closest grabbable candidate
        foreach (BaseGrabbable grabbable in _grabCandidates.Keys) {
            bool canGrab = !(grabbable.isGrabbed && !grabbable.allowOffhandGrab);
            if (!canGrab) {
                continue;
            }

            for (int j = 0; j < grabbable.grabPoints.Length; ++j) {
                Collider grabbableCollider = grabbable.grabPoints[j];
                // Store the closest grabbable
                Vector3 closestPointOnBounds = grabbableCollider.ClosestPointOnBounds(gripTransform.position);
                float grabbableMagSq = (gripTransform.position - closestPointOnBounds).sqrMagnitude;
                if (grabbableMagSq < closestMagSq) {
                    closestMagSq = grabbableMagSq;
                    closestGrabbable = grabbable;
                    closestGrabbableCollider = grabbableCollider;
                }
            }
        }

        // Disable grab volumes to prevent overlaps
        GrabVolumeEnable(false);

        if (closestGrabbable != null) {
            if (closestGrabbable.isGrabbed) {
                closestGrabbable.grabbedBy.OffhandGrabbed(closestGrabbable);
            }

            _grabbedObj = closestGrabbable;
            _grabbedObj.GrabBegin(this, closestGrabbableCollider);

            _lastPos = transform.position;
            _lastRot = transform.rotation;

            // Set up offsets for grabbed object desired position relative to hand.
            if (_grabbedObj.snapPosition) {
                _grabbedObjectPosOff = gripTransform.localPosition;
                if (_grabbedObj.snapOffset) {
                    Vector3 snapOffset = _grabbedObj.snapOffset.position;
                    if (m_controller == OVRInput.Controller.LTouch) snapOffset.x = -snapOffset.x;
                    _grabbedObjectPosOff += snapOffset;
                }
            }
            else {
                Vector3 relPos = _grabbedObj.transform.position - transform.position;
                relPos = Quaternion.Inverse(transform.rotation) * relPos;
                _grabbedObjectPosOff = relPos;
            }

            if (_grabbedObj.snapOrientation) {
                _grabbedObjectRotOff = gripTransform.localRotation;
                if (_grabbedObj.snapOffset) {
                    _grabbedObjectRotOff = _grabbedObj.snapOffset.rotation * _grabbedObjectRotOff;
                }
            }
            else {
                Quaternion relOri = Quaternion.Inverse(transform.rotation) * _grabbedObj.transform.rotation;
                _grabbedObjectRotOff = relOri;
            }

            MoveGrabbedObject(_lastPos, _lastRot);
        }
    }

    protected virtual void MoveGrabbedObject(Vector3 pos, Quaternion rot) {
        if (_grabbedObj == null) {
            return;
        }

        Rigidbody grabbedRigidbody = _grabbedObj.grabbedRigidbody;
        Vector3 grabbablePosition = pos + rot * _grabbedObjectPosOff;
        Quaternion grabbableRotation = rot * _grabbedObjectRotOff;

        grabbedRigidbody.transform.position = grabbablePosition;
        grabbedRigidbody.transform.rotation = grabbableRotation;
    }

    protected void GrabEnd() {
        if (_grabbedObj != null) {
            OVRPose localPose = new OVRPose {
                position = OVRInput.GetLocalControllerPosition(m_controller),
                orientation = OVRInput.GetLocalControllerRotation(m_controller)
            };
            OVRPose offsetPose = new OVRPose {position = _anchorOffsetPosition, orientation = _anchorOffsetRotation};
            localPose = localPose * offsetPose;

            OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
            Vector3 linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(m_controller);
            Vector3 angularVelocity =
                trackingSpace.orientation * OVRInput.GetLocalControllerAngularVelocity(m_controller);

            GrabbableRelease(linearVelocity, angularVelocity);
        }

        // Re-enable grab volumes to allow overlap events
        GrabVolumeEnable(true);
    }

    protected void GrabbableRelease(Vector3 linearVelocity, Vector3 angularVelocity) {
        _grabbedObj.GrabEnd(linearVelocity, angularVelocity);
        _grabbedObj = null;
    }

    protected virtual void GrabVolumeEnable(bool enabled) {
        if (_grabVolumeEnabed == enabled) {
            return;
        }

        _grabVolumeEnabed = enabled;
        for (int i = 0; i < grabVolumes.Length; ++i) {
            Collider grabVolume = grabVolumes[i];
            grabVolume.enabled = _grabVolumeEnabed;
        }

        if (!_grabVolumeEnabed) {
            _grabCandidates.Clear();
        }
    }

    protected virtual void OffhandGrabbed(BaseGrabbable grabbable) {
        if (_grabbedObj == grabbable) {
            GrabbableRelease(Vector3.zero, Vector3.zero);
        }
    }
}