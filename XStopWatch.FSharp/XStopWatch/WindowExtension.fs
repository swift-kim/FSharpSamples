namespace XStopWatch

open System
open System.Runtime.InteropServices
open ElmSharp

type ScreenMode = Default = 0 | AlwaysOn = 1

module Interop =
    [<Literal>]
    let EFLUtil = "libcapi-ui-efl-util.so.0"

    [<DllImport(EFLUtil)>]
    extern int efl_util_set_window_screen_mode(IntPtr window, ScreenMode mode);
    
    [<DllImport(EFLUtil)>]
    extern int efl_util_get_window_screen_mode(IntPtr window, ScreenMode& mode);

type WindowExtension() =
    
    static member SetAlwaysOn(window : Window, isAlwaysOn : bool) =
        match Interop.efl_util_set_window_screen_mode(nativeint window, if isAlwaysOn then ScreenMode.AlwaysOn else ScreenMode.Default) with
        | x when x <> 0 -> raise (ArgumentException("native efl_util_set_window_screen_mode return error, window=[" + (window.Handle.ToInt32() |> string) + "] isAlwaysOn = [" + (isAlwaysOn |> string) + "]"))
        | _ -> ()
        
    static member GetAlwaysOn(window : Window) =
        let mutable mode = ScreenMode.Default

        match Interop.efl_util_get_window_screen_mode(nativeint window, &mode) with
        | x when x <> 0 -> raise (ArgumentException("native efl_util_set_window_screen_mode return error, window=[" + (window.Handle.ToInt32() |> string) + "] isAlwaysOn = [" + ((mode = ScreenMode.AlwaysOn) |> string) + "]"))
        | _ -> ()

        mode = ScreenMode.AlwaysOn
