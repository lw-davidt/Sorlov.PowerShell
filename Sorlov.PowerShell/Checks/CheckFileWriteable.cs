﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CheckFileWriteable : ValidateArgumentsAttribute
{
    private string message = "The file is not writable";

    public CheckFileWriteable(string message)
    {
        this.message = message;
    }

    public CheckFileWriteable()
    {
    }

    protected override void Validate(object arguments, EngineIntrinsics engineIntrinsics)
    {
        if (arguments==null)
            throw new CheckValidationException(message);

        try
        {
            ProviderInfo providerInfo = null;
            Collection<string> paths = engineIntrinsics.SessionState.Path.GetResolvedProviderPathFromPSPath((string)arguments, out providerInfo);

            FileInfo file = new FileInfo(paths[0]);
            if (!file.Exists)
                throw new CheckValidationException(message);

            using (FileStream stream = new FileStream(file.FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.Write))
            {
            }
        }
        catch (Exception)
        {
            throw new CheckValidationException(message);
        }
    }

    public string Message
    {
        get { return message; }
        set { this.message = value; }
    }

}