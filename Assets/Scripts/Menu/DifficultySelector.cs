using UnityEngine;
using UnityEngine.UI;

public class DifficultySelector : MonoBehaviour
{
    [SerializeField] private DifficultyData difficultyData;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider tailSlider;
    private GameObject optionsPanel;

    private void Start()
    {
        difficultyData = Resources.Load<DifficultyData>("DifficultyData");
        speedSlider = GameObject.Find("SpeedSlider").GetComponent<Slider>();
        tailSlider = GameObject.Find("TailSlider").GetComponent<Slider>();
        optionsPanel = GameObject.Find("OptionsPanel");
        optionsPanel.SetActive(false);

        difficultyData.stepRate = PlayerPrefs.GetFloat("StepRate", difficultyData.stepRate);
        difficultyData.startingTailLength = PlayerPrefs.GetInt("TailLength", difficultyData.startingTailLength);

        speedSlider.value = speedSlider.maxValue - difficultyData.stepRate;
        tailSlider.value = difficultyData.startingTailLength;

        speedSlider.onValueChanged.AddListener(delegate { OnSpeedChanged(); });
        tailSlider.onValueChanged.AddListener(delegate { OnTailChanged(); });
    }

    public void OnSpeedChanged()
    {
        difficultyData.stepRate = speedSlider.maxValue - speedSlider.value + 0.05f;
        PlayerPrefs.SetFloat("StepRate", difficultyData.stepRate);
        PlayerPrefs.Save();
    }

    public void OnTailChanged()
    {
        difficultyData.startingTailLength = (int)tailSlider.value;
        PlayerPrefs.SetInt("TailLength", difficultyData.startingTailLength);
        PlayerPrefs.Save();
    }
}
