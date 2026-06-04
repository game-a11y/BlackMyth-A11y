namespace WkAccess.B1.Input;

/// <summary>
/// 从 UGSInputActionIcon / GSInputActionIcon 读取按键图标信息，映射为人类可读的按键名称。
/// 翻译表定义在 KeyNameLocale 中。
/// </summary>
public static class GSInputKeyReader
{
    /// <summary>
    /// 从按键图标控件读取可读按键名。
    /// </summary>
    /// <returns>可读按键名，如 "W键"、"A键(手柄)"；失败返回控件 FName</returns>
    public static string? ReadKeyNameFromIcon(UWidget? iconWidget)
    {
        if (iconWidget == null || !iconWidget.IsValidLowLevel())
            return null;

        try
        {
            var texName = GetTextureName(iconWidget);
            if (!string.IsNullOrEmpty(texName))
                return MapTextureToKeyName(texName!);
        }
        catch { }

        // 回退 1：控件名 → 中文按键名
        var fallback = KeyNameLocale.MapWidgetName(iconWidget.GetFName().ToString());
        if (fallback != null) return fallback;

        // 回退 2：控件 FName 原文
        return iconWidget.GetFName().ToString() ?? null;
    }

    /// <summary>纹理名 → 可读按键名（含回退解析）</summary>
    static string? MapTextureToKeyName(string texName)
    {
        if (KeyNameLocale.KeyNameMap.TryGetValue(texName, out var keyName))
            return keyName;

        return ParseKeyFromTextureName(texName);
    }

    /// <summary>从 UImage / UGSInputActionIcon / GSInputActionIcon 读取当前显示的纹理名</summary>
    static string? GetTextureName(UWidget iconWidget)
    {
        var type = iconWidget.GetType();

        // 方法 1：UImage.Brush.ResourceObject
        var brushProp = type.GetProperty("Brush");
        if (brushProp != null)
        {
            var brush = brushProp.GetValue(iconWidget);
            if (brush != null)
            {
                var resObjProp = brush.GetType().GetProperty("ResourceObject");
                var tex = resObjProp?.GetValue(brush) as UObject;
                if (tex != null) return tex.GetName();
            }
        }

        // 方法 2：InputIconTexture 直接属性
        var texProp = type.GetProperty("InputIconTexture");
        if (texProp != null)
        {
            var tex = texProp.GetValue(iconWidget) as UObject;
            if (tex != null) return tex.GetName();
        }

        // 方法 3：AtlasedSprite
        var spriteProp = type.GetProperty("AtlasedSprite");
        if (spriteProp != null)
        {
            var sprite = spriteProp.GetValue(iconWidget) as UObject;
            if (sprite != null) return sprite.GetName();
        }

        return null;
    }

    /// <summary>回退解析：按 "Icon_DeviceType_KeyName" 格式提取</summary>
    static string? ParseKeyFromTextureName(string texName)
    {
        var parts = texName.Split('_');
        if (parts.Length < 3 || parts[0] != "Icon")
            return texName;

        var deviceType = parts[1];
        var keyName = string.Join("_", parts, 2, parts.Length - 2);

        KeyNameLocale.DeviceLabels.TryGetValue(deviceType, out var deviceLabel);

        if (string.IsNullOrEmpty(deviceLabel))
            return $"{keyName}键";

        return $"{keyName}{deviceLabel}";
    }
}
