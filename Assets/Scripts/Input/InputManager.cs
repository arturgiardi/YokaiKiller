using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class InputManager : MonoBehaviour {

    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    Vector2 Vibration;

    public enum ControllerType {xInput, keyboard, joypad, sony};
    public enum GameButton {None, A, B, X, Y, RB, RT, RS, LB, LT, LS, Back, Start};
    public ControllerType currentController = ControllerType.keyboard;
    public float axisSensitivity;
    public float mouseSensibility;

    public static InputManager singleton;

    public InputMap[] maps;

    public delegate void InputAction();
    public static event InputAction OnPressA;
    public static event InputAction OnPressX;
    public static event InputAction OnPressY;
    public static event InputAction OnPressB;
    public static event InputAction OnPressRB;
    public static event InputAction OnPressRT;
    public static event InputAction OnPressRS;
    public static event InputAction OnPressLB;
    public static event InputAction OnPressLT;
    public static event InputAction OnPressLS;
    public static event InputAction OnPressBack;
    public static event InputAction OnPressStart;

    public delegate void DirectionalInputAction(Vector3 inputDirection);

    public static event InputAction OnReleaseA;
    public static event InputAction OnReleaseX;
    public static event InputAction OnReleaseY;
    public static event InputAction OnReleaseB;
    public static event InputAction OnReleaseRB;
    public static event InputAction OnReleaseRT;
    public static event InputAction OnReleaseRS;
    public static event InputAction OnReleaseLB;
    public static event InputAction OnReleaseLT;
    public static event InputAction OnReleaseLS;
    public static event InputAction OnReleaseBack;
    public static event InputAction OnReleaseStart;

    public delegate void MenuAxisInput(float value);
    public static event MenuAxisInput OnMoveVertical;
    public static event MenuAxisInput OnMoveHorizontal;
    public static event MenuAxisInput OnRestVertical;
    public static event MenuAxisInput OnRestHorizontal;

    //public int[] stdInputMap = NewKeymap();
    //public int[] xInputMap = NewKeymap();

    //public int[] testInputMap = new int[]{10};

    public float deadZone;

    /*
    public string[] aInput;
    public string[] xInput;
    public string[] yInput;
    public string[] bInput;
    public string[] rbInput;
    public string[] rtInput;
    public string[] rsInput;
    public string[] lbInput;
    public string[] ltInput;
    public string[] lsInput;
    public string[] backInput;
    public string[] startInput;
    */

    //AXIS
    public string[] rHorizontal;
    public string[] rVertical;
    public string[] lHorizontal;
    public string[] lVertical;

    //MENU AXIS
    public string[] menuHorizontal;
    public string[] menuVertical;

    public static Vector3 rAxis;
    public static Vector3 lAxis;
    //public Vector3 plaxis;
    public static Vector3 mouseAxis;

    bool aDown;
    bool xDown;
    bool yDown;
    bool bDown;
    bool rbDown;
    bool rtDown;
    bool rsDown;
    bool lbDown;
    bool ltDown;
    bool lsDown;
    bool backDown;
    bool startDown;

    bool movingHorizontal;
    bool movingVertical;

    public bool readingInput = true;

    void Awake()
    {
        if (singleton == null)
        {
            UnsetAll();
            singleton = this;
            //DontDestroyOnLoad(gameObject);
        }  
        else
            Destroy(this);


        

        //xInputMap = NewKeymap();
        //stdInputMap = NewKeymap();
    }

    void OnDisable()
    {
        if(singleton == this)
        {
            singleton = null;
            UnsetAll();
        }
    }

    void Update()
    {
        lAxis = Vector3.zero;
        rAxis = Vector3.zero;
        mouseAxis = Vector3.zero;
        
        if (singleton != this)
            return;
        
        if(!readingInput)
            return;

        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }
        prevState = state;
        state = GamePad.GetState(playerIndex);

        //StandardInputEvaluation(); -> Deprecated

        foreach(InputMap map in maps)
        {
            EvaluateInputMap(map);
        }

        //Mouse Axis
        mouseAxis = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * mouseSensibility;

        if (prevState.IsConnected && !state.IsConnected)
        {
            SetCurrentController(ControllerType.keyboard);
        }
        if(state.IsConnected)
        {
            //xInputEvaluation();
        }

    }

    bool inputDownCheck(string[] inputName)
    {
        if(inputName.Length < 1)
            return false;
        if(!state.IsConnected)
        {
            for(int i=0; i<2; i++)
            {
                if (Input.GetButton(inputName[i]) || Input.GetAxisRaw(inputName[i]) > deadZone)
                {
                    if(i == 0)
                    {
                        SetCurrentController(ControllerType.keyboard);
                    }
                    else
                    {
                        SetCurrentController(ControllerType.joypad);
                    }
                    return true;
                }
            }
            return false;
        }
        else
        {
            if (Input.GetButton(inputName[0]) || Input.GetAxisRaw(inputName[0]) > deadZone)
            {
                SetCurrentController(ControllerType.keyboard);
                return true;
            }
            return false;
        }
    }

    bool keyDownCheck(KeyCode key)
    {
        if(key == KeyCode.None)
            return false;
        if (Input.GetKey(key))
        {
            SetCurrentController(ControllerType.keyboard);
            return true;
        }
        else
            return false;
    }

    
    float AxisValue (string[] inputName)
    {
        if(inputName.Length == 0)
            return 0;
        float highestValue = 0;
        string choosenInput = "";

        foreach (string name in inputName)
        {
            float thisInput = Input.GetAxisRaw(name);
            float currentAbsValue = Mathf.Abs(thisInput);
            if (currentAbsValue > highestValue)
            {
                    choosenInput = name;
                    highestValue = currentAbsValue;
            }

        }
        if(string.IsNullOrEmpty(choosenInput))
            return 0;
        return Input.GetAxisRaw(choosenInput);

    }

    bool AxisCheck(string[] inputName)
    {
        foreach (string name in inputName)
        {
            if (Mathf.Abs(Input.GetAxisRaw(name)) > deadZone)
                return true;
        }
        return false;
    }

    void EvaluateInputMap(InputMap iMap)
    {

        Vector3 rightAxisInput = new Vector3(0,0,0);
        
        //R Axis
        // Horizontal
        if (iMap.rHorizontalInput.axisType == Axis.AxisType.Analog)
        {
            float axisInput = Input.GetAxisRaw(iMap.rHorizontalInput.axisName);

            if(Mathf.Abs(axisInput) > deadZone)
            {
                rightAxisInput.x = axisInput;
            }   
        }
        else
        {
            if(Input.GetKey(iMap.rHorizontalInput.positiveKey))
            {
                rightAxisInput.x = iMap.rHorizontalInput.lastState = 1;
            }
            else if (Input.GetKey(iMap.rHorizontalInput.negativeKey))
            {
                rightAxisInput.x = iMap.rHorizontalInput.lastState = -1;
            }
        }

        // Vertical
        if (iMap.rVerticalInput.axisType == Axis.AxisType.Analog)
        {
            rightAxisInput.y = Input.GetAxis(iMap.rVerticalInput.axisName);
        }
        else
        {
            if (iMap.rVerticalInput.lastState == 1)
            {
                if (Input.GetKeyDown(iMap.rVerticalInput.negativeKey))
                {
                    iMap.rVerticalInput.lastState = -1;
                }
            }
            else if (iMap.rVerticalInput.lastState == -1)
            {
                if (Input.GetKeyDown(iMap.rVerticalInput.positiveKey))
                {
                    iMap.rVerticalInput.lastState = 1;
                }
            }
            else
            {
                if (Input.GetKeyDown(iMap.rVerticalInput.positiveKey))
                {
                    iMap.rVerticalInput.lastState = 1;
                }
                else if (Input.GetKeyDown(iMap.rVerticalInput.negativeKey))
                {
                    iMap.rVerticalInput.lastState = -1;
                }
                else
                {
                    iMap.rVerticalInput.lastState = 0;
                }
            }

            rightAxisInput.y = iMap.rVerticalInput.lastState;

        }

        rightAxisInput = rightAxisInput.magnitude > deadZone ? GetExponentialAxis(rightAxisInput * axisSensitivity) : Vector3.zero;

        Vector3 leftAxisInput = new Vector3(0,0,0);
        
        //L Axis
        // Horizontal
        if (iMap.lHorizontalInput.axisType == Axis.AxisType.Analog)
        {
            float axisInput = Input.GetAxisRaw(iMap.lHorizontalInput.axisName);

            if(Mathf.Abs(axisInput) > deadZone)
            {
                leftAxisInput.x = axisInput;
            }   
        }
        else
        {
            if(Input.GetKey(iMap.lHorizontalInput.positiveKey))
            {
                leftAxisInput.x = iMap.lHorizontalInput.lastState = 1;
            }
            else if (Input.GetKey(iMap.lHorizontalInput.negativeKey))
            {
                leftAxisInput.x = iMap.lHorizontalInput.lastState = -1;
            }
        }

        leftAxisInput = leftAxisInput.magnitude > deadZone ? GetExponentialAxis(leftAxisInput * axisSensitivity) : Vector3.zero;
        lAxis += leftAxisInput;

        // //L Axis
        // Vector3 stdLAxis = new Vector3(AxisValue(lHorizontal), AxisValue(lVertical), 0);
        // stdLAxis = stdLAxis.magnitude > deadZone ? stdLAxis : Vector3.zero;

        // if ((stdLAxis != Vector3.zero))
        // {
        //     SetCurrentController(ControllerType.keyboard);
        //     lAxis += stdLAxis;
        //     rAxis += stdLAxis;
        // }

        //Axis Buttons
        foreach(Axis axis in iMap.axisButtons)
        {
            if(axis.buttonOverride != GameButton.None)
            {
                if(axis.lastState != 1 && Mathf.Abs(Input.GetAxis(axis.axisName)) > deadZone)
                {
                    axis.lastState = 1;
                    switch(axis.buttonOverride)
                    {
                        case GameButton.A:
                            OnPressA();
                        break;
                        case GameButton.B:
                            OnPressB();
                        break;
                        case GameButton.X:
                            OnPressX();
                        break;
                        case GameButton.Y:
                            OnPressY();
                        break;
                        case GameButton.RB:
                            OnPressRB();
                        break;
                        case GameButton.RS:
                            OnPressRS();
                        break;
                        case GameButton.RT:
                            OnPressRT();
                        break;
                        case GameButton.LB:
                            OnPressLB();
                        break;
                        case GameButton.LS:
                            OnPressLS();
                        break;
                        case GameButton.LT:
                            OnPressLT();
                        break;
                        case GameButton.Back:
                            OnPressBack();
                        break;
                        case GameButton.Start:
                            OnPressStart();
                        break;
                    }
                }
                if(axis.lastState != 0 && Mathf.Abs(Input.GetAxis(axis.axisName)) <= deadZone)
                {
                    axis.lastState = 0;
                    switch(axis.buttonOverride)
                    {
                        case GameButton.A:
                            OnReleaseA();
                        break;
                        case GameButton.B:
                            OnReleaseB();
                        break;
                        case GameButton.X:
                            OnReleaseX();
                        break;
                        case GameButton.Y:
                            OnReleaseY();
                        break;
                        case GameButton.RB:
                            OnReleaseRB();
                        break;
                        case GameButton.RS:
                            OnReleaseRS();
                        break;
                        case GameButton.RT:
                            OnReleaseRT();
                        break;
                        case GameButton.LB:
                            OnReleaseLB();
                        break;
                        case GameButton.LS:
                            OnReleaseLS();
                        break;
                        case GameButton.LT:
                            OnReleaseLT();
                        break;
                        case GameButton.Back:
                            OnReleaseBack();
                        break;
                        case GameButton.Start:
                            OnReleaseStart();
                        break;
                    }
                }
            }
        }

        //A

        if (!aDown && Input.GetKeyDown(iMap.aInput))
        {
            aDown = true;
            if(OnPressA != null)
                OnPressA();
            return;
        }
        else if (aDown && Input.GetKeyUp(iMap.aInput))
        {
            aDown = false;
            if(OnReleaseA != null)
                OnReleaseA();
            return;
        }

        //X

        if (!xDown && Input.GetKeyDown(iMap.xInput))
        {
            xDown = true;
            //CallInputEvent(stdInputMap[2]);
            if(OnPressX != null)
                OnPressX();
            return;
        }
        else if (xDown && Input.GetKeyUp(iMap.xInput))
        {
            xDown = false;
            //CallInputEvent(stdInputMap[3]);
            if(OnReleaseX != null)
                OnReleaseX();
            return;
        }

        //Y

        if (!yDown && Input.GetKeyDown(iMap.yInput))
        {
            yDown = true;
            //CallInputEvent(stdInputMap[4]);
            if(OnPressY != null)
                OnPressY();
        }
        else if (yDown && Input.GetKeyUp(iMap.yInput))
        {
            yDown = false;
            //CallInputEvent(stdInputMap[5]);
            if(OnReleaseY != null)
                OnReleaseY();
        }

        //B

        if (!bDown && Input.GetKeyDown(iMap.bInput))
        {
            bDown = true;
            //CallInputEvent(stdInputMap[6]);
            if(OnPressB != null)
                OnPressB();
        }
        else if (bDown && Input.GetKeyUp(iMap.bInput))
        {
            bDown = false;
            //CallInputEvent(stdInputMap[7]);
            if(OnReleaseB != null)
                OnReleaseB();
        }

        //RB

        if (!rbDown && Input.GetKeyDown(iMap.rbInput))
        {
            rbDown = true;
            //CallInputEvent(stdInputMap[8]);
            if(OnPressRB != null)
                OnPressRB();
        }
        else if (rbDown && Input.GetKeyUp(iMap.rbInput))
        {
            rbDown = false;
            //CallInputEvent(stdInputMap[9]);
            if(OnReleaseRB != null)
                OnReleaseRB();
        }

        //RT

        if (!rtDown && Input.GetKeyDown(iMap.rtInput))
        {
            rtDown = true;
            //CallInputEvent(stdInputMap[10]);
            if(OnPressRT != null)
                OnPressRT();
        }
        else if (rtDown && Input.GetKeyUp(iMap.rtInput))
        {
            rtDown = false;
            //CallInputEvent(stdInputMap[11]);
            if(OnReleaseRT != null)
                OnReleaseRT();
        }

        //RS

        if (!rsDown && Input.GetKeyDown(iMap.rsInput))
        {
            rsDown = true;
            //CallInputEvent(stdInputMap[12]);
            if(OnPressRS != null)
                OnPressRS();
        }
        else if (rsDown && Input.GetKeyUp(iMap.rsInput))
        {
            rsDown = false;
            //CallInputEvent(stdInputMap[13]);
            if(OnReleaseRS != null)
                OnReleaseRS();
        }

        //LB

        if (!lbDown && Input.GetKeyDown(iMap.lbInput))
        {
            lbDown = true;
            //CallInputEvent(stdInputMap[14]);
            if(OnPressLB != null)
                OnPressLB();
        }
        else if (lbDown && Input.GetKeyUp(iMap.lbInput))
        {
            lbDown = false;
            //CallInputEvent(stdInputMap[15]);
            if(OnReleaseLB != null)
                OnReleaseLB();
        }

        //LT

        if (!ltDown && Input.GetKeyDown(iMap.ltInput))
        {
            ltDown = true;
            //CallInputEvent(stdInputMap[16]);
            if(OnPressLT != null)
                OnPressLT();
        }
        else if (ltDown && Input.GetKeyUp(iMap.ltInput))
        {
            ltDown = false;
            //CallInputEvent(stdInputMap[17]);
            if(OnReleaseLT != null)
                OnReleaseLT();
        }

        //LS

        if (!lsDown && Input.GetKeyDown(iMap.lsInput))
        {
            lsDown = true;
            //CallInputEvent(stdInputMap[18]);
            if(OnPressLS != null)
                OnPressLS();
        }
        else if (lsDown && Input.GetKeyUp(iMap.lsInput))
        {
            lsDown = false;
            //CallInputEvent(stdInputMap[19]);
            if(OnReleaseLS != null)
                OnReleaseLS();
        }

        //Back

        if (!backDown && Input.GetKeyDown(iMap.backInput))
        {
            backDown = true;
            //CallInputEvent(stdInputMap[20]);
            if(OnPressBack != null)
                OnPressBack();
        }
        else if (backDown && Input.GetKeyUp(iMap.backInput))
        {
            backDown = false;
            //CallInputEvent(stdInputMap[21]);
            if(OnReleaseBack != null)
                OnReleaseBack();
        }

        //Start

        if (!startDown && Input.GetKeyDown(iMap.startInput))
        {
            startDown = true;
            //CallInputEvent(stdInputMap[22]);
            if(OnPressStart != null)
                OnPressStart();
        }
        else if (startDown && Input.GetKeyUp(iMap.startInput))
        {
            startDown = false;
            //CallInputEvent(stdInputMap[23]);
            if(OnReleaseStart != null)
                OnReleaseStart();
        }

        //H Menu Axis
        if (!movingHorizontal && AxisCheck(menuHorizontal))
        {
            movingHorizontal = true;
            if (OnMoveHorizontal != null)
                OnMoveHorizontal(AxisValue(menuHorizontal));
        }
        else if (movingHorizontal && !AxisCheck(menuHorizontal))
        {
            movingHorizontal = false;
            if (OnRestHorizontal != null)
                OnRestHorizontal(0);
        }

        //V Menu Axis
        if (!movingVertical && AxisCheck(menuVertical))
        {
            movingVertical = true;
            if (OnMoveVertical != null)
                OnMoveVertical(AxisValue(menuVertical));
        }
        else if (movingVertical && !AxisCheck(menuVertical))
        {
            movingVertical = false;
            if (OnRestVertical != null)
                OnRestVertical(0);
        }
    }

    /*
    void StandardInputEvaluation()
    {

        //R Axis
        Vector3 stdRAxis = new Vector3(AxisValue(rHorizontal), AxisValue(rVertical), 0);
        stdRAxis = stdRAxis.magnitude > deadZone ? GetExponentialAxis(stdRAxis*axisSensitivity) : Vector3.zero;

        //L Axis
        Vector3 stdLAxis = new Vector3(AxisValue(lHorizontal), AxisValue(lVertical), 0);
        stdLAxis = stdLAxis.magnitude > deadZone ? stdLAxis : Vector3.zero;

        if((stdLAxis!= Vector3.zero) || (stdLAxis != Vector3.zero))
        {
            SetCurrentController(ControllerType.keyboard);
            lAxis += stdLAxis;
            rAxis += stdLAxis;
        }

        //A

        if (!aDown && inputDownCheck(aInput))
        {
            aDown = true;
            //if (stdInputMap[0] != null)
            CallInputEvent(stdInputMap[0]);
        }
        else if (aDown && !inputDownCheck(aInput))
        {
            aDown = false;
            CallInputEvent(stdInputMap[1]);
        }

        //X

        if (!xDown && inputDownCheck(xInput))
        {
            xDown = true;
            CallInputEvent(stdInputMap[2]);
        }
        else if (xDown && !inputDownCheck(xInput))
        {
            xDown = false;
            CallInputEvent(stdInputMap[3]);
        }

        //Y

        if (!yDown && inputDownCheck(yInput))
        {
            yDown = true;
            CallInputEvent(stdInputMap[4]);
        }
        else if (yDown && !inputDownCheck(yInput))
        {
            yDown = false;
            CallInputEvent(stdInputMap[5]);
        }

        //B

        if (!bDown && inputDownCheck(bInput))
        {
            bDown = true;
            CallInputEvent(stdInputMap[6]);
        }
        else if (bDown && !inputDownCheck(bInput))
        {
            bDown = false;
            CallInputEvent(stdInputMap[7]);
        }

        //RB

        if (!rbDown && inputDownCheck(rbInput))
        {
            rbDown = true;
            CallInputEvent(stdInputMap[8]);
        }
        else if (rbDown && !inputDownCheck(rbInput))
        {
            rbDown = false;
            CallInputEvent(stdInputMap[9]);
        }

        //RT

        if (!rtDown && inputDownCheck(rtInput))
        {
            rtDown = true;
            CallInputEvent(stdInputMap[10]);

        }
        else if (rtDown && !inputDownCheck(rtInput))
        {
            rtDown = false;
            CallInputEvent(stdInputMap[11]);
        }

        //RS

        if (!rsDown && inputDownCheck(rsInput))
        {
            rsDown = true;
            CallInputEvent(stdInputMap[12]);
        }
        else if (rsDown && !inputDownCheck(rsInput))
        {
            rsDown = false;
            CallInputEvent(stdInputMap[13]);
        }

        //LB

        if (!lbDown && inputDownCheck(lbInput))
        {
            lbDown = true;
            CallInputEvent(stdInputMap[14]);
        }
        else if (lbDown && !inputDownCheck(lbInput))
        {
            lbDown = false;
            CallInputEvent(stdInputMap[15]);
        }

        //LT

        if (!ltDown && inputDownCheck(ltInput))
        {
            ltDown = true;
            CallInputEvent(stdInputMap[16]);
        }
        else if (ltDown && !inputDownCheck(ltInput))
        {
            ltDown = false;
            CallInputEvent(stdInputMap[17]);
        }

        //LS

        if (!lsDown && inputDownCheck(lsInput))
        {
            lsDown = true;
            CallInputEvent(stdInputMap[18]);
        }
        else if (lsDown && !inputDownCheck(lsInput))
        {
            lsDown = false;
            CallInputEvent(stdInputMap[19]);
        }

        //Back

        if (!backDown && inputDownCheck(backInput))
        {
            backDown = true;
            CallInputEvent(stdInputMap[20]);
        }
        else if (backDown && !inputDownCheck(backInput))
        {
            backDown = false;
            CallInputEvent(stdInputMap[21]);
        }

        //Start

        if (!startDown && inputDownCheck(startInput))
        {
            startDown = true;
            CallInputEvent(stdInputMap[22]);
        }
        else if (startDown && !inputDownCheck(startInput))
        {
            startDown = false;
            CallInputEvent(stdInputMap[23]);
        }

        //Mouse Axis
        mouseAxis = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * mouseSensibility;

        //H Menu Axis
        if (!movingHorizontal && AxisCheck(menuHorizontal))
        {
            movingHorizontal = true;
            if (OnMoveHorizontal != null)
                OnMoveHorizontal(AxisValue(menuHorizontal));
        }
        else if (movingHorizontal && !AxisCheck(menuHorizontal))
        {
            movingHorizontal = false;
            if (OnRestHorizontal != null)
                OnRestHorizontal(0);
        }
        
         //V Menu Axis
        if (!movingVertical && AxisCheck(menuVertical))
        {
            movingVertical = true;
            if (OnMoveVertical != null)
                OnMoveVertical(AxisValue(menuVertical));
        }
        else if (movingVertical && !AxisCheck(menuVertical))
        {
            movingVertical = false;
            if (OnRestVertical != null)
                OnRestVertical(0);
        }
    }
    */

    /*
    void xInputEvaluation()
    {
        //L Axis
        Vector3 xLAxis = new Vector3(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, 0);

        if(state.DPad.Up == ButtonState.Pressed)
        {
            xLAxis.y = 1;
        }
        else if(state.DPad.Down == ButtonState.Pressed)
        {
            xLAxis.y = -1;
        }
        if(state.DPad.Right == ButtonState.Pressed)
        {
            xLAxis.x = 1;
        }
        else if(state.DPad.Left == ButtonState.Pressed)
        {
            xLAxis.x = -1;
        }
        xLAxis = Vector3.ClampMagnitude(xLAxis, 1);
        xLAxis = xLAxis.magnitude > deadZone ? xLAxis : Vector3.zero;
        lAxis += xLAxis;
        

        Vector3 prevXLAxis = new Vector3(prevState.ThumbSticks.Left.X, prevState.ThumbSticks.Left.Y, 0);
        if(prevState.DPad.Up == ButtonState.Pressed)
        {
            prevXLAxis.y = 1;
        }
        else if(prevState.DPad.Down == ButtonState.Pressed)
        {
            prevXLAxis.y = -1;
        }
        if(prevState.DPad.Right == ButtonState.Pressed)
        {
            prevXLAxis.x = 1;
        }
        else if(prevState.DPad.Left == ButtonState.Pressed)
        {
            prevXLAxis.x = -1;
        }
        prevXLAxis = Vector3.ClampMagnitude(prevXLAxis, 1);
        prevXLAxis = prevXLAxis.magnitude > deadZone ? prevXLAxis : Vector3.zero;
        
        if(currentController != ControllerType.xInput)
        {
            if(xLAxis != prevXLAxis)
                SetCurrentController(ControllerType.xInput);
        }
        
        if(Mathf.Abs(xLAxis.y) > Mathf.Abs(xLAxis.x))
        {
            if(prevXLAxis.y == 0 && xLAxis.y != 0)
            {
                if(OnMoveVertical != null)
                    OnMoveVertical((xLAxis.y));
            }
        } 
        if(prevXLAxis.y != 0 && xLAxis.y == 0)
        {
            if(OnRestVertical != null)
                OnRestVertical(0);
        }
        if(Mathf.Abs(xLAxis.x) > Mathf.Abs(xLAxis.y))
        {
            if(prevXLAxis.x == 0 && xLAxis.x != 0)
            {
                if(OnMoveHorizontal != null)
                    OnMoveHorizontal((xLAxis.x));
            }
        }
        if(prevXLAxis.x != 0 && xLAxis.x == 0)
        {
            if(OnRestHorizontal != null)
                OnRestHorizontal(0);
        }

        //L PAD
        


        //if(lAxis != new Vector3(prevState.ThumbSticks.Left.X, prevState.ThumbSticks.Left.Y, 0))

        // -> A
        if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[0]);
        }
        else if (prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[1]);
        }

        // -> X
        if (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[2]);
        }
        else if (prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Released)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[3]);
        }

        // -> Y
        if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[4]);
        }
        else if (prevState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Released)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[5]);
        }

        // -> B
        if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[6]);
        }
        else if (prevState.Buttons.B == ButtonState.Pressed && state.Buttons.B == ButtonState.Released)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[7]);
        }

        // -> RB
        if (prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[8]);
        }
        else if (prevState.Buttons.RightShoulder == ButtonState.Pressed && state.Buttons.RightShoulder == ButtonState.Released)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[9]);
        }

        // -> RT
        if (prevState.Triggers.Right < 0.5f && state.Triggers.Right >= 0.5f)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[010]);
        }
        else if (prevState.Triggers.Right >= 0.5f && state.Triggers.Right < 0.5f)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[11]);
        }

        // -> RS
        if (prevState.Buttons.RightStick == ButtonState.Released && state.Buttons.RightStick == ButtonState.Pressed)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[12]);
        }
        else if (prevState.Buttons.RightStick == ButtonState.Pressed && state.Buttons.RightStick == ButtonState.Released)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[13]);
        }


        // -> LB
        if (prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[14]);
        }
        else if (prevState.Buttons.LeftShoulder == ButtonState.Pressed && state.Buttons.LeftShoulder == ButtonState.Released)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[15]);
        }

        // -> LT
        if (prevState.Triggers.Left < 0.5f && state.Triggers.Left >= 0.5f)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[16]);
        }
        else if (prevState.Triggers.Left >= 0.5f && state.Triggers.Left < 0.5f)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[17]);
        }

        // -> LS
        if (prevState.Buttons.LeftStick == ButtonState.Released && state.Buttons.LeftStick == ButtonState.Pressed)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[18]);
        }
        else if (prevState.Buttons.LeftStick == ButtonState.Pressed && state.Buttons.LeftStick == ButtonState.Released)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[19]);
        }           

        // -> Back
        if (prevState.Buttons.Back == ButtonState.Released && state.Buttons.Back == ButtonState.Pressed)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[20]);
        }
        else if (prevState.Buttons.Back == ButtonState.Pressed && state.Buttons.Back == ButtonState.Released)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[21]);
        }  

        // -> STart
        if (prevState.Buttons.Start == ButtonState.Released && state.Buttons.Start == ButtonState.Pressed)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[22]);
        }
        else if (prevState.Buttons.Start == ButtonState.Pressed && state.Buttons.Start == ButtonState.Released)
        {
            SetCurrentController(ControllerType.xInput);
            CallInputEvent(xInputMap[23]);
        } 


    }
    */

    /*
    private static int[] NewKeymap()
    {
        return new int[]
        {
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10,
            11,
            12,
            13,
            14,
            15,
            16,
            17,
            18,
            19,
            20,
            21,
            22,
            23
        };
    }
    */

    void SetCurrentController(ControllerType controller)
    {
        if(currentController == controller)
            return;
        else
        {
            currentController = controller;
            Debug.LogWarning("Controller changed to: " + controller.ToString());
        }
    }

    /*
    void CallInputEvent(int eventIndex)
    {
        //Debug.Log(eventIndex);
        switch (eventIndex)
        {
            case 0:
                if(OnPressA != null)
                    OnPressA();
                break;
            case 1:
                if(OnReleaseA != null)
                    OnReleaseA();
                break;
            case 2:
                if(OnPressX != null)
                    OnPressX();
                break;
            case 3:
                if(OnReleaseX != null)
                    OnReleaseX();
                break;
            case 4:
                if(OnPressY != null)
                    OnPressY();
                break;
            case 5:
                if(OnReleaseY != null)
                    OnReleaseY();
                break;
            case 6:
                if(OnPressB != null)
                    OnPressB();
                break;
            case 7:
                if(OnReleaseB != null)
                    OnReleaseB();
                break;
            case 8:
                if(OnPressRB != null)
                    OnReleaseRB();
                break;
            case 9:
                if(OnReleaseRB != null)
                    OnReleaseRB();
                break;
            case 10:
                if(OnPressRT != null)
                    OnPressRT();
                break;
            case 11:
                if(OnReleaseRT != null)
                    OnReleaseRT();
                break;
            case 12:
                if(OnPressRS != null)
                    OnPressRS();
                break;
            case 13:
                if(OnReleaseRS != null)
                    OnReleaseRS();
                break;
            case 14:
                if(OnPressLB != null)
                    OnPressLB();
                break;
            case 15:
                if(OnReleaseLB != null)
                    OnReleaseLB();
                break;
            case 16:
                if(OnPressLT != null)
                    OnPressLT();
                break;
            case 17:
                if(OnReleaseLT != null)
                    OnReleaseLT();
                break;
            case 18:
                if(OnPressLS != null)
                    OnPressLS();
                break;
            case 19:
                if(OnReleaseLS != null)
                    OnReleaseLS();
                break;
            case 20:
                if(OnPressBack != null)
                    OnPressBack();
                break;
            case 21:
                if(OnReleaseBack != null)
                    OnReleaseBack();
                break;
            case 22:
                if(OnPressStart != null)
                    OnPressStart();
                break;
            case 23:
                if(OnReleaseStart != null)
                    OnReleaseStart();
                break;
            default:
                return;
        }
    }
    */

    Vector3 GetExponentialAxis(Vector3 axis)
    {
        return new Vector3(axis.x*Mathf.Abs(axis.x), axis.y*Mathf.Abs(axis.y), axis.z*Mathf.Abs(axis.z));
    }
        
    void UnsetAll()
    {
        OnPressA = null;
        OnPressX = null;
        OnPressY = null;
        OnPressB = null;
        OnPressRB = null;
        OnPressRT = null;
        OnPressRS = null;
        OnPressLB = null;
        OnPressLT = null;
        OnPressLS = null;
        OnPressBack = null;
        OnPressStart = null;

        OnReleaseA = null;
        OnReleaseX = null;
        OnReleaseY = null;
        OnReleaseB = null;
        OnReleaseRB = null;
        OnReleaseRT = null;
        OnReleaseRS = null;
        OnReleaseLB = null;
        OnReleaseLT = null;
        OnReleaseLS = null;
        OnReleaseBack = null;
        OnReleaseStart = null;

        OnMoveVertical = null;
        OnMoveHorizontal = null;
        OnRestVertical = null;
        OnRestHorizontal = null;
    }

}
