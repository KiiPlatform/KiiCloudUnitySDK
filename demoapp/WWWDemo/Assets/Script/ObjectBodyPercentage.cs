using UnityEngine;
using System;
using System.IO;
using System.Collections;
using KiiCorp.Cloud.Storage;

[ExecuteInEditMode()]
public class ObjectBodyPercentage : ScriptBase {
	
	void Start () {
	}
	void Update () {
	}
	void OnGUI() {
		GUI.Label (new Rect (10, 10, 800, 50), GetType().ToString());
		if (GUI.Button (new Rect (10, 60, 250, 100), "Upload & Download"))
		{
			KiiUser.LogIn(this.username, this.password, (KiiUser user, Exception e1)=>{
				if (e1 == null)
				{
					KiiObject obj = Kii.Bucket("images").NewKiiObject();
					obj["owner"] = this.username;
					obj.Save((KiiObject o1, Exception e2)=>{
						if (e2 == null)
						{
							Debug.Log("##### Object is saved");
							byte[] image = ReadImage();
							Stream body = new MemoryStream(image);
							o1.UploadBody("image/png", body, (KiiObject o2, Exception e3)=>{
								if (e3 == null)
								{
									Debug.Log("##### Object body is uploaded");
									Stream outputStream = new MemoryStream();
									o2.DownloadBody(outputStream, (KiiObject o3, Stream s, Exception e4)=>{
										if (e4 == null)
										{
											s.Seek(0, SeekOrigin.Begin);
											byte[] downloadedImage = this.ReadStream(s);
											if (image.Length != downloadedImage.Length)
											{
												this.message = "ERROR: expected=" + image.Length + "bytes  actual=" + downloadedImage.Length + "bytes";
												
											}
											else if (!this.Compare(image, downloadedImage))
											{
												this.message = "ERROR: unexpected bytes";
											}
											else
											{
												this.message = "SUCCESS!!!!!";
											}
										}
										else
										{
											this.ShowException("Failed to download object body", e4);
										}
									}
									, (KiiObject o3, float progress)=>{
										Debug.Log("##### Downloading...");
										this.message = "Downloading... " + (progress * 100) + "%";
									});
								}
								else
								{
									this.ShowException("Failed to upload object body", e3);
								}
							}
							, (KiiObject o2, float progress)=>{
								this.message = "Uploading... " + (progress * 100) + "%";
							});
						}
						else
						{
							this.ShowException("Failed to save object", e2);
						}
					});
				}
				else
				{
					this.ShowException("Failed to login", e1);
				}
			});
		}
		GUI.Label (new Rect (10, 175, 500, 1000), this.message);
	}
	private byte[] ReadImage ()
	{
		TextAsset asset = (TextAsset)Resources.Load("1.8MB", typeof(TextAsset));
		byte[] image = asset.bytes;
		if (image == null) {
			Debug.Log ("#####Failed to read image from file.");
		} else {
			Debug.Log ("#####Load image " + image.Length + "bytes");
		}
		return image;
	}
}
