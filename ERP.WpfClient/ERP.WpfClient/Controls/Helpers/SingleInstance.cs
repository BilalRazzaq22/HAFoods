using ERP.WpfClient;
using ERP.WpfClient.Messages.Helpers;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace PrizeBondChecker.WpfClient.Controls.Helpers
{
    internal class SingleInstance : IDisposable
    {
        public delegate void ArgsHandler(MobArgument args);
        public event ArgsHandler ArgsRecieved;
        private readonly Guid _appGuid;
        private readonly string _assemblyName;
        private readonly App _appContext;
        private Mutex _mutex;
        private bool _owned;
        private Window _window;

        private class Bridge
        {
            public event Action<Guid> BringToFront;
            public event Action<Guid, MobArgument> ProcessArgs;
            private static readonly Bridge _instance = new Bridge();

            public void OnBringToFront(Guid appGuid)
            {
                if (BringToFront != null)
                    BringToFront(appGuid);
            }

            public void OnProcessArgs(Guid appGuid, MobArgument args)
            {
                if (ProcessArgs != null)
                    ProcessArgs(appGuid, args);
            }

            static Bridge()
            {
            }

            public static Bridge Instance
            {
                get { return _instance; }
            }
        }

        private class RemotableObject : MarshalByRefObject
        {
            public void BringToFront(Guid appGuid)
            {
                Bridge.Instance.OnBringToFront(appGuid);
            }

            public void ProcessArguments(Guid appGuid, MobArgument args)
            {
                Bridge.Instance.OnProcessArgs(appGuid, args);
            }
        }

        public SingleInstance(Guid appGuid, App appContext)
        {
            _appGuid = appGuid;
            _assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            _appContext = appContext;
            Bridge.Instance.BringToFront += BringToFront;
            Bridge.Instance.ProcessArgs += ProcessArgs;
            _mutex = new Mutex(true, _assemblyName + _appGuid, out _owned);
        }

        public void Dispose()
        {
            if (_owned) // always release a mutex if you own it
            {
                _owned = false;
                _mutex.ReleaseMutex();
            }
        }

        public void ShutDown()
        {
            _appContext.Shutdown();
            if (_owned) // always release a mutex if you own it
            {
                _owned = false;
                _mutex.ReleaseMutex();
            }
        }

        public void Run(Func<Window> showWindow, string[] args)
        {
            if (_owned)
            {
                // show the main app window
                _window = showWindow();
                // and start the service
                //   StartService();
                // _appContext.ReceiveArguments(ProcessMedSolCommandLineArguments(args));
            }
            else
            {
                try
                {
                    //MessageBox.Show("IsNetworkDeployed:" + ApplicationDeployment.IsNetworkDeployed);
                    //MessageBox.Show("ActivationUri:" + ApplicationDeployment.CurrentDeployment.ActivationUri);
                    //MessageBox.Show("ActivationUri.Query:" + ApplicationDeployment.CurrentDeployment.ActivationUri.Query);

                    //if (ApplicationDeployment.IsNetworkDeployed &&
                    //(ApplicationDeployment.CurrentDeployment.ActivationUri != null))
                    //{

                    //    SendWebArgs(ApplicationDeployment.CurrentDeployment.ActivationUri.Query);
                    //}
                    //else
                    //{
                    SendCommandLineArgs(ProcessMedSolCommandLineArguments(args));
                    //}
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Application.Current.Shutdown();
            }
        }

        private MobArgument ProcessMedSolCommandLineArguments(string[] args)
        {
            var mArgs = new MobArgument();
            if (args != null && args.Length >= 2)
            {
                for (var i = 0; i < args.Length; i++)
                {
                    if (args[i].Trim().Equals("ordId"))
                    {
                        int caseId = 0;
                        int.TryParse(args[++i], out caseId);
                        mArgs.UserId = caseId;
                    }
                }
            }

            return mArgs;
        }

        //private void StartService()
        //{
        //    try
        //    {
        //        var channel = new IpcServerChannel("pvp");
        //        ChannelServices.RegisterChannel(channel, false);

        //        RemotingConfiguration.RegisterActivatedServiceType(typeof(RemotableObject));
        //    }
        //    catch
        //    {
        //        // log it
        //    }
        //}

        private void BringToFront(Guid appGuid)
        {
            if (appGuid == _appGuid)
            {
                _window.Dispatcher.BeginInvoke((ThreadStart)delegate ()
                {
                    if (_window.WindowState == WindowState.Minimized)
                        _window.WindowState = WindowState.Normal;
                    _window.Activate();
                });
            }
        }

        private void ProcessArgs(Guid appGuid, MobArgument args)
        {
            if (appGuid == _appGuid && ArgsRecieved != null)
            {
                _window.Dispatcher.BeginInvoke((ThreadStart)delegate ()
                {
                    ArgsRecieved(args);
                });
            }
        }

        //private void SendWebArgs(string queryString)
        //{
        //    var args = new MobArgument();


        //    if (!String.IsNullOrEmpty(queryString))
        //    {
        //        var rawArgs = Utilities.ParseQueryString(queryString);

        //        foreach (var key in rawArgs.AllKeys)
        //        {
        //            if (key.Trim() == "caseId")
        //            {

        //                int caseId = 0;
        //                int.TryParse(rawArgs[key.Trim()], out caseId);

        //                //MessageBox.Show("CaseId: " + caseId);
        //                args.UserId = caseId;
        //            }

        //        }
        //    }

        //    SendArgs(args);
        //}



        private void SendArgs(MobArgument args)
        {
            try
            {
                //IpcClientChannel channel = new IpcClientChannel();
                //ChannelServices.RegisterChannel(channel, false);

                //RemotingConfiguration.RegisterActivatedClientType(typeof(RemotableObject), "ipc://pvp");

                RemotableObject proxy = new RemotableObject();
                proxy.BringToFront(_appGuid);
                proxy.ProcessArguments(_appGuid, args);
            }
            catch (Exception ex)
            { // log it
                var processes = Process.GetProcessesByName("FridayRetail");

                if (processes != null)
                {
                    foreach (var process in processes)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch (Exception ex1)
                        {
                            throw ex1;
                        }

                    }
                }

                MessageBox.Show("Exception Sending Args : " + ex.Message);
            }
        }

        private void SendCommandLineArgs(MobArgument args)
        {
            SendArgs(args);
        }
    }
}
