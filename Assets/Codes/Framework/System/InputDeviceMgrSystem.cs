using System;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface IInputDeviceMgrSystem : ISystem
    {
        void OnEnable();
        void OnDisable();
        void RegisterDeviceChange<T>(Action fun);
    }
    /// <summary>
    /// �����豸�����
    /// </summary>
    public class InputDeviceMgrSystem : AbstractSystem, IInputDeviceMgrSystem
    {
        // �������豸�л��¼�
        private Dictionary<Type, Action> mDeviceSwitchTable;
        // ��ǰ�����豸
        private InputDevice mCurDevice;
        /// <summary>
        /// ע���豸�л��¼�
        /// </summary>
        void IInputDeviceMgrSystem.RegisterDeviceChange<T>(Action fun)
        {
            var type = typeof(T);
            if (mDeviceSwitchTable.ContainsKey(type))
            {
                mDeviceSwitchTable[type] += fun;
            }
            else
            {
                mDeviceSwitchTable.Add(type, fun);
            }
        }
        // ��ʼ��ϵͳ
        protected override void OnInit()
        {
            mDeviceSwitchTable = new Dictionary<Type, Action>();
        }
        /// <summary>
        /// �����豸�л�
        /// </summary>
        public void OnEnable()
        {
            InputSystem.onActionChange += DetectCurrentInputDevice;
            InputSystem.onDeviceChange += OnDeviceChanged;
        }
        /// <summary>
        /// �ر��豸�л�
        /// </summary>
        public void OnDisable()
        {
            InputSystem.onActionChange -= DetectCurrentInputDevice;
            InputSystem.onDeviceChange -= OnDeviceChanged;
            // ����¼�
            mDeviceSwitchTable.Clear();
        }
        // ��⵱ǰ�����豸
        private void DetectCurrentInputDevice(object obj, InputActionChange change)
        {
            if (change != InputActionChange.ActionPerformed) return;
            SwitchingDevice((obj as InputAction).activeControl.device);
        }
        // �豸����¼�
        private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
        {
            switch (change)
            {
                case InputDeviceChange.Reconnected:
                    Debug.Log(device + "��������");
                    // �л�Ϊ��ǰ�����豸
                    SwitchingDevice(device);
                    break;
                case InputDeviceChange.Disconnected:
                    Debug.Log(device + "�Ͽ�����");
                    // �л�Ϊ��ǰ�豸������豸 ��ʱ���ü��̴���
#if UNITY_STANDALONE || UNITY_EDITOR
                    if (device is Gamepad)
                        device = Keyboard.current;
                    SwitchingDevice(device);
#endif
                    break;
            }
        }
        // �л��豸
        private void SwitchingDevice(InputDevice device)
        {
            // ���������豸Ϊ�� �Ҳ�ͬ�ڵ�ǰ�����豸
            if (device == null || device.Equals(mCurDevice)) return;

            mCurDevice = device;

            Type type = null;

            if (device is Keyboard) type = typeof(Keyboard);
            else if (device is Gamepad) type = typeof(Gamepad);
            else if (device is Pointer) type = typeof(Pointer);
            else if (device is Joystick) type = typeof(Joystick);

            // ���ת���ɹ��ҵ�ǰ���ͱ�ע�� ��ִ�ж�Ӧ��ί�з��� 
            if (type == null || !mDeviceSwitchTable.TryGetValue(type, out var callback)) return;

            callback?.Invoke();
        }
    }
}