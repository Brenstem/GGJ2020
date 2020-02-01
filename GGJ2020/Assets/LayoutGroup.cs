using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutGroup : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] Image _mainImage = null;
    [SerializeField] Image _amountImage = null;
    [SerializeField] Text _text = null;

    RepairStage _repairStage;

    public RepairStage GetRepairStageGroup()
    {
        return _repairStage;
    }

    public void Setup(RepairStage repairStage)
    {
        _repairStage = repairStage;

        switch (_repairStage.PickupType)
        {
            case (PickupType.WRENCH):
                {
                    _text.text = string.Empty;
                    _amountImage.gameObject.SetActive(false);
                    break;
                }
            case (PickupType.MOP):
                {
                    _text.text = string.Empty;
                    _amountImage.gameObject.SetActive(false);
                    break;
                }
            case (PickupType.ANTI_FLAMETHROWER):
                {
                    _text.text = string.Empty;
                    _amountImage.gameObject.SetActive(false);
                    break;
                }
            default:
                {
                    Debug.LogWarning("Currently not implemented!");
                    _text.text = "0/" + _repairStage.Amount;
                    break;
                }
        }
        _mainImage.color = _repairStage.MainColor;
    }

    public void AddedItem(int amountDone)
    {
        _text.text = amountDone + "/" + _repairStage.Amount;
    }

    public void Delete()
    {
        GameObject.Destroy(this.gameObject);
    }
}
