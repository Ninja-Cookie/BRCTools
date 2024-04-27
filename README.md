## BRCTools

Tools made for speedrun practice, routing, or just finding new things.

Credits include:
- Ninja Cookie (Creation Of The Tool)
- Bytez (Created Default Save Files & Testing)

Also thanks to original creators of the original tool as well as the upkept NinjaUtils (Renamed to PracticeUtils) for some reference into how to do some things, mainly on the side of Trigger display and research into how Graffiti's worked.

These tools can also be found on [Thunderstore](https://thunderstore.io/c/bomb-rush-cyberfunk/p/Ninja_Cookie/BRCTools/)

![20240427015119_1](https://github.com/Ninja-Cookie/BRCTools/assets/62808028/639a9344-cc85-477c-b329-db6c26b8c0c8)

This was made once again from scratch, all the way up to what it is now.

Some features of highlight include, but not limited to:
- Infinite Boost
- Invulnerability
- Ability to Save/Load Save Data in-game
- Create Save Data Files on-the-spot
- Disable Saving
- Load The Last Save on-the-spot
- Reload level starting from where you entered it
- Better Trigger Display

Speed Values include: `V` for Vertical Speed, `H` for Horizontal Speed, `S` for Storage Speed. (From Billboards)

-----------------------

Installion:
- Download [BepInEx](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.22) if not installed already and follow its install instructions.
- If you have a mod manager, import the Zip using the [Release](https://github.com/Ninja-Cookie/BRCTools/releases) build, else --
- Within your Bomb Rush Cyberfunk BepInEx folder (\BombRushCyberfunk\BepInEx\), extract the `plugins` and `config` folder.
- Run the game and let config files generate, these can then be found in `\BombRushCyberfunk\BepInEx\config\BRCTools\`

Default Keybinds for the menu include `;` to Open and Close it, and `P` to activate Mouse interaction on the menu.
Config Files include: `Settings.cfg` and `Keybinds.cfg`, which allow you to change some settings with the tool and adjust all Keybinds, including unbinding by leaving them blank.

Custom saves can be found in:
`\config\BRCTools\SAVES\`

Modify the descriptions of the linked files using the format:
`filename.brctools: short description of save`

Saves are loaded automatically in alphabetical order of their filenames.
Normal BRC files will not work, files must be created using the tool to generate a `.brctools` file of the Save Data.

-----------------------

[Keybind Reference](https://docs.unity3d.com/ScriptReference/KeyCode.html) (KeyCode Reference | Actual Key To Use):

(Example: if you want a key bound to `KeyCode.Keypad0` which is the `0` key on the numpad, you would type `N0`, as shown below, in the Keybinds config, or for letters, you just put the letter you want such as `K`)
```
KeyCode.Alpha0 = 0 
KeyCode.Alpha1 = 1 
KeyCode.Alpha2 = 2 
KeyCode.Alpha3 = 3 
KeyCode.Alpha4 = 4 
KeyCode.Alpha5 = 5 
KeyCode.Alpha6 = 6 
KeyCode.Alpha7 = 7 
KeyCode.Alpha8 = 8 
KeyCode.Alpha9 = 9 
KeyCode.Keypad0 = N0 
KeyCode.Keypad1 = N1 
KeyCode.Keypad2 = N2 
KeyCode.Keypad3 = N3 
KeyCode.Keypad4 = N4 
KeyCode.Keypad5 = N5 
KeyCode.Keypad6 = N6 
KeyCode.Keypad7 = N7 
KeyCode.Keypad8 = N8 
KeyCode.Keypad9 = N9 
KeyCode.KeypadPeriod = N. 
KeyCode.KeypadDivide = N/ 
KeyCode.KeypadMultiply = N* 
KeyCode.KeypadMinus = N- 
KeyCode.KeypadEnter = NRTN 
KeyCode.KeypadEquals = N= 
KeyCode.Backspace = BKS 
KeyCode.Delete = DEL 
KeyCode.Tab = TAB 
KeyCode.Clear = CLR 
KeyCode.Return = RTN 
KeyCode.Pause = PSE 
KeyCode.Escape = ESC 
KeyCode.Space = SPC 
KeyCode.UpArrow = UP 
KeyCode.DownArrow = DWN 
KeyCode.RightArrow = LFT 
KeyCode.LeftArrow = RGT 
KeyCode.Exclaim = ! 
KeyCode.DoubleQuote = "
KeyCode.Hash = # 
KeyCode.Dollar = $ 
KeyCode.Percent = % 
KeyCode.Ampersand = & 
KeyCode.Quote = ' 
KeyCode.LeftParen = ( 
KeyCode.RightParen = ) 
KeyCode.Asterisk = * 
KeyCode.Plus = + 
KeyCode.Comma = , 
KeyCode.Minus = - 
KeyCode.Period = . 
KeyCode.Slash = / 
KeyCode.Colon = : 
KeyCode.Semicolon = ; 
KeyCode.Less = < 
KeyCode.Equals = = 
KeyCode.Greater = > 
KeyCode.Question = ? 
KeyCode.At = @ 
KeyCode.LeftBracket = [ 
KeyCode.Backslash = \ 
KeyCode.RightBracket = ] 
KeyCode.Caret = ^ 
KeyCode.Underscore = _ 
KeyCode.BackQuote = ` 
KeyCode.LeftCurlyBracket = { 
KeyCode.Pipe = | 
KeyCode.RightCurlyBracket = } 
KeyCode.Tilde = ~ 
KeyCode.Mouse0 = M0 
KeyCode.Mouse1 = M1 
KeyCode.Mouse2 = M2 
KeyCode.Mouse3 = M3 
KeyCode.Mouse4 = M4 
KeyCode.Mouse5 = M5 
KeyCode.Mouse6 = M6 
KeyCode.None = 
```
