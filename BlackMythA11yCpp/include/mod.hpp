#pragma once
#include <Mod/CppUserModBase.hpp>
#include <Unreal/AGameModeBase.hpp>
#include "WkCommon.hpp"
#include "sr.hpp"   // SrApi


namespace A11yMod
{
    /* ---- Constant 常量 ---- */
    // Lua 模块名
    constexpr StringViewType A11yLuaModName = STR("BlackMythA11y");
    // Cpp 模块名
    constexpr StringViewType A11yCppModName = STR("BlackMythA11yCpp");
    
    /* ---- Global Variable 全局变量 ---- */

    /* ---- Class Def 类定义 ---- */
    class BlackMythA11yCpp : public RC::CppUserModBase
    {
    public:
        BlackMythA11yCpp();
        ~BlackMythA11yCpp() override;
        
        auto on_update() -> void override;
        auto on_unreal_init() -> void override;
        
        auto on_lua_start(
            StringViewType mod_name,
            RC::LuaMadeSimple::Lua& lua,
            RC::LuaMadeSimple::Lua& main_lua,
            RC::LuaMadeSimple::Lua& async_lua,
            std::vector<RC::LuaMadeSimple::Lua*>& hook_luas
        ) -> void override;

        auto on_dll_load(StringViewType dll_name) -> void override;

    private:
        SR::SrApi srApi;

        static inline bool bModuleLoaded = false;
        static auto init_game_state_post_hook(RC::Unreal::AGameModeBase* Context) -> void;
    };

};
