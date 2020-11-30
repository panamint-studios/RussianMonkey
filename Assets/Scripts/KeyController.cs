using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public AnimationCurve floatingCurve;
    private float animationDuration = 5f;
    private float animationFrame;
    private Vector3 initialPos;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        PlayAnimation();
    }

    void PlayAnimation()
    {
        animationFrame += Time.deltaTime;
        if(animationFrame > animationDuration)
        {
            animationFrame = 0;
        }

        float t = animationFrame / animationDuration;
        Vector3 animOffset = new Vector3(0, floatingCurve.Evaluate(t), 0);
        transform.position = initialPos + animOffset;
    }
}
