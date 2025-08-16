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

        speedSlider.value = speedSlider.maxValue - difficultyData.stepRate;
        tailSlider.value = difficultyData.startingTailLength;
    }

    public void OnSpeedChanged()
    {
        difficultyData.stepRate = speedSlider.maxValue - speedSlider.value;
    }

    public void OnTailChanged()
    {
        difficultyData.startingTailLength = (int)tailSlider.value;
    }
}
