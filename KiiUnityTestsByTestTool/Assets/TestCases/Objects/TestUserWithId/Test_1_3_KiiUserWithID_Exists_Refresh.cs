using System;
using System.Collections;
using System.Reflection;
using System.Threading;

using UnityEngine;
using JsonOrg;
namespace KiiCorp.Cloud.Storage
{
  public class Test_1_3_KiiUserWithID_Exists_Refresh : TestCaseBase
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

      KiiUser userWithId = KiiUser.UserWithID(user.ID);
      Exception exp = null;
      bool endFlag = false;
      userWithId.Refresh((KiiUser usr,Exception e) =>{
        exp = e;
        endFlag = true;
      });
      while (!endFlag) {
        yield return new WaitForSeconds (1);
      }
      if(exp != null)
        throw new TestFailException();
      if(!username.Equals(userWithId.Username))
        throw new TestFailException();
      resultFlag = true;
    }
  }
}