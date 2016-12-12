﻿using PluginAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Ionic.Zip;
using System.Threading;

class update : ICommand
{
    public string Command
    {
        get
        {
            return "update";
        }
    }

    public string HelpText
    {
        get
        {
            return "Tries to update. Usage: \"update <release number>.\" CD to program dir first.";
        }
    }

    public void CommandMethod(string p)
    {
        string release = p;
        string remoteUri = ("https://github.com/lukasdragon/AquaConsole/releases/download/" + release + "/AquaConsole.zip");
        string fileName = Directory.GetCurrentDirectory();
        string zipname = (fileName + "\\AquaConsole.zip");
        string releaseFolder = (fileName + "\\" + release);
        string pluginsFolder = (fileName + "\\plugins");
        string newPluginsFolder = (releaseFolder + "\\plugins");
        using (var client = new WebClient())
        {
            if (!Utility.FileOrDirectoryExists(fileName))
            {
                Console.WriteLine("AquaConsole release already found! Please delete directory \\" + release + " and try again.");
            }
            else
            {
                WebClient temporaryw = new WebClient();
                temporaryw.DownloadFile(remoteUri, zipname);
                using (ZipFile zip1 = ZipFile.Read(zipname))
                {
                    zip1.ExtractAll(releaseFolder);
                }
                File.Delete(zipname);
                Console.WriteLine("Added version " + release + " in new folder");
                Directory.CreateDirectory(newPluginsFolder);

                string zippi = (newPluginsFolder + "\\temp.zip");
                using (ZipFile zip2 = new ZipFile())
                {
                    zip2.AddDirectory(pluginsFolder, "");
                    zip2.Save(zippi);
                }
                using (ZipFile zip3 = ZipFile.Read(zippi))
                {
                    zip3.ExtractAll(newPluginsFolder);
                }
                Console.WriteLine("Successfully copied over plugins");
                Thread.Sleep(1000);
                File.Delete(zippi);
            }
        }
    }
}