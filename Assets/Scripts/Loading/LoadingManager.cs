using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;


public class LoadingManager : MonoBehaviour
{
    [Scene]
    public string scenePath;

    [Header("2D Camera"), SerializeField]
    GameObject mobileCamera;


    [Header("VR Camera"), SerializeField]
    GameObject vrCamera;
    [Header("VR Hands"), SerializeField]
    GameObject leftHand;
    [SerializeField]
    GameObject rightHand;

    void Start()
    {
        SpawnCamera();

        SceneManager.LoadScene(scenePath);
    }

    void SpawnCamera()
    {
        SpawnMobileCamera();
        SpawnVRCamera();
    }

    void SpawnMobileCamera()
    {
        mobileCamera = Instantiate(mobileCamera);

        // mobileCamera.GetComponentInChildren<InGameUIController>().gameObject.SetActive(false);

        DontDestroyOnLoad(mobileCamera);
    }

    void SpawnVRCamera()
    {
        vrCamera = Instantiate(vrCamera);

        leftHand = Instantiate(leftHand);
        rightHand = Instantiate(rightHand);

        leftHand.GetComponent<Grabber>().SetParentTransform(vrCamera.transform);
        rightHand.GetComponent<Grabber>().SetParentTransform(vrCamera.transform);

        DontDestroyOnLoad(vrCamera);
        DontDestroyOnLoad(rightHand);
        DontDestroyOnLoad(leftHand);
    }
}
