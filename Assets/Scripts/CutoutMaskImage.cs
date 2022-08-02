using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutoutMaskImage : Image
{
    private static readonly int StencilComp = Shader.PropertyToID("_StencilComp");

    public override Material materialForRendering
    {
        get
        {
            Material m = new Material(base.materialForRendering);
            m.SetInt(StencilComp, (int)CompareFunction.NotEqual);
            return m;
        }
    }
}
