using SEHOON.GameSystem;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
public class ThrowError : MonoBehaviour
{
    // Imports the native Windows user32 library
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);

    private bool hasThrownError = false;

    public void OnTriggerEnter2D()
    {
        if (hasThrownError)
        {
            return;
        }
        MessageBox(IntPtr.Zero, "VGhlIGFwcGxpY2F0aW9uIGhhcyBlbmNvdW50ZXJlZCBhIGZhdGFsIGV4Y2VwdGlvbi4=", "U3lzdGVtIEVycm9y", 0x00000010);
        MessageBox(IntPtr.Zero, "IkknbSB0aXJlZCwiIEkgc2F5LA==", "U3lzdGVtIEVycm9y", 0x00000010);
        MessageBox(IntPtr.Zero, "IlRoYXQncyBhbGwuIg==", "U3lzdGVtIEVycm9y", 0x00000010);
        MessageBox(IntPtr.Zero, "QW5kIGluIGEgd2F5LCBJIGd1ZXNzIGl0J3MgdHJ1ZS4=", "U3lzdGVtIEVycm9y", 0x00000010);
        MessageBox(IntPtr.Zero, "V2h5IGFyZSB5b3UgaGVyZT8=", "U3lzdGVtIEVycm9y", 0x00000010);
        hasThrownError = true;
    }
}