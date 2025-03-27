using History;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        private PlayerInput input;
        private List<(InputAction action, Action<InputAction.CallbackContext> command)> actions = new List<(InputAction action, Action<InputAction.CallbackContext> command)>();
        public bool isCharacterBusy = false;
        // Start is called before the first frame update
        private void Awake()
        {
            input = GetComponent<PlayerInput>();

            InitializeActions();
        }

        private void InitializeActions()
        {
            actions.Add((input.actions["Next"], OnNext));
            //actions.Add((input.actions["HistoryBack"], OnHistoryForward));
            //actions.Add((input.actions["HistoryForward"], OnHistoryBack));
            actions.Add((input.actions["HistoryLogs"], OnHistoryToggleLog));
        }

        private void OnEnable()
        {
            foreach (var inputAction in actions)
            {
                inputAction.action.performed += inputAction.command;
            }
        }

        private void OnDisable()
        {
            foreach (var inputAction in actions)
            {
                inputAction.action.performed -= inputAction.command;
            }
        }

        public void OnNext(InputAction.CallbackContext c)
        {
            if (!isCharacterBusy)
            {
                DialogueSystem.instance.OnUserPrompt_Next();
            }
            else
            {
                return;
            }
        }

        public void OnHistoryBack(InputAction.CallbackContext c)
        {
            if (!isCharacterBusy)
            {
                HistoryManager.instance.GoBack();
            }
            else
            {
                return;
            }
            
        }

        public void OnHistoryForward(InputAction.CallbackContext c)
        {
            if (!isCharacterBusy)
            {
                HistoryManager.instance.GoFoward();
            }
            else
            {
                return;
            }
            
        }

        public void OnHistoryToggleLog(InputAction.CallbackContext c)
        {
            var logs = HistoryManager.instance.logManager;
            if (!logs.isOpen)
            {
                logs.Open();
            }
            else
            {
                logs.Close();
            }
        }
    }
}
