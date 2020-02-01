using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LayoutGroup : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] Image _mainImage = null;
    [SerializeField] Image _amountImage = null;
    [SerializeField]  TMP_Text _text = null;

    RepairStage _repairStage;

    public RepairStage GetRepairStageGroup()
    {
        return _repairStage;
    }

    public void SetAlpha(float alpha)
    {
        Color temp = _mainImage.color;
        temp.a = alpha;
        _mainImage.color = temp;
        temp = _text.color;
        temp.a = alpha;
        _text.color = temp;
        temp = _amountImage.color;
        temp.a = alpha;
        _amountImage.color = temp;
    }

    public void Setup(RepairStage repairStage)
    {
        _repairStage = repairStage;

        switch (_repairStage.PickupType)
        {
            case (PickupType.WRENCH):
                {
                    _mainImage.sprite = _repairStage.WrenchSprite;
                    _text.text = string.Empty;
                    _amountImage.gameObject.SetActive(false);
                    break;
                }
            case (PickupType.MOP):
                {
                    _mainImage.sprite = _repairStage.MopSprite;
                    _text.text = string.Empty;
                    _amountImage.gameObject.SetActive(false);
                    break;
                }
            case (PickupType.ANTI_FLAMETHROWER):
                {
                    _mainImage.sprite = _repairStage.AntiFlameSprite;
                    _text.text = string.Empty;
                    _amountImage.gameObject.SetActive(false);
                    break;
                }
            case (PickupType.CHIP):
                {
                    _text.text = "0/" + _repairStage.Amount;
                    _mainImage.sprite = _repairStage.ChipSprite;
                    break;
                }
            case (PickupType.GLUE):
                {
                    _text.text = "0/" + _repairStage.Amount;
                    _mainImage.sprite = _repairStage.GlueSprite;
                    break;
                }
            case (PickupType.METAL):
                {
                    _text.text = "0/" + _repairStage.Amount;
                    _mainImage.sprite = _repairStage.MetalSprite;
                    break;
                }
            case (PickupType.SCREW):
                {
                    _text.text = "0/" + _repairStage.Amount;
                    _mainImage.sprite = _repairStage.ScrewSprite;
                    break;
                }
            case (PickupType.TAPE):
                {
                    _text.text = "0/" + _repairStage.Amount;
                    _mainImage.sprite = _repairStage.TapeSprite;
                    break;
                }
            case (PickupType.WIRE):
                {
                    _text.text = "0/" + _repairStage.Amount;
                    _mainImage.sprite = _repairStage.WireSprite;
                    break;
                }
            default:
                {
                    Debug.LogWarning("Currently not implemented!");
                    break;
                }
        }
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
