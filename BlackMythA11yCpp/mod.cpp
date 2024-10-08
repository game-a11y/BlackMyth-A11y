﻿#include <iostream>
#include <Unreal/UObjectGlobals.hpp>
#include <Unreal/UObject.hpp>
#include <Mod/LuaMod.hpp>
#include <Hooks.hpp>
#include "mod.hpp"


namespace A11yMod
{

    using namespace RC;
    using namespace Unreal;

    BlackMythA11yCpp::BlackMythA11yCpp()
    {
        ModName = STR("BlackMythA11yCpp");
        ModVersion = STR("1.0");
        ModDescription = STR("An accessibility mod for 'Black Myth: Wukong'");
        ModAuthors = STR("inkydragon @ Github");
        // Do not change this unless you want to target a UE4SS version
        // other than the one you're currently building with somehow.
        //ModIntendedSDKVersion = STR("2.6");

        Output::send<LogLevel::Verbose>(MODSTR("BlackMythA11yCpp Mod init.\n"));
    }

    BlackMythA11yCpp::~BlackMythA11yCpp()
    {
    }

    auto BlackMythA11yCpp::on_update() -> void
    {
    }

    /**
     * @brief The 'Unreal' module has been initialized.
     * 
     * Before this fires, you cannot use anything in the 'Unreal' namespace.
     */
    auto BlackMythA11yCpp::on_unreal_init() -> void
    {
        Unreal::Hook::RegisterInitGameStatePostCallback(&init_game_state_post_hook);
    }
    
    auto OnAddedToFocusPath_Hook(UnrealScriptFunctionCallableContext& Context, void* CustomData) -> void
    {
        auto button = Context.Context;
        auto focus_event = (UObject*)CustomData;
        
        auto full_name = button->GetFullName();
        Output::send<LogLevel::Verbose>(
            MODSTR("OnAddedToFocusPath({}): {}\n"),
            focus_event->GetFullName(),  // TODO: None
            button->GetFullName()
        );
        // Output::send<LogLevel::Verbose>(MODSTR("\tText={}\n"), );
    }
    auto Empty_UnrealScriptFunction(UnrealScriptFunctionCallableContext& Context, void* CustomData) -> void {}


    auto BlackMythA11yCpp::init_game_state_post_hook([[maybe_unused]] Unreal::AGameModeBase* Context) -> void
    {
        if (bModuleLoaded) return;
        bModuleLoaded = true;
        Output::send<LogLevel::Verbose>(MODSTR("[init_game_state_post_hook]\n"));

        // You are allowed to use the 'Unreal' namespace in this function and anywhere else after this function has fired.
        auto Object = UObjectGlobals::StaticFindObject<UObject*>(nullptr, nullptr, STR("/Script/CoreUObject.Object"));
        Output::send<LogLevel::Verbose>(MODSTR("Object Name: {}\n"), Object->GetFullName());

        // auto hook_id = UObjectGlobals::RegisterHook(STR("/Script/b1-Managed.BUI_Button:OnAddedToFocusPath"),
        //     OnAddedToFocusPath_Hook, Empty_UnrealScriptFunction, nullptr);
    }

    auto BlackMythA11yCpp::on_lua_start(
        StringViewType mod_name,
        LuaMadeSimple::Lua& lua,
        LuaMadeSimple::Lua& main_lua,
        LuaMadeSimple::Lua& async_lua,
        std::vector<LuaMadeSimple::Lua*>& hook_luas
    ) -> void
    {
        if (!(A11yLuaModName == mod_name)) {
            Output::send<LogLevel::Normal>(MODSTR("Run mod({}) lua script.\n"), mod_name);
            return;
        }
        Output::send<LogLevel::Verbose>(MODSTR("Run mod({}) lua script.\n"), mod_name);
        // TODO: move to on_unreal_init()
        srApi.init_and_check();

        /* A11yTolk Class Begin */
        {
            auto tolk_class = lua.prepare_new_table();
            tolk_class.set_has_userdata(false);

            // tolk.version()
            tolk_class.add_pair("GetVersion", [](const LuaMadeSimple::Lua& lua) -> int {
                lua.set_integer(1);
                lua.set_integer(0);
                lua.set_integer(0);
                return 3;
            });

            // Tolk_Speak
            tolk_class.add_pair("Speak", [](const LuaMadeSimple::Lua& lua) -> int {
                std::string error_overload_not_found{R"(
No overload found for function 'Speak'.
Overloads:
#1: Speak(string OutputText, bool interrupt=false))"};
                lua.discard_value(); // Discard the 'this' param.

                if (!lua.is_string())
                {
                    lua.throw_error(error_overload_not_found);
                }
                auto output_text = ensure_str(lua.get_string());
                // std::wcerr << "wtext=" << output_text << std::endl;
                // Output::send<LogLevel::Verbose>(
                //     MODSTR("Lua.Speak: \"{}\"\n"),
                //     output_text);

                auto interrupt = false;
                if (lua.get_stack_size() > 1)
                {
                    lua.throw_error(error_overload_not_found);
                }
                else if (lua.get_stack_size() == 1)
                {
                    if (!lua.is_bool())
                    {
                        lua.throw_error(error_overload_not_found);
                    }
                    interrupt = lua.get_bool();
                }

                auto pWtext = FromCharTypePtr<TCHAR>(output_text.c_str());
                srApi.speak(pWtext, interrupt);
                return 1;
            });

            tolk_class.make_global("A11yTolk");
            Output::send<LogLevel::Normal>(MODSTR("Set Lua A11yTolk Class.\n"));
        } /* A11yTolk Class END */
    }

    /**
     * @brief 此函数会 hook [所有] DLL 加载
     * 
     * @param dll_name 
     */
    auto BlackMythA11yCpp::on_dll_load(StringViewType dll_name) -> void
    {
        // Output::send<LogLevel::Verbose>(MODSTR("on_dll_load: {}.\n"), dll_name);
    }
};
