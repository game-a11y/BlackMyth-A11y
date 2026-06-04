namespace WkAccess.BM;

/// <summary>
/// 按键图标纹理名 → 可读按键名 翻译。
/// 所有 GSInput 按键图标纹理的文本输出统一经此类。
/// </summary>
internal static class KeyNameLocale
{
    /// <summary>纹理名 → 可读按键名</summary>
    internal static readonly Dictionary<string, string> KeyNameMap = new()
    {
        // ── 键盘字母 ──
        { "Icon_Keyboard_A", "A键" },
        { "Icon_Keyboard_B", "B键" },
        { "Icon_Keyboard_C", "C键" },
        { "Icon_Keyboard_D", "D键" },
        { "Icon_Keyboard_E", "E键" },
        { "Icon_Keyboard_F", "F键" },
        { "Icon_Keyboard_G", "G键" },
        { "Icon_Keyboard_H", "H键" },
        { "Icon_Keyboard_I", "I键" },
        { "Icon_Keyboard_J", "J键" },
        { "Icon_Keyboard_K", "K键" },
        { "Icon_Keyboard_L", "L键" },
        { "Icon_Keyboard_M", "M键" },
        { "Icon_Keyboard_N", "N键" },
        { "Icon_Keyboard_O", "O键" },
        { "Icon_Keyboard_P", "P键" },
        { "Icon_Keyboard_Q", "Q键" },
        { "Icon_Keyboard_R", "R键" },
        { "Icon_Keyboard_S", "S键" },
        { "Icon_Keyboard_T", "T键" },
        { "Icon_Keyboard_U", "U键" },
        { "Icon_Keyboard_V", "V键" },
        { "Icon_Keyboard_W", "W键" },
        { "Icon_Keyboard_X", "X键" },
        { "Icon_Keyboard_Y", "Y键" },
        { "Icon_Keyboard_Z", "Z键" },

        // ── 键盘数字 ──
        { "Icon_Keyboard_0", "0键" },
        { "Icon_Keyboard_1", "1键" },
        { "Icon_Keyboard_2", "2键" },
        { "Icon_Keyboard_3", "3键" },
        { "Icon_Keyboard_4", "4键" },
        { "Icon_Keyboard_5", "5键" },
        { "Icon_Keyboard_6", "6键" },
        { "Icon_Keyboard_7", "7键" },
        { "Icon_Keyboard_8", "8键" },
        { "Icon_Keyboard_9", "9键" },

        // ── 键盘功能键 ──
        { "Icon_Keyboard_F1", "F1键" },
        { "Icon_Keyboard_F2", "F2键" },
        { "Icon_Keyboard_F3", "F3键" },
        { "Icon_Keyboard_F4", "F4键" },
        { "Icon_Keyboard_F5", "F5键" },
        { "Icon_Keyboard_F6", "F6键" },
        { "Icon_Keyboard_F7", "F7键" },
        { "Icon_Keyboard_F8", "F8键" },
        { "Icon_Keyboard_F9", "F9键" },
        { "Icon_Keyboard_F10", "F10键" },
        { "Icon_Keyboard_F11", "F11键" },
        { "Icon_Keyboard_F12", "F12键" },

        // ── 键盘特殊键 ──
        { "Icon_Keyboard_Esc", "Esc键" },
        { "Icon_Keyboard_Escape", "Esc键" },
        { "Icon_Keyboard_Enter", "回车键" },
        { "Icon_Keyboard_Return", "回车键" },
        { "Icon_Keyboard_Space", "空格键" },
        { "Icon_Keyboard_SpaceBar", "空格键" },
        { "Icon_Keyboard_Tab", "Tab键" },
        { "Icon_Keyboard_Shift", "Shift键" },
        { "Icon_Keyboard_LeftShift", "左Shift键" },
        { "Icon_Keyboard_RightShift", "右Shift键" },
        { "Icon_Keyboard_Ctrl", "Ctrl键" },
        { "Icon_Keyboard_Control", "Ctrl键" },
        { "Icon_Keyboard_LeftControl", "左Ctrl键" },
        { "Icon_Keyboard_RightControl", "右Ctrl键" },
        { "Icon_Keyboard_Alt", "Alt键" },
        { "Icon_Keyboard_LeftAlt", "左Alt键" },
        { "Icon_Keyboard_RightAlt", "右Alt键" },
        { "Icon_Keyboard_Backspace", "退格键" },
        { "Icon_Keyboard_Delete", "Delete键" },
        { "Icon_Keyboard_Home", "Home键" },
        { "Icon_Keyboard_End", "End键" },
        { "Icon_Keyboard_PageUp", "PageUp键" },
        { "Icon_Keyboard_PageDown", "PageDown键" },
        { "Icon_Keyboard_Insert", "Insert键" },
        { "Icon_Keyboard_PrintScreen", "PrintScreen键" },
        { "Icon_Keyboard_CapsLock", "CapsLock键" },
        { "Icon_Keyboard_NumLock", "NumLock键" },
        { "Icon_Keyboard_ScrollLock", "ScrollLock键" },
        { "Icon_Keyboard_Pause", "Pause键" },

        // ── 方向键 ──
        { "Icon_Keyboard_Up", "方向上键" },
        { "Icon_Keyboard_Down", "方向下键" },
        { "Icon_Keyboard_Left", "方向左键" },
        { "Icon_Keyboard_Right", "方向右键" },

        // ── 鼠标 ──
        { "Icon_Keyboard_LMB", "鼠标左键" },
        { "Icon_Keyboard_LeftMouseButton", "鼠标左键" },
        { "Icon_Keyboard_RMB", "鼠标右键" },
        { "Icon_Keyboard_RightMouseButton", "鼠标右键" },
        { "Icon_Keyboard_MMB", "鼠标中键" },
        { "Icon_Keyboard_MiddleMouseButton", "鼠标中键" },
        { "Icon_Keyboard_MouseWheelUp", "滚轮上" },
        { "Icon_Keyboard_MouseWheelDown", "滚轮下" },

        // ── 小键盘 ──
        { "Icon_Keyboard_NumPad0", "小键盘0" },
        { "Icon_Keyboard_NumPad1", "小键盘1" },
        { "Icon_Keyboard_NumPad2", "小键盘2" },
        { "Icon_Keyboard_NumPad3", "小键盘3" },
        { "Icon_Keyboard_NumPad4", "小键盘4" },
        { "Icon_Keyboard_NumPad5", "小键盘5" },
        { "Icon_Keyboard_NumPad6", "小键盘6" },
        { "Icon_Keyboard_NumPad7", "小键盘7" },
        { "Icon_Keyboard_NumPad8", "小键盘8" },
        { "Icon_Keyboard_NumPad9", "小键盘9" },
        { "Icon_Keyboard_NumPadAdd", "小键盘加" },
        { "Icon_Keyboard_NumPadSubtract", "小键盘减" },
        { "Icon_Keyboard_NumPadMultiply", "小键盘乘" },
        { "Icon_Keyboard_NumPadDivide", "小键盘除" },
        { "Icon_Keyboard_NumPadDecimal", "小键盘点" },

        // ── Xbox 手柄 ──
        { "Icon_Xbox_A", "A键(手柄)" },
        { "Icon_Xbox_B", "B键(手柄)" },
        { "Icon_Xbox_X", "X键(手柄)" },
        { "Icon_Xbox_Y", "Y键(手柄)" },
        { "Icon_Xbox_LB", "LB键(手柄)" },
        { "Icon_Xbox_RB", "RB键(手柄)" },
        { "Icon_Xbox_LT", "LT键(手柄)" },
        { "Icon_Xbox_RT", "RT键(手柄)" },
        { "Icon_Xbox_LS", "左摇杆按下(手柄)" },
        { "Icon_Xbox_RS", "右摇杆按下(手柄)" },
        { "Icon_Xbox_LeftThumbstick", "左摇杆按下(手柄)" },
        { "Icon_Xbox_RightThumbstick", "右摇杆按下(手柄)" },
        { "Icon_Xbox_Dpad_Up", "十字键上(手柄)" },
        { "Icon_Xbox_Dpad_Down", "十字键下(手柄)" },
        { "Icon_Xbox_Dpad_Left", "十字键左(手柄)" },
        { "Icon_Xbox_Dpad_Right", "十字键右(手柄)" },
        { "Icon_Xbox_Start", "Start键(手柄)" },
        { "Icon_Xbox_Menu", "Menu键(手柄)" },
        { "Icon_Xbox_Back", "Back键(手柄)" },
        { "Icon_Xbox_View", "View键(手柄)" },

        // ── PS 手柄 ──
        { "Icon_PS_Cross", "叉键(PS手柄)" },
        { "Icon_PS_Circle", "圈键(PS手柄)" },
        { "Icon_PS_Square", "方键(PS手柄)" },
        { "Icon_PS_Triangle", "三角键(PS手柄)" },
        { "Icon_PS_L1", "L1键(PS手柄)" },
        { "Icon_PS_R1", "R1键(PS手柄)" },
        { "Icon_PS_L2", "L2键(PS手柄)" },
        { "Icon_PS_R2", "R2键(PS手柄)" },
        { "Icon_PS_L3", "L3键(PS手柄)" },
        { "Icon_PS_R3", "R3键(PS手柄)" },
        { "Icon_PS_Dpad_Up", "十字键上(PS手柄)" },
        { "Icon_PS_Dpad_Down", "十字键下(PS手柄)" },
        { "Icon_PS_Dpad_Left", "十字键左(PS手柄)" },
        { "Icon_PS_Dpad_Right", "十字键右(PS手柄)" },
        { "Icon_PS_Options", "Options键(PS手柄)" },
        { "Icon_PS_Share", "Share键(PS手柄)" },
        { "Icon_PS_Touchpad", "触摸板(PS手柄)" },

        // ── 通用手柄（容错） ──
        { "Icon_Gamepad_A", "A键(手柄)" },
        { "Icon_Gamepad_B", "B键(手柄)" },
        { "Icon_Gamepad_X", "X键(手柄)" },
        { "Icon_Gamepad_Y", "Y键(手柄)" },
        { "Icon_Gamepad_Up", "十字键上(手柄)" },
        { "Icon_Gamepad_Down", "十字键下(手柄)" },
        { "Icon_Gamepad_Left", "十字键左(手柄)" },
        { "Icon_Gamepad_Right", "十字键右(手柄)" },
    };

    /// <summary>已知设备类型标记 → 中文设备名</summary>
    internal static readonly Dictionary<string, string> DeviceLabels = new()
    {
        { "Keyboard", "" },
        { "Xbox", "(手柄)" },
        { "PS", "(PS手柄)" },
        { "Gamepad", "(手柄)" },
    };

    /// <summary>控件名 → 中文按键名（GSInputActionIcon 的 FName）</summary>
    internal static string? MapWidgetName(string? widgetName)
        => widgetName switch
        {
            "InputA" => "A键(手柄)",
            "InputB" => "B键(手柄)",
            "InputX" => "X键(手柄)",
            "InputY" => "Y键(手柄)",
            "InputLB" => "LB键(手柄)",
            "InputRB" => "RB键(手柄)",
            "InputLT" => "LT键(手柄)",
            "InputRT" => "RT键(手柄)",
            "InputLS" => "左摇杆按下(手柄)",
            "InputRS" => "右摇杆按下(手柄)",
            "InputR3" => "右摇杆按下(手柄)",
            "InputVUGp" => "左摇杆(手柄)",
            "InputVUKb" => "方向键(键盘)",
            "InputVDGp" => "十字键(手柄)",
            "InputVDKb" => "方向键(键盘)",
            "InputUp" => "方向上键",
            "InputDown" => "方向下键",
            "InputLeft" => "方向左键",
            "InputRight" => "方向右键",
            "InputIcon" => null,
            _ => null
        };

    /// <summary>UE4 FKey 名 → 中文按键名</summary>
    internal static string? MapFKeyName(string? keyName)
        => keyName switch
        {
            "SpaceBar" => "空格键",
            "Enter" => "回车键",
            "Escape" => "Esc键",
            "BackSpace" => "退格键",
            "Delete" => "Delete键",
            "Tab" => "Tab键",
            "CapsLock" => "CapsLock键",
            "LeftShift" => "左Shift键",
            "RightShift" => "右Shift键",
            "LeftControl" => "左Ctrl键",
            "RightControl" => "右Ctrl键",
            "LeftAlt" => "左Alt键",
            "RightAlt" => "右Alt键",
            "Up" => "方向上键",
            "Down" => "方向下键",
            "Left" => "方向左键",
            "Right" => "方向右键",
            "LeftMouseButton" => "鼠标左键",
            "RightMouseButton" => "鼠标右键",
            "MiddleMouseButton" => "鼠标中键",
            "MouseScrollUp" => "滚轮上",
            "MouseScrollDown" => "滚轮下",
            "Gamepad_FaceButton_Bottom" => "A键(手柄)",
            "Gamepad_FaceButton_Right" => "B键(手柄)",
            "Gamepad_FaceButton_Left" => "X键(手柄)",
            "Gamepad_FaceButton_Top" => "Y键(手柄)",
            "Gamepad_LeftShoulder" => "LB键(手柄)",
            "Gamepad_RightShoulder" => "RB键(手柄)",
            "Gamepad_LeftTrigger" => "LT键(手柄)",
            "Gamepad_RightTrigger" => "RT键(手柄)",
            "Gamepad_LeftThumbstick" => "左摇杆按下(手柄)",
            "Gamepad_RightThumbstick" => "右摇杆按下(手柄)",
            "Gamepad_DPad_Up" => "十字键上(手柄)",
            "Gamepad_DPad_Down" => "十字键下(手柄)",
            "Gamepad_DPad_Left" => "十字键左(手柄)",
            "Gamepad_DPad_Right" => "十字键右(手柄)",
            "Gamepad_Special_Left" => "View键(手柄)",
            "Gamepad_Special_Right" => "Menu键(手柄)",
            null or "" => null,
            _ => keyName!.Length == 1 && char.IsLetterOrDigit(keyName[0])
                ? $"{keyName}键"
                : keyName
        };
}
