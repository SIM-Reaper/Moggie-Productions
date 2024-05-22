using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CamZone : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera = null;

    #endregion

    #region MonoBehavior


    void Start()
    {
        virtualCamera.enabled = false;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            virtualCamera.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            virtualCamera.enabled = false;
    }

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    #endregion
}