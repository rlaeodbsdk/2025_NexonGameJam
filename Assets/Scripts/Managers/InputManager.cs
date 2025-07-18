using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;
    bool _pressed = false;
    public void OnUpdate()
    {
        if (Input.anyKey && KeyAction != null)
        {
            KeyAction.Invoke();
        }
        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if (_pressed)
                {
                    //MouseAction.Invoke(Define.MouseEvent.Click);
                    MouseAction.Invoke(Define.MouseEvent.End);
                }
                _pressed = false;
            }
        }

      
        
    }
    public void Clear()
    {
        MouseAction = null;
        KeyAction = null;
    }
}
