//https://qiita.com/radian-jp/items/e6da29ea187fcf52d861

using System;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

/// <summary>
/// Windows標準タッチキーボード操作クラス。
/// </summary>
public class TouchKeyBoard
{
    [ComImport, Guid("D5120AA3-46BA-44C5-822D-CA8092C1FC72")]
    private class FrameworkInputPane
    {
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("5752238B-24F0-495A-82F1-2FD593056796")]
    private interface IFrameworkInputPane
    {
        [PreserveSig]
        int Advise(
            [MarshalAs(UnmanagedType.IUnknown)] object pWindow,
            [MarshalAs(UnmanagedType.IUnknown)] object pHandler,
            out int pdwCookie
            );

        [PreserveSig]
        int AdviseWithHWND(
            IntPtr hwnd,
            [MarshalAs(UnmanagedType.IUnknown)] object pHandler,
            out int pdwCookie
            );

        [PreserveSig]
        int Unadvise(
            int pdwCookie
            );

        [PreserveSig]
        int Location(
            out Rectangle prcInputPaneScreenLocation
            );
    }

    [ComImport, Guid("4ce576fa-83dc-4F88-951c-9d0782b4e376")]
    private class UIHostNoLaunch
    {
    }

    [ComImport, Guid("37c994e7-432b-4834-a2f7-dce1f13b834b")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface ITipInvocation
    {
        void Toggle(IntPtr hwnd);
    }

    [DllImport("user32.dll", SetLastError = false)]
    private static extern IntPtr GetDesktopWindow();

    /// <summary>
    /// TabTip.exe のパス
    /// </summary>
    public static string TabTipPath { get; set; } = "C:/Program Files/Common Files/Microsoft Shared/ink/tabtip.exe";

    /// <summary>
    /// タッチキーボードを開く。
    /// タッチキーボードが表示されている場合は何もしない。
    /// </summary>
    /// <param name="delayMsec">タッチキーボード表示までの待ち時間。 
    /// TABキーに反応してタッチキーボードが即座に閉じられてしまう場合、待ち時間を調整する。（200～300msec程度）</param>
    public static void Open(int delayMsec)
    {
        if (IsVisible())
        {
            return;
        }

        Task.Run(() =>
        {
            if (delayMsec > 0)
            {
                Thread.Sleep(delayMsec);
            }

            //タッチキーボードのON/OFF切替
            Toggle();
        });
    }

    /// <summary>
    /// タッチキーボードを閉じる。
    /// タッチキーボードが表示されていない場合は何もしない。
    /// </summary>
    public static void Close()
    {
        if (!IsVisible())
        {
            return;
        }

        //タッチキーボードのON/OFF切替
        Toggle();
    }

    /// <summary>
    /// タッチキーボードのON/OFF切り替え
    /// </summary>
    public static void Toggle()
    {
        //UIHostNoLaunch.Toggle で表示切り替え
        try
        {
            UIHostNoLaunch uiHostNoLaunch = null;
            try
            {
                uiHostNoLaunch = new UIHostNoLaunch();
                var tipInvocation = (ITipInvocation)uiHostNoLaunch;
                tipInvocation.Toggle(GetDesktopWindow());
            }
            finally
            {
                if (uiHostNoLaunch != null)
                {
                    Marshal.ReleaseComObject(uiHostNoLaunch);
                }
            }
        }
        catch (COMException)
        {
            //tabtip.exe が実行されていない場合、new UIHostNoLaunch() でCOMExceptionが発生する。
            //Process.GetProcessesByName で事前にチェックするべきかとも考えたが、
            //COMException無視でもそこまで有意な速度差が無いので、チェックしない。
        }

        //上記処理内で例外が発生した場合、tabtip.exe が起動していないので起動する
        var pi = new ProcessStartInfo();
        pi.FileName = TabTipPath;
        pi.UseShellExecute = true;
        Process.Start(pi);
    }

    /// <summary>
    /// タッチキーボードの表示状態を取得する。
    /// </summary>
    /// <returns>true:表示 false:非表示</returns>
    public static bool IsVisible()
    {
        //タッチキーボードの位置・サイズを取得し、幅0なら非表示とみなす
        Rectangle bounds = GetBounds();
        return (bounds.Width != 0);
    }

    /// <summary>
    /// タッチキーボードの位置・サイズを取得する。
    /// </summary>
    /// <returns>タッチキーボードの位置・サイズ</returns>
    public static Rectangle GetBounds()
    {
        IFrameworkInputPane inputPane = null;
        Rectangle rect;
        try
        {
            //タッチキーボードの位置・サイズを取得し、幅0なら非表示とみなす
            inputPane = (IFrameworkInputPane)new FrameworkInputPane();
            inputPane.Location(out rect);
        }
        finally
        {
            if (inputPane != null)
            {
                Marshal.ReleaseComObject(inputPane);
            }
        }

        return rect;
    }
}