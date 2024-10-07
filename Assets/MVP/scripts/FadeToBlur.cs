using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class URPFadeToBlur : MonoBehaviour
{
    public Volume postProcessVolume;  // Reference to the post-processing Volume (URP)
    public float blurFadeDuration = 2f;  // Duration of the blur effect

    private DepthOfField depthOfField;

    private void Start()
    {
        // Try to get the Depth of Field override from the volume profile
        if (postProcessVolume.profile.TryGet(out DepthOfField dof))
        {
            depthOfField = dof;
            depthOfField.active = true;  // Ensure Depth of Field is active
            StartCoroutine(FadeToBlurEffect());
        }
        else
        {
            Debug.LogError("Depth of Field override not found in the volume profile.");
        }
    }

    private IEnumerator FadeToBlurEffect()
    {
        float elapsedTime = 0f;
        float initialStart = 10f;  // Initial far focus distance (no blur)
        float finalStart = 0f;     // Final close focus distance (full blur)
        float initialEnd = 10f;    // Initial end distance
        float finalEnd = 0f;       // Final end distance for maximum blur

        while (elapsedTime < blurFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float lerpValue = elapsedTime / blurFadeDuration;

            // Lerp the focus distance to create the blur effect
            depthOfField.gaussianStart.value = Mathf.Lerp(initialStart, finalStart, lerpValue);
            depthOfField.gaussianEnd.value = Mathf.Lerp(initialEnd, finalEnd, lerpValue);

            yield return null;  // Wait until the next frame
        }

        // Ensure the blur is fully applied at the end
        depthOfField.gaussianStart.value = finalStart;
        depthOfField.gaussianEnd.value = finalEnd;
    }
}
