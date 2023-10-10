using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //make camera shake when shooting a projectile
    public IEnumerator Shake (float duration, float magnitude) {

        //gather original camera position 
        Vector3 originalPos = transform.localPosition;
        //time since shake has started
        float elapsed = 0.0f;

        //only do this if the shaking has been going on for less than desired
        while (elapsed < duration) {
            //randomly shake x and y axes according to a certain range with the desired magnitude
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            //place camera on the newly computed coordinates (leave z untouched, no rotation)
            transform.localPosition = new Vector3(x, y, originalPos.z);

            //increase elapsed time
            elapsed += Time.deltaTime;

            //wait until next frame is loaded
            yield return null;
        }

        //place camera at original posiiton
        transform.localPosition = originalPos;
    }
}
