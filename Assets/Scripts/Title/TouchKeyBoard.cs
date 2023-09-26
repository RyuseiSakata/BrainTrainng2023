//https://qiita.com/radian-jp/items/e6da29ea187fcf52d861

using System;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

/// <summary>
/// Windows�W���^�b�`�L�[�{�[�h����N���X�B
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
    /// TabTip.exe �̃p�X
    /// </summary>
    public static string TabTipPath { get; set; } = "C:/Program Files/Common Files/Microsoft Shared/ink/tabtip.exe";

    /// <summary>
    /// �^�b�`�L�[�{�[�h���J���B
    /// �^�b�`�L�[�{�[�h���\������Ă���ꍇ�͉������Ȃ��B
    /// </summary>
    /// <param name="delayMsec">�^�b�`�L�[�{�[�h�\���܂ł̑҂����ԁB 
    /// TAB�L�[�ɔ������ă^�b�`�L�[�{�[�h�������ɕ����Ă��܂��ꍇ�A�҂����Ԃ𒲐�����B�i200�`300msec���x�j</param>
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

            //�^�b�`�L�[�{�[�h��ON/OFF�ؑ�
            Toggle();
        });
    }

    /// <summary>
    /// �^�b�`�L�[�{�[�h�����B
    /// �^�b�`�L�[�{�[�h���\������Ă��Ȃ��ꍇ�͉������Ȃ��B
    /// </summary>
    public static void Close()
    {
        if (!IsVisible())
        {
            return;
        }

        //�^�b�`�L�[�{�[�h��ON/OFF�ؑ�
        Toggle();
    }

    /// <summary>
    /// �^�b�`�L�[�{�[�h��ON/OFF�؂�ւ�
    /// </summary>
    public static void Toggle()
    {
        //UIHostNoLaunch.Toggle �ŕ\���؂�ւ�
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
            //tabtip.exe �����s����Ă��Ȃ��ꍇ�Anew UIHostNoLaunch() ��COMException����������B
            //Process.GetProcessesByName �Ŏ��O�Ƀ`�F�b�N����ׂ����Ƃ��l�������A
            //COMException�����ł������܂ŗL�ӂȑ��x���������̂ŁA�`�F�b�N���Ȃ��B
        }

        //��L�������ŗ�O�����������ꍇ�Atabtip.exe ���N�����Ă��Ȃ��̂ŋN������
        var pi = new ProcessStartInfo();
        pi.FileName = TabTipPath;
        pi.UseShellExecute = true;
        Process.Start(pi);
    }

    /// <summary>
    /// �^�b�`�L�[�{�[�h�̕\����Ԃ��擾����B
    /// </summary>
    /// <returns>true:�\�� false:��\��</returns>
    public static bool IsVisible()
    {
        //�^�b�`�L�[�{�[�h�̈ʒu�E�T�C�Y���擾���A��0�Ȃ��\���Ƃ݂Ȃ�
        Rectangle bounds = GetBounds();
        return (bounds.Width != 0);
    }

    /// <summary>
    /// �^�b�`�L�[�{�[�h�̈ʒu�E�T�C�Y���擾����B
    /// </summary>
    /// <returns>�^�b�`�L�[�{�[�h�̈ʒu�E�T�C�Y</returns>
    public static Rectangle GetBounds()
    {
        IFrameworkInputPane inputPane = null;
        Rectangle rect;
        try
        {
            //�^�b�`�L�[�{�[�h�̈ʒu�E�T�C�Y���擾���A��0�Ȃ��\���Ƃ݂Ȃ�
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