using Cinemachine;
using System.Collections;
using UnityEngine;
public class Zoom : MonoBehaviour {
    [SerializeField] KeyCode _toggleKey;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    public float _minimum = 3.5f;
    public float _maximum;
    static float t = 0f;
    void Start() {
        _minimum = _virtualCamera.m_Lens.OrthographicSize;
        _maximum = _minimum * 2;
    }
    void Update() {
    }

    IEnumerator Lerp(float start, float end) {
        t = 0f;
        while(_virtualCamera.m_Lens.OrthographicSize != end) {
            _virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(start, end, t);
            t += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

}