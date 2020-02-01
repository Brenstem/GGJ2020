using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RepairStage
{
    Sprite _wireSprite;
    Sprite _tapeSprite;
    Sprite _screwSprite;
    Sprite _glueSprite;
    Sprite _metalSprite;
    Sprite _chipSprite;

    [SerializeField] PickupType _pickupType;
    [SerializeField, Range(0, 10)] int _amount;

    [SerializeField] public PickupType PickupType { get { return _pickupType; } set { _pickupType = value; } }
    [SerializeField] public int Amount { get { return _amount; } set { _amount = value; } }

    public Sprite WireSprite { get { return _wireSprite; } set { _wireSprite = value; } }
    public Sprite TapeSprite { get { return _tapeSprite; } set { _tapeSprite = value; } }
    public Sprite ScrewSprite { get { return _screwSprite; } set { _screwSprite = value; } }
    public Sprite GlueSprite { get { return _glueSprite; } set { _glueSprite = value; } }
    public Sprite MetalSprite { get { return _metalSprite; } set { _metalSprite = value; } }
    public Sprite ChipSprite { get { return _chipSprite; } set { _chipSprite = value; } }

    public RepairStage(PickupType type, int amount, Color color)
    {
        if (amount <= 0)
            throw new System.Exception("Can't add negative amount of items stoopid");

        PickupType = type;
        Amount = amount;
    }

    public void SetSprites(Sprite wireSprite, Sprite tapeSprite, Sprite screwSprite, Sprite glueSprite, Sprite metalSprite, Sprite chipSprite)
    {
        _wireSprite = wireSprite;
        _tapeSprite = tapeSprite;
        _screwSprite = screwSprite;
        _glueSprite = glueSprite;
        _metalSprite = metalSprite;
        _chipSprite = chipSprite;
    }
}


public class Repairable : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] Slider _slider;
    [SerializeField] Image _backgroundSliderImage;
    [SerializeField] GameObject _layoutGroupPrefab;
    [SerializeField] Transform _listOfStuffTransform;

    [SerializeField] Sprite _wireSprite;
    [SerializeField] Sprite _tapeSprite;
    [SerializeField] Sprite _screwSprite;
    [SerializeField] Sprite _glueSprite;
    [SerializeField] Sprite _metalSprite;
    [SerializeField] Sprite _chipSprite;

    [Header("Settings")]
    [SerializeField] float _alphaAmount = 0.4f;
    [SerializeField] Color _startColor = Color.red;
    [SerializeField] Color _endColor = Color.green;
    [SerializeField] Timer _completionTimer;
    [SerializeField] Timer _eachStepRepairTimer;
    [SerializeField] Timer _showDoneTimer;
    [SerializeField] bool _needRepair = true;
    [SerializeField] PickupType _toolRequired;
    [SerializeField] List<RepairStage> _repairStages;

    List<LayoutGroup> _layoutGroups = new List<LayoutGroup>();
    int _currentAmountItemDone = 0;
    int _amountItemToDo = 0;

    public delegate void RepairDoneDelegate(Repairable repairable);
    public event RepairDoneDelegate RepairDoneEvent;

    float _stepRepairAmount;

    private void Awake()
    {
        _stepRepairAmount = _eachStepRepairTimer.Duration;
    }

    private void Start()
    {
        Break(_repairStages);
    }

    public void Break(List<RepairStage> repairStages)
    {
        _needRepair = true;

        for (int i = 0; i < repairStages.Count; i++)
        {
            repairStages[i].SetSprites(_wireSprite, _tapeSprite, _screwSprite, _glueSprite, _metalSprite, _chipSprite);

            GameObject newGroup = GameObject.Instantiate(_layoutGroupPrefab, Vector3.zero, Quaternion.identity, _listOfStuffTransform) as GameObject;
            newGroup.transform.localPosition = Vector3.zero;
            LayoutGroup newLayoutGroup = newGroup.GetComponent<LayoutGroup>();
            newLayoutGroup.Setup(repairStages[i]);
            _layoutGroups.Add(newLayoutGroup);
        }

        SetupLayoutGroup();
    }

    private void SetupLayoutGroup()
    {
        if (_layoutGroups.Count > 0)
        {
            for (int i = 0; i < _layoutGroups.Count; i++)
            {
                _layoutGroups[i].SetAlpha(1 - i * _alphaAmount);
            }

            _toolRequired = _layoutGroups[0].GetRepairStageGroup().PickupType;
            _currentAmountItemDone = 0;
            _amountItemToDo = _layoutGroups[0].GetRepairStageGroup().Amount;
        }
        //EVERYTHING IS DONE!
        else
        {
            _needRepair = false;
            if (RepairDoneEvent != null)
                RepairDoneEvent(this);
        }
    }

    public void Repair(float time)
    {
        _slider.gameObject.SetActive(true);

        if (_toolRequired == PickupType.WRENCH || _toolRequired == PickupType.MOP || _toolRequired == PickupType.ANTI_FLAMETHROWER)
        {
            _eachStepRepairTimer.Time += time;

            if (_eachStepRepairTimer.Expired())
            {
                _eachStepRepairTimer.Reset();
                DoStepRepair();
            }
        }
        else
        {
            AddedOneItem();

            if (_currentAmountItemDone == _amountItemToDo)
                NextRepairItem();
        }
    
    }

    private void AddedOneItem()
    {
        _currentAmountItemDone++;

        _slider.value = ((float)_currentAmountItemDone) / ((float)_amountItemToDo);
        _backgroundSliderImage.color = Color.Lerp(_startColor, _endColor, _slider.value);

        _layoutGroups[0].AddedItem(_currentAmountItemDone);
    }

    public bool CanRepair()
    {
        return _needRepair;
    }

    public bool HaveCurrentTool(PickupType typeHolding)
    {
        if (_toolRequired == PickupType.NOTHING)
            return true;

        return typeHolding == _toolRequired;
    }

    private void DoStepRepair()
    {
        _completionTimer.Time += _stepRepairAmount;
        _slider.value = _completionTimer.Ratio();
        _backgroundSliderImage.color = Color.Lerp(_startColor, _endColor, _completionTimer.Ratio());

        if (_completionTimer.Expired())
        {
            StartCoroutine(RemoveSlider());
            NextRepairItem();
        }
    }

    public void NextRepairItem()
    {
        _eachStepRepairTimer.Reset();
        _completionTimer.Reset();
        _slider.gameObject.SetActive(false);
        _backgroundSliderImage.color = _startColor;
        _slider.value = 0;
        _layoutGroups[0].Delete();
        _layoutGroups.Remove(_layoutGroups[0]);
        SetupLayoutGroup();
    }

    private IEnumerator RemoveSlider()
    {
        while (!_showDoneTimer.Expired())
        {
            _showDoneTimer.Time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _slider.gameObject.SetActive(false);
        _showDoneTimer.Reset();
    }
}
