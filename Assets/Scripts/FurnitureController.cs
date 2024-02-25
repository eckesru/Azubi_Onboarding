using UnityEngine;

public class FurnitureController : MonoBehaviour
{

    [SerializeField] private string furnitureName;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        StartAnimation();
    }

    private void StartAnimation() {

        animator.Play(furnitureName);

    }

}
