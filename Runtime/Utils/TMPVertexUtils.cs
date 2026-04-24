using UnityEngine;

using TMPro;

namespace FlowKit.Utils
{
    internal static class TMPVertexUtils
    {
        public static void ApplySineWave(Vertecies vertecies, TMP_TextInfo textInfo, float time, float frequency, int index, float amplitude)
        {
            var charInfo = textInfo.characterInfo[index];
            if (!charInfo.isVisible)
            {
                return;
            }

            var meshIndex = charInfo.materialReferenceIndex;
            var vertexIndex = charInfo.vertexIndex;
            var verts = textInfo.meshInfo[meshIndex].vertices;

            var wave = GetSineWave(time, frequency, index, amplitude);

            if (vertecies == Vertecies.X)
            {
                verts[vertexIndex + 0].x += wave; // BL
                verts[vertexIndex + 1].x += wave; // TL
                verts[vertexIndex + 2].x += wave; // TR
                verts[vertexIndex + 3].x += wave; // BR
            }
            else
            {
                verts[vertexIndex + 0].y += wave; // BL
                verts[vertexIndex + 1].y += wave; // TL
                verts[vertexIndex + 2].y += wave; // TR
                verts[vertexIndex + 3].y += wave; // BR
            }
        }

        private static float GetSineWave(float time, float frequency, int index, float amplitude) 
            => Mathf.Sin(time * frequency + index * 0.5f) * amplitude;
    }
}
