using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Timer _destroyTimer;
    private Timer _puddleTimer;
    private float startTime;
    [SerializeField] private Text timeText;
    [SerializeField] private float gameTimeTotal;
    [SerializeField] private float minRandomDestroyTime = 1.0f;
    [SerializeField] private float maxRandomDestroyTime = 1.0f;
    [SerializeField] private Transform repairablesContainer;
    [SerializeField] private GameObject puddlePrefab;
    [SerializeField] private List<Repairable> repairables;
    [SerializeField] private AudioClip audioClip;
    private AudioSource audioSource;

    private int _repairablesCount;

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
    
    private void UpdateRepairables() {
        repairables = new List<Repairable>();
        for (int i = 0; i < repairablesContainer.childCount; i++) {
            repairables.Add(repairablesContainer.GetChild(i).GetComponent<Repairable>());
        }
        _repairablesCount = repairables.Count;
    }

    private void Awake() {
        UpdateRepairables();
        audioSource = GetComponent<AudioSource>();
        Initialize();
    }

    // Start is called before the first frame update
    void Initialize() {
        startTime = Time.time;
        _destroyTimer = new Timer(5);
        _puddleTimer = new Timer(puddleWorstCaseTime + puddleTimerIncrease * repairables.Count / _repairablesCount);
        foreach (Repairable r in repairables) {
            r.RepairDoneEvent += AddRepairable;
        }
    }

    private void DestroyRepariable(Repairable item) {
        List<PickupType> availableMats = item.GetAvailableMaterials();

        List<int> indexes = new List<int>();
        for (int i = 0; i < (availableMats.Count <= 2 ? availableMats.Count : 3); i++) {
            int val = Random.Range(0, availableMats.Count - 1);
            if (!indexes.Contains(val))
                indexes.Add(val);
            else
                i--;
        }

        List<RepairStage> repairStages = new List<RepairStage>();
        for (int i = 0; i < (availableMats.Count <= 2 ? availableMats.Count : 3); i++) {
            repairStages.Add(new RepairStage(availableMats[indexes[i]], Random.Range(1, 3), Color.black));
        }

        item.Break(repairStages);

        repairables.Remove(item);
    }

    private void AddRepairable(Repairable item) {
        audioClip = item.audioClip;
        audioSource.Play();
        repairables.Add(item);
    }

    [SerializeField] private float puddleWorstCaseTime;
    [SerializeField] private float puddleTimerIncrease;
    [SerializeField] private Rect puddleSpawnArea;
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
        _puddleTimer.Duration = puddleWorstCaseTime + puddleTimerIncrease * repairables.Count / _repairablesCount;
        _puddleTimer.Time += Time.deltaTime;
        if (_puddleTimer.Expired()) {
            GameObject obj = Instantiate(puddlePrefab, new Vector3(Random.Range(puddleSpawnArea.x, puddleSpawnArea.width), 0, Random.Range(puddleSpawnArea.y, puddleSpawnArea.height)), Quaternion.identity, repairablesContainer);
            obj.transform.position = new Vector3(obj.transform.position.x, 0, obj.transform.position.z);
            //obj.transform.GetChild(0).transform.RotateAround(obj.transform.position, obj.transform.up, Random.Range(0, 359));
            var r = obj.GetComponent<Repairable>();
            AddRepairable(r);
            DestroyRepariable(r);
            _puddleTimer.Reset();
        }

        string text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        timeText.text = text;
    }
}
