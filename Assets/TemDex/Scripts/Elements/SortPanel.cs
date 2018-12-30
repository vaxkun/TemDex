using UnityEngine;
using System.Collections;

public class SortPanel : PokePanel
{
    [Header("Obj refs")]
    [SerializeField]
    private SortLine[] _sortLines;

    public void UpdateSortCheckboxes(SortMethod method, SortOrder order)
    {
        int methodId = (int) method;
        int orderId = (int) order;

        for (int i = 0; i < _sortLines.Length; i++)
        {
            if (i == methodId)
                _sortLines[i].SetChecked(orderId);
            else _sortLines[i].UncheckAll();
        }
    }
}
