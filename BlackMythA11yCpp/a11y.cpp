#include <stdio.h>
#include <Unreal/UObjectGlobals.hpp>
#include <Unreal/UObject.hpp>
#include <a11y.hpp>


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

    }
};
