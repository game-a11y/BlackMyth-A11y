using CommB1;
using b1.Localization;

namespace WkAccess.BM.UI;

/// <summary>
/// 控件树导航辅助 — 供 B1WidgetExtractors 内部使用。
/// </summary>
internal static class B1WidgetResolvers
{
    /// <summary>在指定根控件下按名称查找文本控件并返回其文本。</summary>
    public static string? FindTextByName(UUserWidget? root, string childName)
    {
        if (root == null || !root.IsValidLowLevel()) return null;
        try
        {
            var w = GSUIUtil.FindChildWidget(root, childName);
            if (w == null || !w.IsValidLowLevel()) return null;
            if (w is UTextBlock tb)
                return tb.GetText()?.ToString();
            var m = w.GetType().GetMethod("GetText", Type.EmptyTypes);
            if (m != null)
            {
                var t = m.Invoke(w, null)?.ToString();
                if (!string.IsNullOrEmpty(t)) return t;
            }
            // Text 属性 fallback（GSRichScaleText 等）
            var textProp = w.GetType().GetProperty("Text");
            if (textProp != null)
            {
                var t = textProp.GetValue(w)?.ToString();
                if (!string.IsNullOrEmpty(t)) return t;
            }
            if (w is UUserWidget uw)
            {
                var p = uw.GetType().GetProperty("Content");
                if (p?.GetValue(uw) is UTextBlock tb2)
                    return tb2.GetText()?.ToString();
            }
        }
        catch { }
        return null;
    }

    /// <summary>清理 EquipName：占位符检测 + ToFText 解析本地化</summary>
    public static string? CleanEquipName(string? raw)
    {
        if (string.IsNullOrEmpty(raw)) return null;
        if (raw!.Contains("名字名字")) return null;
        try
        {
            var text = raw.ToFText().ToString();
            if (string.IsNullOrEmpty(text) || text.Contains("名字名字")) return null;
            var cleaned = System.Text.RegularExpressions.Regex.Replace(text, "<[^>]+>", "");
            return string.IsNullOrEmpty(cleaned) ? null : cleaned;
        }
        catch { return raw; }
    }

    /// <summary>
    /// 将 protobuf 本地化 key（如 "FUStInteractionMappingDesc.100901.InteractName"）
    /// 解析为显示文本（如 "上香"）。内部调用 ToFText().ToString()。
    /// </summary>
    public static string? ResolveFText(string? raw)
    {
        return CleanEquipName(raw);
    }

    /// <summary>从控件树确定 QuickItem 的位置（在同级兄弟中的序号）</summary>
    public static int GetQuickItemPosition(UUserWidget w)
    {
        try
        {
            var parent = w.GetParent();
            if (parent != null)
            {
                var count = parent.GetChildrenCount();
                for (int i = 0; i < count; i++)
                {
                    if (parent.GetChildAt(i) == w)
                        return i;
                }
            }
        }
        catch { }
        return -1;
    }

    /// <summary>从快捷物品数据解析物品名</summary>
    public static string? ResolveQuickItemNameByPos(int position)
    {
        try
        {
            if (GSG.GamePlayer == null) return null;
            if (position < 0) return null;
            foreach (var s in GSG.GamePlayer.Actor.Wear.ShortcutsList.ValueList)
            {
                if (s.Position == position && s.ItemId > 0)
                {
                    var desc = GameDBRuntime.GetItemDesc(s.ItemId);
                    return CleanEquipName(desc?.Name);
                }
            }
        }
        catch { }
        return null;
    }

    /// <summary>从玩家装备数据解析物品名（绕过 UI 时序问题）</summary>
    public static string? ResolveEquipName(int slotIdx)
    {
        try
        {
            var slotType = (EEquipSlotType)slotIdx;
            var position = DSEquipMain.GetEquipTypeBySlotType(slotType);
            if (GSG.GamePlayer == null) return null;
            var wearList = GSG.GamePlayer.Actor.Wear.EquipList.ValueList;
            foreach (var w in wearList)
            {
                if (w.Position == position)
                {
                    var desc = GameDBRuntime.GetEquipDesc(w.Id);
                    return CleanEquipName(desc?.EquipName);
                }
            }
        }
        catch { }
        return null;
    }

    /// <summary>从 slot FName 解析索引（"BI_EquipSlotItem_5" → 5）</summary>
    public static int ParseSlotIndex(string fname)
    {
        var i = fname.LastIndexOf('_');
        if (i >= 0 && int.TryParse(fname.Substring(i + 1), out var idx))
            return idx;
        return -1;
    }
}
