using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SetTimeScaleSO_", menuName = "ScriptableObjects/UnityEvent Commands/Set Time Scale")]
public class SetTimeScaleSO : ScriptableObject
{
    [SerializeField] private LeanTweenType fadeInTweenType = LeanTweenType.easeInCirc;

    private int currentTweenId;

    public void SetTimeScale(float newTimeScale)
    {
        LeanTween.cancel(currentTweenId);

        Time.timeScale = newTimeScale;

    }
    public void FadeInTimeScale(float duration)
    {
        LeanTween.cancel(currentTweenId);

        currentTweenId = LeanTween.value(Time.timeScale, 1, duration)
            .setIgnoreTimeScale(true)
            .setEase(fadeInTweenType)
            .setOnUpdate((float value) =>
            {
                Time.timeScale = value;

            })
            .setOnComplete(() =>
            {
                Time.timeScale = 1f;
            })
            .uniqueId;
        

    }
}
