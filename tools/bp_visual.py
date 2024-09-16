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

def ObjectPath2BpId(objectPath: str) -> int:
    # NOTE: 此处假设在同一个类里
    # "b1/Content/00Main/UI/BluePrintsV3/Btn/BI_FirstStartBtn.2"
    match = re.search(r'\.(\d+)$', objectPath)
    if match:
        num_str = match.group(1)
        return int(num_str)
    
    # Bad
    return -1

def get_slots(widget: dict) -> List[dict]:
    """获取所有子组件"""
    sub_widgets = []
    
    # 根据类型分发
    Type = widget["Type"]
    if Type in {"WidgetTree"}:
        return [widget["Properties"]["RootWidget"]]
    elif Type in {"CanvasPanel", "HorizontalBox", "ScaleBox"}:
        return widget["Properties"]["Slots"]
    elif Type in {"CanvasPanelSlot", "HorizontalBoxSlot", "ScaleBoxSlot"}:
        return [widget["Properties"]["Content"]]
    return sub_widgets


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
    print(f"{OuterClass}.WidgetTree")
    # -----------------------------------------------------
    
    # 开始递归打印子组件
    RootWidget = WidgetTree["Properties"]["RootWidget"]
    ObjectPath = RootWidget["ObjectPath"]
    bp_id = ObjectPath2BpId(ObjectPath)
    print_WidgetTree_rec(BP, bp_id, 1)
    
    # -----------------------------------------------------
    print("--- Class WidgetTree END ---")

def print_WidgetTree_rec(BP: list, bp_id: int, lv: int, slot_idx: int=None):
    """递归 辅助打印函数"""
    assert BP is not None and len(BP) > 0
    assert lv > 0

    if bp_id and bp_id <= 0:
        logger.warning(f"[lv{lv}] bad bp_id={bp_id};  递归中止 ret")
        return
    
    # 当前组件
    cur_class = BP[bp_id]
    class_name = cur_class["Name"]
    Type = cur_class["Type"]
    if Type.endswith("Slot"):  # 打印容器 index
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
        ObjectName = slot["ObjectName"]
        ObjectPath = slot["ObjectPath"]

        rec_bp_id = ObjectPath2BpId(ObjectPath)
        print_WidgetTree_rec(BP, rec_bp_id, lv+1, slot_idx=index)


if __name__ == '__main__':
    logger.info("Run as scripts")

    # 输入参数
    bp_fullpath = "b1/Content/00Main/UI/BluePrintsV3/Btn"
    bp_name = "BI_FirstStartBtn"
    bp_name = "BI_StartGame"
    bp_name = "BI_SettingTab"
    
    bp_fullpath = "b1/Content/00Main/UI/BluePrintsV3/StartGame"
    bp_name = "BI_ArchivesBtnV2"
    
    bp_fullpath = "b1/Content/00Main/UI/BluePrintsV3/Setting/Item/"
    bp_name = "BI_SettingKeyItem"

    # -----------------------------------------------------------
    b1_base_dir = "../ref/Exports"
    # bp_json_path = f"../ref/{bp_name}.json"
    bp_json_path = f"{b1_base_dir}/{bp_fullpath}/{bp_name}.json"

    # --- 开始读取 json
    bp_json = None
    with open(bp_json_path, 'r', encoding='utf-8') as file:
        bp_json = json.load(file)
    print(f"Total {len(bp_json)} items")
    print(get_all_types(bp_json))
    # print(get_all_classes(bp_json))
    # USE    set(i.get('Class') for i in bp_json)
    
    main_class_name = f"{bp_name}_C"
    print(f"main_class_name: {main_class_name}")

    print_WidgetTree(bp_json)

