using PrizeBondChecker.Core.CoreUtilities;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Threading;

namespace PrizeBondChecker.WpfClient.Controls.Helpers
{
    public static class Utilities
    {
        private static Regex _alphaNumRegEx = new Regex(@"[^a-zA-Z0-9\-\s&]+");
        private static Regex _numberRegex = new Regex("[^0-9]+");
        public static string GetShortDateString(this DateTime? val)
        {
            return val.HasValue ? val.Value.ToShortDateString() : string.Empty;
        }

        //public static ImageSource GetImageSource(byte[] content)
        //{
        //    if (content != null && content.LongLength > 0)
        //    {
        //        using (var stream = new MemoryStream(content))
        //        {
        //            var bitmap = new BitmapImage();
        //            stream.Position = 0;
        //            bitmap.BeginInit();
        //            bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
        //            bitmap.CacheOption = BitmapCacheOption.OnLoad;
        //            bitmap.UriSource = null;
        //            bitmap.StreamSource = stream;
        //            bitmap.EndInit();

        //            bitmap.Freeze();

        //            return bitmap;
        //        }

        //    }
        //    return null;
        //}

        //public static BitmapImage LoadBitmapFromResource(string pathInApplication, Assembly assembly = null)
        //{
        //    if (assembly == null)
        //    {
        //        assembly = Assembly.GetCallingAssembly();
        //    }

        //    if (pathInApplication[0] == '/')
        //    {
        //        pathInApplication = pathInApplication.Substring(1);
        //    }
        //    return new BitmapImage(new Uri(@"pack://application:,,,/" + assembly.GetName().Name + ";component/" + pathInApplication, UriKind.Absolute));
        //}

        //public static string QuerySafeValue(this string value)
        //{
        //    if (string.IsNullOrEmpty(value))
        //    {
        //        return value;
        //    }


        //    value = value.Replace("'", "");
        //    value = value.Replace(";", "");
        //    value = value.Replace("&", "");
        //    value = value.Replace("INSERT ", "");
        //    value = value.Replace("DELETE ", "");
        //    value = value.Replace("EXEC ", "");
        //    value = value.Replace("EXEC ", "");

        //    return value;
        //}

        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                var m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static NameValueCollection ParseQueryString(string s)
        {
            var nvc = new NameValueCollection();

            // remove anything other than query string from url
            if (s.Contains("?"))
            {
                s = s.Substring(s.IndexOf('?') + 1);
            }

            foreach (string vp in Regex.Split(s, "&"))
            {
                string[] singlePair = Regex.Split(vp, "=");
                if (singlePair.Length == 2)
                {
                    nvc.Add(singlePair[0], singlePair[1]);
                }
                else
                {
                    // only one key with no value specified in query string
                    nvc.Add(singlePair[0], string.Empty);
                }
            }

            return nvc;
        }

        public static void SetTimeout(int milliseconds, Action func)
        {
            var timer = new DispatcherTimerContainingAction
            {
                Interval = new TimeSpan(0, 0, 0, 0, milliseconds),
                Action = func
            };

            timer.Tick += OnTimeout;
            timer.Start();
        }

        private static void OnTimeout(object sender, EventArgs arg)
        {
            var t = sender as DispatcherTimerContainingAction;
            t.Stop();
            t.Action();
            t.Tick -= OnTimeout;
        }

        //public static DateTimeOffset GetLocalTime()
        //{
        //    try
        //    {
        //        var response = HttpWebRequestHelper.GetHttpWebResponseData(HttpWebRequestHelper.SendGetRequest("http://worldtimeapi.org/api/timezone/Asia/Karachi", null));
        //        JsonSerializer serializer = new JsonSerializer();
        //        serializer.DateParseHandling = DateParseHandling.DateTimeOffset;

        //        var sr = new StringReader(response);

        //        var reader = new JsonTextReader(sr);

        //        JObject data = serializer.Deserialize<JObject>(reader);

        //        DateTimeOffset offset = (DateTimeOffset)data["datetime"];

        //        return offset;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //        return new DateTimeOffset();
        //    }

        //}

        //public static ImageSource ByteToImage(byte[] imageData)
        //{


        //    BitmapImage biImg = new BitmapImage();
        //    MemoryStream ms = new MemoryStream(imageData);
        //    biImg.BeginInit();
        //    biImg.StreamSource = ms;
        //    biImg.EndInit();
        //    biImg.Freeze();

        //    ImageSource imgSrc = biImg as ImageSource;

        //    return imgSrc;
        //}

        //public static bool IsPaymentSenseRunning()
        //{
        //    try
        //    {
        //        return CheckIfWindowsServiceIsRunning("PS Connect");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex.Message, ex);
        //        return false;
        //    }
        //}

        private static bool CheckIfWindowsServiceIsRunning(string serviceName)
        {
            var sc = new ServiceController(serviceName);

            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    return true;

                //case ServiceControllerStatus.Stopped:
                //    return "Stopped";
                //case ServiceControllerStatus.Paused:
                //    return "Paused";
                //case ServiceControllerStatus.StopPending:
                //    return "Stopping";
                //case ServiceControllerStatus.StartPending:
                //    return "Starting";
                default:
                    return false;
            }
        }

        public static string FormatOrderItemDescirptionforPrint(int qty, string description, bool isforKitchen = false)
        {
            if (string.IsNullOrWhiteSpace(description))
                return description;

            var lines = description.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            for (var i = 0; i < lines.Length; i++)
            {
                if (i == 0)
                {
                    sb.Append(GetProductLines(isforKitchen ? 28 : 28, qty, lines[0].Trim()));
                }
                else
                {
                    sb.Append(GetOptionsLines(isforKitchen ? 30 : (lines[i].Trim().StartsWith("#") ? 28 : 36), qty,
                        lines[i].Trim(), !isforKitchen));
                }
            }
            return sb.ToString();
        }

        public static string GetOptionsLines(int numOfChrPL, int qty, string optionLine, bool smallOptions, bool inclQty = false)
        {
            if (string.IsNullOrWhiteSpace(optionLine))
                return optionLine;

            int modifier = 2;

            if (qty > 1 && !inclQty)
                modifier = 4;

            if (qty > 9 && !inclQty)
                modifier = 5;

            var tempProdLines = new StringBuilder();
            bool isNotesLine = false;

            var regex = new Regex(@"[^a-zA-Z0-9\-\s&]+");
            optionLine = regex.Replace(optionLine, String.Empty);
            if (smallOptions)
                tempProdLines.AppendFormat("<span style='font-size:8pt'>");
            if (optionLine.StartsWith("#"))
            {
                isNotesLine = true;
                optionLine = "##" + optionLine.Replace("# ", "") + "##";
            }

            if (optionLine.Length > numOfChrPL - modifier)
            {
                var temp = "";
                var i = 0;
                do
                {
                    temp = GetWordsToFitOnLine(optionLine, numOfChrPL - modifier);
                    if (temp != null && temp.Length > (numOfChrPL - modifier))
                    {
                        temp = optionLine.Substring(0, numOfChrPL - modifier).Trim();
                    }
                    optionLine = optionLine.Replace(temp, "").Trim();

                    if (i == 0)
                    {
                        tempProdLines.AppendFormat("{0}{1}{2}{3}{4}<br/>", GetHtmlSpaces(modifier), (inclQty && qty > 1 ? qty.ToString() : ""), (isNotesLine ? "<b>" : "^"), WebUtility.HtmlEncode(temp), (isNotesLine ? "</b>" : ""));
                    }
                    else
                    {
                        tempProdLines.AppendFormat("{0}{1}<br/>", GetHtmlSpaces(modifier + 1), WebUtility.HtmlEncode(temp));
                    }
                    i++;
                } while (optionLine.Length > numOfChrPL - modifier);

                if (optionLine.Length > 0)
                {
                    tempProdLines.AppendFormat("{0}{1}<br/>", GetHtmlSpaces(modifier + 1), WebUtility.HtmlEncode(optionLine));
                }
            }
            else
            {
                tempProdLines.AppendFormat("{0}{1}{2}{3}{4}", GetHtmlSpaces(modifier), (inclQty && qty > 1 ? qty.ToString() : ""), (isNotesLine ? "<b>" : "^"), WebUtility.HtmlEncode(optionLine), (isNotesLine ? "</b>" : ""));
            }
            if (!tempProdLines.ToString().EndsWith("<br/>"))
                tempProdLines.Append("<br/>");
            if (smallOptions)
                tempProdLines.Append("</span>");
            return tempProdLines.ToString();

        }

        private static string GetWordsToFitOnLine(string text, int chrLimit)
        {
            if (!String.IsNullOrWhiteSpace(text) && text.Length > chrLimit)
            {
                var words = text.Split(' ');
                var newStr = "";
                var i = 1;
                if (words.Length > 1)
                {
                    newStr = words[0];
                    while (newStr.Length <= chrLimit && i < words.Length)
                    {
                        if (String.Format("{0} {1}", newStr, words[i].Trim()).Length <= chrLimit)
                        {
                            newStr = String.Format("{0} {1}", newStr, words[i].Trim());
                        }
                        else
                        {
                            break;
                        }
                        i++;
                    }

                    return newStr;
                }
            }
            return text;
        }


        public static string AlphaNumericOnly(this string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;
            return _alphaNumRegEx.Replace(s, String.Empty);
        }

        public static bool NumericOnly(this string s)
        {
            return _numberRegex.IsMatch(s);
        }
        public static string HtmlAlphaNumericOnly(this string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;

            return _alphaNumRegEx.Replace(s, String.Empty).Replace("&", "");
        }

        public static string GetProductLines(int numOfChrPL, int qty, string prodLine)
        {
            if (string.IsNullOrWhiteSpace(prodLine))
                return prodLine;

            int modifier = 0;

            if (qty > 1)
                modifier = 2;

            if (qty > 9)
                modifier = 3;


            prodLine = _alphaNumRegEx.Replace(prodLine, String.Empty);

            if (prodLine.Length > numOfChrPL - modifier)
            {


                var tempProdLines = new StringBuilder();
                var temp = "";
                var i = 0;
                do
                {
                    temp = GetWordsToFitOnLine(prodLine, numOfChrPL - modifier);
                    if (temp != null && temp.Length > (numOfChrPL - modifier))
                    {
                        temp = prodLine.Substring(0, numOfChrPL - modifier).Trim();
                    }
                    prodLine = prodLine.Replace(temp, "");
                    if (i == 0)
                    {
                        if (qty > 1)
                        {
                            tempProdLines.AppendFormat("<b>{0}*{1}<br/>", qty, WebUtility.HtmlEncode(temp));
                        }
                        else
                        {
                            tempProdLines.AppendFormat("<b>{0}<br/>", WebUtility.HtmlEncode(temp));
                        }
                    }
                    else
                    {
                        tempProdLines.AppendFormat("{0}{1}<br/>", GetHtmlSpaces(modifier), WebUtility.HtmlEncode(temp));
                    }
                    i++;
                } while (prodLine.Length > numOfChrPL - modifier);

                if (prodLine.Length > 0)
                {
                    tempProdLines.AppendFormat("{0}{1}<br/>", GetHtmlSpaces(modifier), WebUtility.HtmlEncode(prodLine));
                }
                tempProdLines.Append("</b>");
                return tempProdLines.ToString();
            }
            else
            {
                if (qty > 1)
                {
                    return String.Format("<b>{0}*{1}</b><br/>", qty, WebUtility.HtmlEncode(prodLine));
                }
                return String.Format("<b>{0}</b><br/>", WebUtility.HtmlEncode(prodLine));
            }
        }

        private static string GetHtmlSpaces(int qty)
        {
            var spaces = "";
            for (var i = 1; i <= qty; i++)
            {
                //if (i%2 == 0)
                //{
                //    spaces += "&nbsp;";
                //}
                //else
                //{
                spaces += "&nbsp;";
                //}

            }

            return spaces;
        }
        //download file or download image

        public static byte[] DownloadImageFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            try
            {
                using (var client = new WebClient())
                {
                    byte[] imageBytes;
                    using (var webClient = new WebClient())
                    {
                        imageBytes = webClient.DownloadData(url);
                    }
                    client.Dispose();
                    return imageBytes;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                return null;
            }
        }
    }

    public class DispatcherTimerContainingAction : DispatcherTimer
    {
        /// <summary>
        /// uncomment this to see when the DispatcherTimerContainingAction is collected
        /// if you remove  t.Tick -= _onTimeout; line from _onTimeout method
        /// you will see that the timer is never collected
        /// </summary>
        //~DispatcherTimerContainingAction()
        //{
        //    throw new Exception("DispatcherTimerContainingAction is disposed");
        //}

        public Action Action { get; set; }
    }
}
