using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{

    public static GameAssets Instance;

    [SerializeField] private Transform _chatBubble;
    public Transform chatBubble {get {return _chatBubble;} private set {_chatBubble = value;}}


    // Start is called before the first frame update
    void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
