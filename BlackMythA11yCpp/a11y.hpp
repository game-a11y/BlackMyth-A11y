#pragma once
#include <Mod/CppUserModBase.hpp>

#define MODSTR(_in_mod_str) STR("[BlackMythA11yCpp] " _in_mod_str)


namespace A11yMod
{

    class BlackMythA11yCpp : public RC::CppUserModBase
    {
    public:
        BlackMythA11yCpp();
        ~BlackMythA11yCpp() override;
        
        auto on_update() -> void override;
        auto on_unreal_init() -> void override;
    };

};
