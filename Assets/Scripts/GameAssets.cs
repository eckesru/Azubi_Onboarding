using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{

    public static GameAssets Instance;

    [SerializeField] private Transform _chatBubble;
    public Transform chatBubble {get {return _chatBubble;} private set {_chatBubble = value;}}


    void Awake() {
        Instance = this;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
