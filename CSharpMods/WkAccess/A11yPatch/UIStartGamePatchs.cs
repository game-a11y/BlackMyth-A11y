using HarmonyLib;
using System.Reflection;

namespace WkAccess.A11yPatch;

/// <summary>
/// UIStartGame 相关补丁
/// </summary>
public class UIStartGamePatchs
{

    //[HarmonyPatch]
    internal static class UIStartGame__OnLoadingScreenClose
    {
        public static MethodBase TargetMethod()
        {
            try
            {
                return AccessTools.Method("B1UI.GSUI.UIStartGame:OnLoadingScreenClose");
            }
            catch (Exception ex)
            {
                A11yLog.Exception($"获取 UIStartGame.OnLoadingScreenClose 方法失败", ex);
                return null;
            }
        }

        public static void Prefix()
        {
            A11yLog.Info("UIStartGame.OnLoadingScreenClose 方法执行完成");
        }
    }

    //[HarmonyPatch]
    internal static class UIStartGame__PlayAnimtionOnConstruct
    {
        public static MethodBase TargetMethod()
        {
            try
            {
                return AccessTools.Method("B1UI.GSUI.UIStartGame:PlayAnimtionOnConstruct");
            }
            catch (Exception ex)
            {
                A11yLog.Exception($"获取 UIStartGame.PlayAnimtionOnConstruct 方法失败", ex);
                return null;
            }
        }

        public static void Prefix()
        {
            A11yLog.Info("UIStartGame.PlayAnimtionOnConstruct 方法执行完成");
        }
    }

}