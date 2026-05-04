using UnityEngine;

[System.Serializable]
public class Axis
{
    public enum AxisType {None,Analog,Digital};
    public AxisType axisType;
    public string axisName;
    public KeyCode positiveKey;
    public KeyCode negativeKey;
    public InputManager.GameButton buttonOverride;
    public float lastState;
}

[System.Serializable]
public class AxisButton
{
    public Axis axis;
    public KeyCode key;
}

[CreateAssetMenu(fileName = "NewInputMap", menuName = "Input")]
public class InputMap : ScriptableObject
{
    //input type
    public InputManager.ControllerType controllerType;
    //buttons
    public KeyCode aInput;
    public KeyCode xInput;
    public KeyCode yInput;
    public KeyCode bInput;
    public KeyCode rbInput;
    public KeyCode rtInput;
    public KeyCode rsInput;
    public KeyCode lbInput;
    public KeyCode ltInput;
    public KeyCode lsInput;
    public KeyCode backInput;
    public KeyCode startInput;
    //axis
    public Axis rHorizontalInput;
    public Axis lHorizontalInput;
    public Axis rVerticalInput;
    public Axis lVerticalInput;
    public Axis menuHorizontalInput;
    public Axis menuVerticalInput;

    public Axis[] axisButtons;
    
}
