using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

[RequireComponent(typeof(SVGImage))]
public class SpriteSvgImport : BaseSvgImport
{
    void Start()
    {
        if (svg != "") SetSprite();
    }

    void SetSprite()
    {
        List<VectorUtils.Geometry> geometries = GetGeometries();

        Sprite sprite = VectorUtils.BuildSprite(geometries, pixelsPerUnit, VectorUtils.Alignment.Center, Vector2.zero, 128, flipYAxis);
        GetComponent<SVGImage>().sprite = sprite;
    }

    public void SetNewSvgString(string newString)
    {
        svg = newString;
        SetSprite();
    }
}
