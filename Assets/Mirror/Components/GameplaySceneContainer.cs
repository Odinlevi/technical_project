using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplaySceneContainer", menuName = "GameplaySceneContainer")]
public class GameplaySceneContainer : ScriptableObject
{
    [Scene]
    [SerializeField] private string[] _gameplayScenes;

    private int _currentSceneIndex;

    public string Scene
    {
        get
        {
            var sceneIndex = _currentSceneIndex;
            _currentSceneIndex = (_currentSceneIndex + 1) % _gameplayScenes.Length;
            return _gameplayScenes[sceneIndex];
        }
    }
    
    public void Reset()
    {
        _currentSceneIndex = 0;
    }
}
