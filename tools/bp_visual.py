# -*- coding: utf-8 -*-
"""
@author: inky
"""
from typing import List
import json


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


allowed_subclass_type = {
# UI class
 "UScriptClass'WidgetTree'",
 "UScriptClass'CanvasPanel'",
 "UScriptClass'CanvasPanelSlot'",
 "UScriptClass'HorizontalBox'",
 "UScriptClass'HorizontalBoxSlot'",
 # "UScriptClass'Function'",
 
# Text
 "UScriptClass'TextBlock'",
 "UScriptClass'GSFocusWidget'",
 "UScriptClass'GSScaleText'",

# ignore
  # "UScriptClass'Image'",
  # "USharpClass'GSImage'",
  # "UScriptClass'MovieScene'",
  # "UScriptClass'MovieScene**'",
  # "UScriptClass'WidgetAnimation'",

 "UScriptClass'WidgetBlueprintGeneratedClass'",
}
def print_class_tree(bp_in: list, class_name: str, lv: int = 0):
    """深度优先打印 UI 树"""
    if bp_in is None or class_name is None or lv < 0:
        return
    b_on_top = 0 == lv
    if b_on_top:
        print("--- Class Tree Start ---")

    indent = ' ' * lv
    print(f"{indent}{class_name}")
    outer_list = find_node_by_outer(bp_in, class_name)
    if len(outer_list) == 0:
        return
    
    for SubClass in outer_list:
        subclass_type = SubClass.get('Class')
        if subclass_type not in allowed_subclass_type:
            continue

        sub_class_name = SubClass.get('Name')
        print_class_tree(bp_in, sub_class_name, lv+1)

    if b_on_top:
        print("--- Class Tree END ---")


if __name__ == '__main__':
    # 输入参数
    bp_fullpath = "b1/Content/00Main/UI/BluePrintsV3/Btn"
    bp_name = "BI_FirstStartBtn"
    bp_name = "BI_StartGame"
    bp_name = "BI_SettingTab"
    
    # bp_fullpath = "b1/Content/00Main/UI/BluePrintsV3/StartGame"
    # bp_name = "BI_ArchivesBtnV2"
    
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

    print_class_tree(bp_json, main_class_name)
