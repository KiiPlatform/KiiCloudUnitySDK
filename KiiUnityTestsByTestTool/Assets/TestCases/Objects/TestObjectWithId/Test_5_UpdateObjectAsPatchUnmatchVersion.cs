using System;
using System.Collections;
using System.Reflection;
using System.Threading;

using UnityEngine;
using JsonOrg;
namespace KiiCorp.Cloud.Storage
{
  public class Test_5_UpdateObjectAsPatchUnmatchVersion : TestCaseBase
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
      obj [objKey] = "UpdateA";
      SDKTestHack.SetField(obj, "mEtag", "999");
      endFlag = false;
      exp = null;
      obj.Save(false, callback);
      while (!endFlag) {
        yield return new WaitForSeconds (1);
      }
      if(exp == null)
        throw new TestFailException();
      CloudException ex = (CloudException)exp;
      if(409 != ex.Status)
        throw new TestFailException();
      Debug.Log("object updated has expected conflict response .");
      resultFlag = true;
    }
  }
}