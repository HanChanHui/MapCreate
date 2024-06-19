using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class CustomSlider : MonoBehaviour
{

    private VisualElement m_Root;

    private VisualElement m_Slider;
    private Label m_SliderNumber;

    // Start is called before the first frame update
    void Start()
    {
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        m_Slider = m_Root.Q<Slider>("slider");

    }

    void SliderValueChanged(ChangeEvent<float> value)
    {
        float v = Mathf.Round(value.newValue);
        m_SliderNumber.text = v.ToString();
    }
    
}
