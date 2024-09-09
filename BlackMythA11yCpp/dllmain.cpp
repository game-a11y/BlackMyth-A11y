#include <a11y.hpp>

#define MOD_API_EXPORT  __declspec(dllexport)


/**
* export the start_mod() and uninstall_mod() functions to
* be used by the core ue4ss system to load in our dll mod
*/
extern "C"
{
    MOD_API_EXPORT RC::CppUserModBase* start_mod()
    {
        return new A11yMod::BlackMythA11yCpp();
    }

    MOD_API_EXPORT void uninstall_mod(RC::CppUserModBase* mod)
    {
        delete mod;
    }
}
