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
using System.Diagnostics;

namespace RTT
{
    public partial class Timer : Form
    {
        Stopwatch stopwatch;

        IEnumerable<User> _users = null;
        User _currentUser = null;

        public Timer()
        {
            InitializeComponent();
            BindUsers();

            stopwatch = new Stopwatch();

            solveTimer.Interval = 10 * 1;
            solveTimer.Tick += solveTimer_Tick;
        }

        void solveTimer_Tick(object sender, EventArgs e)
        {
            lblTime.Text = stopwatch.Elapsed.ToString();
        }

        public void BindUsers()
        {
            _users = Database.DBContext.Users;

            cboUsers.DataSource = _users.ToList();
            cboUsers.DisplayMember = "Username";
            cboUsers.ValueMember = "UserId";
        }
        private void usersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var userForm = new UserForm();

            userForm.ShowDialog();
        }

        private void cboUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentUser = cboUsers.SelectedItem as User;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            stopwatch.Start();
            solveTimer.Start();
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            stopwatch.Stop();
            solveTimer.Stop();
        }

    }
}
