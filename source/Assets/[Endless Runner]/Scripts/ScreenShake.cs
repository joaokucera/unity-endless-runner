using System.Collections;
using UnityEngine;

namespace EndlessRunner
{
    [AddComponentMenu("CUSTOM / Screen Shake")]
    public class ScreenShake : Singleton<ScreenShake>
    {
        private const float ShakeDecay = 0.0025f;
        private const float ShakeCoefIntensity = 0.025f;
        private const float ShakeMultiplier = 0.5f;

        public static void Shake()
        {
            Instance.StartCoroutine(Instance.UpdateShake());
        }

        IEnumerator UpdateShake()
        {
            var camera = GlobalVariables.CameraMain;

            var originPosition = camera.transform.position;
            var originRotation = camera.transform.rotation;

            float shakeIntensity = ShakeCoefIntensity;

            while (shakeIntensity > 0)
            {
                camera.transform.position = originPosition + Random.insideUnitSphere * shakeIntensity;

                camera.transform.rotation = new Quaternion
                (
                    originRotation.x + Random.Range(-shakeIntensity, shakeIntensity) * ShakeMultiplier,
                    originRotation.y + Random.Range(-shakeIntensity, shakeIntensity) * ShakeMultiplier,
                    originRotation.z + Random.Range(-shakeIntensity, shakeIntensity) * ShakeMultiplier,
                    originRotation.w + Random.Range(-shakeIntensity, shakeIntensity) * ShakeMultiplier
                );

                shakeIntensity -= ShakeDecay;

                originPosition = camera.transform.position;

                yield return null;
            }

            camera.transform.position = originPosition;
            camera.transform.rotation = originRotation;
        }
    }
}