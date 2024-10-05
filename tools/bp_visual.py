# -*- coding: utf-8 -*-
"""
@author: inky
"""
from typing import List
import json
import re
# 3rd
from loguru import logger


def get_attr_set(bp_in: list, attr_name: str) -> set:
    return set(i.get(attr_name) for i in bp_in)

def get_all_types(bp_in: list) -> set:
    """打印所有类型"""
    return get_attr_set(bp_in, 'Type')

def get_all_classes(bp_in: list) -> set:
    """打印所有类型"""
    return get_attr_set(bp_in, 'Class')


def find_node_by_name(bp_in: list, name: str) -> dict:
    """按名称寻找节点"""
    for i in bp_in:
        if i.get('Name') == name:
            return i
    return None

def find_node_by_outer(bp_in: list, outer_name: str) -> List[dict]:
    """寻找所有子节点"""
    outer_list = []
    for i in bp_in:
        if i.get('Outer') == outer_name:
            outer_list.append(i)
    return outer_list

def SplitUObjectFullName(fullname: str):
    assert len(fullname) > 0
    pattern = r"([^']+)\'([^.]+)\.(.+)'"
    pattern = ''.join([
        r"([^']+)",  # 对象类型名称
        r"'",
            r"([^.]+)",  # 对象包路径
            r"\.",
            r"(.+)",  # 对象类名
        r"'",
    ])
    match = re.match(pattern, fullname)
    if not match:
        return

    obj_type = match.group(1)  # 对象类型名称
    obj_path = match.group(2)  # 对象包路径
    obj_class = match.group(3) # 对象类名
    return (obj_type, obj_path, obj_class)
    

def ObjectPath2BpId(objectPath: str) -> int:
    # NOTE: 此处假设在同一个类里
    # "b1/Content/00Main/UI/BluePrintsV3/Btn/BI_FirstStartBtn.2"
    match = re.search(r'\.(\d+)$', objectPath)
    if match:
        num_str = match.group(1)
        return int(num_str)
    
    # Bad
    return -1


# 跳过容量为 1 的包装容器
g_SKIP_WRAP_SLOT = True
# g_SKIP_WRAP_SLOT = False
def get_slots(widget: dict) -> List[dict]:
    """获取所有子组件"""
    Type = widget["Type"]
    Name = widget["Name"]
    Class = widget["Class"]

    #   C# 模块
    if Class.startswith('USharpClass'):  
        return []
    if "Template" in widget:  # 外部类
        return [widget["Template"]]

    if Name.startswith('Default__'):
        # 顶级类对象 => 转发到 .WidgetTree 继续解析
        if "Properties" not in widget:
            return [] # 非模板类
        
        obj_type, obj_path, obj_class = SplitUObjectFullName(Class)
        classpath = obj_path
        slot = {
          "ObjectName": str(Class),
          "ObjectPath": f"{classpath}.-1"  # 从最后一个 WidgetTree 开始
        }
        return [slot]

    Properties = widget["Properties"]
    # 检查包装器类型
    is_slot_type = (
        Type.endswith("Slot")
        and "Slots" not in Properties
        and "Content" in Properties
    )
    if Type in {"WidgetTree"}:
        return [Properties["RootWidget"]]
    elif "Slots" in Properties:
        return Properties["Slots"]
    elif is_slot_type:
        return [Properties["Content"]]
    # --- 叶子 Widget
    elif "Slot" in Properties:
        return []
    else:
        logger.warning(f"Unknown widget: {Type}")
        logger.debug(widget)
    
    return []


def readBpJsonByClassPath(obj_path: str, b1_base_dir: str = "../ref/Exports") -> List[dict]:
    """通过类路径读取类（命名空间）"""
    bp_json_path = f"{b1_base_dir}/{obj_path}.json"

    # --- 开始读取 json
    bp_json = None
    # logger.debug(f"Reading json: {obj_path}.json")
    with open(bp_json_path, 'r', encoding='utf-8') as file:
        bp_json = json.load(file)
    return bp_json

def getUObjectByObjectPath(ObjectPath: str) -> dict:
    """通过 ObjectPath 寻找对象"""
    # "b1/Content/00Main/UI/BluePrintsV3/BI_BtnTemplate_Special.33"
    # r"([^.]+)\.(\d+)"
    # NOTE: 此处兼容负数：classpath.-1
    match = re.search(r"([^.]+)\.([-]?\d+)", ObjectPath)
    if match:
        classpath_str = match.group(1)
        obj_id_str = match.group(2)
        obj_id = int(obj_id_str)
        bp_json = readBpJsonByClassPath(classpath_str)
        return bp_json[obj_id]
    
    # Bad
    logger.warning(f"Cannot match ObjectPath={ObjectPath}")
    return None


def get_WidgetTree(BP: list) -> dict:
    """获取 WidgetTree"""
    assert BP is not None and len(BP) > 0
    
    def is_WidgetTree(widget: dict) -> bool:
        "判断是否为 WidgetTree"
        if WidgetTree is None:
            return False
        if "WidgetTree" != WidgetTree["Type"]:
            return False
        if "UScriptClass'WidgetTree'" != WidgetTree["Class"]:
            return False
    
        return True
    
    # 一般在最后一个
    WidgetTree = BP[-1]
    if is_WidgetTree(WidgetTree):
        return WidgetTree
    
    # 按名字查找
    WidgetTree = find_node_by_name(BP, "WidgetTree")
    if is_WidgetTree(WidgetTree):
        return WidgetTree
    
    return None

def print_WidgetTree(BP: list):
    """深度优先打印 UI WidgetTree。
    会打印出中间的 Slot 容器
    """
    assert BP is not None and len(BP) > 0

    # 查找 WidgetTree
    WidgetTree = get_WidgetTree(BP)
    if WidgetTree is None:
        logger.warn("Cannot find WidgetTree.  Skip Print")
        return

    print("--- Class WidgetTree Start ---")
    # 打印顶层类
    OuterClass = WidgetTree.get("Outer")
    widge_tree_name = f"{OuterClass}.WidgetTree.RootWidget"
    print(widge_tree_name)
    # -----------------------------------------------------
    
    # 开始递归打印子组件
    RootWidget = WidgetTree["Properties"]["RootWidget"]
    ObjectPath = RootWidget["ObjectPath"]
    # bp_id = ObjectPath2BpId(ObjectPath)
    print_WidgetTree_rec(ObjectPath)
    
    # -----------------------------------------------------
    print(f"--- Class {widge_tree_name} END ---")

def print_WidgetTree_rec(ObjectPath: str, lv: int=0, slot_idx: int=None, is_in_slot: bool=False):
    """递归 辅助打印函数"""
    global g_SKIP_WRAP_SLOT
    assert lv >= 0

    # 当前组件
    cur_class = getUObjectByObjectPath(ObjectPath)
    if cur_class is None:
        logger.warning(f"[lv{lv}] cur_class is None;  递归中止 ret")
        return

    class_name = cur_class["Name"]
    Type = cur_class["Type"]
    if g_SKIP_WRAP_SLOT and Type.endswith("Slot"):
        # ---- 跳过打印容器
        slots = get_slots(cur_class)
        assert 1 == len(slots)
        ObjectPath = slots[0]["ObjectPath"]
        print_WidgetTree_rec(ObjectPath, lv, slot_idx, is_in_slot=True)
        return  # 不再重复打印

    if Type.endswith("Slot") or is_in_slot:  # 打印容器 index
        indent = ' ' * (lv-1)
        print(f"{indent}[{slot_idx}] {class_name}")
    else:
        indent = ' ' * lv
        print(f"{indent}{class_name}")

    # --- 获取子组件
    slots = get_slots(cur_class)
    if slots is None or 0 == len(slots):
        # logger.debug(f"[lv{lv}] slots is None;  ret")
        return
    
    for index, slot in enumerate(slots):
        # ObjectName = slot["ObjectName"]
        ObjectPath = slot["ObjectPath"]

        # rec_bp_id = ObjectPath2BpId(ObjectPath)
        print_WidgetTree_rec(ObjectPath, lv+1, slot_idx=index)


if __name__ == '__main__':
    logger.info("Run as scripts")
    
    ## 性能测试工具
    bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/BenchMark/BUI_BenchMark_V2.BUI_BenchMark_V2_C'"
    bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Btn/BI_BenchMarkHistoryBtn.BI_BenchMarkHistoryBtn_C'"
    
    
    ## 加载界面
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Loading/BUI_LoadingV2.BUI_LoadingV2_C'"

    ## 首次加载 主界面
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Btn/BI_FirstStartBtn.BI_FirstStartBtn_C'"
    
    ## 主界面/主菜单
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Btn/BI_StartGame.BI_StartGame_C'"
    # 主界面/主菜单/载入游戏
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/StartGame/BI_ArchivesBtnV2.BI_ArchivesBtnV2_C'"
    # 主界面/主菜单/小曲
    # bp_fullpath = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/AccordionView/BI_AccordionChildBtn_Echo.BI_AccordionChildBtn_Echo_C'"
    # 主界面/主菜单/设置
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Btn/BI_SettingTab.BI_SettingTab_C'"
    # 主界面/设置: 左右单项选择
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Setting/Item/BI_SettingFixedItem.BI_SettingFixedItem_C'"
    # 主界面/设置: 下拉单项选择
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Setting/BI_SettingMenuItem.BI_SettingMenuItem_C'"
    # 主界面/设置: 下拉单选-下拉项
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrints  V3/Setting/BI_ModeBtnItem.BI_ModeBtnItem_C'"
    # 主界面/设置: 水平滑块
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Setting/Item/BI_SettingSliderItem.BI_SettingSliderItem_C'"
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Slider/BI_SettingSlider.BI_SettingSlider_C'"
    # 主界面/设置: 图标按钮
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Setting/Item/BI_SettingIconItem.BI_SettingIconItem_C'"
    # 主界面/设置: 文本按钮
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Btn/BI_SettingMainBtn.BI_SettingMainBtn_C'"
    # 主界面/设置: 图标按钮-键盘键位
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Setting/Item/BI_SettingKeyItem.BI_SettingKeyItem_C'"
    # 主界面/设置: 亮度调整
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Setting/BUI_BrightnessSetting.BUI_BrightnessSetting_C'"
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Slider/BI_SettingSliderL.BI_SettingSliderL_C'"
    # 设置右侧描述文本
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Setting/BUI_Setting.BUI_Setting_C'"
    # 二次确定对话框
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Btn/BI_ReconfirmBtn.BI_ReconfirmBtn_C'"
    
    ## 游戏中 ------------------
    # 游戏中:土地庙:菜单
    bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Btn/BI_ShrineMenuParent.BI_ShrineMenuParent_C'"
    bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Btn/BI_ShrineMenuChild.BI_ShrineMenuChild_C'"
    
    ## 游戏中:背包栏
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Tab/BI_CommTxtTab.BI_CommTxtTab_C'"
    # _bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Remake_CE04/Remake_Item/BI_EquipItem_Slot.BI_EquipItem_Slot_C'"
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Remake_CE04/Remake_Item/BI_InventoryItem.BI_InventoryItem_C'"
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/TravelNotes/BI_TravelNotesMain_ListBar.BI_TravelNotesMain_ListBar_C'"
    ## 游戏中:背包:技能
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/LearnSpell/Btn/BI_AbilityIcon_GP_Basic.BI_AbilityIcon_GP_Basic_C'"
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/LearnSpell/Btn/BI_AbilityIcon_GP_Advance.BI_AbilityIcon_GP_Advance_C'"
    # 游戏中:背包:技能:根基
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/LearnSpell/BUI_LearnTalent.BUI_LearnTalent_C'"
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/LearnSpell/BI_SpellDetailNew.BI_SpellDetailNew_C'"
    # 少了 TxtSpellDesc 字段
    """
    "TxtSpellNextDesc"
    263     VerticalBoxSlot_2
    246     VerticalBox_0
    256     VerticalBoxSlot_3
    243     VBoxMiddle
    VBoxMiddle 没有 slot 上级，可能是 bug
    """
    # TODO: BI_DetailLongPress_C
    # 
    
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/LearnSpell/Btn/BI_SpellPanelTitle_Btn.BI_SpellPanelTitle_Btn_C'"
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/LearnSpell/Node/BI_TalentItem_1_1.BI_TalentItem_1_1_C'"
    # 游戏中:背包
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Role/BUI_EquipMain.BUI_EquipMain_C'"
    bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Remake_CE04/Remake_Item/BI_EquipItem_Slot.BI_EquipItem_Slot_C'"
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Remake_CE04/Remake_Item/BI_GearItem_Slot.BI_GearItem_Slot_C'"
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/Remake_CE04/Remake_Item/BI_QuickItem.BI_QuickItem_C'"
    ## 游戏中:背包:游记
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/TravelNotes/BI_TravelNotesMain_Tab.BI_TravelNotesMain_Tab_C'"
    # bp_fullname = "WidgetBlueprintGeneratedClass'b1/Content/00Main/UI/BluePrintsV3/TravelNotes/BI_TravelNotesMain_ListBar.BI_TravelNotesMain_ListBar_C'"
    

    # -----------------------------------------------------------
    # 确定文件路径
    fsplit = SplitUObjectFullName(bp_fullname)
    if fsplit is None:
        logger.warning(f"SplitUObjectFullName failed!  bp_fullname={bp_fullname}")
        exit()
    obj_type, obj_path, obj_class = fsplit
    b1_base_dir = "../ref/Exports"
    bp_json_path = f"{b1_base_dir}/{obj_path}.json"

    # --- 开始读取 json
    bp_json = readBpJsonByClassPath(obj_path)
    
    # --- 输出整体信息
    print(f"Total {len(bp_json)} items")
    print(get_all_types(bp_json))
    # print(get_all_classes(bp_json))
    # USE    set(i.get('Class') for i in bp_json)
    main_class_name = obj_class
    print(f"main_class_name: {main_class_name}")

    # --- 
    print_WidgetTree(bp_json)
