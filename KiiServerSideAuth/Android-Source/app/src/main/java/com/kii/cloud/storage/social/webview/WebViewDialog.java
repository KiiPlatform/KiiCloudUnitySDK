package com.kii.cloud.storage.social.webview;

import java.io.PrintWriter;
import java.io.StringWriter;

import org.json.JSONException;
import org.json.JSONObject;

import android.app.Activity;
import android.app.Dialog;
import android.net.Uri;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup.LayoutParams;
import android.view.Window;
import android.webkit.CookieManager;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.Button;

import com.unity3d.player.UnityPlayer;

public class WebViewDialog {

    private static Dialog CurrentDialog;

    public static void showDialog(
            final String gameObjectName,
            final String targetUrl,
            final String resultUrl,
            final String userAgent)
    {
        final Activity current = UnityPlayer.currentActivity;
        current.runOnUiThread(new Runnable() {
            private boolean isFinished = false;

            @Override
            public void run() {
                int dialogId = current.getResources().getIdentifier(
                        "kii_server_side_auth_dialog", "layout",
                        current.getPackageName());
                if (dialogId == 0) {
                    finish(gameObjectName, createErrorJSON(
                            "layout.kii_server_side_auth_dialog is not found."));
                    return;
                }
                int webViewId = current.getResources().getIdentifier(
                        "kii_server_side_auth_dialog_webview", "id",
                        current.getPackageName());
                if (webViewId == 0) {
                    finish(gameObjectName, createErrorJSON(
                            "id.kii_server_side_auth_dialog_webview is not found."));
                    return;
                }
                int buttonId = current.getResources().getIdentifier(
                        "kii_server_side_auth_dialog_button", "id",
                        current.getPackageName());
                if (buttonId == 0) {
                    finish(gameObjectName, createErrorJSON(
                            "id.kii_server_side_auth_dialog_button is not found."));
                    return;
                }

                Dialog dialog = new Dialog(current);
                dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
                dialog.setContentView(dialogId);
                dialog.setCanceledOnTouchOutside(false);

                WebView webView = (WebView)dialog.findViewById(webViewId);
                webView.getSettings().setCacheMode(WebSettings.LOAD_NO_CACHE);
                webView.getSettings().setJavaScriptEnabled(true);
                webView.getSettings().setLoadWithOverviewMode(true);
                webView.getSettings().setUseWideViewPort(true);
                if (userAgent != null) {
                    webView.getSettings().setUserAgentString(userAgent);
                }
                webView.getSettings().setBuiltInZoomControls(true);
                webView.setWebViewClient(new WebViewClient() {
                    @Override
                    public void onLoadResource (WebView view, String url) {
                        if (isResultUri(resultUrl, url)) {
                            view.stopLoading();
                        }
                    }

                    @Override
                    public void onPageFinished(WebView target, String url) {
                        if (isResultUri(resultUrl, url)) {
                            finish(gameObjectName, createFinishedJSON(url));
                        }
                    }

                    @Override
                    public void onReceivedError(
                        WebView view,
                        int errorCode,
                        String description,
                        String failingUrl)
                    {
                        switch (errorCode) {
                            case WebViewClient.ERROR_CONNECT:
                            case WebViewClient.ERROR_TIMEOUT:
                            case WebViewClient.ERROR_HOST_LOOKUP:
                                // Expected errors.
                                break;
                            default:
                            {
                                Log.d("WebViewDialog", String.format(
                                            "unexpected error. code=%d, description=%s",
                                            errorCode, description));
                            }
                        }
                        finish(gameObjectName, createRetryLaterJSON());
                    }

                    private boolean isResultUri(
                            String redirectUrl,
                            String url)
                    {
                    	Uri redirectUri = Uri.parse(redirectUrl);
                    	Uri uri = Uri.parse(url);
                        if (!redirectUri.getScheme().equals(uri.getScheme())) {
                            return false;
                        } else if (!redirectUri.getHost().equals(uri.getHost())) {
                            return false;
                        } else if (!redirectUri.getPath().equals(uri.getPath())) {
                            return false;
                        }
                        return true;
                    }
                });
                webView.loadUrl(targetUrl);

                Button button = (Button)dialog.findViewById(buttonId);
                button.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        finish(gameObjectName, createCanceledJSON());
                    }
                });

                dialog.show();
                dialog.getWindow().setLayout(LayoutParams.MATCH_PARENT,
                        LayoutParams.MATCH_PARENT);
                CurrentDialog = dialog;
            }

            private void finish(
                    String gameObjectName,
                    JSONObject json)
            {
                synchronized (this) {
                    if (this.isFinished) {
                        return;
                    }
                    this.isFinished = true;
                }
                UnityPlayer.UnitySendMessage(gameObjectName,
                        "OnSocialAuthenticationFinished", json.toString());
                CookieManager.getInstance().removeSessionCookie();
                if (CurrentDialog != null) {
                    CurrentDialog.dismiss();
                    CurrentDialog = null;
                }
            }

        });
    }

    private static JSONObject createFinishedJSON(String url) {
        try {
            JSONObject value = new JSONObject();
            value.put("url", url);
            return createJSON("finished", value);
        } catch (JSONException e) {
            return createExceptionJSON(e);
        }
    }

    private static JSONObject createRetryLaterJSON() {
        try {
            return createJSON("retry", null);
        } catch (JSONException e) {
            return createExceptionJSON(e);
        }
    }

    private static JSONObject createCanceledJSON() {
        try {
            return createJSON("canceled", null);
        } catch (JSONException e) {
            return createExceptionJSON(e);
        }
    }

    private static JSONObject createJSON(String type, JSONObject value) throws JSONException {
        JSONObject retval = new JSONObject();
        retval.put("type", type);
        if (value != null) {
            retval.put("value", value);
        }
        return retval;
    }

    private static JSONObject createExceptionJSON(Exception exception) {
        StringWriter sw = new StringWriter();
        PrintWriter pw = new PrintWriter(sw);
        exception.printStackTrace(pw);

        return createErrorJSON(sw.toString());
    }

    private static JSONObject createErrorJSON(String message) {
        JSONObject retval = new JSONObject();
        try {
            JSONObject value = new JSONObject();
            value.put("message", message);

            retval.put("type", "error");
            retval.put("value", value);
        } catch (JSONException e) {
            Log.e("WebViewDialog", "createErrorJSON", e);
        }
        return retval;
    }

}
