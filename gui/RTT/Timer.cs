﻿using RTT.Core.Models;
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
using System.IO.Ports;

namespace RTT
{
    public partial class Timer : Form
    {
        Stopwatch stopwatch;
        System.Timers.Timer solveTimer;

        IEnumerable<User> _users = null;
        User _currentUser = null;

        public Timer()
        {
            InitializeComponent();
            BindUsers();
            BindGrids();

            stopwatch = new Stopwatch();
            
            solveTimer = new System.Timers.Timer();
            solveTimer.Interval = 10 * 1;
            solveTimer.Elapsed += solveTimer_Tick;
        }

        public void BindGrids()
        {
            dgvLast10.DataSource = Database.TopSolveTimes(10, _currentUser.UserId).ToList();
            dgvTop10.DataSource = Database.TopSolveTimes(10).ToList();
        }


        private delegate void UpdateStatusDelegate(string status);
        private void UpdateStatus(string status)
        {
            if (this.lblTime.InvokeRequired)
            {
                this.Invoke(new UpdateStatusDelegate(this.UpdateStatus), new object[] { status });
                return;
            }

            this.lblTime.Text = status;
        }

        void solveTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus(stopwatch.Elapsed.ToString(@"mm\:ss\.ff"));
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
            dgvLast10.DataSource = Database.TopSolveTimes(10, _currentUser.UserId).ToList();
            

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void btnReady_Click(object sender, EventArgs e)
        {
            btnReady.Text = "Pick up cube to begin";
            btnReady.Enabled = false;

            OpenSerialPort("COM3");
        }
        
        private SerialPort _serialPort;
        private int _previousValue = -10000;

        private void OpenSerialPort(string portName)
        {
            _serialPort = new SerialPort(portName);
            _serialPort.Open();
            _serialPort.DataReceived += serialPort_DataReceived;
        }

        private void serialPort_DataReceived(object s, SerialDataReceivedEventArgs e)
        {
            int value = Convert.ToInt32(_serialPort.ReadLine());

            if (_previousValue == -10000)
            {
                _previousValue = value;
            }
            else
            {
                if (value != 0)
                {
                    decimal difference = (_previousValue - value) * -1;

                    if (difference > 80)
                    {
                        if (!stopwatch.IsRunning)
                        {
                            Debug.WriteLine("Starting timer");
                            Start();
                            
                        }
                    }
                    else
                    {
                        if (stopwatch.IsRunning)
                        {
                            Debug.WriteLine("Stopping timer");
                            Stop();
                        }
                    }
                }
                else
                {
                    if (stopwatch.IsRunning)
                    {
                        Debug.WriteLine("Stopping timer");
                        Stop();
                    }
                }
            }
        }

        private void Start()
        {
            stopwatch.Start();
            solveTimer.Start();

            //cboUsers.Enabled = false;
            //btnSave.Enabled = false;
            
        }

        private void Stop()
        {
            stopwatch.Stop();
            solveTimer.Stop();

            //cboUsers.Enabled = true;
            //btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var solveTime = new SolveTime();

            solveTime.UserId = _currentUser.UserId;
            solveTime.SolveDate = DateTime.Now;
            solveTime.ElapsedTime = stopwatch.Elapsed;

            Database.DBContext.SolveTimes.Add(solveTime);

            Database.DBContext.SaveChanges();

            stopwatch.Reset();
            lblTime.Text = "00:00:00";

            BindGrids();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvLast10_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
