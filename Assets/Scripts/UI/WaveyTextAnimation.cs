using System.Collections;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class WaveyTextAnimation : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private TMP_Vertex[][] _default;

    [SerializeField] private float speed;
    [SerializeField] private float magnitude;
    [SerializeField] private float characterDesync;
    
    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.ForceMeshUpdate();
        _default = _text.textInfo.characterInfo.Select(c => 
            new TMP_Vertex[] {c.vertex_BL, c.vertex_BR, c.vertex_TL, c.vertex_TR}
        ).ToArray();
    }

    private void Update()
    {
        if (_default == null) return;
        
        _text.ForceMeshUpdate();
        
        for (var i = 0; i < _text.textInfo.characterInfo.Length; i++)
        {
            var charInfo = _text.textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;
            
            var vertices = _text.textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            for (var j = 0; j < 4; j++)
            {
                var original = vertices[charInfo.vertexIndex + j];
                vertices[charInfo.vertexIndex + j] = 
                    original + new Vector3(0, math.sin(Time.realtimeSinceStartup * speed + i * characterDesync) * magnitude, 0);
            }
        }

        for (var i = 0; i < _text.textInfo.meshInfo.Length; i++)
        {
            var meshInfo = _text.textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            _text.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
