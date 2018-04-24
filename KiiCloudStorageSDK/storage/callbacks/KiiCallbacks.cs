using System;
using System.Collections.Generic;
using System.IO;
using JsonOrg;

namespace KiiCorp.Cloud.Storage {

    internal delegate void KiiJsonCallback(JsonObject json, Exception e);

    /// <summary>
    /// This callback is used when API returns generic types.
    /// </summary>
    public delegate void KiiGenericsCallback<T>(T target, Exception e);

    /// <summary>
    /// This callback is used when API returns KiiUser
    /// </summary>
    public delegate void KiiUserCallback(KiiUser user, Exception e);

    /// <summary>
    /// This callback is used when API does not returns target.
    /// </summary>
    public delegate void KiiCallback(Exception e);

    /// <summary>
    /// This callback is used when API returns a list of KiiUser
    /// </summary>
    public delegate void KiiUserListCallback(IList<KiiUser> user, Exception e);

    /// <summary>
    /// This callback is used when API returns a list of KiiGroup
    /// </summary>
    public delegate void KiiGroupListCallback(IList<KiiGroup> list, Exception e);

    /// <summary>
    /// This callback is used when API returns KiiGroup
    /// </summary>
    public delegate void KiiGroupCallback(KiiGroup group, Exception e);

    /// <summary>
    /// This callback is used when API returns KiiObject.
    /// </summary>
    /// <remarks>
    /// <para>It is not guaranteed that this callback will be called on main thread.</para>
    /// <para>Please refer remark section of each API to know how to handle a response.</para>
    /// </remarks>
    /// <param name="obj">
    /// The receiver KiiObject.
    /// </param>
    /// <param name="e">
    /// The exception thrown in execution. If this value is null, execution is done successfully. 
    /// </param>
    public delegate void KiiObjectCallback(KiiObject obj, Exception e);

    /// <summary>
    /// This callback is used when API returns KiiObject and stream
    /// </summary>
    /// <remarks>
    /// <para>It is not guaranteed that this callback will be called on main thread.</para>
    /// <para>Please refer remark section of each API to know how to handle a response.</para>
    /// </remarks>
    /// <param name="obj">
    /// The receiver KiiObject.
    /// </param>
    /// <param name="stream">
    /// The stream you passed to an argument.
    /// </param>
    /// <param name="e">
    /// The exception thrown in execution. If this value is null, the execution is done successfully. 
    /// </param>
    public delegate void KiiObjectBodyDownloadCallback(KiiObject obj, Stream stream, Exception e);

    /// <summary>
    /// This callback is used by the API to notify upload or download progress.
    /// </summary>
    /// <remarks>
    /// This callback doesn't work properly when use KiiInitializeBehaviour in order to initialize the KiiCloudSDK.
    /// <para>It is not guaranteed that this callback will be called on main thread.</para>
    /// <para>This callback will be called at least once.</para>
    /// <para>Please refer remark section of each API to know how to handle a response.</para>
    /// </remarks>
    /// <param name="obj">
    /// The receiver KiiObject.
    /// </param>
    /// <param name="completedInBytes">
    /// Completed size of transfer in bytes.
    /// </param>
    /// <param name="totalSizeinBytes">
    /// Total size of transfer in bytes.
    /// </param>
    public delegate void KiiObjectBodyProgressCallback(KiiObject obj, long completedInBytes, long totalSizeinBytes);

    /// <summary>
    /// This callback is used by the API to notify upload or download progress.
    /// </summary>
    /// <remarks>
    /// Use this callback instead of <see cref="KiiHttpClientProgressCallback"/> if your app runs on the Unity Game Engine.
    /// <para>It is not guaranteed that this callback will be called from the main thread.</para>
    /// <para>This callback will be called at least once.</para>
    /// <para>Please refer to the remarks section of each API to know how to handle a response.</para>
    /// 
    /// <para>The value of progress can differ from the expected value in certain conditions.</para>
    /// <list type="table">
    ///   <listheader>
    ///     <term>Platform</term>
    ///     <description>Upload</description>
    ///     <description>Download</description>
    ///   </listheader>
    ///   <item>
    ///     <term>Android</term>
    ///     <description>progress is always 1</description>
    ///     <description>OK (may depend on the device)</description>
    ///   </item>
    ///   <item>
    ///     <term>iOS</term>
    ///     <description>OK</description>
    ///     <description>OK</description>
    ///   </item>
    ///   <item>
    ///     <term>WebPlayer</term>
    ///     <description>progress is always 0</description>
    ///     <description>OK</description>
    ///   </item>
    /// </list>
    /// </remarks>
    /// <param name="obj">
    /// The receiver KiiObject.
    /// </param>
    /// <param name="progress">
    /// This is a value between zero and one; 0 means no progess, 1 means download/upload complete.
    /// </param>
    public delegate void KiiObjectBodyProgressPercentageCallback(KiiObject obj, float progress);

    /// <summary>
    /// This callback is used when API returns KiiObject and the URL for attached file.
    /// </summary>
    /// <remarks>
    /// <para>It is not guaranteed that this callback will be called on main thread.</para>
    /// <para>Please refer remark section of each API to know how to handle a response.</para>
    /// </remarks>
    /// <param name="obj">
    /// The receiver KiiObject.
    /// </param>
    /// <param name="url">
    /// The URL for attached file.
    /// </param>
    /// <param name="e">
    /// The exception thrown in execution. If this value is null, execution is done successfully. 
    /// </param>
    public delegate void KiiObjectPublishCallback(KiiObject obj, string url, Exception e);

    /// <summary>
    /// This callback is used by Query API
    /// </summary>
    public delegate void KiiQueryCallback<T>(KiiQueryResult<T> result, Exception e);

    /// <summary>
    /// This callback is used when API returns KiiBucket
    /// </summary>
    public delegate void KiiBucketCallback(KiiBucket bucket, Exception e);

    /// <summary>
    /// This callback is used when count aggregation execution completed.
    /// </summary>
    /// <remarks>
    /// <para>It is not guaranteed that this callback will be called on main thread.</para>
    /// <para>Please refer remark section of each API to know how to handle a response.</para>
    /// </remarks>
    /// <param name="bucket">
    /// On which the query executed.
    /// </param>
    /// <param name="query">
    /// Executed query.
    /// </param>
    /// <param name="count">
    /// Number of objects matched specified query.
    /// </param>
    /// <param name="e">
    /// The exception thrown in execution. If this value is null, execution is done successfully.
    /// </param>
    public delegate void CountCallback(KiiBucket bucket, KiiQuery query, int count, Exception e);

    /// <summary>
    /// This callback is used when API returns ServerCodeExecResult.
    /// </summary>
    /// <remarks>
    /// <para>It is not guaranteed that this callback will be called on main thread.</para>
    /// <para>Please refer remark section of each API to know how to handle a response.</para>
    /// </remarks>
    /// <param name="entry">
    /// The receiver KiiServerCodeEntry.
    /// </param>
    /// <param name="argument">
    /// The given argument.
    /// </param>
    /// <param name="execResult">
    /// The result of Server code execution.
    /// </param>
    /// <param name="e">
    /// The exception thrown in execution. If this value is null, execution is done successfully. 
    /// </param>
    public delegate void KiiServerCodeEntryCallback(KiiServerCodeEntry entry, KiiServerCodeEntryArgument argument,
                                                    KiiServerCodeExecResult execResult, Exception e);

    /// <summary>
    /// Kii push installation callback.
    /// </summary>
    /// <param name="e">If exception is null, execution is succeeded.</param>
    public delegate void KiiPushInstallationCallback(Exception e);
    
    /// <summary>
    /// Kii subscription callback.
    /// </summary>
    /// <param name="target">The target KiiSubscribable.</param>
    /// <param name="e">If exception is null, execution is succeeded.</param>
    public delegate void KiiSubscriptionCallback(KiiSubscribable target, Exception e);

    /// <summary>
    /// Kii check subscription callback.
    /// </summary>
    /// <remarks></remarks>
    /// <param name="target">The target KiiSubscribable.</param>
    /// <param name="isSubscribed">true if the target is subscribed, otherwise false.</param>
    /// <param name="e">If exception is null, execution is succeeded.</param>
    public delegate void KiiCheckSubscriptionCallback(KiiSubscribable target, bool isSubscribed, Exception e);

    /// <summary>
    /// Kii topic callback.
    /// </summary>
    /// <param name="target">The target KiiTopic.</param>
    /// <param name="e">If exception is null, execution is succeeded.</param>
    public delegate void KiiTopicCallback(KiiTopic target, Exception e);

    /// <summary>
    /// Kii push message callback.
    /// </summary>
    /// <param name="target">The target KiiPushMessage.</param>
    /// <param name="e">If exception is null, execution is succeeded.</param>
    public delegate void KiiPushMessageCallback(KiiPushMessage target, Exception e);

    /// <summary>
    /// Kii ACL callback.
    /// </summary>
    public delegate void KiiACLCallback<T, U>(KiiACLEntry<T, U> entry, Exception e) where T : AccessControllable;
}

