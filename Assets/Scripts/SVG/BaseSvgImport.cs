using System.Collections.Generic;
using System.IO;
using Unity.VectorGraphics;
using UnityEngine;

public class BaseSvgImport : MonoBehaviour
{
    public string svg = "";
    public int pixelsPerUnit;
    public bool flipYAxis;

    protected List<VectorUtils.Geometry> GetGeometries()
    {
        using var textReader = new StringReader(svg);
        var sceneInfo = SVGParser.ImportSVG(textReader);

        return VectorUtils.TessellateScene(sceneInfo.Scene, new VectorUtils.TessellationOptions
        {
            StepDistance = 10,
            SamplingStepSize = 100,
            MaxCordDeviation = 0.5f,
            MaxTanAngleDeviation = 0.1f
        });
    }
}
