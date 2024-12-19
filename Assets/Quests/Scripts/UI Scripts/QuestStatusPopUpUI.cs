using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestStatusPopUpUI : MonoBehaviour
{
    [SerializeField] private GameObject UIContainer;
    private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI questStatusText;
    [SerializeField] private TextMeshProUGUI questTitleText;
    [SerializeField] private TextMeshProUGUI questObjectiveText;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip questOngoingClip, questCompletedClip, questFailedClip, questClaimedClip;
    public static QuestStatusPopUpUI Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        canvasGroup = UIContainer.GetComponent<CanvasGroup>();
        UIContainer.SetActive(false);
    }
    /// <summary>
    /// For the questStatusIndex: 0 - Quest Ongoing, 1 - Quest Completed, 2 - Quest Failed, 3 - Quest Claimed & Completed
    /// </summary>
    public void ShowQuestPopUpWithFadeIn(QuestsAndDialoguesSO quest, int questStatusIndex)
    {
        UIContainer.SetActive(true);
        canvasGroup.alpha = 0;
        switch (questStatusIndex)
        {
            case 0:
                questStatusText.text = "Quest Ongoing";
                questStatusText.color = Color.blue;
                questObjectiveText.text = quest.questObjective;
                PlayAudio(questOngoingClip);
                break;
            case 1:
                questStatusText.text = "Quest Completed";
                questStatusText.color = Color.green;
                questObjectiveText.text = quest.questObjectiveCompleted;
                PlayAudio(questCompletedClip);
                break;
            case 2:
                questStatusText.text = "Quest Failed";
                questStatusText.color = Color.red;
                questObjectiveText.text = quest.questObjectiveFailed;
                PlayAudio(questFailedClip);
                break;
            case 3:
                questStatusText.text = "Quest Claimed";
                questStatusText.color = Color.yellow;
                questObjectiveText.text = quest.questObjectiveRewardClaimedAndCompleted;
                PlayAudio(questClaimedClip);
                break;
            default:
                break;
        }
        questTitleText.text = quest.questName;
        StartCoroutine(FadeIn());
    }

    private void PlayAudio(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private IEnumerator FadeIn()
    {
        float currentTime = 0f;
        while (currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, currentTime / fadeInDuration);
            yield return null;
        }
        yield return new WaitForSeconds(displayDuration);

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float currentTime = 0f;
        while (currentTime < fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, currentTime / fadeOutDuration);
            yield return null;
        }
    }
}
