using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiningImage : MonoBehaviour {

	void Update ()
    {
        transform.Rotate(new Vector3(0, 0, -10), 90 * Time.deltaTime);
    }
}
