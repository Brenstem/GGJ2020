using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneSwitch : MonoBehaviour
{
    [SerializeField] private string eventName;
    [SerializeField] private string unitySceneName;
    private void Start() {
        EventManager.StartListening(eventName, SceneSwitch);
    }

    private void SceneSwitch() {
        SceneManager.LoadScene(unitySceneName, LoadSceneMode.Single);
    }

    private void OnDestroy() {
        EventManager.StopListening(eventName, SceneSwitch);
    }
}
