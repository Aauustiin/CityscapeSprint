using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI
{
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
}
