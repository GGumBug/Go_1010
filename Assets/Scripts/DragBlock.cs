using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBlock : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve  curveMovement;
    [SerializeField]
    private AnimationCurve  curveScale;

    private float           appearTime = 0.5f;
    private float           returnTime = 0.1f;

    [field:SerializeField] //프로퍼티를 인스펙터창에서 제어 할 경우 field를 붙인다.
    public Vector2Int       blockCount {private set; get;}

    public void Setup(Vector3 parentPosition)
    {
        StartCoroutine(OnMoveTo(parentPosition, appearTime));
    }

    private void OnMouseDown() {
        StopCoroutine("OnScaleTo");
        StartCoroutine("OnScaleTo", Vector3.one);
    }

    private void OnMouseDrag() {
        //현재 모든 블록의 피봇 위치는 정중앙에 설정 돼 있기 때문에, x값은 그대로 사용하고

        // y값은 y출 블록 개수 절반에 gap만큼 추가한 위치로 사용

        //Camera.main.ScreenToWorldPoint로 Vector3를 구하면 z값이 -10이 나오기 때문에 10을 더해서 z값을 0으로 만들어준다.
        Vector3 gap = new Vector3(0, blockCount.y * 0.5f + 1, 10);
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + gap;
    }

    private void OnMouseUp() {
        StopCoroutine("OnScaleTo");
        StartCoroutine("OnScaleTo", Vector3.one * 0.5f);
        StartCoroutine(OnMoveTo(transform.parent.position, returnTime));
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

    private IEnumerator OnScaleTo(Vector3 end)
    {
        Vector3 start = transform.localScale;
        float current = 0;
        float precent = 0;

        while (precent < 1)
        {
            current += Time.deltaTime;
            precent = current / returnTime;

            transform.localScale = Vector3.Lerp(start, end, curveScale.Evaluate(precent));

            yield return null;
        }
    }
}
