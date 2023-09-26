using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics;

public class NameInputField : InputField
{

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        //ExternalCall("C:\\Program Files\\Common Files\\Microsoft Shared\\ink\abtip.exe", null, false);
        //ExternalCall("C:/Program Files/Common Files/Microsoft Shared/ink/tabtip.exe", null, false);
        TouchKeyBoard.Open(1);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        TouchKeyBoard.Close();
        //ExternalCall("C:/Program Files/Common Files/Microsoft Shared/ink/tabtip.exe", null, true);

    }


    private static Process ExternalCall(string filename, string arguments, bool hideWindow)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = filename;
        startInfo.Arguments = arguments;

        // if just command, we don't want to see the console displayed
        if (hideWindow)
        {
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
        }

        Process process = new Process();
        process.StartInfo = startInfo;
        process.Start();

        return process;
    }
}
