﻿using System;
using System.IO;
using DWSIM.UI.Controls;
using Eto.Forms;
using SkiaSharp;
using System.Runtime.InteropServices;

namespace DWSIM.UI.Desktop
{
    public class Program
    {

        [STAThread]
        public static void Main(string[] args)
        {

            // set global settings

            GlobalSettings.Settings.CultureInfo = "en";
            GlobalSettings.Settings.EnableGPUProcessing = false;

            if (RunningPlatform() == OSPlatform.Windows)
            {

                DWSIM.UI.Desktop.WPF.StyleSetter.SetTheme("aero", "normalcolor");

                DWSIM.UI.Desktop.WPF.StyleSetter.SetStyles();

                var platform = new Eto.Wpf.Platform();

                platform.Add<FlowsheetSurfaceControl.IFlowsheetSurface>(() => new WPF.FlowsheetSurfaceControlHandler());

                new Application(platform).Run(new MainForm());
            }
            else if (RunningPlatform() == OSPlatform.Linux)
            {

                DWSIM.UI.Desktop.GTK.StyleSetter.SetStyles();

                var platform = new Eto.GtkSharp.Platform();

                platform.Add<FlowsheetSurfaceControl.IFlowsheetSurface>(() => new GTK.FlowsheetSurfaceControlHandler());

                new Application(platform).Run(new MainForm());
            }
            else if (RunningPlatform() == OSPlatform.Mac)
            {

                try
                {

                    DWSIM.UI.Desktop.Mac.StyleSetter.SetStyles();

                    var platform = new Eto.Mac.Platform();

                    platform.Add<FlowsheetSurfaceControl.IFlowsheetSurface>(() => new Mac.FlowsheetSurfaceControlHandler());

                    new Application(platform).Run(new MainForm());
                
                }
                catch (Exception ex)
                {
                    File.WriteAllText(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "log.txt"), ex.ToString());
                    if (ex.InnerException != null) File.WriteAllText(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "log.txt"), ex.InnerException.ToString());
                }

            }


        }

        public enum OSPlatform
        {
            Windows,
            Linux,
            Mac
        }

        public static OSPlatform RunningPlatform()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    // Well, there are chances MacOSX is reported as Unix instead of MacOSX.
                    // Instead of platform check, we'll do a feature checks (Mac specific root folders)
                    if (Directory.Exists("/Applications")
                        & Directory.Exists("/System")
                        & Directory.Exists("/Users")
                        & Directory.Exists("/Volumes"))
                        return OSPlatform.Mac;
                    else
                        return OSPlatform.Linux;

                case PlatformID.MacOSX:
                    return OSPlatform.Mac;

                default:
                    return OSPlatform.Windows;
            }
        }

    }

}