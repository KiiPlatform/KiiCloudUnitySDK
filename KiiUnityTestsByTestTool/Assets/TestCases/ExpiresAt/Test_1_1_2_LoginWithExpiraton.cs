using System;
using System.Collections;
using System.Reflection;
using System.Threading;

using UnityEngine;
using JsonOrg;
namespace KiiCorp.Cloud.Storage
{
  public class Test_1_1_2_LoginWithExpiraton : TestCaseBase
  {        
    void Start()
    {
      
      StartCoroutine(TestStep());
    }
    
    private IEnumerator TestStep()
    {
      Kii.AccessTokenExpiration = 3600;

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


      IDictionary dict = KiiUser.CurrentUser.GetAccessTokenDictionary();
      if(dict == null)
        throw new TestFailException();

      string token = dict["access_token"].ToString();
      if(token == null)
        throw new TestFailException();

      long expiresAt = Convert.ToInt64(dict["expires_at"].ToString());
      if(expiresAt == long.MaxValue || expiresAt <= 0)
        throw new TestFailException();

      Exception exp = null;
      bool endFlag = false;
      KiiObjectCallback callback = (KiiObject obj, Exception e) => {
        exp = e;
        endFlag = true;
      };

      KiiObject ob = user.Bucket(StringUtils.RandomAlphabetic(10)).NewKiiObject();
      ob["key"] = "value";
      ob.Save(callback);

      while (!endFlag) {
        yield return new WaitForSeconds (1);
      }
      
      if(exp != null)
        throw new TestFailException();

      resultFlag = true;
    }
  }
}