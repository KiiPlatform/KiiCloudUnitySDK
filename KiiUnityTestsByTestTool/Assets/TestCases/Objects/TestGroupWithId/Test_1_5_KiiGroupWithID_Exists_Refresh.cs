using System;
using System.Collections;
using System.Reflection;
using System.Threading;

using UnityEngine;
using JsonOrg;
namespace KiiCorp.Cloud.Storage
{
  public class Test_1_5_KiiGroupWithID_Exists_Refresh : TestCaseBase
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

      // create group
      string groupname = StringUtils.RandomAlphabetic(20);
      KiiGroup group = Kii.Group(groupname);

      Exception exp = null;
      bool endFlag = false;
      KiiGroupCallback callback = (KiiGroup grp, Exception e) => {
        exp = e;
        endFlag = true;
      };

      group.Save(callback);
      while (!endFlag) {
        yield return new WaitForSeconds (1);
      }

      if(exp != null)
        throw new TestFailException();

      // refresh 
      KiiGroup groupWithId = KiiGroup.GroupWithID(group.ID);
      exp = null;
      endFlag = false;
      groupWithId.Refresh(callback);
      while (!endFlag) {
        yield return new WaitForSeconds (1);
      }
      if(exp != null)
        throw new TestFailException();
      if(!groupname.Equals(groupWithId.Name))
        throw new TestFailException();
      resultFlag = true;
    }
  }
}