using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int CurrentPostition { get; private set; } = -1;
    [SerializeField]
    GameObject model;

    [SerializeField]
    Vector3 offset;
    private void Start()
    {
        model.SetActive(false);
    }

    // Start is called before the first frame update
    public void SetPosition(int moveBy)
    {
        if(CurrentPostition == -1)
        {
            if(moveBy != 1)
            {
                return;
            }
        }

        model.SetActive(true);

        CurrentPostition += moveBy;
        CurrentPostition = Mathf.Clamp(CurrentPostition, -2, 99);

        int y = (int)Mathf.Floor(CurrentPostition / 10f);
        int x = CurrentPostition % 10;

        StartCoroutine(MoveToPosition(new Vector3(WrappedPosition(x, y), 0, y) + offset));
    }
    public bool Moving { get; private set; } = false;
    IEnumerator MoveToPosition(Vector3 position)
    {
        Moving = true;
        while(Vector3.Distance(transform.position, position) > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, 10 * Time.smoothDeltaTime);
            yield return null;
        }
        Moving = false;
    }

    public void ApplyModifier(PositionModifier modifier)
    {
        CurrentPostition = modifier.End;
        SetPosition(0);
    }
    int WrappedPosition(int x, int y)
    {
        if (y % 2 != 0)
        {
            return 9 - x;
        }

        return x;
    }

}
