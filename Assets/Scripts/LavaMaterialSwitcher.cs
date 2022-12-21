using System.Collections.Generic;
using UnityEngine;

public class LavaMaterialSwitcher : MonoBehaviour
{
    [SerializeField] List<Material> _materials;
    [SerializeField] Timer _timer;
    [SerializeField] MeshRenderer _meshRenderer;

    float[] _timeStamps;
    int _index = 0;

    void Start()
    {
        _timeStamps = new float[_materials.Count - 1];
        _timeStamps[0] = 0;
        for (int i = 1; i < _timeStamps.Length; i++)
        {
            float timeStamp = _timer.MaxTime / (_materials.Count - 1) * i;
            _timeStamps[i] = timeStamp;
        }

        _meshRenderer.material = _materials[0];
    }

    void Update()
    {
        if (_timer.TimeRemaining == 0)
        {
            _meshRenderer.material = _materials[_materials.Count - 1];
        }
        else
        {
            for (int i = 0; i < _timeStamps.Length; i++)
            {
                if (_timer.TimeRemaining > _timeStamps[i]) _index = i + 2;
            }
            _meshRenderer.material = _materials[_materials.Count - _index];
        }
    }
}
