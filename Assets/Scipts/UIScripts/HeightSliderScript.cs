using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeightSliderScript : MonoBehaviour
{
    [SerializeField] private Slider heightSlider=null;
    [SerializeField] private Text heightSliderText=null;
    [SerializeField] private MazeSpawner MazeSpawner=null;
    void Start()
    {
        heightSlider.onValueChanged.AddListener((value)=>
            heightSliderText.text=value.ToString("0")+"x"+value.ToString());
        
        heightSlider.onValueChanged.AddListener((SliderValue) =>
            MazeSpawner.sizeMaze = Mathf.RoundToInt(SliderValue));
    }

    
    void Update()
    {
        
    }
}
