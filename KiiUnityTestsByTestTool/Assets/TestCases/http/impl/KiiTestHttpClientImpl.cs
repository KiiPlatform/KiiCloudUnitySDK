  using System;
  using System.IO;
  using System.Net;
  using System.Text;

  namespace KiiCorp.Cloud.Storage
  {
    /// <summary>
    /// Implementation of Kii http client.
    /// </summary>
    internal class KiiTestHttpClientImpl : KiiTestHttpClient
    {
      protected static Encoding enc = Encoding.GetEncoding("UTF-8");
      protected HttpWebRequest request;
      private KiiTestHttpHeaderList headers;
      private string appID;
      private string appKey;

      public KiiTestHttpClientImpl(string url, string appID, string appKey, KiiTestHttpMethod method)
      {
        request = (HttpWebRequest)WebRequest.Create(url);
        this.appID = appID;
        this.appKey = appKey;
        headers = new KiiTestHttpHeaderListImpl(request);
        this.headers ["X-Kii-AppID"] = this.appID;
        this.headers ["X-Kii-AppKey"] = this.appKey;
        switch (method)
        {
          case KiiTestHttpMethod.GET:
            request.Method = "GET";
            break;
          case KiiTestHttpMethod.POST:
            request.Method = "POST";
            break;
          case KiiTestHttpMethod.PUT:
            request.Method = "PUT";
            break;
          case KiiTestHttpMethod.DELETE:
            request.Method = "DELETE";
            break;
        }

        if (Kii.Logger != null)
        {
          Kii.Logger.Debug("Request {0} {1}", request.Method, url);
        }

      }

          #region properties
      public string Body
      {
        set
        {
          if (Kii.Logger != null)
          {
            Kii.Logger.Debug("request body : {0}", value);
          }
          byte[] data = enc.GetBytes(value);
          request.ContentLength = data.Length;

          System.IO.Stream reqStream = null;
          try
          {
            reqStream = request.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
          } finally
          {
            if (reqStream != null)
            {
              reqStream.Close();
            }
          }
        }
      }

      public string ContentType
      {
        set
        {
          request.ContentType = value;
          if (Kii.Logger != null)
          {
            Kii.Logger.Debug("request Content-Type: {0}", value);
          }
        }
      }

      public string Accept
      {
        set
        {
          request.Accept = value;
          if (Kii.Logger != null)
          {
            Kii.Logger.Debug("request Accept: {0}", value);
          }
        }
      }

      public KiiTestHttpHeaderList Headers
      {
        get
        {
          return headers;
        }
      }

          #endregion

          

      public KiiTestHttpResponse SendRequest()
      {
        try
        {
          HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();

          KiiTestHttpResponse res = new KiiTestHttpResponse();
          // status
          res.Status = (int)httpResponse.StatusCode;
          // Etag
          res.ETag = httpResponse.Headers ["ETag"];
          // ContentType
          res.ContentType = httpResponse.ContentType;
          // read body
          res.Body = ReadBodyFromResponse(httpResponse);

          return res;
        } catch (System.Net.WebException e)
        {
          Console.Write("Exception " + e.Message);
          System.Net.HttpWebResponse err = (System.Net.HttpWebResponse)e.Response;
          // read body
          string body = ReadBodyFromResponse(err);

          throw new KiiTestHttpException((int)err.StatusCode, body);
        }
      }

      protected string ReadBodyFromResponse(System.Net.HttpWebResponse response)
      {
        System.IO.Stream s = null;
        System.IO.StreamReader sr = null;
        try
        {
          s = response.GetResponseStream();
          sr = new System.IO.StreamReader(s, enc);
          string body = sr.ReadToEnd();
          
          if (Kii.Logger != null)
          {
            Kii.Logger.Debug("Response HTTP {0}\nResponse body : {1}", response.StatusCode, body);
          }
          
          return body;
        } finally
        {
          if (sr != null)
          {
            sr.Close();
          }
          if (s != null)
          {
            s.Close();
          }
        }
      }


    }
  }

