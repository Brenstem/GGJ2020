using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Timer _destroyTimer;
    private float startTime;
    [SerializeField] private Text timeText;
    [SerializeField] private float gameTimeTotal;
    [SerializeField] private float minRandomDestroyTime = 1.0f;
    [SerializeField] private float maxRandomDestroyTime = 1.0f;
    [SerializeField] private Transform repairablesContainer;
    [SerializeField] private List<Repairable> repairables;

    private static GameManager _instance;
    private static GameManager instance {
        get {
            if (!_instance) {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance) { // Will return null if there are more than one EventManager classes
                    _instance.Initialize();
                }
                else {
                    Debug.LogWarning("There can only be one GameManager script on a GameObject active at a time");
                }
            }
            return _instance;
        }
    }

    private void Awake() {
        repairables = new List<Repairable>();
        for (int i = 0; i < repairablesContainer.childCount; i++) {
            repairables.Add(repairablesContainer.GetChild(i).GetComponent<Repairable>());
        }
        Initialize();
    }

    // Start is called before the first frame update
    void Initialize() {
        startTime = Time.time;
        _destroyTimer = new Timer(5);
        foreach (Repairable r in repairables) {
            r.RepairDoneEvent += AddRepairable;
        }
    }

    private void DestroyRepariable(Repairable item) {
        List<PickupType> availableMats = item.GetAvailableMaterials();
        int itemCount = Random.Range(1, 3);

        HashSet<int> matIndexes = new HashSet<int>();
        while (matIndexes.Count < itemCount) {
            matIndexes.Add(Random.Range(0, availableMats.Count - 1));
        }

        List<RepairStage> repairStages = new List<RepairStage>();
        foreach (int index in matIndexes) {
            repairStages.Add(new RepairStage(availableMats[index], Random.Range(1, 3), Color.black));
        }

        item.Break(repairStages);

        repairables.Remove(item);
    }

    private void AddRepairable(Repairable item) {
        repairables.Add(item);
    }

    // Update is called once per frame
    void Update()
    {
        var guiTime = gameTimeTotal - Time.time - startTime;
        
        int minutes = (int)guiTime / 60;
        int seconds = (int)guiTime % 60;
        int fraction = (int)(guiTime * 100) % 100;
        if (minutes + seconds + fraction <= 0) {
            minutes = seconds = fraction = 0;
            EventManager.TriggerEvent("Quit");
        }
        _destroyTimer.Time += Time.deltaTime;
        if (_destroyTimer.Expired()) {
            _destroyTimer.Reset();
            _destroyTimer = new Timer(Random.Range(minRandomDestroyTime, maxRandomDestroyTime));
            if (repairables.Count >= 1) {
                DestroyRepariable(repairables[Random.Range(0, repairables.Count - 1)]);
            }
        }

        string text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        timeText.text = text;
    }
}
