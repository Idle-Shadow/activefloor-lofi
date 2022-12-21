using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMaterialSwitcher : MonoBehaviour
{
    [SerializeField] List<Material> _materials;
    [SerializeField] Timer _timer;
    [SerializeField] MeshRenderer _meshRenderer;

    int index = 0;

    void Start()
    {
        _meshRenderer.material = _materials[0];
        StartCoroutine(Next());
    }

    void Update()
    {

    }

    IEnumerator Next()
    {
        while (index < 5)
        {
            yield return new WaitForSeconds(5);
            index++;
            _meshRenderer.material = _materials[index];
        }
    }
}
