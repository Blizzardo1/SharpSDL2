#region License
/* SDL2# - C# Wrapper for SDL2
 *
 * Copyright (c) 2013-2020 Ethan Lee.
 *
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from
 * the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 * claim that you wrote the original software. If you use this software in a
 * product, an acknowledgment in the product documentation would be
 * appreciated but is not required.
 *
 * 2. Altered source versions must be plainly marked as such, and must not be
 * misrepresented as being the original software.
 *
 * 3. This notice may not be removed or altered from any source distribution.
 *
 * Ethan "flibitijibibo" Lee <flibitijibibo@flibitijibibo.com>
 *
 */
#endregion


#region Using Statements
using System;
using System.Runtime.InteropServices;
#endregion
namespace SDL2
{
    #region scancode.h
    /* Scancodes based off USB keyboard page (0x07) */
    public enum Scancode
    {
        Unknown = 0,

        A = 4,
        B = 5,
        C = 6,
        D = 7,
        E = 8,
        F = 9,
        G = 10,
        H = 11,
        I = 12,
        J = 13,
        K = 14,
        L = 15,
        M = 16,
        N = 17,
        O = 18,
        P = 19,
        Q = 20,
        R = 21,
        S = 22,
        T = 23,
        U = 24,
        V = 25,
        W = 26,
        X = 27,
        Y = 28,
        Z = 29,

        D1 = 30,
        D2 = 31,
        D3 = 32,
        D4 = 33,
        D5 = 34,
        D6 = 35,
        D7 = 36,
        D8 = 37,
        D9 = 38,
        D0 = 39,

        Return = 40,
        Escape = 41,
        Backspace = 42,
        Tab = 43,
        Space = 44,

        Minus = 45,
        Equals = 46,
        LeftBracket = 47,
        RightBracket = 48,
        Backslash = 49,
        Nonushash = 50,
        Semicolon = 51,
        Apostrophe = 52,
        Grave = 53,
        Comma = 54,
        Period = 55,
        Slash = 56,

        CapsLock = 57,

        F1 = 58,
        F2 = 59,
        F3 = 60,
        F4 = 61,
        F5 = 62,
        F6 = 63,
        F7 = 64,
        F8 = 65,
        F9 = 66,
        F10 = 67,
        F11 = 68,
        F12 = 69,

        PrintScreen = 70,
        ScrollLock = 71,
        Pause = 72,
        Insert = 73,
        Home = 74,
        PageUp = 75,
        Delete = 76,
        End = 77,
        PageDown = 78,
        Right = 79,
        Left = 80,
        Down = 81,
        Up = 82,

        NumlockClear = 83,
        KeypadDivide = 84,
        KeypadMultiply = 85,
        KeypadMinus = 86,
        KeypadPlus = 87,
        KeypadEnter = 88,
        Keypad1 = 89,
        Keypad2 = 90,
        Keypad3 = 91,
        Keypad4 = 92,
        Keypad5 = 93,
        Keypad6 = 94,
        Keypad7 = 95,
        Keypad8 = 96,
        Keypad9 = 97,
        Keypad0 = 98,
        KeypadPeriod = 99,

        NonusBackslash = 100,
        Application = 101,
        Power = 102,
        KeypadEquals = 103,
        F13 = 104,
        F14 = 105,
        F15 = 106,
        F16 = 107,
        F17 = 108,
        F18 = 109,
        F19 = 110,
        F20 = 111,
        F21 = 112,
        F22 = 113,
        F23 = 114,
        F24 = 115,
        Execute = 116,
        Help = 117,
        Menu = 118,
        Select = 119,
        Stop = 120,
        Again = 121,
        Undo = 122,
        Cut = 123,
        Copy = 124,
        Paste = 125,
        Find = 126,
        Mute = 127,
        VolumeUp = 128,
        VolumeDown = 129,
        /* not sure whether there's a reason to enable these */
        /*	Lockingcapslock = 130, */
        /*	Lockingnumlock = 131, */
        /*	Lockingscrolllock = 132, */
        KeypadComma = 133,
        KeypadEqualsas400 = 134,

        International1 = 135,
        International2 = 136,
        International3 = 137,
        International4 = 138,
        International5 = 139,
        International6 = 140,
        International7 = 141,
        International8 = 142,
        International9 = 143,
        Lang1 = 144,
        Lang2 = 145,
        Lang3 = 146,
        Lang4 = 147,
        Lang5 = 148,
        Lang6 = 149,
        Lang7 = 150,
        Lang8 = 151,
        Lang9 = 152,

        Alterase = 153,
        Sysreq = 154,
        Cancel = 155,
        Clear = 156,
        Prior = 157,
        Return2 = 158,
        Separator = 159,
        Out = 160,
        Oper = 161,
        ClearAgain = 162,
        Crsel = 163,
        Exsel = 164,

        Keypad00 = 176,
        Keypad000 = 177,
        ThousandsSeparator = 178,
        DecimalSeparator = 179,
        CurrencyUnit = 180,
        CurrencySubUnit = 181,
        KeypadLeftParen = 182,
        KeypadRightParen = 183,
        KeypadLeftBrace = 184,
        KeypadRightBrace = 185,
        KeypadTab = 186,
        KeypadBackspace = 187,
        KeypadA = 188,
        KeypadB = 189,
        KeypadC = 190,
        KeypadD = 191,
        KeypadE = 192,
        KeypadF = 193,
        KeypadXor = 194,
        KeypadPower = 195,
        KeypadPercent = 196,
        KeypadLess = 197,
        KeypadGreater = 198,
        KeypadAmpersand = 199,
        KeypadDblampersand = 200,
        KeypadVerticalbar = 201,
        KeypadDblverticalbar = 202,
        KeypadColon = 203,
        KeypadHash = 204,
        KeypadSpace = 205,
        KeypadAt = 206,
        KeypadExclam = 207,
        KeypadMemStore = 208,
        KeypadMemRecall = 209,
        KeypadMemClear = 210,
        KeypadMemAdd = 211,
        KeypadMemSubtract = 212,
        KeypadMemMultiply = 213,
        KeypadMemDivide = 214,
        KeypadPlusminus = 215,
        KeypadClear = 216,
        KeypadClearentry = 217,
        KeypadBinary = 218,
        KeypadOctal = 219,
        KeypadDecimal = 220,
        KeypadHexadecimal = 221,

        LCtrl = 224,
        LShift = 225,
        LAlt = 226,
        LGui = 227,
        RCtrl = 228,
        RShift = 229,
        RAlt = 230,
        RGui = 231,

        Mode = 257,

        /* These come from the Usb consumer page (0x0C) */
        AudioNext = 258,
        AudioPrev = 259,
        AudioStop = 260,
        AudioPlay = 261,
        AudioMute = 262,
        MediaSelect = 263,
        Www = 264,
        Mail = 265,
        Calculator = 266,
        Computer = 267,
        AcSearch = 268,
        AcHome = 269,
        AcBack = 270,
        AcForward = 271,
        AcStop = 272,
        AcRefresh = 273,
        AcBookmarks = 274,

        /* These come from other sources, and are mostly mac related */
        BrightnessDown = 275,
        BrightnessUp = 276,
        DisplaySwitch = 277,
        KbdillumToggle = 278,
        KbdillumDown = 279,
        KbdillumUp = 280,
        Eject = 281,
        Sleep = 282,

        App1 = 283,
        App2 = 284,

        /* These come from the Usb consumer page (0x0C) */
        AudioRewind = 285,
        AudioFastForward = 286,

        /* This is not a key, simply marks the number of scancodes
         * so that you know how big to make your arrays. */
        NumScancodes = 512
    }
    #endregion
}
