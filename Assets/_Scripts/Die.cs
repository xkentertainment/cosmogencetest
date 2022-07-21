using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    [SerializeField]
    List<Vector3> orientations;

    [SerializeField]
    Transform camera;

    bool rolled = false;
    private void Start()
    {
        orientations = new List<Vector3>()
        {
            -transform.up,//1
            -transform.right,//2
            -transform.forward,//3
            transform.right,//4
            transform.forward,//5
            transform.up//6
        };
    }
    public void PlayRollAnim()
    {
        rolled = false;

        StartCoroutine(RollingAnimation());

    }
    IEnumerator RollingAnimation()
    {
        while(!rolled)
        {
            transform.Rotate(new Vector3(250, -504, 240) * Time.smoothDeltaTime);
            yield return null;
        }
    }
    public int Roll()
    {
        int result = Random.Range(1, 7);

        transform.up = orientations[result - 1];

        Debug.Log(result);

        rolled = true;
        return result;
    }
}
