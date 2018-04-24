using System;
using System.Collections;
using System.Reflection;
using System.Threading;

using UnityEngine;
using JsonOrg;
namespace KiiCorp.Cloud.Storage
{
  public class Test_7_UpdateObjectNotAsPatch : TestCaseBase
  {        
    void Start()
    {
            
      StartCoroutine(TestStep());
    }

    private IEnumerator TestStep()
    {
      
      string username = StringUtils.RandomAlphabetic(20);
      string password = StringUtils.RandomAlphabetic(20);
      KiiUser user = KiiUser.BuilderWithName(username).Build();
      var task = RegisterUser(user, password, (KiiUser u, Exception e) => {
        if (e != null)
        {
          throw new TestFailException();
        }
        Debug.Log("Register user.");
      });
      yield return StartCoroutine(task);
      
      // Create object
      string bucketName = StringUtils.RandomAlphabetic(10);
      string objectId = StringUtils.RandomAlphabetic(10);
      KiiBucket bucket = user.Bucket(bucketName);
      KiiObject obj = bucket.NewKiiObject(objectId);
      string objKey = "KeyA";
      string objValue = "ValueA";
      obj [objKey] = objValue;
      
      Exception exp = null;
      bool endFlag = false;
      KiiObjectCallback callback = (KiiObject obj1, Exception e) => {
        exp = e;
        endFlag = true;
      };
      obj.SaveAllFields(true, callback);
      while (!endFlag) {
        yield return new WaitForSeconds (1);
      }
      if(exp != null)
        throw new TestFailException();
      Debug.Log("object created");
      resultFlag = true;
      
      // refresh
      endFlag = false;
      exp = null;
      obj.Refresh(callback);
      while (!endFlag) {
        yield return new WaitForSeconds (1);
      }
      if(exp != null)
        throw new TestFailException();
      Debug.Log("object refreshed");
      
      // Update with patch
      obj ["KeyB"] = "ValueB";
      endFlag = false;
      exp = null;
      obj.SaveAllFields(true, callback);
      while (!endFlag) {
        yield return new WaitForSeconds (1);
      }
      if(exp != null)
        throw new TestFailException();
      
      Debug.Log("object updated.");
      // verify update
      string uri = SDKUtils.GetObjectUFEUri(obj);
      Debug.Log("object ufe uri :"+uri);
      string body = ApiHelper.get(uri, Kii.AppId, Kii.AppKey, KiiUser.AccessToken);
      JsonObject json = new JsonObject(body);
      if(!"ValueB".Equals(json.GetString("KeyB")))
        throw new TestFailException();
      if(json.Has("KeyA"))
        throw new TestFailException();
      resultFlag = true;
    }
  }
}