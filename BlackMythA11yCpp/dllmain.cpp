#include <stdio.h>
#include <Mod/CppUserModBase.hpp>
#include <Unreal/UObjectGlobals.hpp>
#include <Unreal/UObject.hpp>

#define MODSTR(_in_mod_str) STR("[BlackMythA11yCpp] " _in_mod_str)

using namespace RC;
using namespace Unreal;

class BlackMythA11yCpp : public CppUserModBase
{
public:
    BlackMythA11yCpp() : CppUserModBase()
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

    ~BlackMythA11yCpp() override
    {
    }

    auto on_update() -> void override
    {
    }

    /**
     * @brief The 'Unreal' module has been initialized.
     * 
     * Before this fires, you cannot use anything in the 'Unreal' namespace.
     */
    auto on_unreal_init() -> void override
    {
        // You are allowed to use the 'Unreal' namespace in this function and anywhere else after this function has fired.
        auto Object = UObjectGlobals::StaticFindObject<UObject*>(nullptr, nullptr, STR("/Script/CoreUObject.Object"));
        Output::send<LogLevel::Verbose>(MODSTR("Object Name: {}\n"), Object->GetFullName());
    }
};


#define MOD_API_EXPORT  __declspec(dllexport)
/**
* export the start_mod() and uninstall_mod() functions to
* be used by the core ue4ss system to load in our dll mod
*/
extern "C"
{
    MOD_API_EXPORT RC::CppUserModBase* start_mod()
    {
        return new BlackMythA11yCpp();
    }

    MOD_API_EXPORT void uninstall_mod(RC::CppUserModBase* mod)
    {
        delete mod;
    }
}
#undef MOD_API_EXPORT
