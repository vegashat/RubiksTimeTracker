using RTT.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTT
{
    public partial class UserForm : Form
    {
        IEnumerable<User> _users = null;
        User _currentUser = null;

        public UserForm()
        {
            InitializeComponent();
            BindUsers();
        }

        public void BindUsers()
        {
            _users = Database.DBContext.Users;

            cboUsers.DataSource = _users.ToList();
            cboUsers.DisplayMember = "Username";
            cboUsers.ValueMember = "UserId";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentUser = cboUsers.SelectedItem as User;

            if (_currentUser != null)
            {
                txtUsername.Text = _currentUser.Username.ToString();
                txtFirstName.Text = _currentUser.FirstName;
                txtLastName.Text = _currentUser.LastName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        { 
            if (_currentUser == null)
            {
                _currentUser = new User();
                Database.DBContext.Users.Add(_currentUser);
            }
            else
            {
                Database.DBContext.Users.Attach(_currentUser);
            }

            _currentUser.Username = txtUsername.Text;
            _currentUser.FirstName = txtFirstName.Text;
            _currentUser.LastName = txtLastName.Text;

            Database.DBContext.SaveChanges();
        }

        private void btnAddUse_Click(object sender, EventArgs e)
        {
            _currentUser = null;

            txtUsername.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;

        }

    }
}
