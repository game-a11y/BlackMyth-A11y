#include <stdio.h>
#include <Unreal/UObjectGlobals.hpp>
#include <Unreal/UObject.hpp>
#include <Mod/LuaMod.hpp>
#include <mod.hpp>
#include "Hooks.hpp"

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

        sr_load_lib();
        Output::send<LogLevel::Verbose>(MODSTR("BlackMythA11yCpp Mod init.\n"));
    }

    BlackMythA11yCpp::~BlackMythA11yCpp()
    {
        sr_unload_lib();
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

    auto BlackMythA11yCpp::init_game_state_post_hook([[maybe_unused]] Unreal::AGameModeBase* Context) -> void
    {
        if (bModuleLoaded) return;
        bModuleLoaded = true;
        Output::send<LogLevel::Verbose>(MODSTR("[init_game_state_post_hook]\n"));

        // You are allowed to use the 'Unreal' namespace in this function and anywhere else after this function has fired.
        auto Object = UObjectGlobals::StaticFindObject<UObject*>(nullptr, nullptr, STR("/Script/CoreUObject.Object"));
        Output::send<LogLevel::Verbose>(MODSTR("Object Name: {}\n"), Object->GetFullName());
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
        sr_init_and_check();

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
#1: Speak(string OutputText))"};
                lua.discard_value(); // Discard the 'this' param.

                if (!lua.is_string())
                {
                    lua.throw_error(error_overload_not_found);
                }
                auto output_text = std::string{lua.get_string()};

                Output::send<LogLevel::Normal>(
                    MODSTR("From Lua:  Speak(\"{}\")\n"),
                    FromCharTypePtr<TCHAR>(ensure_str(output_text).c_str()));
                return 1;
            });
            
            // TODO:
            //  Tolk_IsLoaded();
            //  Tolk_HasSpeech();
            //  Tolk_DetectScreenReader();
            //  Tolk_Silence();

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
