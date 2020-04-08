﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;
using MiscUtil.Conversion;
using CRC;
using StumpyUGXTools;


static class Program
{
    public static string version = "0.5.1";
    public static Editor editor = new Editor();

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        editor.InitializeComponent();
        editor.version.Text = version;
        Application.Run(editor);
    }
}

