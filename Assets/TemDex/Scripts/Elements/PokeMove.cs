using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PokeMove : MonoBehaviour {

    public static readonly Vector2 PanelSize = new Vector2(5, 5);
    public const float PanelHeight = 47.5f;

    public UIState TargetState;

    public RectTransform RectTransform { get; private set; }

    [Header("Obj refs")]
    [SerializeField] private Text _moveNameText;
    [SerializeField] private PokeTypeLabel _moveTypeLabel;

    private PrimaryMove _curPrimaryMove;
    private SecondaryMove _curSecondaryMove;

    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void Set(PrimaryMove move)
    {
        _curPrimaryMove = move;

        _moveNameText.text = _curPrimaryMove.Name;
        _moveTypeLabel.SetType(_curPrimaryMove.Type);
    }

    public void Set(SecondaryMove move)
    {
        _curSecondaryMove = move;

        _moveNameText.text = _curSecondaryMove.Name;
        _moveTypeLabel.SetType(_curSecondaryMove.Type);
    }

    public void OnMoveClick()
    {
        switch (TargetState)
        {
            case UIState.QuickMovePage:
                if (_curPrimaryMove == null)
                    return;
                UIManager.Instance.ShowQuickMovePage(_curPrimaryMove);
                break;
            case UIState.ChargeMovePage:
                if (_curSecondaryMove == null)
                    return;
                UIManager.Instance.ShowSecondaryMovePage(_curSecondaryMove);
                break;

        }
        
    }
    
    
}
