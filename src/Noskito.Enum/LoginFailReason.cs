﻿namespace Noskito.Enum
{
    public enum LoginFailReason
    {
        OldClient = 1,
        UnhandledError = 2,
        Maintenance = 3,
        AlreadyConnected = 4,
        AccountOrPasswordWrong = 5,
        CantConnect = 6,
        Banned = 7,
        WrongCountry = 8,
        WrongCaps = 9
    }
}