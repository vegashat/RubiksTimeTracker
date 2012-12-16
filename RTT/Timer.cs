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

        // The last time the timer was started
        private DateTime _startTime = DateTime.MinValue;

        // Time between now and when the timer was started last
        private TimeSpan _currentElapsedTime = TimeSpan.Zero;

        // Time between now and the first time timer was started after a reset
        private TimeSpan _totalElapsedTime = TimeSpan.Zero;

        // Whether or not the timer is currently running
        private bool _timerRunning = false;


        public Timer()
        {
            InitializeComponent();
            BindUsers();

            solveTimer.Interval = 1000 * 1;
            solveTimer.Tick += solveTimer_Tick;
        }

        void solveTimer_Tick(object sender, EventArgs e)
        {
            // We do this to chop off any stray milliseconds resulting from 
            // the Timer's inherent inaccuracy, with the bonus that the 
            // TimeSpan.ToString() method will now show correct HH:MM:SS format
            var timeSinceStartTime = DateTime.Now - _startTime;
            timeSinceStartTime = new TimeSpan(timeSinceStartTime.Hours,
                                              timeSinceStartTime.Minutes,
                                              timeSinceStartTime.Seconds);

            // The current elapsed time is the time since the start button was
            // clicked, plus the total time elapsed since the last reset
            _currentElapsedTime = timeSinceStartTime + _totalElapsedTime;

            // These are just two Label controls which display the current 
            // elapsed time and total elapsed time
            lblTime.Text = timeSinceStartTime.ToString();
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

        private void Timer_Load(object sender, EventArgs e)
        {

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            var solveTime = new SolveTime();

            solveTime.UserId = _currentUser.UserId;
            solveTime.SolveDate = DateTime.Now;
            solveTime.ElapsedTime = _currentElapsedTime;

            Database.DBContext.SolveTimes.Add(solveTime);

            Database.DBContext.SaveChanges();
        }


    }
}
