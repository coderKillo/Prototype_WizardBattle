using UnityEngine;

public class LayerMaskUtils
{
    static public bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((mask.value & (1 << obj.layer)) > 0);
    }
}
