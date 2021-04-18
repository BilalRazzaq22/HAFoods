using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model.User;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ERP.WpfClient.ViewModel.User
{
    public class UserViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private readonly IGenericRepository<Entities.DBModel.Users.User> _userRepository;
        private UserModel _userModel;
        private ObservableCollection<UserModel> _customerList;
        private string _userButton;
        private string _userParameter;
        private UserGroupModel _userGroupModel;
        private ObservableCollection<UserGroupModel> _userGroupList;

        #endregion

        #region Ctor

        public UserViewModel()
        {
            UserCommands = new RelayCommand<object>(ExecuteUserCommand);
            DeleteUserCommand = new RelayCommand<object>(ExecuteDeleteUserCommand);
            //this.UserCommands = new CustomerCommand(this);
            _userRepository = App.Resolve<IGenericRepository<Entities.DBModel.Users.User>>();
            UserModel = new UserModel();
            UserList = new ObservableCollection<UserModel>();
            UserButton = "Save";
            UserParameter = "SaveUser";
        }

        #endregion

        #region Properties
        public RelayCommand<object> UserCommands { get; set; }
        public RelayCommand<object> DeleteUserCommand { get; set; }

        //public CustomerCommand UserCommands { get; set; }

        public UserModel UserModel
        {
            get { return _userModel; }
            set { _userModel = value; RaisePropertyChanged("UserModel"); }
        }

        public ObservableCollection<UserModel> UserList
        {
            get { return _customerList; }
            set { _customerList = value; RaisePropertyChanged("UserList"); }
        }

        public UserGroupModel UserGroupModel
        {
            get { return _userGroupModel; }
            set { _userGroupModel = value; RaisePropertyChanged("UserGroupModel"); }
        }

        public ObservableCollection<UserGroupModel> UserGroupList
        {
            get { return _userGroupList; }
            set { _userGroupList = value; RaisePropertyChanged("UserGroupList"); }
        }

        public string UserParameter
        {
            get { return _userParameter; }
            set { _userParameter = value; RaisePropertyChanged("UserParameter"); }
        }

        public string UserButton
        {
            get { return _userButton; }
            set { _userButton = value; RaisePropertyChanged("UserButton"); }
        }

        #endregion

        #region Methods

        private void ExecuteUserCommand(object str)
        {
            if (str as string == "SaveUser")
            {
                SaveUser();
            }
            else if (str as string == "UpdateUser")
            {
                UpdateUser();
            }
            else if (str as string == "Clear")
            {
                Reset();
            }
            else if (str != null)
            {
                EditUser(str as UserModel);
            }
        }

        private void Reset()
        {
            UserModel = new UserModel();
            UserButton = "Save";
            UserParameter = "SaveUser";
            GetUserGroup();
        }

        private void ExecuteDeleteUserCommand(object obj)
        {
            if (obj != null)
            {
                ApplicationManager.Instance.ShowConfirmDialog("Are you sure you want to Delete the user?", () =>
                {
                    DeleteCustomer(obj as UserModel);
                    ApplicationManager.Instance.HideDialog();
                }, () => ApplicationManager.Instance.HideMessageBox(), useYesNo: true);
            }
        }

        public void SaveUser()
        {
            if (IsValidateUser(UserModel))
            {
                ApplicationManager.Instance.ShowMessageBox("User already exists");
            }
            else
            {
                UserModel.UserGroup = UserGroupModel.GroupName;
                var model = _userRepository.Add(MapperProfile.iMapper.Map<Entities.DBModel.Users.User>(UserModel));
                UserModel.Id = model.Id;
                UserList.Add(UserModel);
                Reset();
            }
        }

        public void EditUser(UserModel userModel)
        {
            UserButton = "Update";
            UserParameter = "UpdateUser";
            UserModel.Id = userModel.Id;
            UserModel.Username = userModel.Username;
            UserModel.UserGroup = userModel.UserGroup;
            UserModel.Email = userModel.Email;
            UserModel.Password = userModel.Password;
            UserGroupModel = UserGroupList.FirstOrDefault(x => x.GroupName == userModel.UserGroup);
        }

        public void UpdateUser()
        {
            UserModel.UserGroup = UserGroupModel.GroupName;
            _userRepository.Update(MapperProfile.iMapper.Map<Entities.DBModel.Users.User>(UserModel), UserModel.Id);
            Reset();
        }

        public void DeleteCustomer(UserModel userModel)
        {
            _userRepository.Delete(userModel.Id);
            UserList.Remove(userModel);
        }

        private void Init()
        {
            var bw = new BackgroundWorker();
            List<Entities.DBModel.Users.User> users = null;
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    ApplicationManager.Instance.ShowBusyInidicator("Loading Data... !");
                    users = _userRepository.Get();
                    InitUserGroupList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Post Error\nMessage: " + ex.Message, "HA Foods");
                }
            };

            bw.RunWorkerCompleted += async (sender, args) =>
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    UserList = MapperProfile.iMapper.Map<ObservableCollection<UserModel>>(users);
                }));
                ApplicationManager.Instance.HideBusyInidicator();
                Reset();
            };

            bw.RunWorkerAsync();
        }

        private void InitUserGroupList()
        {
            UserGroupList = new ObservableCollection<UserGroupModel>();
            UserGroupList.Add(new UserGroupModel() { GroupName = "Admin" });
            UserGroupList.Add(new UserGroupModel() { GroupName = "Manager" });
            UserGroupList.Add(new UserGroupModel() { GroupName = "Guest" });
            UserGroupList.Add(new UserGroupModel() { GroupName = "User" });
        }

        private void GetUserGroup()
        {
            UserGroupModel = UserGroupList.FirstOrDefault();
        }

        private bool IsValidateUser(UserModel userModel)
        {
            return _userRepository.Get().Any(x => x.Email == userModel.Email);
        }

        public void OnBringIntoView()
        {
            Init();
            //Reset();
            //LoadCustomerReport();
        }

        #endregion
    }
}
