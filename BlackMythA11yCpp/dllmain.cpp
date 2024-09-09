#include <stdio.h>
#include <Mod/CppUserModBase.hpp>

#define MY_AWESOME_MOD_API __declspec(dllexport)


class BlackMythA11yCpp : public RC::CppUserModBase
{
public:
    BlackMythA11yCpp() : CppUserModBase()
    {
        ModName = STR("BlackMythA11yCpp");
        ModVersion = STR("1.0");
        ModDescription = STR("This is my awesome mod");
        ModAuthors = STR("UE4SS Team");
        // Do not change this unless you want to target a UE4SS version
        // other than the one you're currently building with somehow.
        //ModIntendedSDKVersion = STR("2.6");
        
        printf("BlackMythA11yCpp Mod init.\n");
    }

    ~BlackMythA11yCpp() override
    {
    }

    auto on_update() -> void override
    {
    }
};


extern "C"
{
    MY_AWESOME_MOD_API RC::CppUserModBase* start_mod()
    {
        return new BlackMythA11yCpp();
    }

    MY_AWESOME_MOD_API void uninstall_mod(RC::CppUserModBase* mod)
    {
        delete mod;
    }
}
