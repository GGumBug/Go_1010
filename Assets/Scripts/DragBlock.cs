using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBlock : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve  curveMovement;

    private float           appearTime = 0.5f;

    public void Setup(Vector3 parentPosition)
    {
        StartCoroutine(OnMoveTo(parentPosition, appearTime));
    }

    private IEnumerator OnMoveTo(Vector3 end, float time)
    {
        Vector3 start   = transform.position;
        float   current  = 0;
        float   percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / appearTime;

            transform.position = Vector3.Lerp(start, end, curveMovement.Evaluate(percent));

            yield return null;
        }
    }
}
