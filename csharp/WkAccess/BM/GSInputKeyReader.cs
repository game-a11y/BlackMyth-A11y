using WkAccess.BM.Locale;

namespace WkAccess.BM;

/// <summary>
/// 从按键图标控件读取原始按键名（不翻译为中文）。后期根据键盘-手柄转换。
/// 翻译表定义在 KeyNameLocale 中。
/// </summary>
public static class GSInputKeyReader
{
    /// <summary>
    /// 从按键图标控件读取原始按键名（纹理名或 FName），不做中文翻译。
    /// </summary>
    /// <returns>原始纹理名或控件 FName，如 "Icon_Xbox_A"；失败返回 null</returns>
    public static string? ReadKeyNameFromIcon(UWidget? iconWidget)
    {
        if (iconWidget == null || !iconWidget.IsValidLowLevel())
            return null;

        try
        {
            var texName = GetTextureName(iconWidget);
            if (!string.IsNullOrEmpty(texName))
                return texName;
        }
        catch { }

        // 回退：控件 FName 原文
        return iconWidget.GetFName().ToString() ?? null;
    }

    /// <summary>
    /// 将原始按键名翻译为中文可读名。用于焦点朗读等需要人类可读的场景。
    /// </summary>
    /// <returns>如 "W键"、"A键(手柄)"；无法翻译时返回原始名</returns>
    public static string? TranslateKeyName(string? rawName)
    {
        if (string.IsNullOrEmpty(rawName)) return null;

        // 纹理名 → 中文按键名
        var translated = MapTextureToKeyName(rawName!);
        if (translated != rawName) return translated;

        // 控件 FName → 中文按键名
        var widgetFallback = KeyNameLocale.MapWidgetName(rawName!);
        if (widgetFallback != null) return widgetFallback;

        return rawName;
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
