using System;
using System.Collections;
using System.Reflection;
using System.Threading;

using UnityEngine;
using JsonOrg;
namespace KiiCorp.Cloud.Storage
{
  public class Test_1_9_KiiGroupWithID_NotExists_Refresh : TestCaseBase
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
     
      Exception exp = null;
      bool endFlag = false;
      KiiGroupCallback callback = (KiiGroup grp, Exception e) => {
        exp = e;
        endFlag = true;
      };

      string groupname = StringUtils.RandomAlphabetic(20);
      KiiGroup groupWithId = KiiGroup.GroupWithID(groupname);
      exp = null;
      endFlag = false;
      groupWithId.Refresh(callback);
      while (!endFlag) {
        yield return new WaitForSeconds (1);
      }
      if(exp == null)
        throw new TestFailException();
      if(404 != ((CloudException)exp).Status)
        throw new TestFailException();
      resultFlag = true;
    }
  }
}