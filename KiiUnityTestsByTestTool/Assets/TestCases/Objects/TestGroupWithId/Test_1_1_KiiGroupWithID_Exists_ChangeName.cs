using System;
using System.Collections;
using System.Reflection;
using System.Threading;

using UnityEngine;
using JsonOrg;
namespace KiiCorp.Cloud.Storage
{
  public class Test_1_1_KiiGroupWithID_Exists_ChangeName : TestCaseBase
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

      // change name
      string newGroupName = StringUtils.RandomAlphabetic(20);
      exp = null;
      endFlag = false;
      groupWithId.ChangeName(newGroupName, callback);
      while (!endFlag) {
        yield return new WaitForSeconds (1);
      }
      if(!newGroupName.Equals(groupWithId.Name))
        throw new TestFailException();

      //check groupname changed in server.
      string respString = ApiHelper.get(SDKUtils.GetGroupUFEUri(groupWithId), Kii.AppId, Kii.AppKey, KiiUser.AccessToken);
      JsonObject groupJson = new JsonObject(respString);
      string updatedName = groupJson.GetString("name");
      if(!newGroupName.Equals(updatedName))
        throw new TestFailException();
      resultFlag = true;
    }
  }
}