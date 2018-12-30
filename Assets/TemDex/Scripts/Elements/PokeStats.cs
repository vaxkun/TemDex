using UnityEngine;
using System.Collections;

public class PokeStats : MonoBehaviour {

	[Header("Obj refs")]
    [SerializeField] private RectTransform _statsBg;
    [SerializeField] private RectTransform[] _stats;

    public void SetStats(params float[] stats)
    {
        float panelSize = _statsBg.sizeDelta.x;

        for (int i = 0; i < _stats.Length; i++)
        {
            _stats[i].sizeDelta = new Vector2(panelSize * Mathf.Clamp01(stats[i]), _stats[i].sizeDelta.y);
        }
    }
    
}
