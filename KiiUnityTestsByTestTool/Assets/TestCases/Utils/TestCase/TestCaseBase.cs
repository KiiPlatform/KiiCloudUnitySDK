using System;
using System.Collections;
using UnityEngine;

namespace KiiCorp.Cloud.Storage
{
    class TestLogger : IKiiLogger
    {
        public void Debug (string message, params object[] args)
        {
            UnityEngine.Debug.Log(string.Format(message, args));
        }
    }

    public class TestCaseBase : MonoBehaviour
    {
        protected bool resultFlag = false;

        void Start ()
        {
            Debug.Log ("TestCase starts.");
            Kii.Logger = new TestLogger ();
            resultFlag = false;
        }
            
        void Update ()
        {
            if (resultFlag) {
                throw new TestSuccessException ();
            }    
        }

        protected IEnumerator LogInUser (string username, string password, KiiUserCallback callback)
        {
            
            // TODO: Move following code to common method/class
            bool endFlag = false;
            KiiUser.LogIn (username, password, (KiiUser u, Exception e) => {
                callback (u, e);
                endFlag = true;
            });
            
            while (!endFlag) {
                yield return new WaitForSeconds (1);
            }
            yield return KiiUser.CurrentUser;
        }

        protected IEnumerator RegisterUser (KiiUser user, string password, KiiUserCallback callback)
        {

            // TODO: Move following code to common method/class
            bool endFlag = false;
            user.Register (password, (KiiUser u, Exception e) => {
                callback (u, e);
                endFlag = true;
            });

            while (!endFlag) {
                yield return new WaitForSeconds (1);
            }
            yield return user;
        }
    }
}