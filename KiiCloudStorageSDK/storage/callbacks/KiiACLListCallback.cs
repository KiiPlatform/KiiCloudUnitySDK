using System;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage {
    // This file is hack for MonoDevelop 4.0.1
    // I don't know why but Compiler says "Missing XML comment" when this we put this delegate at the end of KiiCallbacks.cs

    /// <summary>
    /// This callback is used when API return a list of KiiACLEntry
    /// </summary>
    public delegate void KiiACLListCallback<T, U>(IList<KiiACLEntry<T, U>> list, Exception e) where T : AccessControllable;

}
