using UnityEngine;
using System;
using System.IO;
using System.Collections;

public abstract class ScriptBase : MonoBehaviour {
	protected string message = "";
	protected string username = "WebPUser0001";
	protected string password = "pa$$sword";
	protected void ShowException(string msg, Exception e)
	{
		this.message = "ERROR: " + msg + "\n" + e.GetType () + "\n";
		if (e.Data != null)
		{
			this.message += "Data=" + e.Data.ToString() + "\n";
		}
		if (e.InnerException != null)
		{
			this.message += "InnerExcepton=" + e.InnerException.GetType() + "\n";
			this.message += "InnerExcepton.Message=" + e.InnerException.Message + "\n";
			this.message += "InnerExcepton.Stacktrace=" + e.InnerException.StackTrace + "\n";
		}
		this.message += "Source=" + e.Source + "\n";
		this.message += e.Message + "\n" + e.StackTrace;
	}
	protected byte[] ReadStream(Stream stream)
	{
		MemoryStream ms = new MemoryStream();
		try
		{
			byte[] buff = new byte[65536];
			while (true)
			{
				int read = stream.Read(buff, 0, buff.Length);
				if (read > 0)
				{
					ms.Write(buff, 0, read);
				}
				else
				{
					break;
				}
			}
			return ms.ToArray ();
		}
		finally
		{
			ms.Dispose();
		}
	}
	protected bool Compare(byte[] b1, byte[] b2)
	{
		if (b1.Length != b2.Length)
		{
			return false;
		}
		for (int i = 0; i < b1.Length; i++)
		{
			if (!b1[i].Equals(b2[i]))
			{
				return false;
			}
		}
		return true;
	}
}
