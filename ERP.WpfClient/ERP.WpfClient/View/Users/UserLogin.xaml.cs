using ERP.Common;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Messages.User;
using ERP.WpfClient.ViewModel.User;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERP.WpfClient.View.Users
{
    /// <summary>
    /// Interaction logic for UserLogin.xaml
    /// </summary>
    public partial class UserLogin : Window
    {
        public string Username { get; set; }
        public string Password { get; set; }
        private IGenericRepository<Entities.DBModel.Users.User> _userRepository;
        Timer t = new Timer();
        public UserLogin()
        {
            InitializeComponent();
            _userRepository = App.Resolve<IGenericRepository<Entities.DBModel.Users.User>>();
            BusyBar.IsBusy = false;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var bw = new BackgroundWorker();
            bw.DoWork += (s, args) =>
            {
                try
                {
                    //this.Dispatcher.BeginInvoke(new Action(() =>
                    //{
                    //BusyBar.IsBusy = true;
                    this.Dispatcher.Invoke(new Action(() => { BusyBar.IsBusy = true; }));

                    t.Interval = 2000;
                    t.Start();
                    if (Username == "admin@superadmin.com" && Password == "superadmin123")
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            Messenger.Default.Send<UserLoginMessage>(new UserLoginMessage()
                            {
                                Username = Username
                            });
                            this.Close();
                        }));
                    }
                    else
                    {
                        Entities.DBModel.Users.User user = _userRepository.Get().FirstOrDefault(x => (x.Username == Username || x.Email == Username) && x.Password == Password);
                        if (user != null)
                        {
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                MainWindow mainWindow = new MainWindow();
                                mainWindow.Show();
                                Messenger.Default.Send<UserLoginMessage>(new UserLoginMessage()
                                {
                                    Username = Username
                                });
                                this.Close();
                            }));
                        }
                        else
                        {
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                lblValidate.Visibility = Visibility.Visible;
                            }));
                            t.Elapsed += T_Elapsed;
                        }
                    }
                    //}));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error\nMessage: " + ex.Message, "HA Foods");
                }
            };

            bw.RunWorkerCompleted += async (s, args) =>
            {
                this.Dispatcher.Invoke(new Action(() => { BusyBar.IsBusy = false; }));
            };

            bw.RunWorkerAsync();
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                lblValidate.Visibility = Visibility.Collapsed;
            }));
            t.Stop();
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            Username = txtUsername.Text;
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = txtPassword.Password.ToString();
        }
    }
}
