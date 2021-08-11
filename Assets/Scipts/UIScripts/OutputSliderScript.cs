using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputSliderScript : MonoBehaviour
{
    [SerializeField] private Slider outputSlider=null;
    [SerializeField] private Text outputSliderText=null;
    [SerializeField] private MazeSpawner MazeSpawner=null;
    void Start()
    {
        MazeSpawner.NumberOfOutputs = 0;
        outputSlider.onValueChanged.AddListener((value)=>
            outputSliderText.text=value.ToString("0"));
        
        outputSlider.onValueChanged.AddListener((SliderValue) =>
            MazeSpawner.NumberOfOutputs = Mathf.RoundToInt(SliderValue));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
